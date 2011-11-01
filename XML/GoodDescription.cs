using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml
{
  public abstract class GoodDescription
  {
    public abstract string GetDescription();
    public abstract double? GetNetMass();
    public abstract string GetUnits();
    public abstract string GetPCNTariffCode();
    public abstract double? GetGrossMass();
    public abstract string GetProcedure();
    public abstract string GetPackage();
    public abstract string GetTotalAmountInvoiced();
    public abstract double? GetCartonsInKg();
    public abstract double? GetItemNo();
  }
}
