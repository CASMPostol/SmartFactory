//<summary>
//  Title   : class MainWindowModel
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

using CAS.SmartFactory.IPR.Client.UserInterface.StateMachine;
using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;

namespace CAS.SmartFactory.IPR.Client.UserInterface.ViewModel
{
  internal class MainWindowModel : StateMachine.StateMachineContext, IDisposable
  {

    #region public
    //creator
    public MainWindowModel()
    {
      //InitializeUI();
      AssemblyName _name = Assembly.GetExecutingAssembly().GetName();
      this.Title = this.Title + " Rel " + _name.Version.ToString(4);
      URL = Properties.Settings.Default.SiteURL;
      DatabaseName = Properties.Settings.Default.DatabaseName;
      ProgressBarMaximum = 100;
      Progress = 0;
      //create state machine
      new AbstractMachine.SetupDataDialogMachine(this);
      new AbstractMachine.ActivationMachine(this);
      new AbstractMachine.ArchivingMachine(this);
      new AbstractMachine.FinishedMachine(this);
      this.OpenEntryState();
    }
    //UI API
    public string Title
    {
      get
      {
        return b_Title;
      }
      set
      {
        RaiseHandler<string>(value, ref b_Title, "Title", this);
      }
    }
    public string URL
    {
      get
      {
        return b_URL;
      }
      set
      {
        RaiseHandler<string>(value, ref b_URL, "URL", this);
      }
    }
    public string DatabaseName
    {
      get
      {
        return b_DatabaseName;
      }
      set
      {
        RaiseHandler<string>(value, ref b_DatabaseName, "DatabaseName", this);
      }
    }
    public int IPRAccountArchivalDelay
    {
      get
      {
        return b_IPRAccountArchivalDelay;
      }
      set
      {
        RaiseHandler<int>(value, ref b_IPRAccountArchivalDelay, "IPRAccountArchivalDelay", this);
      }
    }
    public int Progress
    {
      get
      {
        return b_Progress;
      }
      set
      {
        RaiseHandler<int>(value, ref b_Progress, "Progress", this);
      }
    }
    public int ProgressBarMaximum
    {
      get
      {
        return b_ProgressBarMaximum;
      }
      set
      {
        RaiseHandler<int>(value, ref b_ProgressBarMaximum, "ProgressBarMaximum", this);
      }
    }
    public ObservableCollection<string> ProgressList
    {
      get
      {
        return b_ProgressList;
      }
      set
      {
        RaiseHandler<System.Collections.ObjectModel.ObservableCollection<string>>(value, ref b_ProgressList, "ProgressList", this);
      }
    }
    public string State
    {
      get
      {
        return b_State;
      }
      set
      {
        RaiseHandler<string>(value, ref b_State, "State", this);
      }
    } 
                
    #endregion

    #region private
    //backing fields
    private string b_Title;
    private int b_Progress;
    private int b_IPRAccountArchivalDelay;
    private string b_URL;
    private string b_DatabaseName;
    private int b_ProgressBarMaximum;
    private ObservableCollection<string> b_ProgressList;
    private string b_State;
    #region StateMachineContext

    internal override void Close()
    {
      throw new NotImplementedException();
    }
    internal override void UpdateProgressBar(int progress)
    {
      while (ProgressBarMaximum - Progress < progress)
        ProgressBarMaximum *= 2;
      Progress += progress;
    }
    internal override void WriteLine(string value)
    {
      ProgressList.Add(value);
      UpdateProgressBar(1);
    }
    internal override void Exception(Exception exception)
    {
      string _mssg = String.Format("Program stopped by exception: {0}", exception.Message);
      MessageBox.Show(_mssg, "Operation error", MessageBoxButton.OK, MessageBoxImage.Error);
      //Close();
    }
    #endregion
    #endregion

    #region IDisposable
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public void Dispose()
    {
      ;
    }
    #endregion

  }
}
