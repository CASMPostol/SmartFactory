﻿//<summary>
//  Title   : static class JsonSerializer
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2013-11-06 09:19:50 +0100 (Śr, 06 lis 2013) $
//  $Rev: 9920 $
//  $LastChangedBy: mpostol $
//  $URL: svn://svnserver.hq.cas.com.pl/VS/trunk/PR42-SmartFactory/Customs/Messages/Serialization/JsonSerializer.cs $
//  $Id: JsonSerializer.cs 9920 2013-11-06 08:19:50Z mpostol $
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
      
using System;
using System.Runtime.Serialization;             // Reference System.Runtime.Serialization
using System.Runtime.Serialization.Json;        // Reference System.ServiceModel.Web
using System.Text;

namespace CAS.SmartFactory.CW.Dashboards.CheckListWebPart.Serialization
{
  /// <summary>
  /// static class JsonSerializer
  /// </summary>
  public static class JsonSerializer
  {
    /// <summary>
    /// Serializes the specified object.
    /// </summary>
    /// <typeparam name="Type">The type of the <paramref name="objectToSerialize"/>.</typeparam>
    /// <param name="objectToSerialize">The object to be serialized.</param>
    public static string Serialize<Type>( Type objectToSerialize )
    {
      DataContractJsonSerializer _Srlzr = new DataContractJsonSerializer( typeof( Type ) );
      using ( System.IO.MemoryStream _writer = new System.IO.MemoryStream() )
      {
        _Srlzr.WriteObject( _writer, objectToSerialize );
        UTF8Encoding _encoding = new UTF8Encoding();
        _writer.Flush();
        return ( _encoding.GetString( _writer.GetBuffer(), 0, Convert.ToInt32( _writer.Length ) ) );
      }
    }
    /// <summary>
    /// Deserializes the specified object <paramref name="serializedObject"/> represented as the Json ASCII string.
    /// </summary>
    /// <param name="serializedObject">The serialized object.</param>
    /// <returns></returns>
    public static Type Deserialize<Type>( string serializedObject )
    {
      DataContractJsonSerializer _Srlzr = new DataContractJsonSerializer( typeof( Type ) );
      using ( System.IO.MemoryStream _writer = new System.IO.MemoryStream() )
      {
        UTF8Encoding _encoding = new UTF8Encoding();
        byte[] _string = _encoding.GetBytes( serializedObject );
        _writer.Write( _string, 0, _string.Length );
        _writer.Flush();
        _writer.Position = 0;
        return (Type)_Srlzr.ReadObject( _writer );
      }
    }
  }
}
