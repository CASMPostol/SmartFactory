//<summary>
//  Title   : public class CustomsOffice
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System.Runtime.Serialization;

namespace CAS.SmartFactory.Customs.Messages.Serialization
{
  /// <summary>
  /// public class CustomsOffice
  /// </summary>
  [DataContract]
  public class CustomsOffice
  {

    #region private backing filds
    private CustomsOfficeUCLokalizacja lokalizacjaField;
    private CustomsOfficeUCUCKontrolny uCKontrolnyField;
    private CustomsOfficeUCSkladCelny skladCelnyField;
    private CustomsOfficeUCUCTranzytowy[] uCTranzytowyField;
    private string uCZgloszeniaField;
    private string uCGranicznyField;
    private string uCPrzeznaczeniaField;
    #endregion

    #region public DataMember fields
    [DataMember]
    public CustomsOfficeUCLokalizacja Lokalizacja
    {
      get
      {
        return this.lokalizacjaField;
      }
      set
      {
        this.lokalizacjaField = value;
      }
    }
    [DataMember]
    public CustomsOfficeUCUCKontrolny UCKontrolny
    {
      get
      {
        return this.uCKontrolnyField;
      }
      set
      {
        this.uCKontrolnyField = value;
      }
    }
    [DataMember]
    public CustomsOfficeUCSkladCelny SkladCelny
    {
      get
      {
        return this.skladCelnyField;
      }
      set
      {
        this.skladCelnyField = value;
      }
    }
    [DataMember]
    public CustomsOfficeUCUCTranzytowy[] UCTranzytowy
    {
      get
      {
        return this.uCTranzytowyField;
      }
      set
      {
        this.uCTranzytowyField = value;
      }
    }
    [DataMember]
    public string UCZgloszenia
    {
      get
      {
        return this.uCZgloszeniaField;
      }
      set
      {
        this.uCZgloszeniaField = value;
      }
    }
    [DataMember]
    public string UCGraniczny
    {
      get
      {
        return this.uCGranicznyField;
      }
      set
      {
        this.uCGranicznyField = value;
      }
    }
    [DataMember]
    public string UCPrzeznaczenia
    {
      get
      {
        return this.uCPrzeznaczeniaField;
      }
      set
      {
        this.uCPrzeznaczeniaField = value;
      }
    }
    #endregion

    #region Serialization
    /// <summary>
    /// Serializes this instance.
    /// </summary>
    public string Serialize()
    {
      return JsonSerializer.Serialize<CustomsOffice>( this );
    }
    /// <summary>
    /// Deserializes the specified serialized object.
    /// </summary>
    /// <param name="serializedObject">The serialized object.</param>
    public static CustomsOffice Deserialize( string serializedObject )
    {
      return JsonSerializer.Deserialize<CustomsOffice>( serializedObject );
    }
    #endregion

  }
  /// <summary>
  /// partial class CustomsOfficeUCLokalizacja
  /// </summary>
  [DataContract]
  public partial class CustomsOfficeUCLokalizacja
  {

    #region private backing filds
    private string miejsceField;
    private string ucField;
    private string opisField;
    #endregion

    #region public DataMember fields
    [DataMember]
    public string Miejsce
    {
      get
      {
        return this.miejsceField;
      }
      set
      {
        this.miejsceField = value;
      }
    }

    [DataMember]
    public string UC
    {
      get
      {
        return this.ucField;
      }
      set
      {
        this.ucField = value;
      }
    }

    [DataMember]
    public string Opis
    {
      get
      {
        return this.opisField;
      }
      set
      {
        this.opisField = value;
      }
    }
    #endregion
  }
  /// <summary>
  /// partial class CustomsOfficeUCUCKontrolny
  /// </summary>
  [DataContract]
  public partial class CustomsOfficeUCUCKontrolny
  {

    #region private backing filds
    private string uCKontrolnyField;
    private string nazwaField;
    private string ulicaNumerField;
    private string kodPocztowyField;
    private string miejscowoscField;
    private string krajField;
    #endregion

    #region public DataMember fields
    [DataMember]
    public string UCKontrolny
    {
      get
      {
        return this.uCKontrolnyField;
      }
      set
      {
        this.uCKontrolnyField = value;
      }
    }

    [DataMember]
    public string Nazwa
    {
      get
      {
        return this.nazwaField;
      }
      set
      {
        this.nazwaField = value;
      }
    }

    [DataMember]
    public string UlicaNumer
    {
      get
      {
        return this.ulicaNumerField;
      }
      set
      {
        this.ulicaNumerField = value;
      }
    }

    [DataMember]
    public string KodPocztowy
    {
      get
      {
        return this.kodPocztowyField;
      }
      set
      {
        this.kodPocztowyField = value;
      }
    }

    [DataMember]
    public string Miejscowosc
    {
      get
      {
        return this.miejscowoscField;
      }
      set
      {
        this.miejscowoscField = value;
      }
    }

    [DataMember]
    public string Kraj
    {
      get
      {
        return this.krajField;
      }
      set
      {
        this.krajField = value;
      }
    }
    #endregion

  }
  /// <summary>
  /// public partial class CustomsOfficeUCSkladCelny
  /// </summary>
  [DataContract]
  public partial class CustomsOfficeUCSkladCelny
  {

    #region private backing filds
    private string typField;
    private string miejsceField;
    private string krajField;
    #endregion

    #region public DataMember fields
    [DataMember]
    public string Typ
    {
      get
      {
        return this.typField;
      }
      set
      {
        this.typField = value;
      }
    }

    [DataMember]
    public string Miejsce
    {
      get
      {
        return this.miejsceField;
      }
      set
      {
        this.miejsceField = value;
      }
    }

    [DataMember]
    public string Kraj
    {
      get
      {
        return this.krajField;
      }
      set
      {
        this.krajField = value;
      }
    }
    #endregion

  }
  /// <summary>
  /// public partial class CustomsOfficeUCUCTranzytowy
  /// </summary>
  [DataContract]
  public partial class CustomsOfficeUCUCTranzytowy
  {

    #region private backing filds
    private decimal pozIdField;
    private string uCTranzytowyField;
    #endregion

    #region public DataMember fields
    [DataMember]
    public decimal PozId
    {
      get
      {
        return this.pozIdField;
      }
      set
      {
        this.pozIdField = value;
      }
    }

    [DataMember]
    public string UCTranzytowy
    {
      get
      {
        return this.uCTranzytowyField;
      }
      set
      {
        this.uCTranzytowyField = value;
      }
    }
    #endregion

  }

}
