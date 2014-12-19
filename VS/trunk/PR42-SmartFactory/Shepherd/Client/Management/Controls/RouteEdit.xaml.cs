//<summary>
//  Title   : class Main 
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

using CAS.Common.Interactivity.InteractionRequest;
using Microsoft.Win32;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;

namespace CAS.SmartFactory.Shepherd.Client.Management.Controls
{
  /// <summary>
  /// Interaction logic for Main.xaml
  /// </summary>
  [PartCreationPolicy(CreationPolicy.NonShared)]
  public partial class RouteEdit : UserControl
  {

    #region creator
    /// <summary>
    /// Initializes a new instance of the <see cref="Main"/> class.
    /// </summary>
    public RouteEdit()
    {
      InitializeComponent();
    }
    #endregion

    #region Imports
    /// Sets the ViewModel.
    /// </summary>
    /// <remarks>
    /// This set-only property is annotated with the <see cref="ImportAttribute"/> so it is injected by MEF with
    /// the appropriate view model.
    /// </remarks>
    [Import]
    public RouteEditViewModel StateMachineContext
    {
      set
      {
        DataContext = value;
        value.ReadSiteContentConfirmation.Raised += ReadSiteContentConfirmation_Raised;
        value.ReadRouteFileNameConfirmation.Raised += ReadRouteFileNameConfirmation_Raised;
      }
    }
    #endregion

    #region handlers
    private void ReadRouteFileNameConfirmation_Raised(object sender, InteractionRequestedEventArgs<IConfirmation> e)
    {
      OpenFileDialog _ofd = (OpenFileDialog)e.Context.Content;
      if (_ofd.ShowDialog().GetValueOrDefault(false))
        e.Context.Confirmed = true;
      else
        e.Context.Confirmed = false;
      e.Callback();
    }
    private void ReadSiteContentConfirmation_Raised(object sender, InteractionRequestedEventArgs<IConfirmation> e)
    {
      if (MessageBox.Show((string)e.Context.Content, e.Context.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        e.Context.Confirmed = true;
      else
        e.Context.Confirmed = false;
      e.Callback();
    }
    #endregion

  }
}
