using System;
namespace CAS.SmartFactory.Shepherd.DataModel.Entities.SheepingSubstateMachine
{
  internal interface IContextTrigers
  {
    void SetAwaiting(bool value);
    void SetEndTime();
    void SetShippingState( CAS.SmartFactory.Shepherd.DataModel.Entities.ShippingState shippingState );
  }
}
