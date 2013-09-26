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

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data.Entities
{
  internal partial class CustomsWarehouseDisposalRowData : Element, INotifyPropertyChanged, IEditableObject
  {

    private System.Nullable<double> _no;
    private string _sADDocumentNo;
    private System.Nullable<System.DateTime> _sADDate;
    private string _sKUDescription;
    private System.Nullable<double> _cW_DeclaredNetMass;
    private string _cW_Wz1;
    private string _cW_Wz2;
    private string _cW_Wz3;
    private System.Nullable<double> _cW_SettledNetMass;
    private System.Nullable<double> _cW_SettledGrossMass;
    private System.Nullable<double> _tobaccoValue;
    private System.Nullable<double> _remainingQuantity;
    private System.Nullable<double> _cW_RemainingPackage;
    private System.Nullable<double> _cW_RemainingTobaccoValue;
    private string _customsProcedure;
    private System.Nullable<double> _dutyPerSettledAmount;
    private System.Nullable<double> _vATPerSettledAmount;
    private System.Nullable<double> _dutyAndVAT;
    private System.Nullable<double> _cW_PackageToClear;
    private System.Nullable<double> _cW_AddedKg;
    private System.Nullable<bool> _accountClosed;
    private System.Nullable<bool> _archival;

    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate();
    partial void OnCreated();
    #endregion
    public CustomsWarehouseDisposalRowData()
    {
      this.OnCreated();
    }

    [CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data.ColumnAttribute(Name = "SADDocumentNo", Storage = "_sADDocumentNo", FieldType = "Text")]
    public string SADDocumentNo
    {
      get
      {
        return this._sADDocumentNo;
      }
      set
      {
        if ((value != this._sADDocumentNo))
        {
          this.OnPropertyChanging("SADDocumentNo", this._sADDocumentNo);
          this._sADDocumentNo = value;
          this.OnPropertyChanged("SADDocumentNo");
        }
      }
    }

    [CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data.ColumnAttribute(Name = "SADDate", Storage = "_sADDate", FieldType = "DateTime")]
    public System.Nullable<System.DateTime> SADDate
    {
      get
      {
        return this._sADDate;
      }
      set
      {
        if ((value != this._sADDate))
        {
          this.OnPropertyChanging("SADDate", this._sADDate);
          this._sADDate = value;
          this.OnPropertyChanged("SADDate");
        }
      }
    }

    [CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data.ColumnAttribute(Name = "SKUDescription", Storage = "_sKUDescription", FieldType = "Text")]
    public string SKUDescription
    {
      get
      {
        return this._sKUDescription;
      }
      set
      {
        if ((value != this._sKUDescription))
        {
          this.OnPropertyChanging("SKUDescription", this._sKUDescription);
          this._sKUDescription = value;
          this.OnPropertyChanged("SKUDescription");
        }
      }
    }

    [CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data.ColumnAttribute(Name = "CW_DeclaredNetMass", Storage = "_cW_DeclaredNetMass", FieldType = "Number")]
    public System.Nullable<double> CW_DeclaredNetMass
    {
      get
      {
        return this._cW_DeclaredNetMass;
      }
      set
      {
        if ((value != this._cW_DeclaredNetMass))
        {
          this.OnPropertyChanging("CW_DeclaredNetMass", this._cW_DeclaredNetMass);
          this._cW_DeclaredNetMass = value;
          this.OnPropertyChanged("CW_DeclaredNetMass");
        }
      }
    }

    [CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data.ColumnAttribute(Name = "CW_Wz1", Storage = "_cW_Wz1", FieldType = "Text")]
    public string CW_Wz1
    {
      get
      {
        return this._cW_Wz1;
      }
      set
      {
        if ((value != this._cW_Wz1))
        {
          this.OnPropertyChanging("CW_Wz1", this._cW_Wz1);
          this._cW_Wz1 = value;
          this.OnPropertyChanged("CW_Wz1");
        }
      }
    }

    [CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data.ColumnAttribute(Name = "CW_Wz2", Storage = "_cW_Wz2", FieldType = "Text")]
    public string CW_Wz2
    {
      get
      {
        return this._cW_Wz2;
      }
      set
      {
        if ((value != this._cW_Wz2))
        {
          this.OnPropertyChanging("CW_Wz2", this._cW_Wz2);
          this._cW_Wz2 = value;
          this.OnPropertyChanged("CW_Wz2");
        }
      }
    }

    [CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data.ColumnAttribute(Name = "CW_Wz3", Storage = "_cW_Wz3", FieldType = "Text")]
    public string CW_Wz3
    {
      get
      {
        return this._cW_Wz3;
      }
      set
      {
        if ((value != this._cW_Wz3))
        {
          this.OnPropertyChanging("CW_Wz3", this._cW_Wz3);
          this._cW_Wz3 = value;
          this.OnPropertyChanged("CW_Wz3");
        }
      }
    }

    [CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data.ColumnAttribute(Name = "CW_SettledNetMass", Storage = "_cW_SettledNetMass", FieldType = "Number")]
    public System.Nullable<double> CW_SettledNetMass
    {
      get
      {
        return this._cW_SettledNetMass;
      }
      set
      {
        if ((value != this._cW_SettledNetMass))
        {
          this.OnPropertyChanging("CW_SettledNetMass", this._cW_SettledNetMass);
          this._cW_SettledNetMass = value;
          this.OnPropertyChanged("CW_SettledNetMass");
        }
      }
    }

    [CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data.ColumnAttribute(Name = "CW_SettledGrossMass", Storage = "_cW_SettledGrossMass", FieldType = "Number")]
    public System.Nullable<double> CW_SettledGrossMass
    {
      get
      {
        return this._cW_SettledGrossMass;
      }
      set
      {
        if ((value != this._cW_SettledGrossMass))
        {
          this.OnPropertyChanging("CW_SettledGrossMass", this._cW_SettledGrossMass);
          this._cW_SettledGrossMass = value;
          this.OnPropertyChanged("CW_SettledGrossMass");
        }
      }
    }

    [CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data.ColumnAttribute(Name = "TobaccoValue", Storage = "_tobaccoValue", FieldType = "Number")]
    public System.Nullable<double> TobaccoValue
    {
      get
      {
        return this._tobaccoValue;
      }
      set
      {
        if ((value != this._tobaccoValue))
        {
          this.OnPropertyChanging("TobaccoValue", this._tobaccoValue);
          this._tobaccoValue = value;
          this.OnPropertyChanged("TobaccoValue");
        }
      }
    }

    [CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data.ColumnAttribute(Name = "RemainingQuantity", Storage = "_remainingQuantity", FieldType = "Number")]
    public System.Nullable<double> RemainingQuantity
    {
      get
      {
        return this._remainingQuantity;
      }
      set
      {
        if ((value != this._remainingQuantity))
        {
          this.OnPropertyChanging("RemainingQuantity", this._remainingQuantity);
          this._remainingQuantity = value;
          this.OnPropertyChanged("RemainingQuantity");
        }
      }
    }

    [CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data.ColumnAttribute(Name = "CW_RemainingPackage", Storage = "_cW_RemainingPackage", FieldType = "Number")]
    public System.Nullable<double> CW_RemainingPackage
    {
      get
      {
        return this._cW_RemainingPackage;
      }
      set
      {
        if ((value != this._cW_RemainingPackage))
        {
          this.OnPropertyChanging("CW_RemainingPackage", this._cW_RemainingPackage);
          this._cW_RemainingPackage = value;
          this.OnPropertyChanged("CW_RemainingPackage");
        }
      }
    }

    [CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data.ColumnAttribute(Name = "CW_RemainingTobaccoValue", Storage = "_cW_RemainingTobaccoValue", FieldType = "Number")]
    public System.Nullable<double> CW_RemainingTobaccoValue
    {
      get
      {
        return this._cW_RemainingTobaccoValue;
      }
      set
      {
        if ((value != this._cW_RemainingTobaccoValue))
        {
          this.OnPropertyChanging("CW_RemainingTobaccoValue", this._cW_RemainingTobaccoValue);
          this._cW_RemainingTobaccoValue = value;
          this.OnPropertyChanged("CW_RemainingTobaccoValue");
        }
      }
    }

    [CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data.ColumnAttribute(Name = "CustomsProcedure", Storage = "_customsProcedure", FieldType = "Text")]
    public string CustomsProcedure
    {
      get
      {
        return this._customsProcedure;
      }
      set
      {
        if ((value != this._customsProcedure))
        {
          this.OnPropertyChanging("CustomsProcedure", this._customsProcedure);
          this._customsProcedure = value;
          this.OnPropertyChanged("CustomsProcedure");
        }
      }
    }

    [CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data.ColumnAttribute(Name = "DutyPerSettledAmount", Storage = "_dutyPerSettledAmount", FieldType = "Number")]
    public System.Nullable<double> DutyPerSettledAmount
    {
      get
      {
        return this._dutyPerSettledAmount;
      }
      set
      {
        if ((value != this._dutyPerSettledAmount))
        {
          this.OnPropertyChanging("DutyPerSettledAmount", this._dutyPerSettledAmount);
          this._dutyPerSettledAmount = value;
          this.OnPropertyChanged("DutyPerSettledAmount");
        }
      }
    }

    [CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data.ColumnAttribute(Name = "VATPerSettledAmount", Storage = "_vATPerSettledAmount", FieldType = "Number")]
    public System.Nullable<double> VATPerSettledAmount
    {
      get
      {
        return this._vATPerSettledAmount;
      }
      set
      {
        if ((value != this._vATPerSettledAmount))
        {
          this.OnPropertyChanging("VATPerSettledAmount", this._vATPerSettledAmount);
          this._vATPerSettledAmount = value;
          this.OnPropertyChanged("VATPerSettledAmount");
        }
      }
    }

    [CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data.ColumnAttribute(Name = "DutyAndVAT", Storage = "_dutyAndVAT", FieldType = "Number")]
    public System.Nullable<double> DutyAndVAT
    {
      get
      {
        return this._dutyAndVAT;
      }
      set
      {
        if ((value != this._dutyAndVAT))
        {
          this.OnPropertyChanging("DutyAndVAT", this._dutyAndVAT);
          this._dutyAndVAT = value;
          this.OnPropertyChanged("DutyAndVAT");
        }
      }
    }

    [CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data.ColumnAttribute(Name = "CW_PackageToClear", Storage = "_cW_PackageToClear", FieldType = "Number")]
    public System.Nullable<double> CW_PackageToClear
    {
      get
      {
        return this._cW_PackageToClear;
      }
      set
      {
        if ((value != this._cW_PackageToClear))
        {
          this.OnPropertyChanging("CW_PackageToClear", this._cW_PackageToClear);
          this._cW_PackageToClear = value;
          this.OnPropertyChanged("CW_PackageToClear");
        }
      }
    }

    [CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data.ColumnAttribute(Name = "CW_AddedKg", Storage = "_cW_AddedKg", FieldType = "Number")]
    public System.Nullable<double> CW_AddedKg
    {
      get
      {
        return this._cW_AddedKg;
      }
      set
      {
        if ((value != this._cW_AddedKg))
        {
          this.OnPropertyChanging("CW_AddedKg", this._cW_AddedKg);
          this._cW_AddedKg = value;
          this.OnPropertyChanged("CW_AddedKg");
        }
      }
    }

    [CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data.ColumnAttribute(Name = "AccountClosed", Storage = "_accountClosed", Required = true, FieldType = "Boolean")]
    public System.Nullable<bool> AccountClosed
    {
      get
      {
        return this._accountClosed;
      }
      set
      {
        if ((value != this._accountClosed))
        {
          this.OnPropertyChanging("AccountClosed", this._accountClosed);
          this._accountClosed = value;
          this.OnPropertyChanged("AccountClosed");
        }
      }
    }

    [CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data.ColumnAttribute(Name = "Archival", Storage = "_archival", FieldType = "Boolean")]
    public System.Nullable<bool> Archival
    {
      get
      {
        return this._archival;
      }
      set
      {
        if ((value != this._archival))
        {
          this.OnPropertyChanging("Archival", this._archival);
          this._archival = value;
          this.OnPropertyChanged("Archival");
        }
      }
    }

    [CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data.AssociationAttribute(Name = "CWL_CWDisposal2DisposalRequestLibraryID", Storage = "_cWL_CWDisposal2DisposalRequestLibraryID", MultivalueType = CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data.AssociationType.Single, List = "Disposal Request Library")]
    public int CWL_CWDisposal2DisposalRequestLibraryID
    {
      get;
      set;
    }

    [CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data.AssociationAttribute(Name = "CWL_CWDisposal2CustomsWarehouseID", Storage = "_cWL_CWDisposal2CustomsWarehouseID", MultivalueType = CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data.AssociationType.Single, List = "Customs Warehouse")]
    public int CWL_CWDisposal2CustomsWarehouseID
    {
      get;
      set;
    }

    #region INotifyPropertyChanged Members
    public event PropertyChangedEventHandler PropertyChanged;
    #endregion

    private bool m_Editing;
    private CustomsWarehouseDisposalRowData temp_Task;

    #region IEditableObject Members
    public void BeginEdit()
    {
      if (m_Editing == false)
      {
        temp_Task = this.MemberwiseClone() as CustomsWarehouseDisposalRowData;
        m_Editing = true;
      }
    }
    public void CancelEdit()
    {
      if (!m_Editing)
        return;
      m_Editing = false;
      throw new NotImplementedException();
    }
    public void EndEdit()
    {
      if (!m_Editing)
        return;
      temp_Task = null;
      m_Editing = false;
    }
    #endregion

    protected override void OnPropertyChanged(string propertyName)
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
      base.OnPropertyChanged(propertyName);
    }

  }
}
