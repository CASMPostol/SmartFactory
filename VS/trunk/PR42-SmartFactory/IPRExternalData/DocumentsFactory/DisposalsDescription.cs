﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml.DocumentsFactory
{
  /// <uwagi/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute( "xsd", "2.0.50727.3038" )]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute( "code" )]
  [System.Xml.Serialization.XmlTypeAttribute( Namespace = "http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory" )]
  public partial class MaterialsOnOneAccount
  {

    private double totalField;

    private MaterialRecord[] dustRecordField;

    /// <uwagi/>
    public double Total
    {
      get
      {
        return this.totalField;
      }
      set
      {
        this.totalField = value;
      }
    }

    /// <uwagi/>
    public MaterialRecord[] DustRecord
    {
      get
      {
        return this.dustRecordField;
      }
      set
      {
        this.dustRecordField = value;
      }
    }
  }

  /// <uwagi/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute( "xsd", "2.0.50727.3038" )]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute( "code" )]
  [System.Xml.Serialization.XmlTypeAttribute( Namespace = "http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory" )]
  public partial class MaterialRecord
  {

    private string customDocumentNoField;

    private System.DateTime dateField;

    private string materialSKUField;

    private string materialBatchField;

    private string finishedGoodBatchField;

    private double amountField;

    /// <uwagi/>
    public string CustomDocumentNo
    {
      get
      {
        return this.customDocumentNoField;
      }
      set
      {
        this.customDocumentNoField = value;
      }
    }

    /// <uwagi/>
    public System.DateTime Date
    {
      get
      {
        return this.dateField;
      }
      set
      {
        this.dateField = value;
      }
    }

    /// <uwagi/>
    public string MaterialSKU
    {
      get
      {
        return this.materialSKUField;
      }
      set
      {
        this.materialSKUField = value;
      }
    }

    /// <uwagi/>
    public string MaterialBatch
    {
      get
      {
        return this.materialBatchField;
      }
      set
      {
        this.materialBatchField = value;
      }
    }

    /// <uwagi/>
    public string FinishedGoodBatch
    {
      get
      {
        return this.finishedGoodBatchField;
      }
      set
      {
        this.finishedGoodBatchField = value;
      }
    }

    /// <uwagi/>
    public double Amount
    {
      get
      {
        return this.amountField;
      }
      set
      {
        this.amountField = value;
      }
    }
  }
}
