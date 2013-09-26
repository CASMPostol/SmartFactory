using System;
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
  public class DisposalRequest: Element
  {
    private double? _DeclaredNetMass;
    private string _sKU;
    private string _sKUDescription;
    private double? _totalStock;
    private double? _remainingPackages;
    private double? _packagesToClear;
    private double? _addedKg;
    private string _Batch;
    private double? _massPerPackage;
    public String SKU
    {
      get
      {
        return this._sKU;
      }
      set
      {
        if ((value != this._sKU))
        {
          this.OnPropertyChanging("SKU", this._sKU);
          this._sKU = value;
          this.OnPropertyChanged("SKU");
        }
      }
    }
    public String SKUDescription
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
    public String Batch
    {
      get
      {
        return this._Batch;
      }
      set
      {
        if ((value != this._Batch))
        {
          this.OnPropertyChanging("Batch", this._Batch);
          this._Batch = value;
          this.OnPropertyChanged("Batch");
        }
      }
    }
    public System.Nullable<double> TotalStock
    {
      get
      {
        return this._totalStock;
      }
      set
      {
        if ((value != this._totalStock))
        {
          this.OnPropertyChanging("TotalStock", this._totalStock);
          this._totalStock = value;
          this.OnPropertyChanged("TotalStock");
        }
      }
    }
    public System.Nullable<double> MassPerPackage
    {
      get
      {
        return this._massPerPackage;
      }
      set
      {
        if ((value != this._massPerPackage))
        {
          this.OnPropertyChanging("MassPerPackage", this._massPerPackage);
          this._massPerPackage = value;
          this.OnPropertyChanged("MassPerPackage");
        }
      }
    }

    public System.Nullable<double> DeclaredNetMass
    {
      get
      {
        return this._DeclaredNetMass;
      }
      set
      {
        if ((value != this._DeclaredNetMass))
        {
          this.OnPropertyChanging("CW_SettledNetMass", this._DeclaredNetMass);
          this._DeclaredNetMass = value;
          this.OnPropertyChanged("CW_SettledNetMass");
        }
      }
    }
    public System.Nullable<double> AddedKg
    {
      get
      {
        return this._addedKg;
      }
      set
      {
        if ((value != this._addedKg))
        {
          this.OnPropertyChanging("CW_AddedKg", this._addedKg);
          this._addedKg = value;
          this.OnPropertyChanged("CW_AddedKg");
        }
      }
    }
    //TODO
    public System.Nullable<double> RemainingPackages
    {
      get
      {
        return this._remainingPackages;
      }
      set
      {
        if ((value != this._remainingPackages))
        {
          this.OnPropertyChanging("RemainingPackages", this._remainingPackages);
          this._remainingPackages = value;
          this.OnPropertyChanged("RemainingPackages");
        }
      }
    }
    public System.Nullable<double> PackagesToClear
    {
      get
      {
        return this._packagesToClear;
      }
      set
      {
        if ((value != this._packagesToClear))
        {
          this.OnPropertyChanging("PackagesToClear", this._packagesToClear);
          this._packagesToClear = value;
          this.OnPropertyChanged("PackagesToClear");
        }
      }
    }
  }
}
