﻿//<summary>
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
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data;

namespace CAS.SmartFactory.CW.Dashboards.ExitSheetWebPart
{
  /// <summary>
  /// class MainPageData
  /// </summary>
  internal class MainPageData : INotifyPropertyChanged, IDisposable
  {

    #region ctor
    public MainPageData()
    {

    }
    #endregion
    
    #region public properties
    private string b_LabelOGLNumber;

    public string LabelOGLNumber
    {
      get { return b_LabelOGLNumber; }
      set { b_LabelOGLNumber = value; }
    }

    private DateTime b_LabelZDdnia;

    public DateTime LabelZDdnia
    {
      get { return b_LabelZDdnia; }
      set { b_LabelZDdnia = value; }
    }

    private DateTime b_LabelOGLDate;

    public DateTime LabelOGLDate
    {
      get { return b_LabelOGLDate; }
      set { b_LabelOGLDate = value; }
    }
    private string b_LabelTobaccoName;

    public string LabelTobaccoName
    {
      get { return b_LabelTobaccoName; }
      set { b_LabelTobaccoName = value; }
    }
    private string b_LabelGrade;

    public string LabelGrade
    {
      get { return b_LabelGrade; }
      set { b_LabelGrade = value; }
    }

    private string b_LabelSKU;

    public string LabelSKU
    {
      get { return b_LabelSKU; }
      set { b_LabelSKU = value; }
    }

    private string b_LabelBatch;

    public string LabelBatch
    {
      get { return b_LabelBatch; }
      set { b_LabelBatch = value; }
    }

    private double b_LabelSettledNetMass;

    public double LabelSettledNetMass
    {
      get { return b_LabelSettledNetMass; }
      set { b_LabelSettledNetMass = value; }
    }

    private double b_PackageToClear;

    public double PackageToClear
    {
      get { return b_PackageToClear; }
      set { b_PackageToClear = value; }
    }

    private string b_LabelSAD;

    public string SAD
    {
      get { return b_LabelSAD; }
      set { b_LabelSAD = value; }
    }

    private string b_LabelRemainingQuantity;

    public string RemainingQuantity
    {
      get { return b_LabelRemainingQuantity; }
      set { b_LabelRemainingQuantity = value; }
    }
    private string b_LabelRemainingPackage;

    public string RemainingPackage
    {
      get { return b_LabelRemainingPackage; }
      set { b_LabelRemainingPackage = value; }
    }
    private int b_LablePackageQuantity;

    public int PackageQuantity
    {
      get { return b_LablePackageQuantity; }
      set { b_LablePackageQuantity = value; }
    }

    private string b_LableWarehouseName;

    public string WarehouseName
    {
      get { return b_LableWarehouseName; }
      set { b_LableWarehouseName = value; }
    }
    #endregion    

    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion

    #region IDisposable Members

    public void Dispose()
    {
      if (m_Context != null)
        m_Context.Dispose();
      m_Disposed = true;
    }

    #endregion

    #region private

    #region vars
    private bool m_Disposed = false;
    private DataContextAsync m_Context = new DataContextAsync();
    #endregion

    #endregion
    /// <summary>
    /// Deserializes the specified serialized object.
    /// </summary>
    /// <param name="serializedObject">The serialized object.</param>
    public static MainPageData Deserialize(string serializedObject)
    {
      return JsonSerializer.Deserialize<CustomsOffice>(serializedObject);
    }

    //TODO
    internal void GetData(string m_URL, int? m_SelectedID)
    {
      throw new NotImplementedException();
    }
  }
}