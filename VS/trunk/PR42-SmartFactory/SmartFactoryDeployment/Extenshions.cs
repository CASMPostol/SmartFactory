using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace CAS.SmartFactory.Deployment
{
  /// <summary>
  /// Project extenshion definitions
  /// </summary>
  public static class Extenshions
  {
    /// <summary>
    /// Adds the message.
    /// </summary>
    /// <param name="_box">The _box.</param>
    /// <param name="_msg">The _MSG.</param>
    public static void AddMessage(this ListBox _box, string _msg)
    {
      _box.Items.Add(_msg);
      _box.SelectedItem = _msg;
      _box.Refresh();
    }
    /// <summary>
    /// Parses the specified _value to get valid <see cref="Guid"/>.
    /// </summary>
    /// <param name="_value">The <see cref="String"/> _value.</param>
    /// <returns>Valid <see cref="Guid "/></returns>
    public static Guid Parse(this String _value)
    {
      if (String.IsNullOrEmpty(_value))
        return Guid.Empty;
      return new Guid(_value);
    }
    internal static FileInfo GetFileInfo()
    {
      string path = Path.Combine(
        Path.GetDirectoryName(Assembly.GetCallingAssembly().Location), 
        Properties.Settings.Default.InstallationStateFileName);
      return new FileInfo(path);
    }
  }
}
