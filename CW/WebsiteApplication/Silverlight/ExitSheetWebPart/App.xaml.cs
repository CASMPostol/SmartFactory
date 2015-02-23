//<summary>
//  Title   : partial class App
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
      
using System;
using System.Windows;

namespace CAS.SmartFactory.CW.Dashboards.ExitSheetWebPart
{
  /// <summary>
  /// Class App.
  /// </summary>
  public partial class App : Application
  {

    public App()
    {
      this.Startup += this.Application_Startup;
      this.Exit += this.Application_Exit;
      this.UnhandledException += this.Application_UnhandledException;
      InitializeComponent();
    }
    private void Application_Startup(object sender, StartupEventArgs e)
    {
      if (!e.InitParams.ContainsKey((CommonDefinition.HiddenFieldDataParameterName)))
        this.RootVisual = new MainPage();
      else
      {
        string _HiddenFieldDataName = e.InitParams[CommonDefinition.HiddenFieldDataParameterName];
        this.RootVisual = new MainPage(_HiddenFieldDataName);
      }
    }
    private void Application_Exit(object sender, EventArgs e)
    {

    }
    private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
    {
      // If the app is running outside of the debugger then report the exception using
      // the browser's exception mechanism. On IE this will display it a yellow alert 
      // icon in the status bar and Firefox will display a script error.
      if (!System.Diagnostics.Debugger.IsAttached)
      {
        // NOTE: This will allow the application to continue running after an exception has been thrown
        // but not handled. 
        // For production applications this error handling should be replaced with something that will 
        // report the error to the website and stop the application.
        e.Handled = true;
        Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
      }
    }
    private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
    {
      try
      {
        string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
        errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");
        System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
      }
      catch (Exception)
      {
      }
    }
  }
}
