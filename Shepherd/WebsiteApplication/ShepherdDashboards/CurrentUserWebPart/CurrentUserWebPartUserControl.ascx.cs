//<summary>
//  Title   : class CurrentUserWebPartUserControl
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
      
using System;
using System.Web.UI;

namespace CAS.SmartFactory.Shepherd.Dashboards.CurrentUserWebPart
{
  /// <summary>
  /// CurrentUserWebPartUserControl UserControl
  /// </summary>
  public partial class CurrentUserWebPartUserControl : UserControl
  {
    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {}
    internal void DisplayUserName(IUserDescriptor _UserDescriptor)
    {
      if (_UserDescriptor != null)
        this.m_UserNameLiteral.Text = String.Format("Welcome {0} from {1} !", _UserDescriptor.User, _UserDescriptor.Company);
      else
        this.m_UserNameLiteral.Text = "Information about user is unavailable";
    }
  }
}
