using System.IO;
using System.Xml.Serialization;
using Microsoft.SharePoint;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System;

namespace CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm
{
  public partial class CigaretteExportFormCollection
  {
    /// <summary>
    /// Adds the document to collection.
    /// </summary>
    /// <param name="destinationCollection">The destination collection.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <returns>An object of <see cref="SPFile"/> containing the serialized <paramref name="destinationCollection"/></returns>
    public SPFile AddDocument2Collection( SPFileCollection destinationCollection, string fileName )
    {
      SPFile _docFile = default( SPFile );
      XmlSerializer _srlzr = new XmlSerializer( typeof( CigaretteExportFormCollection ) );
      XmlWriterSettings _setting = new XmlWriterSettings()
      {
        Indent = true,
        IndentChars = "  ",
        NewLineChars = "\r\n"
      };
      using ( MemoryStream _docStrm = new MemoryStream() )
      using ( XmlWriter file = XmlWriter.Create( _docStrm, _setting ) )
      {
        file.WriteProcessingInstruction( "xml-stylesheet", "type=\"text/xsl\" href=\"CigaretteExportFormCollection.xslt\"" );
        _srlzr.Serialize( file, this );
        _docFile = destinationCollection.Add( fileName + ".xml", _docStrm, true );
      }
      return _docFile;
    }
  }
  /// <summary>
  /// Class representing Amount Of Money
  /// </summary>
  public partial class AmountOfMoney
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="AmountOfMoney"/> class.
    /// </summary>
    /// <param name="iprMaterialValueTotal">The ipr material value total.</param>
    /// <param name="iprMaterialDutyTotal">The ipr material duty total.</param>
    /// <param name="iprMaterialVATTotal">The ipr material VAT total.</param>
    /// <param name="currency">The currency.</param>
    public AmountOfMoney( double iprMaterialValueTotal, double iprMaterialDutyTotal, double iprMaterialVATTotal, string currency )
    {
      this.IPRMaterialValueTotal = iprMaterialValueTotal;
      this.IPRMaterialDutyTotal = iprMaterialDutyTotal;
      this.IPRMaterialVATTotal = iprMaterialVATTotal;
      this.Currency = currency;
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="AmountOfMoney"/> class.
    /// </summary>
    [Obsolete( "Is to be used only by the XML serializer" )]
    public AmountOfMoney() { }
  }
  /// <summary>
  /// Class to calculate total sum of money.
  /// </summary>
  public partial class TotalAmountOfMoney
  {
    private Dictionary<string, AmountOfMoney> _totals = new Dictionary<string, AmountOfMoney>();
    /// <summary>
    /// Adds the specified currency, amount pair to the totals.
    /// </summary>
    /// <param name="currency">The currency.</param>
    /// <param name="amount">The amount of money.</param>
    public void Add( AmountOfMoney amount )
    {
      if ( _totals.ContainsKey( amount.Currency ) )
      {
        AmountOfMoney _tts = _totals[ amount.Currency ];
        _tts.IPRMaterialDutyTotal += amount.IPRMaterialDutyTotal;
        _tts.IPRMaterialValueTotal += amount.IPRMaterialValueTotal;
        _tts.IPRMaterialVATTotal += amount.IPRMaterialVATTotal;
      }
      else
        _totals.Add( amount.Currency, new AmountOfMoney( amount.IPRMaterialValueTotal, amount.IPRMaterialDutyTotal, amount.IPRMaterialVATTotal, amount.Currency ) );
    }
    /// <summary>
    /// Assigne current totals to the AmountOfMoney.
    /// </summary>
    public void AssignTotals()
    {
      this.AmountOfMoney = _totals.Values.ToArray<AmountOfMoney>();
    }
  }
}
