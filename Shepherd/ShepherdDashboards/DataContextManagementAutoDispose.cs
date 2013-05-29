//<summary>
//  Title   : DataContextManagementAutoDispose class
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Web.UI;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.Shepherd.Dashboards
{
  internal class DataContextManagementAutoDispose<type>
    where type: DataContext, new()
  {
    /// <summary>
    /// Gets the data context management.
    /// </summary>
    /// <param name="parent">The parent.</param>
    /// <returns></returns>
    public static DataContextManagementAutoDispose<type> GetDataContextManagement( UserControl parent )
    {
      if ( m_DataContextManagement == null )
        m_DataContextManagement = new DataContextManagementAutoDispose<type>( parent );
      return m_DataContextManagement;
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
    /// <summary>
    /// Initializes a new instance of the <see cref="DataContextManagementAutoDispose{type}"/> class.
    /// </summary>
    /// <param name="parent">The parent.</param>
    private DataContextManagementAutoDispose( UserControl parent )
    {
      m_Control = parent.Page;
      parent.Unload += parent_Unload;
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
    private Page m_Control;
    private static DataContextManagementAutoDispose<type> m_DataContextManagement = null;
  }
}
