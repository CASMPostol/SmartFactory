//<summary>
//  Title   : Sheeping Substate Machine Context
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;

namespace CAS.SmartFactory.Shepherd.DataModel.Entities.ShippingSubstateMachine
{
  /// <summary>
  /// Shipment Substrate Machine Context
  /// </summary>
  internal class Context: IContextTrigers
  {

    #region creator
    internal Context( Shipping parent )
    {
      m_Parent = parent;
    }
    #endregion

    /// <summary>
    /// Context Exception
    /// </summary>
    public class ContexException: Exception
    {
      public ContexException( string message ) : base( message ) { }
      public static ContexException TriggerError( string trigger, ShippingState2 contextState )
      {
        string _msg = String.Format
          ( "ShippingSubstateMachine error: the {0} trigger is not allowed in the current state {1}",
            trigger,
            contextState
          );
        return new ContexException( _msg );
      }
    }
    #region IContextTrigers Members
    public void SetAwaiting( bool value )
    {
      m_AbstractState.SetAwaiting( value );
    }
    public void SetEndTime()
    {
      m_AbstractState.SetEndTime();
    }
    public void SetShippingState( ShippingState shippingState )
    {
      m_AbstractState.SetShippingState( shippingState );
    }
    public void SetPartner( Partner partner )
    {
      m_AbstractState.SetPartner( partner );
    }
    #endregion

    #region private

    #region states implementation
    private abstract class AbstractState: IContextTrigers
    {

      internal AbstractState( Context parent )
      {
        Parent = parent;
      }

      #region IContextTrigers Members
      public virtual void SetAwaiting( bool value )
      {
        string _msg = String.Format
          ( "ShippingSubstateMachine error: SetAwaiting( {0} ) trigger is not allowed in the current state {1}",
            value,
            this.Parent.m_Parent.ShippingState2.Value
          );
        throw new ContexException( _msg );
      }
      public virtual void SetEndTime() { }
      public virtual void SetShippingState( ShippingState shippingState )
      {
        throw ContexException.TriggerError( String.Format( "SetShippingState( {0} )", shippingState ), this.Parent.m_Parent.ShippingState2.Value );
      }
      public virtual void SetPartner( Partner partner )
      {
        throw ContexException.TriggerError( "SetPartner", this.Parent.m_Parent.ShippingState2.Value );
      }
      #endregion

      #region private
      protected virtual void Transition( ShippingState2 newState )
      {
        Parent.m_Parent.ShippingState2 = newState;
      }
      protected Context Parent { get; private set; }
      #endregion

    }
    private class CreationState: AbstractState
    {

      internal CreationState( Context parent )
        : base( parent )
      { }

      #region IContextTrigers
      public override void SetPartner( Partner partner )
      {
        if ( partner != null )
          Transition( ShippingState2.LackOfData );
      }
      public override void SetAwaiting( bool value ) { }
      public override void SetShippingState( ShippingState shippingState )
      {
        switch ( shippingState )
        {
          case ShippingState.Creation:
            break;
          case ShippingState.Delayed:
            Transition( ShippingState2.Delayed );
            break;
          case ShippingState.Cancelation:
            Transition( ShippingState2.Cancelation );
            break;
          case ShippingState.WaitingForCarrierData:
            Transition( ShippingState2.LackOfData );
            break;
        }
      }
      #endregion

    }
    private class LackOfDataState: AbstractState
    {
      internal LackOfDataState( Context parent )
        : base( parent )
      { }

      #region IContextTrigers
      public override void SetPartner( Partner partner )
      {
        if ( partner == null )
          Transition( ShippingState2.Creation );
      }
      public override void SetShippingState( ShippingState shippingState )
      {
        switch ( shippingState )
        {
          case ShippingState.Delayed:
            Transition( ShippingState2.Delayed );
            break;
          case ShippingState.Cancelation:
            Transition( ShippingState2.Cancelation );
            break;
          case ShippingState.Confirmed:
          case ShippingState.WaitingForConfirmation:
            Transition( ShippingState2.Confirmed );
            break;
          case ShippingState.WaitingForCarrierData:
          case ShippingState.Creation:
            break;
        }
      }
      #endregion

    }
    private class ConfirmedState: AbstractState
    {
      internal ConfirmedState( Context parent )
        : base( parent )
      { }

