using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAS.Common.ViewModel.Wizard;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.PubSubEvents;

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
    public void ButtonsPanelViewModelCreationTestMethod()
    {
      using (ShellViewModelMoc shell = new ShellViewModelMoc())
      {
        Controls.ButtonsPanelViewModel _bp = new Controls.ButtonsPanelViewModel(shell);
        Assert.IsNotNull(_bp);
      }
    }
    private class ShellViewModelMoc : ShellViewModel
    {
      public ShellViewModelMoc()
        : base(new RegionManager(), new EventAggregator())
      { }

    }
  }
}

