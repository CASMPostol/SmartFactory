using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Shepherd.DataModel.Entities.SheepingSubstateMachine
{
  internal class Context: IContextTrigers
  {
    internal Context( Shipping parent )
    {
      m_Parent = parent;
      switch ( parent.ShippingState2.Value )
      {
        case ShippingState2.Cancelation:
          m_AbstractState = new CancelationState(this);
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
    private abstract class AbstractState: IContextTrigers
    {
      internal AbstractState( Context parent )
      {
        m_parent = parent;
      }

      #region IContextTrigers Members
      public virtual void SetAwaiting( bool value )
      {
        throw new NotImplementedException();
      }
      public virtual void SetEndTime()
      {
        throw new NotImplementedException();
      }
      public virtual void SetShippingState( ShippingState shippingState )
      {
        throw new NotImplementedException();
      }
      #endregion

      #region private
      protected virtual void Transition( ShippingState2 newState )
      {
        m_parent.m_Parent.ShippingState2 = newState;
      }
      private Context m_parent;
      #endregion
    }
    private class CreationState: AbstractState
    {
      internal CreationState( Context parent )
        : base( parent )
      { }

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
        }
      }
    }
    private class LackOfDataState: AbstractState
    {
      internal LackOfDataState( Context parent )
        : base( parent )
      { }

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
        }
      }
    }
    private class ConfirmedState: AbstractState
    {
      internal ConfirmedState( Context parent )
        : base( parent )
      { }

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
        }
      }
    }
    private class DelayedState: AbstractState
    {
      internal DelayedState( Context parent )
        : base( parent )
      { }
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
    }
    private class WaitingState: AbstractState
    {
      internal WaitingState( Context parent )
        : base( parent )
      { }
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
        }
      }
    }
    private class CancelationState: AbstractState
    {
      internal CancelationState( Context parent )
        : base( parent )
      { }
      public override void SetShippingState( ShippingState shippingState )
      {
        switch ( shippingState )
        {
          case ShippingState.Canceled:
            Transition( ShippingState2.Canceled );
            break;
        }
      }
    }
    private class CompletedState: AbstractState
    {
      internal CompletedState( Context parent )
        : base( parent )
      { }
      public override void SetShippingState( ShippingState shippingState )
      {
        switch ( shippingState )
        {
          case ShippingState.Completed:
            Transition( ShippingState2.Left );
            break;
        }
      }
    }
    private class StartedState: AbstractState
    {
      internal StartedState( Context parent )
        : base( parent )
      { }
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
        }
      }
    }
    private class LeftState: AbstractState
    {
      internal LeftState( Context parent )
        : base( parent )
      { }

    }
    private class CanceledState: AbstractState
    {
      internal CanceledState( Context parent )
        : base( parent )
      { }

    }
    private AbstractState m_AbstractState;
    internal Shipping m_Parent;
    #region IContextTrigers Members
    public void SetAwaiting( bool value )
    {
      m_AbstractState.SetAwaiting( value );
    }
    public void SetEndTime()
    {
      m_AbstractState.SetEndTime();
    }
    public void SetShippingState( ShippingState shippingState ) { }
    #endregion

  }
}
