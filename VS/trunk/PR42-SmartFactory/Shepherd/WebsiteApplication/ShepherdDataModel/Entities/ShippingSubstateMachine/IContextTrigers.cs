//<summary>
//  Title   : Sheeping substate machine context trigers interface
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

namespace CAS.SmartFactory.Shepherd.DataModel.Entities.ShippingSubstateMachine
{
  /// <summary>
  /// Sheeping substate machine context trigers interface
  /// </summary>
  internal interface IContextTrigers
  {
    /// <summary>
    /// Sets the partner.
    /// </summary>
    /// <param name="partner">The partner.</param>
    void SetPartner( Partner partner );
    /// <summary>
    /// Sets that the truck is awaiting.
    /// </summary>
    /// <param name="value">if set to <c>true</c> the truck is aeaiting.</param>
    void SetAwaiting(bool value);
    /// <summary>
    /// Sets the end time of the inbound or outbound operation.
    /// </summary>
    void SetEndTime();
    /// <summary>
    /// Sets the state of the shipping.
    /// </summary>
    /// <param name="shippingState">State of the shipping.</param>
    void SetShippingState( ShippingState shippingState );
  }
}
