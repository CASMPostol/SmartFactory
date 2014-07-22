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

namespace CAS.SmartFactory.IPR.Client.UserInterface.ViewModel
{
  internal class MainWindowModel : StateMachine.StateMachineContext
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
      ProgressList = new ObservableCollection<string>();
      //create state machine
      new SetupDataDialogMachine(this);
      Components.Add(new ActivationMachine(this));
      Components.Add(new ArchivingMachine(this));
      new FinishedMachine(this);
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
    #endregion

    #region private
    //backing fields
    private string b_Title;
    private int b_IPRAccountArchivalDelay;
    private string b_URL;
    private string b_DatabaseName;
    private ObservableCollection<string> b_ProgressList;

    #region StateMachineContext
    public override void ProgressChang(IAbstractMachine activationMachine, System.ComponentModel.ProgressChangedEventArgs entitiesState)
    {
      base.ProgressChang(activationMachine, entitiesState);
      if (entitiesState.UserState is string)
        ProgressList.Add((String)entitiesState.UserState);
    }
    #endregion

    #endregion

  }
}
