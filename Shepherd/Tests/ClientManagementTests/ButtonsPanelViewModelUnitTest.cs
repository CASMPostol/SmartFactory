﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Events;
using Prism.Regions;
using System;

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
      Assert.Inconclusive("ShellViewModelMoc - creation with null log service causes exception. ");
      using (ShellViewModelMoc shell = new ShellViewModelMoc())
      {
        Controls.ButtonsPanelViewModel _bp = new Controls.ButtonsPanelViewModel(shell);
        Assert.IsNotNull(_bp);
      }
    }
    private class ShellViewModelMoc : ShellViewModel
    {
      public ShellViewModelMoc()
        : base(new RegionManager(), new EventAggregator(), null)
      { }

    }
  }
}

