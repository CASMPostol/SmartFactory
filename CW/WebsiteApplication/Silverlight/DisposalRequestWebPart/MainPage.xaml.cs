//<summary>
//  Title   : Name of Application
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

using CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Linq;
using Microsoft.SharePoint.Client;
using System;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Printing;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart
{

  /// <summary>
  /// Main page UserControl
  /// </summary>
  public partial class MainPage: UserControl
  {

    #region public
    public MainPage()
    {
      InitializeComponent();
    }
    public MainPage( string hiddenFieldDataName )
      : this()
    {
      m_at = "creator";
      HtmlDocument doc = HtmlPage.Document;
      HtmlElement hiddenField = doc.GetElementById( hiddenFieldDataName );
      string message = hiddenField.GetAttribute( "value" );
      int _id = 0;
      if ( Int32.TryParse( message, out _id ) )
        m_SelectedID = _id;
    }
    #endregion

    #region private

    #region private vars
    private int? m_SelectedID = new Nullable<int>();
    private string m_at;
    private string m_URL = string.Empty;
    #endregion

    #region event handlers
    private void UserControl_Loaded( object sender, RoutedEventArgs e )
    {
      try
      {
        ClientContext _ClientContext = ClientContext.Current;
        if ( _ClientContext == null )
          throw new ArgumentNullException( "clientContext", String.Format( "Cannot get the {0} ", "ClientContext" ) );
        m_URL = _ClientContext.Url;
        CreateViewModel();
      }
      catch ( Exception ex )
      {
        ExceptionHandling( ex );
      }
    }
    private void UserControl_Unloaded( object sender, RoutedEventArgs e )
    {
      DisposeMainPageData();
    }
    private void x_ButtonAddNew_Click( object sender, RoutedEventArgs e )
    {
      try
      {
        AddNew _newChildWondow = new AddNew( MainPageData.DataContextAsync );
        _newChildWondow.Closed += AddNewChildWondow_Closed;
        _newChildWondow.Show();
      }
      catch ( Exception _ex )
      {
        ExceptionHandling( _ex );
      }
    }
    private void AddNewChildWondow_Closed( object sender, EventArgs e )
    {
      try
      {
        AddNew _childWondow = (AddNew)sender;
        if ( !_childWondow.DialogResult.HasValue || !_childWondow.DialogResult.Value )
          return;
        MainPageData.CreateDisposalRequest( _childWondow.Accounts, _childWondow.ToDispose, _childWondow.CustomsProcedure );
      }
      catch ( Exception _ex )
      {
        ExceptionHandling( _ex );
      }
    }
    private void x_ButtonEndofBatch_Click( object sender, RoutedEventArgs e )
    {
      try
      {
        DisposalRequest _request = this.x_DataGridListView.SelectedItem as DisposalRequest;
        if ( _request == null )
          return;
        _request.EndOfBatch();
      }
      catch ( Exception _ex )
      {
        ExceptionHandling( _ex );
      }
    }
    private void x_ButtonEndOfOgl_Click( object sender, RoutedEventArgs e )
    {
      try
      {
        DisposalRequest _request = this.x_DataGridListView.SelectedItem as DisposalRequest;
        if ( _request == null )
          return;
        _request.EndOfOgl();
      }
      catch ( Exception _ex )
      {
        ExceptionHandling( _ex );
      }
    }
    private void x_ButtonDelete_Click( object sender, RoutedEventArgs e )
    {
      try
      {

      }
      catch ( Exception _ex )
      {
        ExceptionHandling( _ex );
      }
    }
    private void x_ButtonSave_Click( object sender, RoutedEventArgs e )
    {
      try
      {
        this.MainPageData.SubmitChanges();
      }
      catch ( Exception _ex )
      {
        ExceptionHandling( _ex );
      }
    }
    private void x_ButtonCancel_Click( object sender, RoutedEventArgs e )
    {
      if ( MessageBox.Show( "All modification will be discarded", "Request Editor", MessageBoxButton.OKCancel ) != MessageBoxResult.OK )
        return;
      DisposeMainPageData();
      CreateViewModel();
    }
    private void CreateViewModel()
    {
      this.MainPageData = new MainPageData();
      this.MainPageData.GetData(m_URL, m_SelectedID);
    }
    #endregion

    private void ExceptionHandling( Exception ex )
    {
      MessageBox.Show( ex.Message + " AT: " + m_at, "Loaded event error", MessageBoxButton.OK );
    }
    private MainPageData MainPageData
    {
      get { return ( (MainPageData)this.DataContext ); }
      set { this.DataContext = value; this.UpdateLayout(); }
    }
    private void DisposeMainPageData()
    {
      if ( this.MainPageData == null )
        return;
      MainPageData _MainPageData = MainPageData;
      MainPageData = null;
      _MainPageData.Dispose();
    }

    #endregion

  }

}
