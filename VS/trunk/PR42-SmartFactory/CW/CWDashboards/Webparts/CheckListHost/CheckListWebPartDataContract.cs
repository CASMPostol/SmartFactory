//<summary>
//  Title   : Name of Application
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

namespace CAS.SmartFactory.CW.Dashboards.Webparts.CheckListHost
{
  /// <summary>
  /// class CheckListWebPartDataContract
  /// </summary>
  [DataContract]
  public partial class CheckListWebPartDataContract
  {
    public CheckListWebPartDataContract()
    {
      Today = DateTime.Today;
      DisposalsList = new DisposalDescription[] 
      { new DisposalDescription() { OGLDate = DateTime.Today, OGLNumber = "N/A", PackageToClear = 0 }, 
        new DisposalDescription() { OGLDate = DateTime.Today, OGLNumber = "N/A", PackageToClear = 1 } };
    }
    #region public
    [DataMember]
    public DateTime Today
    {
      get { return b_DocumentDate; }
      set { b_DocumentDate = value; }
    }
    [DataMember]
    public DisposalDescription[] DisposalsList
    {
      get { return b_DisposalsList; }
      set { b_DisposalsList = value; }
    }
    #endregion

    #region private
    private DateTime b_DocumentDate;
    private DisposalDescription[] b_DisposalsList;
    #endregion

  }
  /// <summary>
  /// class DisposalDescription
  /// </summary>
  [DataContract]
  public class DisposalDescription
  {
    #region public
    public DateTime OGLDate
    {
      get { return b_OGLDate; }
      set { b_OGLDate = value; }
    }
    public string OGLNumber
    {
      get { return b_OGLNumber; }
      set { b_OGLNumber = value; }
    }
    public int PackageToClear
    {
      get { return b_PackageToClear; }
      set { b_PackageToClear = value; }
    }
    #endregion

    #region private
    private DateTime b_OGLDate;
    private string b_OGLNumber;
    private int b_PackageToClear;
    #endregion
  }

}
