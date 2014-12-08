using CAS.SmartFactory.Shepherd.Client.Management.Infrastructure;
using CAS.SmartFactory.Shepherd.Client.Management.Infrastructure.Behaviors;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CAS.SmartFactory.Shepherd.Client.Management.Controls
{
  /// <summary>
  /// Interaction logic for SetupPanel.xaml
  /// </summary>
  [ViewExport(RegionName = RegionNames.ActionRegion)]
  [PartCreationPolicy(CreationPolicy.Shared)]
  public partial class SetupPanel : UserControl
  {
    public SetupPanel()
    {
      InitializeComponent();
    }
  }
}
