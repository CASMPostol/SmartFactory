using System;
using System.Web.UI;

namespace CAS.SmartFactory.Shepherd.Dashboards.CurrentUserWebPart
{
  public partial class CurrentUserWebPartUserControl : UserControl
  {
    protected void Page_Load(object sender, EventArgs e)
    {}
    internal void DisplayUserName(IUserDescriptor _UserDescriptor)
    {
      this.m_UserNameLiteral.Text = String.Format("Welcome {0} from {1} !", _UserDescriptor.User, _UserDescriptor.Company);
    }
  }
}
