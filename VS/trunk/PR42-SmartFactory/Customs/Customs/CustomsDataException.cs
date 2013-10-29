using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml.Customs
{
  public class CustomsDataException : ApplicationException
  {
    /// <summary>
    /// Initializes a new instance of the System.ApplicationException class with a specified error message.
    /// </summary>
    /// <param name="_src">The name of the application or the object that causes the error.</param>
    /// <param name="message">A message that describes the error.</param>
    public CustomsDataException(string _src, string message)
      : base(message)
    {
      Source = _src;
    }
  }
}
