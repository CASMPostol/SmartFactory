using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CAS.SmartFactory.Deployment
{
  /// <summary>
  /// Project extenshion definitions
  /// </summary>
  public static class Extenshions
  {
    public static void AddMessage(this ListBox _box, string _msg)
    {
      _box.Items.Add(_msg);
      _box.SelectedItem = _msg;
      _box.Refresh();
    }
  }
}
