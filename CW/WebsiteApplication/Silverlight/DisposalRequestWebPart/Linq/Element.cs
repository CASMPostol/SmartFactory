//<summary>
//  Title   : partial class Entities
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
      
using System;
using System.ComponentModel;
using CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Linq
{
  /// <summary>
  /// partial class Entities
  /// </summary>
  public partial class Element: IEditableObject
  {

    #region IEditableObject Members
    public void BeginEdit()
    {
      ITrackEntityState _this = this as ITrackEntityState;
      if ( _this.EntityState == EntityState.Unchanged )
        return;
      _this.EntityState = EntityState.ToBeUpdated;
    }
    public void CancelEdit()
    {
      ITrackEntityState _this = this as ITrackEntityState;
      if ( _this.EntityState == EntityState.Unchanged || _this.EntityState != EntityState.ToBeUpdated )
        return;
      RevertChanges();
      _this.EntityState = EntityState.Unchanged;
    }
    public void EndEdit()
    {
      ITrackEntityState _this = this as ITrackEntityState;
      if ( _this.EntityState == EntityState.Unchanged )
        return;
      _this.EntityState = EntityState.Unchanged;
      ( (ITrackOriginalValues)this ).OriginalValues.Clear();
    }
    #endregion

    #region private
    private void RevertChanges()
    {
      throw new NotImplementedException();
    }
    #endregion

  }
}
