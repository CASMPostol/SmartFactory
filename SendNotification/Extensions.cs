using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Shepherd.SendNotification
{
  public static class Extensions
  {
    public static string Cast2String(this object value)
    {
      return value == null ? String.Empty : (string)value;
    }
  }
}
