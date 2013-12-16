//<summary>
//  Title   : enum EntityState
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>


namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{  
  /// <summary>
  ///  Records the changed state of an entity (usually a list item; but possibly a detached entity).
  /// </summary>
  public enum EntityState
  {   
    /// <summary>
    ///  The entity is not changed.
    /// </summary>
    Unchanged = 0,   
    /// <summary>
    ///  The entity will be inserted into a list.
    /// </summary>
    ToBeInserted = 1,    
    /// <summary>
    /// The entity will be updated.
    /// </summary>
    ToBeUpdated = 2,     
    /// <summary>
    /// The entity will be recycled.
    /// </summary>
    ToBeRecycled = 3,     
    /// <summary>
    /// The entity will be deleted.
    /// </summary>
    ToBeDeleted = 4,     
    /// <summary>
    /// The entity has been deleted or recycled.
    /// </summary>
    Deleted = 5,
  }
}
