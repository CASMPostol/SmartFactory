using System;
using System.Collections.Generic;
using System.ComponentModel;
using CAS.SmartFactory.IPR.WebsiteModel;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CigarettesMaterialxML = CAS.SmartFactory.xml.erp.CigarettesMaterial;
using CutfillerMaterialxML = CAS.SmartFactory.xml.erp.CutfillerMaterial;
using MaterialXml = CAS.SmartFactory.xml.erp.Material;
using SKUXml = CAS.SmartFactory.xml.erp.SKU;

namespace CAS.SmartFactory.IPR.ListsEventsHandlers
{
  internal static class SKUGetFromXML
  {
    #region public
    internal static void GetXmlContent
      ( SKUXml xmlDocument, Entities edc, Document entry, ProgressChangedEventHandler progressChanged )
    {
      switch ( xmlDocument.Type )
      {
        case CAS.SmartFactory.xml.erp.SKU.SKUType.Cigarettes:
          GetXmlContent(
            xmlDocument.GetMaterial(),
            edc,
            entry,
            ( MaterialXml xml, Document lib, Entities context, List<String> warnings ) => { return SKUCigarette( (CigarettesMaterialxML)xml, lib, context, warnings ); },
            progressChanged );
          break;
        case CAS.SmartFactory.xml.erp.SKU.SKUType.Cutfiller:
          GetXmlContent(
            xmlDocument.GetMaterial(),
            edc,
            entry,
            ( MaterialXml xml, Document lib, Entities context, List<String> warnings ) => { return SKUCutfiller( (CutfillerMaterialxML)xml, lib, context, warnings ); },
            progressChanged );
          break;
      }
    }
    #endregion

    #region private
    private delegate SKUCommonPart CreateMaterialXml( MaterialXml xml, Document lib, Entities context, List<String> warnings );
    private static void GetXmlContent
      ( MaterialXml[] material, Entities edc, Document parent, CreateMaterialXml creator, ProgressChangedEventHandler progressChanged )
    {
      List<SKUCommonPart> entities = new List<SKUCommonPart>();
      List<string> warnings = new List<string>();
      int _entries = 0;
      int _newEntries = 0;
      foreach ( MaterialXml item in material )
      {
        try
        {
          _entries++;
          progressChanged( null, new ProgressChangedEventArgs( 1, "Processing: " + item.GetMaterial() ) );
          SKUCommonPart entity = SKUCommonPart.Find( edc, item.GetMaterial() );
          if ( entity != null )
            continue;
          SKUCommonPart sku = creator( item, parent, edc, warnings );
          if ( sku != null )
          {
            _newEntries++;
            entities.Add( sku );
          }
        }
        catch ( Exception ex )
        {
          string message = String.Format( "Cannot create: {0}:{1} because of the error: {2}", item.GetMaterial(), item.GetMaterialDescription(), ex.Message );
          ActivityLogCT.WriteEntry( edc, "SKU entry error", message );
        }
      }
      if ( entities.Count > 0 )
      {
        edc.SKU.InsertAllOnSubmit( entities );
        progressChanged( null, new ProgressChangedEventArgs( 1, "Submiting Changes" ) );
        edc.SubmitChanges();
      }
      if ( warnings.Count > 0 )
        throw new InputDataValidationException( "SKU message import errors.", "XML import", new ErrorsList( warnings, true ) );
      string _pattern = "Finished content analysis, there are {0} entries, {1} new entries, {2} erroneous entries";
      ActivityLogCT.WriteEntry( "SKU Message Import", String.Format( _pattern, _entries, _newEntries, warnings.Count ), edc.Web );
    }
    private static SKUCigarette SKUCigarette( CigarettesMaterialxML xmlDocument, Document parent, Entities edc, List<String> warnings )
    {
      bool _menthol = xmlDocument.IsMenthol;
      SKUCigarette _ret = new SKUCigarette()
      {
        ProductType = ProductType.Cigarette,
        Brand = xmlDocument.Brand_Description,
        Family = xmlDocument.Family_Des,
        Menthol = xmlDocument.Menthol,
        MentholMaterial = _menthol,
        PrimeMarket = xmlDocument.Prime_Market,
      };
      if ( !_ret.ProcessData( xmlDocument.Cigarette_Length, xmlDocument.Filter_Segment_Length, edc, warnings ) )
        return null;
      SKUGetFromXML.UpdateSKUCommonPart( _ret, xmlDocument, parent );
      return _ret;
    }
    private static SKUCutfiller SKUCutfiller( CutfillerMaterialxML xmlDocument, Document parent, Entities edc, List<String> warnings )
    {
      SKUCutfiller _ret = new SKUCutfiller()
      {
        ProductType = ProductType.Cutfiller,
        BlendPurpose = String.IsNullOrEmpty( xmlDocument.BlendPurpose ) ? String.Empty : xmlDocument.BlendPurpose
      };
      if ( !_ret.ProcessData( String.Empty, String.Empty, edc, warnings ) )
        return null;
      SKUGetFromXML.UpdateSKUCommonPart( _ret, xmlDocument, parent );
      return _ret;
    }
    private static void UpdateSKUCommonPart( SKUCommonPart skuCommonPart, MaterialXml xml, Document parent )
    {
      skuCommonPart.SKULibraryIndex = parent;
      skuCommonPart.SKU = xml.GetMaterial();
      skuCommonPart.Title = xml.GetMaterialDescription();
    }
    private const string m_Source = "SKU Processing";
    private const string m_Message = "I cannot find material with SKU: {0}";
    #endregion

  }
}
