//<summary>
//  Title   : class MainPageData
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Runtime.Serialization;

namespace CAS.SmartFactory.CW.Dashboards.Webparts.ExitSheetHost
{
  /// <summary>
  /// class MainPageData
  /// </summary>
  [DataContract]
  internal partial class ExitSheeDataContract
  {

    #region public properties
    [DataMember]
    public string LabelOGLNumber
    {
      get { return b_LabelOGLNumber; }
      set { b_LabelOGLNumber = value; }
    }
    [DataMember]
    public DateTime LabelZDdnia
    {
      get { return b_LabelZDdnia; }
      set { b_LabelZDdnia = value; }
    }
    [DataMember]
    public DateTime LabelOGLDate
    {
      get { return b_LabelOGLDate; }
      set { b_LabelOGLDate = value; }
    }
    [DataMember]
    public string LabelTobaccoName
    {
      get { return b_LabelTobaccoName; }
      set { b_LabelTobaccoName = value; }
    }
    [DataMember]
    public string LabelGrade
    {
      get { return b_LabelGrade; }
      set { b_LabelGrade = value; }
    }
    [DataMember]
    public string LabelSKU
    {
      get { return b_LabelSKU; }
      set { b_LabelSKU = value; }
    }
    [DataMember]
    public string LabelBatch
    {
      get { return b_LabelBatch; }
      set { b_LabelBatch = value; }
    }
    [DataMember]
    public double LabelSettledNetMass
    {
      get { return b_LabelSettledNetMass; }
      set { b_LabelSettledNetMass = value; }
    }
    [DataMember]
    public double PackageToClear
    {
      get { return b_PackageToClear; }
      set { b_PackageToClear = value; }
    }
    [DataMember]
    public string SAD
    {
      get { return b_LabelSAD; }
      set { b_LabelSAD = value; }
    }
    [DataMember]
    public string RemainingQuantity
    {
      get { return b_LabelRemainingQuantity; }
      set { b_LabelRemainingQuantity = value; }
    }
    [DataMember]
    public string RemainingPackage
    {
      get { return b_LabelRemainingPackage; }
      set { b_LabelRemainingPackage = value; }
    }
    [DataMember]
    public int PackageQuantity
    {
      get { return b_LablePackageQuantity; }
      set { b_LablePackageQuantity = value; }
    }
    [DataMember]
    public string WarehouseName
    {
      get { return b_LableWarehouseName; }
      set { b_LableWarehouseName = value; }
    }
    #endregion

    #region private backed fields.
    private string b_LabelOGLNumber;
    private DateTime b_LabelZDdnia;
    private DateTime b_LabelOGLDate;
    private string b_LabelTobaccoName;
    private string b_LabelGrade;
    private string b_LabelSKU;
    private string b_LabelBatch;
    private double b_LabelSettledNetMass;
    private double b_PackageToClear;
    private string b_LabelSAD;
    private string b_LabelRemainingQuantity;
    private string b_LabelRemainingPackage;
    private int b_LablePackageQuantity;
    private string b_LableWarehouseName;
    #endregion

  }
}
