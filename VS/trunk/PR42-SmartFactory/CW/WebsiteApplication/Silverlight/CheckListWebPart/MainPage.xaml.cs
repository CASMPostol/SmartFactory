//<summary>
//  Title   : class MainPage 
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

using CAS.Common.Serialization;
using CAS.SmartFactory.CW.Dashboards.CheckListWebPart.Schema;
using CAS.SmartFactory.CW.Dashboards.Webparts.CheckListHost;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Printing;

namespace CAS.SmartFactory.CW.Dashboards.CheckListWebPart
{

  /// <summary>
  /// class MainPage 
  /// </summary>
  public partial class MainPage : UserControl
  {

    #region creators
    public MainPage()
    {
      InitializeComponent();
      m_PrintDocument = new PrintDocument();
      m_PrintDocument.PrintPage += PrintDocument_PrintPageEventHandler;
    }
    public MainPage(string hiddenFieldDataName)
      : this()
    {
      HtmlDocument doc = HtmlPage.Document;
      m_HiddenField = doc.GetElementById(hiddenFieldDataName);
    }
    #endregion

    #region private

    //vars
    private PrintDocument m_PrintDocument = null;
    private HtmlElement m_HiddenField = null;
    private string m_at;

    //handlers
    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      try
      {
        m_at = "MainPage.UserControl_Loaded";
        if (m_HiddenField != null)
        {
          string message = m_HiddenField.GetAttribute("value");
          if (!String.IsNullOrEmpty(message))
            x_GridToBePrinted.DataContext = CAS.Common.Serialization.JsonSerialization.Deserialize<CheckListWebPartDataContract>(message);
        }
        else
#if DEBUG
          x_GridToBePrinted.DataContext = new CheckListWebPartDataContract() { Today = DateTime.Today, DisposalsList = new DisposalDescription[] { new DisposalDescription() { OGLDate = DateTime.UtcNow, OGLNumber = "OGL Number: 12345", PackageToClear = 1234 / 678 } } };
#else
          throw new ArgumentNullException("CheckListWebPartDataContract", "Cannot find data contract.");
#endif
      }
      catch (Exception ex)
      {
        ExceptionHandling(ex);
      }
    }
    private void UserControl_Unloaded(object sender, RoutedEventArgs e) { }
    private void PrintDocument_PrintPageEventHandler(object sender, PrintPageEventArgs e)
    {
      e.PageVisual = x_GridToBePrinted;
      e.HasMorePages = false;
    }
    private void x_ButtonPrint_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        m_at = "MainPage.x_ButtonPrint_Click";
        m_PrintDocument.Print("Check list");
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(ex.Message, "Print Exception", MessageBoxButton.OK);
      }
    }
    private void x_ButtonExport_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        SaveFileDialog _textDialog = new SaveFileDialog() { Filter = "XML Files | *.xml", DefaultExt = "xml" };
        bool? result = _textDialog.ShowDialog();
        if (!result.GetValueOrDefault(false))
          return;
        System.IO.Stream fileStream = _textDialog.OpenFile();
        CheckListWebPartDataContract _contract = x_GridToBePrinted.DataContext as CheckListWebPartDataContract;
        if (_contract == null)
          throw new ArgumentNullException("CheckListWebPartDataContract", "The data is not available.");
        CheckList _newDocument = new CheckList()
        {
          CheckListContentAray = _contract.DisposalsList.Select<DisposalDescription, ArrayOfContentsContentArray>
            (x => new ArrayOfContentsContentArray() { OGLDate = x.OGLDate, OGLNumber = x.OGLNumber, PackageToClear = x.PackageToClear }).ToArray<ArrayOfContentsContentArray>()
        };
        XmlSerialization.WriteXmlFile<CheckList>(_newDocument, fileStream, "CheckList");
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Print Exception", MessageBoxButton.OK);
      }
    }

    //methods
    private void ExceptionHandling(Exception ex)
    {
      MessageBox.Show(ex.Message + " AT: " + m_at, "Loading event error", MessageBoxButton.OK);
    }
    private Webparts.CheckListHost.CheckListWebPartDataContract MainPageData
    {
      get { return ((Webparts.CheckListHost.CheckListWebPartDataContract)x_GridToBePrinted.DataContext); }
      set { x_GridToBePrinted.DataContext = value; this.UpdateLayout(); }
    }
    #endregion

  }

}
