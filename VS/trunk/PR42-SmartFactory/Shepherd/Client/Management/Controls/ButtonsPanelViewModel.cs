//<summary>
//  Title   : ButtonsPanelViewModel
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

using CAS.Common.ViewModel.Wizard;
using System.ComponentModel.Composition;

/// <summary>
/// The Controls namespace.
/// </summary>
namespace CAS.SmartFactory.Shepherd.Client.Management.Controls
{
  [Export(typeof(IButtonsPanelBase))]
  public class ButtonsPanelViewModel : ButtonsPanelBase
  {
    [ImportingConstructor]
    public ButtonsPanelViewModel(ShellViewModel parentViewMode)
      : base(parentViewMode)
    {
      if (parentViewMode == null)
        throw new System.ArgumentNullException("parentViewMode", "Error in ImportingConstructor of the ButtonsPanelViewModel");
    }

  }
}
