//<summary>
//  Title   : IButtonsPanelViewModel
//  System  : Microsoft VisulaStudio 2013 / C#
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
      
using CAS.Common.ViewModel;
using System;
using System.Windows;

namespace CAS.SmartFactory.Shepherd.Client.Management.Controls
{
  public interface IButtonsPanelViewModel
  {
    ICommandWithUpdate LeftButtonCommand { get; set; }
    string LeftButtonTitle { get; set; }
    Visibility LeftButtonVisibility { get; set; }
    ICommandWithUpdate LeftMiddleButtonCommand { get; set; }
    string LeftMiddleButtonTitle { get; set; }
    Visibility LeftMiddleButtonVisibility { get; set; }
    ICommandWithUpdate RightButtonCommand { get; set; }
    string RightButtonTitle { get; set; }
    Visibility RightButtonVisibility { get; set; }
    ICommandWithUpdate RightMiddleButtonCommand { get; set; }
    string RightMiddleButtonTitle { get; set; }
    Visibility RightMiddleButtonVisibility { get; set; }
  }
}
