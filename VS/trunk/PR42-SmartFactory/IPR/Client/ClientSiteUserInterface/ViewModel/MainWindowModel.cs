//<summary>
//  Title   : class MainWindowModel
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.SharePoint.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace CAS.SmartFactory.IPR.Client.UserInterface.ViewModel
{
  internal class MainWindowModel : PropertyChangedBase
  {

    #region public
    //creator
    public MainWindowModel()
    {
      ProgressBarMaximum = 100;
      AssemblyName _name = Assembly.GetExecutingAssembly().GetName();
      this.Title = this.Title + " Rel " + _name.Version.ToString(4);
      m_StateMAchine = new LocalMachine(this);
      m_StateMAchine.OpenEntryState();
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
    public ICommand ButtonCancel
    {
      get
      {
        return b_ButtonCancel;
      }
      set
      {
        RaiseHandler<ICommand>(value, ref b_ButtonCancel, "ButtonCancel", this);
      }
    }
    public ICommand ButtonGoBackward
    {
      get
      {
        return b_ButtonGoBackward;
      }
      set
      {
        RaiseHandler<ICommand>(value, ref b_ButtonGoBackward, "ButtonGoBackward", this);
      }
    }
    public ICommand ButtonGoForward
    {
      get
      {
        return b_ButtonGoForward;
      }
      set
      {
        RaiseHandler<ICommand>(value, ref b_ButtonGoForward, "ButtonGoForward", this);
      }
    }
    public ICommand ButtonRun
    {
      get
      {
        return b_ButtonRun;
      }
      set
      {
        RaiseHandler<ICommand>(value, ref b_ButtonRun, "ButtonRun", this);
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
    private ICommand b_ButtonCancel;
    private ICommand b_ButtonGoBackward;
    private ICommand b_ButtonGoForward;
    private ICommand b_ButtonRun;
    //vars
    private bool m_UpdateProgressBarBusy = false;
    private LocalMachine m_StateMAchine;
    //types
    private class LocalMachine : StateMachine.StateMachineContext
    {

      #region creator
      public LocalMachine(MainWindowModel parent)
        : base()
      {
        m_Parent = parent;
      }
      #endregion
      public bool ButtonCancel
      {
        get
        {
          return b_ButtonCancel;
        }
        set
        {
          RaiseHandler<bool>(value, ref b_ButtonCancel, "ButtonCancel", this);
        }
      }
      public bool ButtonGoBackward
      {
        get
        {
          return b_ButtonGoBackward;
        }
        set
        {
          RaiseHandler<bool>(value, ref b_ButtonGoBackward, "ButtonGoBackward", this);
        }
      }
      public bool ButtonGoForward
      {
        get
        {
          return b_ButtonGoForward;
        }
        set
        {
          RaiseHandler<bool>(value, ref b_ButtonGoForward, "ButtonGoForward", this);
        }
      }
      public bool ButtonRun
      {
        get
        {
          return b_ButtonRun;
        }
        set
        {
          RaiseHandler<bool>(value, ref b_ButtonRun, "ButtonRun", this);
        }
      }

      #region StateMachineContext
      internal override void SetupUserInterface(StateMachine.Events allowedEvents)
      {
        ButtonCancel = (allowedEvents & StateMachine.Events.Cancel) != 0;
        ButtonGoBackward = (allowedEvents & StateMachine.Events.Previous) != 0;
        ButtonGoForward = (allowedEvents & StateMachine.Events.Next) != 0;
        ButtonRun = (allowedEvents & StateMachine.Events.RunAsync) != 0;
      }
      internal override void Close()
      {
        m_Parent.Close();
      }
      internal override void Progress(int progress)
      {
        m_Parent.UpdateProgressBar(progress);
      }
      internal override void WriteLine()
      {
        m_Parent.UpdateProgressBar(1);
      }
      internal override void WriteLine(string value)
      {
        m_Parent.WriteLine(value);
        m_Parent.UpdateProgressBar(1);
      }
      internal override void Exception(Exception exception)
      {
        string _mssg = String.Format("Program stopped by exception: {0}", exception.Message);
        MessageBox.Show(_mssg, "Operation error", MessageBoxButton.OK, MessageBoxImage.Error);
        //Close();
      }
      internal override void EnteringState()
      {
      }
      #endregion

      #region private
      //vars backing fields
      private bool b_ButtonCancel;
      private bool b_ButtonGoBackward;
      private bool b_ButtonGoForward;
      private bool b_ButtonRun;
      //vars general purpose
      private MainWindowModel m_Parent;
      #endregion
    }
    //methods
    private void UpdateProgressBar(int progress)
    {
      if (m_UpdateProgressBarBusy)
        return;
      m_UpdateProgressBarBusy = true;
      while (ProgressBarMaximum - Progress < progress)
        ProgressBarMaximum *= 2;
      Progress += progress;
      m_UpdateProgressBarBusy = false;
    }
    private void WriteLine(string value)
    {
      ProgressList.Add(value);
    }
    private void Close()
    {
      throw new NotImplementedException();
    }
    #endregion

  }
}
