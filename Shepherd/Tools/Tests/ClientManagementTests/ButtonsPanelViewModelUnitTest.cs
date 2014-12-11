using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAS.Common.ViewModel.Wizard;

namespace CAS.SmartFactory.Shepherd.Client.Management.Tests
{
  [TestClass]
  public class ButtonsPanelViewModelUnitTest
  {
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ButtonsPanelViewModelNullArgumentTestMethod()
    {
      Controls.ButtonsPanelViewModel _bp = new Controls.ButtonsPanelViewModel(null);
    }
    [TestMethod]
    public void ButtonsPanelViewModelCtorTestMethod()
    {
      Controls.ButtonsPanelViewModel _bp = new Controls.ButtonsPanelViewModel(new ShellViewModel(null, null));
      
    }

    private class  ShellViewModelMoc : StateMachineContext
    {
      public ShellViewModelMoc(): base()
      {

      }
    }
  }
}
