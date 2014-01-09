using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Shepherd.ImportExport
{
  public partial class FileDialog : Component
  {
    public FileDialog()
    {
      InitializeComponent();
    }

    public FileDialog(IContainer container)
    {
      container.Add(this);

      InitializeComponent();
    }
  }
}
