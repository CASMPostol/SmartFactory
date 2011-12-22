using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Data;
using CAS.SmartFactory.Shepherd.Dashboards.Entities;

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
      m_Control.DisplayUserName(m_UserDescriptor);
      Controls.Add(m_Control);
    }
    private CurrentUserWebPartUserControl m_Control;
    private class UserDescriptor : DataTable, IWebPartRow, CAS.SmartFactory.Shepherd.Dashboards.CurrentUserWebPart.IUserDescriptor
    {
      #region IWebPartRow
      public PropertyDescriptorCollection Schema { get; private set; }
      public void GetRowData(RowCallback callback)
      {
        callback(this.Row0);
      }
      #endregion

      #region public
      internal UserDescriptor(SPUser _user)
      {
        this.TableName = "User Descriptor";
        this.User = _user;
        AddColumn("Email");
        AddColumn("ID");
        AddColumn("LoginName");
        AddColumn("Name");
        AddColumn("Notes");
        AddColumn("Company");
        DataRow row = this.NewRow();
        row["Email"] = User.Email;
        row["ID"] = User.ID;
        row["LoginName"] = User.LoginName;
        row["Name"] = User.Name;
        try
        {
          using (EntitiesDataContext edc = new EntitiesDataContext(SPContext.Current.Web.Url))
          {
            row["Company"] = Company = Partner.FindPartner(edc, _user).Tytuł;
          }
        }
        catch (Exception)
        {
          row["Company"] = Company = "Not registered !!!";
        }
        this.Rows.Add(row);
        Schema = TypeDescriptor.GetProperties(this.Row0);
      }
      public SPUser User { get; private set; }
      public string Company { get; private set; }
      #endregion

      #region private
      private DataRowView Row0 { get { return DefaultView[0]; } }
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
