﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CAS.SmartFactory.IPR;
using CigarettesMaterialxML = CAS.SmartFactory.xml.erp.CigarettesMaterial;
using CutfillerMaterialxML = CAS.SmartFactory.xml.erp.CutfillerMaterial;
using MaterialXml = CAS.SmartFactory.xml.erp.Material;
using SKUXml = CAS.SmartFactory.xml.erp.SKU;

namespace CAS.SmartFactory.Linq.IPR
{
  public abstract partial class SKUCommonPart
  {
    #region public
    public SKUCommonPart(MaterialXml xml, Dokument parent, Entities edc)
      : this()
    {
      this.SKULibraryIndex = parent;
      this.SKU = xml.GetMaterial();
      this.Title = xml.GetMaterialDescription();
    }
    internal static SKUCommonPart Find(Entities edc, string index)
    {
      SKUCommonPart newSKU = null;
      try
      {
        newSKU = (
            from idx in edc.SKU
            where idx.SKU.Contains(index)
            orderby idx.Wersja descending
            select idx).First();
      }
      catch (Exception) { }
      return newSKU;
    }
    /// <summary>
    /// Gets the lookup.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <param name="index">The index.</param>
    /// <returns></returns>
    internal static SKUCommonPart GetLookup(Entities edc, string index)
    {
      try
      {
       return (
            from idx in edc.SKU
            where idx.SKU.Contains(index)
            orderby idx.Wersja descending
            select idx).First();
      }
      catch (Exception ex)
      {
        throw new IPRDataConsistencyException(m_Source, String.Format(m_Message, index), ex, "SKU lookup error");
      }
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
            (MaterialXml xml, Dokument lib, Entities context) => { return new SKUCigarette((CigarettesMaterialxML)xml, lib, context); },
            progressChanged);
          break;
        case CAS.SmartFactory.xml.erp.SKU.SKUType.Cutfiller:
          GetXmlContent(
            xmlDocument.GetMaterial(),
            edc,
            entry,
            (MaterialXml xml, Dokument lib, Entities context) => { return new SKUCutfiller((CutfillerMaterialxML)xml, lib, context); }, progressChanged);
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
          SKUCommonPart entity = Find(edc, item.GetMaterial());
          if (entity != null)
            continue;
          SKUCommonPart sku = creator(item, parent, edc);
          sku.ProcessData(item, edc);
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
    private void ProcessData(MaterialXml xml, Entities edc)
    {
      this.FormatIndex = GetFormatLookup(xml, edc);
      this.IPRMaterial = GetIPRMaterial(edc);
    }
    protected abstract Format GetFormatLookup(MaterialXml xml, Entities edc);
    protected abstract bool? GetIPRMaterial(Entities edc);
    private const string m_Source = "SKU Processing";
    private const string m_Message = "I cannot find material with SKU: {0}";
    #endregion
  }
}
