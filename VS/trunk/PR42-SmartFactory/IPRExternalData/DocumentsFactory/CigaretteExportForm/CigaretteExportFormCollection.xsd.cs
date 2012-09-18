using System.IO;
using System.Xml.Serialization;
using Microsoft.SharePoint;
using System.Xml;
using System.Collections.Generic;
using System.Linq;

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
  public partial class AmountOfMoney
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="AmountOfMoney"/> class.
    /// </summary>
    /// <param name="amount">The amount.</param>
    /// <param name="currency">The currency.</param>
    public AmountOfMoney( double amount, string currency )
    {
      this.Amount = amount;
      this.Currency = currency;
    }
    
    public AmountOfMoney() { }
  }
  public partial class TotalAmountOfMoney
  {
    private Dictionary<string, AmountOfMoney> _totals = new Dictionary<string, AmountOfMoney>();
    /// <summary>
    /// Adds the specified currency, amount pair to the totals.
    /// </summary>
    /// <param name="currency">The currency.</param>
    /// <param name="amount">The amount of money.</param>
    public void Add( string currency, double amount )
    {
      if ( _totals.ContainsKey( currency ) )
        _totals[ currency ].Amount += amount;
      else
        _totals.Add( currency, new AmountOfMoney( amount, currency ) );
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
