using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.Shepherd.Dashboards
{
  internal class DataContextManagementAutoDispose<type>
    where type: DataContext, new()
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="DataContextManagementAutoDispose{type}"/> class.
    /// </summary>
    /// <param name="parent">The parent.</param>
    public DataContextManagementAutoDispose( UserControl parent )
    {
      m_Control = parent;
      parent.Unload += parent_Unload;
    }
    public type DataContext
    {
      get
      {
        if ( m_DataContext == null )
          m_DataContext = new type();
        return m_DataContext;
      }
    }
    private void parent_Unload( object sender, EventArgs e )
    {
      if ( m_DataContext == null )
        return;
      m_Control.Unload -= parent_Unload;
      m_DataContext.Dispose();
      m_DataContext = null;
    }
    private type m_DataContext;
    private UserControl m_Control;
  }
}