      #region IContextTrigers
      public override void SetAwaiting( bool value )
      {
        if ( !value )
          return;
        Transition( ShippingState2.Waiting );
      }
      public override void SetPartner( Partner partner )
      {
        if ( partner == null )
          Transition( ShippingState2.Creation );
      }
      public override void SetShippingState( ShippingState shippingState )
      {
        switch ( shippingState )
        {
          case ShippingState.Delayed:
            Transition( ShippingState2.Delayed );
            break;
          case ShippingState.Cancelation:
            Transition( ShippingState2.Cancelation );
            break;
          case ShippingState.WaitingForCarrierData:
            Transition( ShippingState2.LackOfData );
            break;
          case ShippingState.Creation:
            if ( Parent.m_Parent.PartnerTitle == null )
              Transition( ShippingState2.Creation );
            else
              Transition( ShippingState2.LackOfData );
            break;
          case ShippingState.Underway:
            Transition( ShippingState2.Started );
            break;
          case ShippingState.WaitingForConfirmation:
          case ShippingState.Confirmed:
            break;
        }
      }
      #endregion

    }
    private class DelayedState: AbstractState
    {

      internal DelayedState( Context parent )
        : base( parent )
      { }

      #region IContextTrigers
      public override void SetAwaiting( bool value )
      {
        if ( !value )
          return;
        Transition( ShippingState2.Waiting );
      }
      public override void SetShippingState( ShippingState shippingState )
      {
        switch ( shippingState )
        {
          case ShippingState.Delayed:
            break;
          case ShippingState.Cancelation:
            Transition( ShippingState2.Cancelation );
            break;
          case ShippingState.WaitingForConfirmation:
          case ShippingState.Confirmed:
            Transition( ShippingState2.Confirmed );
            break;
          case ShippingState.WaitingForCarrierData:
            Transition( ShippingState2.LackOfData );
            break;
          case ShippingState.Creation:
            if ( Parent.m_Parent.PartnerTitle == null )
              Transition( ShippingState2.Creation );
            else
              Transition( ShippingState2.LackOfData );
            break;
          case ShippingState.Underway:
            Transition( ShippingState2.Started );
            break;
        }
      }
      #endregion

    }
    private class WaitingState: AbstractState
    {

      internal WaitingState( Context parent )
        : base( parent )
      { }

      #region IContextTrigers
      public override void SetAwaiting( bool value )
      {
        if ( value )
          return;
        Transition( ShippingState2.Confirmed );
      }
      public override void SetShippingState( ShippingState shippingState )
      {
        switch ( shippingState )
        {
          case ShippingState.Cancelation:
            Transition( ShippingState2.Cancelation );
            break;
          case ShippingState.Underway:
            Transition( ShippingState2.Started );
            break;
          case ShippingState.Confirmed:
            if ( !Parent.m_Parent.TruckAwaiting.Value )
              Transition( ShippingState2.Confirmed );
            break;
        }
      }
      #endregion

    }
    private class StartedState: AbstractState
    {
      internal StartedState( Context parent )
        : base( parent )
      { }

      #region IContextTrigers
      public override void SetAwaiting( bool value )
      {

      }
      public override void SetEndTime()
      {
        Transition( ShippingState2.Completed );
      }
      public override void SetShippingState( ShippingState shippingState )
      {
        switch ( shippingState )
        {
          case ShippingState.Completed:
            Transition( ShippingState2.Left );
            break;
          case ShippingState.Confirmed:
            if ( Parent.m_Parent.TruckAwaiting.Value )
              Transition( ShippingState2.Waiting );
            else
              Transition( ShippingState2.Confirmed );
            break;
          case ShippingState.Delayed:
            Transition( ShippingState2.Delayed );
            break;
          case ShippingState.Underway:
            break;
        }
      }
      #endregion

    }
    private class CompletedState: AbstractState
    {
      internal CompletedState( Context parent )
        : base( parent )
      { }

