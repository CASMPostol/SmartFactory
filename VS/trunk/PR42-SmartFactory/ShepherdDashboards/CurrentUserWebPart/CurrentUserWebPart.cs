using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Data;

namespace CAS.SmartFactory.Shepherd.Dashboards.CurrentUserWebPart
{
  [ToolboxItemAttribute(false)]
  public class CurrentUserWebPart : WebPart
  {

    #region private
    // Visual Studio might automatically update this path when you change the Visual Web Part project item.
    private const string _ascxPath = @"~/_CONTROLTEMPLATES/CAS.SmartFactory.Shepherd.Dashboards/CurrentUserWebPart/CurrentUserWebPartUserControl.ascx";
    protected override void CreateChildControls()
    {
      m_Control = Page.LoadControl(_ascxPath) as CurrentUserWebPartUserControl;
      m_Control.DisplayUserName(m_UserDescriptor.User.Name);
      Controls.Add(m_Control);
    }
    private CurrentUserWebPartUserControl m_Control;
    private class UserDescriptor : DataTable, IWebPartRow
    {
      #region IWebPartRow
      public PropertyDescriptorCollection Schema
      {
        get { return m_Schema; }
      }
      public void GetRowData(RowCallback callback)
      {
        callback(this.Row);
      }
      #endregion

      #region public
      internal UserDescriptor(SPUser _user)
      {
        this.User = _user;
        AddColumn("Email");
        AddColumn("ID");
        AddColumn("LoginName");
        AddColumn("Name");
        AddColumn("Notes");
        DataRow row = this.NewRow();
        row["Email"] = User.Email;
        row["ID"] = User.ID;
        row["LoginName"] = User.LoginName;
        row["Name"] = User.Name;
        row["Notes"] = User.Notes;
        this.Rows.Add(row);
        m_Schema = TypeDescriptor.GetProperties(this.Row);
      }
      internal SPUser User { get; private set; }
      #endregion

      #region private
      private DataRowView Row { get { return DefaultView[0]; } }
      private PropertyDescriptorCollection m_Schema;
      private void AddColumn(string _name)
      {
        DataColumn col = new DataColumn();
        col.DataType = typeof(string);
        col.ColumnName = _name;
        this.Columns.Add(col);
      }
      #endregion
    }
    private UserDescriptor m_UserDescriptor; 
    #endregion

    #region public
    [ConnectionProvider("Current User", "CurrentUserProviderPoint", AllowsMultipleConnections = true)]
    public IWebPartRow GetConnectionInterface()
    {
      return m_UserDescriptor;
    }
    public CurrentUserWebPart()
      : base()
    {
      SPWeb currentWeb = SPControl.GetContextWeb(this.Context);
      m_UserDescriptor = new UserDescriptor(currentWeb.CurrentUser);
    }
    #endregion
  }
}
