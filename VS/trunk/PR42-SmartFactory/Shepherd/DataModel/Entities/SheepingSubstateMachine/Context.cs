using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Shepherd.DataModel.Entities.SheepingSubstateMachine
{
  internal class Context : IContextTrigers
  {
    internal Context( Shipping Shipping )
    {
      switch ( Shipping.ShippingState2.Value )
      {
        case ShippingState2.Cancelation:
          break;
        case ShippingState2.Canceled:
          break;
        case ShippingState2.Completed:
          break;
        case ShippingState2.Confirmed:
          break;
        case ShippingState2.Creation:
          break;
        case ShippingState2.Delayed:
          break;
        case ShippingState2.LackOfData:
          break;
        case ShippingState2.Left:
          break;
        case ShippingState2.Started:
          break;
        case ShippingState2.Waiting:
          break;
        default:
          break;
      }
    }
    private abstract class AbstractState
    {
    }

    #region IContextTrigers Members
    public void SetAwaiting( bool value )
    {
      throw new NotImplementedException();
    }
    public void SetEndTime()
    {
      throw new NotImplementedException();
    }
    public void SetShippingState( ShippingState shippingState ) { }

    #endregion

    #region IContextTrigers Members



    #endregion
  }

}
