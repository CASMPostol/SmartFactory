using System;
using System.Collections.Generic;
using System.ComponentModel;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CigarettesMaterialxML = CAS.SmartFactory.xml.erp.CigarettesMaterial;
using CutfillerMaterialxML = CAS.SmartFactory.xml.erp.CutfillerMaterial;
using MaterialXml = CAS.SmartFactory.xml.erp.Material;
using SKUXml = CAS.SmartFactory.xml.erp.SKU;

namespace CAS.SmartFactory.IPR.ListsEventsHandlers.Dictionaries
{
  internal static class SKUGetFromXML
  {
    #region public
    internal static void UpdateSKUCommonPart(SKUCommonPart skuCommonPart, MaterialXml xml, Dokument parent)
    {
      skuCommonPart.SKULibraryIndex = parent;
      skuCommonPart.SKU = xml.GetMaterial();
      skuCommonPart.Title = xml.GetMaterialDescription();
    }
    internal static void GetXmlContent
      (SKUXml xmlDocument, Entities edc, Dokument entry, ProgressChangedEventHandler progressChanged)
    {
      switch (xmlDocument.Type)
      {
        case CAS.SmartFactory.xml.erp.SKU.SKUType.Cigarettes:
          GetXmlContent(
            xmlDocument.GetMaterial(),
            edc,
            entry,
            (MaterialXml xml, Dokument lib, Entities context) => { return SKUCigarette((CigarettesMaterialxML)xml, lib, context); },
            progressChanged);
          break;
        case CAS.SmartFactory.xml.erp.SKU.SKUType.Cutfiller:
          GetXmlContent(
            xmlDocument.GetMaterial(),
            edc,
            entry,
            ( MaterialXml xml, Dokument lib, Entities context ) => { return SKUCutfiller( (CutfillerMaterialxML)xml, lib, context ); }, progressChanged );
          break;
      }
    }
    #endregion

    #region private
    private delegate SKUCommonPart CreateMaterialXml(MaterialXml xml, Dokument lib, Entities context);
    private static void GetXmlContent
      (MaterialXml[] material, Entities edc, Dokument parent, CreateMaterialXml creator, ProgressChangedEventHandler progressChanged)
    {
      List<SKUCommonPart> entities = new List<SKUCommonPart>();
      foreach (MaterialXml item in material)
      {
        try
        {
          progressChanged(null, new ProgressChangedEventArgs(1, "Processing: " + item.GetMaterial()));
          SKUCommonPart entity = SKUCommonPart.Find( edc, item.GetMaterial() );
          if (entity != null)
            continue;
          SKUCommonPart sku = creator(item, parent, edc);
          entities.Add(sku);
        }
        catch (Exception ex)
        {
          string message = String.Format("Cannot create: {0}:{1} because of the error: {2}", item.GetMaterial(), item.GetMaterialDescription(), ex.Message);
          Anons.WriteEntry(edc, "SKU entry error", message);
        }
      }
      if (entities.Count > 0)
        edc.SKU.InsertAllOnSubmit(entities);
    }
    private static SKUCigarette SKUCigarette( CigarettesMaterialxML xmlDocument, Dokument parent, Entities edc )
    {
      bool _menthol = xmlDocument.Menthol.StartsWith( "M" );
      SKUCigarette _ret = new SKUCigarette()
      {
        ProductType = ProductType.Cigarette,
        Brand = xmlDocument.Brand_Description,
        Family = xmlDocument.Family_Des,
        Menthol = xmlDocument.Menthol,
        MentholMaterial = _menthol,
        PrimeMarket = xmlDocument.Prime_Market,
      };
      _ret.ProcessData( xmlDocument.Cigarette_Length, xmlDocument.Filter_Segment_Length, edc );
      SKUGetFromXML.UpdateSKUCommonPart( _ret, xmlDocument, parent );
      return _ret;
    }
    private static SKUCutfiller SKUCutfiller( CutfillerMaterialxML xmlDocument, Dokument parent, Entities edc )
    {
      SKUCutfiller _ret = new SKUCutfiller()
      {
        ProductType = ProductType.Cutfiller,
        BlendPurpose = String.IsNullOrEmpty( xmlDocument.BlendPurpose ) ? String.Empty : xmlDocument.BlendPurpose
      };
      _ret.ProcessData( String.Empty, String.Empty, edc );
      SKUGetFromXML.UpdateSKUCommonPart( _ret, xmlDocument, parent );
      return _ret;
    }
    private const string m_Source = "SKU Processing";
    private const string m_Message = "I cannot find material with SKU: {0}";
    #endregion

  }
}
