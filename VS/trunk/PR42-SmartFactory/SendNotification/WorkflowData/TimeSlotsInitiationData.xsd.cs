using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace CAS.SmartFactory.Shepherd.SendNotification.WorkflowData
{
  public partial class TimeSlotsInitiationData
  {
    internal string Serialize()
    {
      try
      {
        using (MemoryStream stream = new MemoryStream())
        {
          XmlSerializer serializer = new XmlSerializer(typeof(TimeSlotsInitiationData));
          serializer.Serialize(stream, this);
          stream.Position = 0;
          byte[] bytes = new byte[stream.Length];
          stream.Read(bytes, 0, bytes.Length);
          return Encoding.UTF8.GetString(bytes);
        }
      }
      catch (Exception ex)
      {
        string _frmt = "Serialize aborted because of error: {0}";
        throw new ApplicationException(String.Format(_frmt, ex.Message));
      }
    }
    internal static TimeSlotsInitiationData Deserialize(string _input)
    {
      try
      {
        // deserialize initiation data; 
        using (StringReader _is = new StringReader(_input))
        {
          XmlSerializer serializer = new XmlSerializer(typeof(TimeSlotsInitiationData));
          XmlTextReader reader = new XmlTextReader(_is);
          return (TimeSlotsInitiationData)serializer.Deserialize(reader);
        }
      }
      catch (Exception ex)
      {
        string _frmt = "Deserialize aborted because of error: {0}";
        throw new ApplicationException(String.Format(_frmt, ex.Message));
      }
    }
  }
}
