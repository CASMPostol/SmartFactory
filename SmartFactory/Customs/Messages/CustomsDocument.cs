//<summary>
//  Title   : public abstract class CustomsDocument
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
using System.Xml;
using System.Xml.Serialization;
using CAS.SmartFactory.Customs.Messages.CELINA.CLNE;
using CAS.SmartFactory.Customs.Messages.CELINA.PZC;
using CAS.SmartFactory.Customs.Messages.CELINA.SAD;
using CAS.SmartFactory.Customs.Messages.ECS;

namespace CAS.SmartFactory.Customs.Messages
{
  /// <summary>
  /// Represents any custom document from xml file.
  /// </summary>
  [Serializable()]
  public abstract class CustomsDocument
  {
    /// <summary>
    /// Imports the document.
    /// </summary>
    /// <param name="documetStream">The documet stream.</param>
    /// <returns></returns>
    /// <exception cref="System.ApplicationException">
    /// The file does not contain valid xml file.
    /// or
    /// The file does not contain a valid customs declaration xml document
    /// </exception>
    static public CustomsDocument ImportDocument( System.IO.Stream documetStream )
    {
      Type type = null;
      using ( XmlReader reader = XmlReader.Create( documetStream ) )
      {
        if ( reader.MoveToContent() != XmlNodeType.Element )
          throw new ApplicationException( "The file does not contain valid xml file." );
        if ( reader.Name.Contains( "SAD" ) )
          type = typeof( SAD );
        else if ( reader.Name.Contains( "IE529" ) )
          type = typeof( IE529 );
        else if ( reader.Name.Contains( "CLNE" ) )
          type = typeof( CLNE );
        else if ( reader.Name.Contains( "PZC" ) )
          type = typeof( PZC );
      }
      if ( type == null )
        throw new ApplicationException( "The file does not contain a valid customs declaration xml document" );
      documetStream.Seek( 0, System.IO.SeekOrigin.Begin );
      using ( XmlReader reader = XmlReader.Create( documetStream, new XmlReaderSettings() { } ) )
      {
        XmlSerializer invoice = new XmlSerializer( type );
        return (CustomsDocument)invoice.Deserialize( reader );
      }
    }

    #region public abstract
    /// <summary>
    /// Document Type - rrot element
    /// </summary>
    public enum DocumentType
    {
      /// <summary>
      /// The sad customs message
      /// </summary>
      SAD,
      /// <summary>
      /// The IE529 customs message
      /// </summary>
      IE529,
      /// <summary>
      /// The CLNE customs message
      /// </summary>
      CLNE,
      /// <summary>
      /// The PZC customs message
      /// </summary>
      PZC
    }
    /// <summary>
    /// Gets the SAD good.
    /// </summary>
    /// <returns></returns>
    public abstract GoodDescription[] GetSADGood();
    /// <summary>
    /// Gets the reference number.
    /// </summary>
    /// <returns></returns>
    public abstract string GetReferenceNumber();
    /// <summary>
    /// The name of the root message.
    /// </summary>
    /// <returns></returns>
    public abstract DocumentType MessageRootName();
    /// <summary>
    /// Gets the currency.
    /// </summary>
    /// <returns></returns>
    public abstract string GetCurrency();
    /// <summary>
    /// Gets the customs debt date.
    /// </summary>
    /// <returns></returns>
    public abstract DateTime? GetCustomsDebtDate();
    /// <summary>
    /// Gets the document number.
    /// </summary>
    /// <returns></returns>
    public abstract string GetDocumentNumber();
    /// <summary>
    /// Gets the exchange rate.
    /// </summary>
    /// <returns></returns>
    public abstract double? GetExchangeRate();
    /// <summary>
    /// Gets the gross mass.
    /// </summary>
    /// <returns></returns>
    public abstract double? GetGrossMass();
    #endregion

  }
}
