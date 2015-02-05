//<summary>
//  Title   : Name of Application
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace CAS.SmartFactory.Customs.Messages.Serialization
{
  [DataContract]
  public partial class Organization
  {

    #region Backing fields
    private decimal pozIdField;
    private string nazwaField;
    private string ulicaNumerField;
    private string kodPocztowyField;
    private string miejscowoscField;
    private string krajField;
    private string tINField;
    private string regonField;
    private string peselField;
    private string eORIField;
    #endregion

    #region public serializable properties
    /// <summary>
    /// Gets or sets the poz unique identifier.
    /// </summary>
    /// <value>
    /// The poz unique identifier.
    /// </value>
    /// <uwagi />
    [DataMember]
    public decimal Id
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
    /// <summary>
    /// Gets or sets the nazwa.
    /// </summary>
    /// <value>
    /// The nazwa.
    /// </value>
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
    /// <summary>
    /// Gets or sets the ulica numer.
    /// </summary>
    /// <value>
    /// The ulica numer.
    /// </value>
    [DataMember]
    public string UlicaNr
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
    /// <summary>
    /// Gets or sets the kod pocztowy.
    /// </summary>
    /// <value>
    /// The kod pocztowy.
    /// </value>
    [DataMember]
    public string Kod
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
    /// <summary>
    /// Gets or sets the miejscowosc.
    /// </summary>
    /// <value>
    /// The miejscowosc.
    /// </value>
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
    /// <summary>
    /// Gets or sets the kraj.
    /// </summary>
    /// <value>
    /// The kraj.
    /// </value>
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
    /// <summary>
    /// Gets or sets the tin.
    /// </summary>
    /// <value>
    /// The tin.
    /// </value>
    [DataMember]
    public string TIN
    {
      get
      {
        return this.tINField;
      }
      set
      {
        this.tINField = value;
      }
    }
    [DataMember]
    public string Regon
    {
      get
      {
        return this.regonField;
      }
      set
      {
        this.regonField = value;
      }
    }
    /// <summary>
    /// Gets or sets the pesel.
    /// </summary>
    /// <value>
    /// The pesel.
    /// </value>
    [DataMember]
    public string Pesel
    {
      get
      {
        return this.peselField;
      }
      set
      {
        this.peselField = value;
      }
    }
    /// <summary>
    /// Gets or sets the eori.
    /// </summary>
    /// <value>
    /// The eori.
    /// </value>
    [DataMember]
    public string EORI
    {
      get
      {
        return this.eORIField;
      }
      set
      {
        this.eORIField = value;
      }
    }
    #endregion

    #region Serialization
    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns></returns>
    public string Serialize()
    {
      return JsonSerializer.Serialize<Organization>( this );
    }
    /// <summary>
    /// Deserializes the specified serialized object.
    /// </summary>
    /// <param name="serializedObject">The serialized object.</param>
    /// <returns></returns>
    public static Organization Deserialize( string serializedObject )
    {
      return JsonSerializer.Deserialize<Organization>( serializedObject );
    }
    #endregion

  }
}
