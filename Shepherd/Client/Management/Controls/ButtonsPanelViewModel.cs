//<summary>
//  Title   : ButtonsPanelViewModel
//  System  : Microsoft VisulaStudio 2013 / C#
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

using CAS.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CAS.SmartFactory.Shepherd.Client.Management.Controls
{
  [Export]  
  internal class ButtonsPanelViewModel: CAS.Common.ComponentModel.PropertyChangedBase, CAS.SmartFactory.Shepherd.Client.Management.Controls.IButtonsPanelViewModel
  {
    [ImportingConstructor]
    public ButtonsPanelViewModel(ShellViewModel parentViewMode)
    {
      if (parentViewMode == null)
        throw new ArgumentNullException("parentViewMode");
      parentViewMode.PropertyChanged += parentViewMode_PropertyChanged;
    }
    void parentViewMode_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
    }
    public string LeftButtonTitle
    {
      get
      {
        return b_LeftButtonTitle;
      }
      set
      {
        RaiseHandler<string>(value, ref b_LeftButtonTitle, "LeftButtonTitle", this);
      }
    }
    public string LeftMiddleButtonTitle
    {
      get
      {
        return b_LeftMiddleButtonTitle;
      }
      set
      {
        RaiseHandler<string>(value, ref b_LeftMiddleButtonTitle, "LeftMiddleButtonTitle", this);
      }
    }
    public string RightMiddleButtonTitle
    {
      get
      {
        return b_RightMiddleButtonTitle;
      }
      set
      {
        RaiseHandler<string>(value, ref b_RightMiddleButtonTitle, "RightMiddleButtonTitle", this);
      }
    }
    public string RightButtonTitle
    {
      get
      {
        return b_RightButtonTitle;
      }
      set
      {
        RaiseHandler<string>(value, ref b_RightButtonTitle, "RightButtonTitle", this);
      }
    }

    private ICommandWithUpdate b_LeftButtonCommand;
    public ICommandWithUpdate LeftButtonCommand
    {
      get
      {
        return b_LeftButtonCommand;
      }
      set
      {
        RaiseHandler<ICommandWithUpdate>(value, ref b_LeftButtonCommand, "LeftButtonCommand", this);
      }
    }
    private ICommandWithUpdate b_LeftMiddleButtonCommand;
    public ICommandWithUpdate LeftMiddleButtonCommand
    {
      get
      {
        return b_LeftMiddleButtonCommand;
      }
      set
      {
        RaiseHandler<ICommandWithUpdate>(value, ref b_LeftMiddleButtonCommand, "LeftMiddleButtonCommand", this);
      }
    }
    private ICommandWithUpdate b_RightMiddleButtonCommand;
    public ICommandWithUpdate RightMiddleButtonCommand
    {
      get
      {
        return b_RightMiddleButtonCommand;
      }
      set
      {
        RaiseHandler<ICommandWithUpdate>(value, ref b_RightMiddleButtonCommand, "RightMiddleButtonCommand", this);
      }
    }
    private ICommandWithUpdate b_RightButtonCommand;
    public ICommandWithUpdate RightButtonCommand
    {
      get
      {
        return b_RightButtonCommand;
      }
      set
      {
        RaiseHandler<ICommandWithUpdate>(value, ref b_RightButtonCommand, "RightButtonCommand", this);
      }
    }
    private Visibility b_LeftButtonVisibility;
    public System.Windows.Visibility LeftButtonVisibility
    {
      get
      {
        return b_LeftButtonVisibility;
      }
      set
      {
        RaiseHandler<System.Windows.Visibility>(value, ref b_LeftButtonVisibility, "LeftButtonVisibility", this);
      }
    }
    private Visibility b_LeftMiddleButtonVisibility;
    public Visibility LeftMiddleButtonVisibility
    {
      get
      {
        return b_LeftMiddleButtonVisibility;
      }
      set
      {
        RaiseHandler<Visibility>(value, ref b_LeftMiddleButtonVisibility, "LeftMiddleButtonVisibility", this);
      }
    }
    private Visibility b_RightMiddleButtonVisibility;
    public Visibility RightMiddleButtonVisibility
    {
      get
      {
        return b_RightMiddleButtonVisibility;
      }
      set
      {
        RaiseHandler<Visibility>(value, ref b_RightMiddleButtonVisibility, "RightMiddleButtonVisibility", this);
      }
    }
    private Visibility b_RightButtonVisibility = Visibility.Hidden;
    public Visibility RightButtonVisibility
    {
      get
      {
        return b_RightButtonVisibility;
      }
      set
      {
        RaiseHandler<Visibility>(value, ref b_RightButtonVisibility, "RightButtonVisibility", this);
      }
    } 
    private string b_LeftButtonTitle = "Left";
    private string b_LeftMiddleButtonTitle = "Left Middle";
    private string b_RightMiddleButtonTitle = "Right Middle";
    private string b_RightButtonTitle = "Right";
                 
  }
}
