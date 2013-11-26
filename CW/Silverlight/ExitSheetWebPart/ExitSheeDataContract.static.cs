//<summary>
//  Title   : class ExitSheeDataContract
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>


using CAS.SmartFactory.CW.Dashboards.ExitSheetWebPart.Serialization;

namespace CAS.SmartFactory.CW.Dashboards.Webparts.ExitSheetHost
{

  /// <summary>
  /// class ExitSheeDataContract
  /// </summary>
  partial class ExitSheeDataContract
  {
    /// <summary>
    /// Deserializes the specified serialized object.
    /// </summary>
    /// <param name="serializedObject">The serialized object.</param>
    public static ExitSheeDataContract Deserialize( string serializedObject )
    {

      return JsonSerializer.Deserialize<ExitSheeDataContract>( serializedObject );
    }
    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns><see cref="string"/> as serialized this object.</returns>
    public string Serialize()
    {
      return JsonSerializer.Serialize<ExitSheeDataContract>( this );
    }

  }
}
