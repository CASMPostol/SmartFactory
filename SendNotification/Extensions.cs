using System;
using CAS.SmartFactory.Shepherd.SendNotification.Entities;

namespace CAS.SmartFactory.Shepherd.SendNotification
{
  public static class Extensions
  {
    public static string Cast2String(this object value)
    {
      return value == null ? String.Empty : (string)value;
    }
    public static string Title(this Element _val)
    {
      return _val == null ? String.Empty : _val.Tytuł;
    }
    public static string UnknownIfEmpty(this String _val)
    {
      return String.IsNullOrEmpty(_val) ? CommonDefinition.UnknownEmail : _val;
    }
  }
}
