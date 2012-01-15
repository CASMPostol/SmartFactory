using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using CAS.SmartFactory.Deployment.Properties;
using System.IO;
using System.Windows.Forms;


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
      InitializeComponent();
      //Directory.SetCurrentDirectory(Path.GetDirectoryName(this.GetType().Assembly.Location));
      //SetUpData.TraceEvent.TraceVerbose(27, "Installer", String.Format("Setting current directory to {0}.", Directory.GetCurrentDirectory()));
      //using (SetUpData _sud = new SetUpData())
      //{
      //  Application.Run(_sud);
      //  if (_sud.DialogResult != System.Windows.Forms.DialogResult.OK)
      //  {
      //    if (!MBox())
      //      throw new ApplicationException(String.Format(Resources.SeeLogFile, Directory.GetCurrentDirectory()));
      //  }
      //}
      base.Install(stateSaver);
    }
    //private static bool MBox()
    //{
    //  return System.Windows.Forms.MessageBox.Show(
    //    Resources.InstallationFailed, Resources.CancelInstallationCaption,
    //    System.Windows.Forms.MessageBoxButtons.OKCancel,
    //    System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK;
    //}
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
    /// When overridden in a derived class, restores the pre-installation state of the computer.
    /// </summary>
    /// <param name="savedState">An <see cref="T:System.Collections.IDictionary"/> that contains the pre-installation state of the computer.</param>
    /// <exception cref="T:System.ArgumentException">
    /// The <paramref name="savedState"/> parameter is null.
    /// -or-
    /// The saved-state <see cref="T:System.Collections.IDictionary"/> might have been corrupted.
    ///   </exception>
    ///   
    /// <exception cref="T:System.Configuration.Install.InstallException">
    /// An exception occurred during the <see cref="M:System.Configuration.Install.Installer.Rollback(System.Collections.IDictionary)"/> phase of the installation. This exception is ignored and the rollback continues. However, the computer might not be fully reverted to its initial state after the rollback completes.
    ///   </exception>
    public override void Rollback(IDictionary savedState)
    {
      //Directory.SetCurrentDirectory(Path.GetDirectoryName(this.GetType().Assembly.Location));
      //Uninstall m_dialog = new Uninstall();
      //m_dialog.ShowDialog();
      base.Rollback(savedState);
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
      //Directory.SetCurrentDirectory(Path.GetDirectoryName(this.GetType().Assembly.Location));
      //Uninstall m_dialog = new Uninstall();
      //m_dialog.ShowDialog();
      base.Uninstall(savedState);
    }
  }
}
