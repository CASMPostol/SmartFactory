﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using CAS.SmartFactory.Deployment.Properties;


namespace CAS.SmartFactory.Deployment
{
  /// <summary>
  /// Installer of the web application
  /// </summary>
  [RunInstaller(true)]
  public partial class Installer : System.Configuration.Install.Installer
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Installer"/> class.
    /// </summary>
    public Installer()
    {
      InitializeComponent();
    }
    /// <summary>
    /// Performs the installation.
    /// </summary>
    /// <param name="stateSaver">An <see cref="T:System.Collections.IDictionary"/> used to save information needed to perform a commit, rollback, or uninstall operation.</param>
    /// <exception cref="T:System.ArgumentException">
    /// The <paramref name="stateSaver"/> parameter is null.
    ///   </exception>
    ///   
    /// <exception cref="T:System.Exception">
    /// An exception occurred in the <see cref="E:System.Configuration.Install.Installer.BeforeInstall"/> event handler of one of the installers in the collection.
    /// -or-
    /// An exception occurred in the <see cref="E:System.Configuration.Install.Installer.AfterInstall"/> event handler of one of the installers in the collection.
    ///   </exception>
    public override void Install(IDictionary stateSaver)
    {
      base.Install(stateSaver);
      SetUpData _dialog = new SetUpData() { Manual = true };
      if (_dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
        System.Windows.Forms.MessageBox.Show(
          Resources.AreYouSure2Cancel, Resources.CancelInstallationCaption,
          System.Windows.Forms.MessageBoxButtons.OKCancel,
          System.Windows.Forms.MessageBoxIcon.Question);

    }
    /// <summary>
    /// Completes the install transaction.
    /// </summary>
    /// <param name="savedState">An <see cref="T:System.Collections.IDictionary"/> that contains the state of the computer after all the installers in the collection have run.</param>
    /// <exception cref="T:System.ArgumentException">
    /// The <paramref name="savedState"/> parameter is null.
    /// -or-
    /// The saved-state <see cref="T:System.Collections.IDictionary"/> might have been corrupted.
    ///   </exception>
    ///   
    /// <exception cref="T:System.Configuration.Install.InstallException">
    /// An exception occurred during the <see cref="M:System.Configuration.Install.Installer.Commit(System.Collections.IDictionary)"/> phase of the installation. This exception is ignored and the installation continues. However, the application might not function correctly after the installation is complete.
    ///   </exception>
    public override void Commit(IDictionary savedState)
    {
      base.Commit(savedState);
    }
    /// <summary>
    /// When overridden in a derived class, removes an installation.
    /// </summary>
    /// <param name="savedState">An <see cref="T:System.Collections.IDictionary"/> that contains the state of the computer after the installation was complete.</param>
    /// <exception cref="T:System.ArgumentException">
    /// The saved-state <see cref="T:System.Collections.IDictionary"/> might have been corrupted.
    ///   </exception>
    ///   
    /// <exception cref="T:System.Configuration.Install.InstallException">
    /// An exception occurred while uninstalling. This exception is ignored and the uninstall continues. However, the application might not be fully uninstalled after the uninstallation completes.
    ///   </exception>
    public override void Uninstall(IDictionary savedState)
    {
      base.Uninstall(savedState);
    }
  }
}
