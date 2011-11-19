using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace CAS.SmartFactory.IPR.ListsEventsHandlers.Dictionaries.SKUEventHandlers
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class SKUEventHandlers : SPItemEventReceiver
    {
       /// <summary>
       /// An item was added.
       /// </summary>
       public override void ItemAdded(SPItemEventProperties properties)
       {
           base.ItemAdded(properties);
       }


    }
}
