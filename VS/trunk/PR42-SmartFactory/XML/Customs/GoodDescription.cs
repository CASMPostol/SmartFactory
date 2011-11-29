﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml.Customs
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
    public abstract double? GetTotalAmountInvoiced();
    public abstract double? GetCartonsInKg();
    public abstract double? GetItemNo();
    public abstract DutiesDescription[] GetSADDuties();
    public abstract PackageDescription[] GetSADPackage();
    public abstract QuantityDescription[] GetSADQuantity();
    public abstract RequiredDocumentsDescription[] GetSADRequiredDocuments();
  }
}
