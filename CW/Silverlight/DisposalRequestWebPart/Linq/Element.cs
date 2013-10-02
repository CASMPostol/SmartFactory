using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Linq
{
  public partial class Entities: IEditableObject
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