      #region IContextTrigers
      public override void SetShippingState( ShippingState shippingState )
      {
        switch ( shippingState )
        {
          case ShippingState.Completed:
            Transition( ShippingState2.Left );
            break;
          case ShippingState.Underway:
            break;
        }
      }
      #endregion

    }
    private class CancelationState: AbstractState
    {
      internal CancelationState( Context parent )
        : base( parent )
      { }

      #region IContextTrigers
      public override void SetShippingState( ShippingState shippingState )
      {
        switch ( shippingState )
        {
          case ShippingState.Cancelation:
            break;
          case ShippingState.Canceled:
            Transition( ShippingState2.Canceled );
            break;
        }
      }
      #endregion

    }
    private class LeftState: AbstractState
    {
      internal LeftState( Context parent )
        : base( parent )
      { }

      #region IContextTrigers
      public override void SetShippingState( ShippingState shippingState )
      {
        switch ( shippingState )
        {
          case ShippingState.Completed:
            break;
        }
      }
      #endregion

    }
    private class CanceledState: AbstractState
    {
      internal CanceledState( Context parent )
        : base( parent )
      { }

      #region IContextTrigers
      public override void SetShippingState( ShippingState shippingState )
      {
        switch ( shippingState )
        {
          case ShippingState.Canceled:
            break;
        }
      }
      #endregion

    }
    #endregion

    private AbstractState p_AbstractState = null;
    private AbstractState m_AbstractState
    {
      get
      {
        if ( p_AbstractState == null )
          p_AbstractState = CreateAbstractState( m_Parent );
        return p_AbstractState;
      }
    }
    private AbstractState CreateAbstractState( Shipping m_Parent )
    {
      AbstractState _ret = null;
      switch ( m_Parent.ShippingState2.GetValueOrDefault( GetDefaultValue( m_Parent ) ) )
      {
        case ShippingState2.Cancelation:
          _ret = new CancelationState( this );
          break;
        case ShippingState2.Canceled:
          _ret = new CanceledState( this );
          break;
        case ShippingState2.Completed:
          _ret = new CompletedState( this );
          break;
        case ShippingState2.Confirmed:
          _ret = new ConfirmedState( this );
          break;
        case ShippingState2.Creation:
          _ret = new CreationState( this );
          break;
        case ShippingState2.Delayed:
          _ret = new DelayedState( this );
          break;
        case ShippingState2.LackOfData:
          _ret = new LackOfDataState( this );
          break;
        case ShippingState2.Left:
          _ret = new LeftState( this );
          break;
        case ShippingState2.Started:
          _ret = new StartedState( this );
          break;
        case ShippingState2.Waiting:
          _ret = new WaitingState( this );
          break;
      }
      return _ret;
    }
    private ShippingState2 GetDefaultValue( Shipping shipping )
    {
      ShippingState2 _ret = default( ShippingState2 );
      switch ( shipping.ShippingState.GetValueOrDefault( ShippingState.Creation ) )
      {
        case ShippingState.Cancelation:
          _ret = ShippingState2.Cancelation;
          break;
        case ShippingState.Canceled:
          _ret = ShippingState2.Canceled;
          break;
        case ShippingState.Completed:
          _ret = ShippingState2.Left;
          break;
        case ShippingState.Confirmed:
          if ( shipping.TruckAwaiting.Value )
            _ret = ShippingState2.Waiting;
          else
            _ret = ShippingState2.Confirmed;
          break;
        case ShippingState.Creation:
          _ret = ShippingState2.Creation;
          break;
        case ShippingState.Delayed:
          if ( shipping.TruckAwaiting.Value )
            _ret = ShippingState2.Waiting;
          else
            _ret = ShippingState2.Delayed;
          break;
        case ShippingState.WaitingForCarrierData:
          _ret = ShippingState2.LackOfData;
          break;
        case ShippingState.WaitingForConfirmation:
          _ret = ShippingState2.LackOfData;
          break;
        case ShippingState.Underway:
          if ( shipping.WarehouseEndTime > DateTime.Now )
            _ret = ShippingState2.Completed;
          else
            _ret = ShippingState2.Started;
          break;
      }
      return _ret;
    }
    private Shipping m_Parent;

    #endregion

  }
}
