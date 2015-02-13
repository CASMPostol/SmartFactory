using System;
using System.ComponentModel;
using System.IO;
using CAS.SmartFactory.IPR.WebsiteModel;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using Microsoft.SharePoint;
using SKUXml = CAS.SmartFactory.xml.erp.SKU;

namespace CAS.SmartFactory.IPR.ListsEventsHandlers
{
  /// <summary>
  /// List Item Events
  /// </summary>
  public class SKUEventHandlers: SPItemEventReceiver
  {
    /// <summary>
    /// An item was added.
    /// </summary>
    public override void ItemAdded( SPItemEventProperties properties )
    {
      string At = "SKU Library";
      try
      {
        if ( !properties.ListTitle.Contains( "SKU Library" ) )
        {
          //TODO  [pr4-3435] Item add event - selective handling mechanism. http://itrserver/Bugs/BugDetail.aspx?bid=3435
          base.ItemAdded( properties );
          return;
        }
        this.EventFiringEnabled = false;
        if ( properties.ListItem.File == null )
          throw new System.ArgumentException( "Import of SKU xml message failed because the file is empty.", "File" );
        At = "using";
        using ( Stream _strm = properties.ListItem.File.OpenBinaryStream() )
        using ( Entities edc = new Entities( properties.WebUrl ) )
        {
          String message = String.Format( "Import of the SKU message {0} starting.", properties.ListItem.File.Name );
          ActivityLogCT.WriteEntry( edc, m_Title, message );
          At = "ImportDocument";
          SKUXml xml = SKUXml.ImportDocument( _strm );
          At = "GetAtIndex";
          Document entry = Element.GetAtIndex<Document>( edc.SKULibrary, properties.ListItem.ID );
          At = "GetXmlContent";
          SKUGetFromXML.GetXmlContent( xml, edc, entry, ( object obj, ProgressChangedEventArgs progres ) => { At = (string)progres.UserState; } );
          ActivityLogCT.WriteEntry( edc, m_Title, String.Format( "Import of the sku message {0} finished", properties.ListItem.File.Name ) );
        }
      }
      catch ( InputDataValidationException _ioex )
      {
        _ioex.ReportActionResult( properties.WebUrl, properties.ListItem.File.Name );
        string _fileName = GetFileName( properties );
        ActivityLogCT.WriteEntry( m_Title, String.Format( "SKU message {0} import has been stoped because of errors.", _fileName ), properties.WebUrl );
      }
      catch ( Exception ex )
      {
        string _pattern = @"SKU message {2} import has been stoped because of an unexpected fatal error <b>{0}</b> at: <b>{1}</b>";
        string _fileName = GetFileName( properties );
        ActivityLogCT.WriteEntry( m_Title, String.Format( _pattern, ex.Message, At, _fileName ), properties.WebUrl );
      }
      finally
      {
        this.EventFiringEnabled = true;
      }
      base.ItemAdded( properties );
    }
    private static string GetFileName( SPItemEventProperties properties )
    {
      return properties.ListItem == null || properties.ListItem.File == null ? "-- No File --" : properties.ListItem.File.Name;
    }
    private const string m_Title = "SKU Message Import";
  }
}



