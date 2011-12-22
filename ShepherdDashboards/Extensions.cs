using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Shepherd.Dashboards
{
  public static class Extensions
  {
    public static string ToString(this int? _val)
    {
      return _val.HasValue ? _val.Value.ToString() : String.Empty;
    }
    public static string ToString(this DateTime? _val)
    {
      return _val.HasValue ? _val.Value.ToString() : String.Empty;
    }
  }
}
