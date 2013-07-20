//<summary>
//  Title   : Sheeping Substate Machine Context
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;

namespace CAS.SmartFactory.Shepherd.DataModel.Entities.SheepingSubstateMachine
{
  /// <summary>
  /// Sheeping Substate Machine Context
  /// </summary>
  internal class Context: IContextTrigers
  {

    #region ctor
    internal Context( Shipping parent )
    {
      m_Parent = parent;
      switch ( parent.ShippingState2.Value )
      {
        case ShippingState2.Cancelation:
          m_AbstractState = new CancelationState( this );
          break;
        case ShippingState2.Canceled:
          m_AbstractState = new CanceledState( this );
          break;
        case ShippingState2.Completed:
          m_AbstractState = new CompletedState( this );
          break;
        case ShippingState2.Confirmed:
          m_AbstractState = new ConfirmedState( this );
          break;
        case ShippingState2.Creation:
          m_AbstractState = new CreationState( this );
          break;
        case ShippingState2.Delayed:
          m_AbstractState = new DelayedState( this );
          break;
        case ShippingState2.LackOfData:
          m_AbstractState = new LackOfDataState( this );
          break;
        case ShippingState2.Left:
          m_AbstractState = new LeftState( this );
          break;
        case ShippingState2.Started:
          m_AbstractState = new StartedState( this );
          break;
        case ShippingState2.Waiting:
          m_AbstractState = new WaitingState( this );
          break;
      }
    }
    #endregion

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
        throw new NotImplementedException();
      }
      public virtual void SetEndTime()
      {
      }
      public virtual void SetShippingState( ShippingState shippingState )
      {
        throw new NotImplementedException();
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
          case ShippingState.WaitingForConfirmation:
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
            Transition( ShippingState2.Confirmed );
            break;
          case ShippingState.WaitingForConfirmation:
          case ShippingState.WaitingForCarrierData:
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
          case ShippingState.WaitingForConfirmation:
          case ShippingState.WaitingForCarrierData:
            Transition( ShippingState2.LackOfData );
            break;
          case ShippingState.Creation:
            Transition( ShippingState2.Creation );
            break;
          case ShippingState.Underway:
            Transition( ShippingState2.Started );
            break;
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
          case ShippingState.WaitingForCarrierData:
            Transition( ShippingState2.LackOfData );
            break;
          case ShippingState.Creation:
            Transition( ShippingState2.Creation );
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

    private AbstractState m_AbstractState;
    private Shipping m_Parent;

    #endregion
  }
}
