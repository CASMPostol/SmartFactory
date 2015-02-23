//_______________________________________________________________
//  Title   : MainPage
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate:  $
//  $Rev: $
//  $LastChangedBy: $
//  $URL: $
//  $Id:  $
//
//  Copyright (C) 2015, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using System;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;

namespace CAS.SmartFactory.CW.Dashboards.GenerateSadConsignmentWebPart
{
  public partial class MainPage : UserControl
  {
    public MainPage()
    {
      InitializeComponent();
    }
    public MainPage(string hiddenFieldDataName)
      : this()
    {
      HtmlDocument doc = HtmlPage.Document;
      m_HiddenField = doc.GetElementById(hiddenFieldDataName);
    }
    private HtmlElement m_HiddenField = null;
    private string m_at;

    private void x_ButtonGenerateSad_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        m_at = "MainPage.UserControl_Loaded";
        if (m_HiddenField == null)
          throw new ArgumentNullException("HiddenField", "Cannot find the document in the page context.");
        m_at = "HtmlDecode";
        string _content = System.Windows.Browser.HttpUtility.HtmlDecode(m_HiddenField.GetAttribute("value"));
        SaveFileDialog textDialog;
        textDialog = new SaveFileDialog();
        textDialog.Filter = "XML Files | *.xml";
        textDialog.DefaultExt = "txt";
        bool? result = textDialog.ShowDialog();
        if (!result.GetValueOrDefault(false))
          return;
        System.IO.Stream fileStream = textDialog.OpenFile();
        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fileStream))
        {
          sw.Write(_content);
          sw.Flush();
          sw.Close();
        }
      }
      catch (Exception ex)
      {
        ExceptionHandling(ex);
      }
    }
    private void ExceptionHandling(Exception ex)
    {
      MessageBox.Show(ex.Message + " AT: " + m_at, "Loaded event error", MessageBoxButton.OK);
    }
  }
}
