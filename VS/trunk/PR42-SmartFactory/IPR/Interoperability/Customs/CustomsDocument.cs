using System;
using System.Xml;
using System.Xml.Serialization;

namespace CAS.SmartFactory.xml.Customs
{
  /// <summary>
  /// Represents any custom document from xml file.
  /// </summary>
  [Serializable()]
  public abstract class CustomsDocument
  {
    static public CustomsDocument ImportDocument(System.IO.Stream documetStream)
    {
      Type type = null;
      using (XmlReader reader = XmlReader.Create(documetStream))
      {
        if (reader.MoveToContent() != XmlNodeType.Element)
          throw new ApplicationException("The file does not contain valid xml file.");
        if (reader.Name.Contains("SAD"))
          type = typeof(SAD.SAD);
        else if (reader.Name.Contains("IE529"))
          type = typeof(IE529.IE529);
        else if (reader.Name.Contains("CLNE"))
          type = typeof(CLNE.CLNE);
        else if (reader.Name.Contains("PZC"))
          type = typeof(PZC.PZC);
      }
      if (type == null)
        throw new ApplicationException("The file does not contain a valid customs declaration xml document");
      documetStream.Seek(0, System.IO.SeekOrigin.Begin);
      using (XmlReader reader = XmlReader.Create(documetStream, new XmlReaderSettings() { }))
      {
        XmlSerializer invoice = new XmlSerializer(type);
        return (CustomsDocument)invoice.Deserialize(reader);
      }
    }

    #region public abstract
    /// <summary>
    /// Document Type - rrot element
    /// </summary>
    public enum DocumentType { SAD, IE529, CLNE, PZC }
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
