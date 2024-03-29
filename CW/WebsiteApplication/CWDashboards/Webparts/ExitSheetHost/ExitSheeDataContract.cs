﻿//<summary>
//  Title   : class MainPageData
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
using System.Runtime.Serialization;

namespace CAS.SmartFactory.CW.Dashboards.Webparts.ExitSheetHost
{
  /// <summary>
  /// class MainPageData
  /// </summary>
  [DataContract]
  public partial class ExitSheeDataContract
  {

    #region public properties
    /// <summary>
    /// Gets or sets the label ogl number.
    /// </summary>
    /// <value>
    /// The label ogl number.
    /// </value>
    [DataMember]
    public string OGLNumber
    {
      get { return b_LabelOGLNumber; }
      set { b_LabelOGLNumber = value; }
    }
    /// <summary>
    /// Gets or sets the grade.
    /// </summary>
    /// <value>
    /// The grade.
    /// </value>
    [DataMember]
    public string Grade
    {
      get { return b_LabelGrade; }
      set { b_LabelGrade = value; }
    }
    /// <summary>
    /// Gets or sets the label settled net mass.
    /// </summary>
    /// <value>
    /// The label settled net mass.
    /// </value>
    [DataMember]
    public double SettledNetMass
    {
      get { return b_LabelSettledNetMass; }
      set { b_LabelSettledNetMass = value; }
    }
    /// <summary>
    /// Gets or sets the package automatic clear.
    /// </summary>
    /// <value>
    /// The package automatic clear.
    /// </value>
    [DataMember]
    public int PackageToClear
    {
      get { return b_PackageToClear; }
      set { b_PackageToClear = value; }
    }
    /// <summary>
    /// Gets or sets the sad.
    /// </summary>
    /// <value>
    /// The sad.
    /// </value>
    [DataMember]
    public string SAD
    {
      get { return b_LabelSAD; }
      set { b_LabelSAD = value; }
    }
    /// <summary>
    /// Gets or sets the remaining quantity.
    /// </summary>
    /// <value>
    /// The remaining quantity.
    /// </value>
    [DataMember]
    public double RemainingQuantity
    {
      get { return b_LabelRemainingQuantity; }
      set { b_LabelRemainingQuantity = value; }
    }
    /// <summary>
    /// Gets or sets the remaining package.
    /// </summary>
    /// <value>
    /// The remaining package.
    /// </value>
    [DataMember]
    public int RemainingPackage
    {
      get { return b_LabelRemainingPackage; }
      set { b_LabelRemainingPackage = value; }
    }
    /// <summary>
    /// Gets or sets the name of the warehouse.
    /// </summary>
    /// <value>
    /// The name of the warehouse.
    /// </value>
    [DataMember]
    public string WarehouseName
    {
      get { return b_LableWarehouseName; }
      set { b_LableWarehouseName = value; }
    }
    #endregion

    #region private backed fields.
    private string b_LabelOGLNumber;
    private string b_LabelGrade;
    private double b_LabelSettledNetMass;
    private int b_PackageToClear;
    private string b_LabelSAD;
    private double b_LabelRemainingQuantity;
    private int b_LabelRemainingPackage;
    private string b_LableWarehouseName;
    #endregion

  }
}
