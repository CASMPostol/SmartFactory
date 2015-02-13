//<summary>
//  Title   : class ExportWebPartUserControl
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.SharePoint;
using CAS.SharePoint.Linq;
using CAS.SharePoint.Web;
using CAS.SmartFactory.IPR.Dashboards.Clearance;
using CAS.SmartFactory.IPR.WebsiteModel;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;

namespace CAS.SmartFactory.IPR.Dashboards.Webparts.ExportWebPart
{
  /// <summary>
  /// Export Web Part User Control
  /// </summary>
  public partial class ExportWebPartUserControl : UserControl
  {
    #region creator
    /// <summary>
    /// Initializes a new instance of the <see cref="ExportWebPartUserControl" /> class.
    /// </summary>
    public ExportWebPartUserControl()
    {
      m_StateMachineEngine = new LocalStateMachineEngine(this);
      m_DataContextManagement = new DataContextManagement<Entities>(this);
    }
    #endregion

    #region public
    internal void SetInterconnectionData(Dictionary<ConnectionSelector, IWebPartRow> m_ProvidersDictionary)
    {
      foreach (var _item in m_ProvidersDictionary)
      {
        switch (_item.Key)
        {
          case ConnectionSelector.BatchInterconnection:
            new BatchInterconnectionData().SetRowData(_item.Value, m_StateMachineEngine.NewDataEventHandler);
            break;
          case ConnectionSelector.InvoiceInterconnection:
            new InvoiceInterconnectionData().SetRowData(_item.Value, m_StateMachineEngine.NewDataEventHandler);
            break;
          case ConnectionSelector.InvoiceContentInterconnection:
            new InvoiceContentInterconnectionnData().SetRowData(_item.Value, m_StateMachineEngine.NewDataEventHandler);
            break;
          default:
            break;
        }
      }
    }
    #endregion

    #region UserControl override
    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnInit(EventArgs e)
    {
      Page.RegisterRequiresControlState(this);
      base.OnInit(e);
    }
    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
      string at = "Starting";
      try
      {
        if (!IsPostBack)
        {
          at = "InitMahine";
          m_StateMachineEngine.InitMahine();
        }
        at = "Event handlers";
        m_SaveButton.Click += new EventHandler(m_StateMachineEngine.SaveButton_Click);
        m_NewButton.Click += new EventHandler(m_StateMachineEngine.NewButton_Click);
        m_CancelButton.Click += new EventHandler(m_StateMachineEngine.CancelButton_Click);
        m_EditButton.Click += new EventHandler(m_StateMachineEngine.EditButton_Click);
        m_DeleteButton.Click += new EventHandler(m_StateMachineEngine.DeleteButton_Click);
        m_ExportButton.Click += new EventHandler(m_StateMachineEngine.m_ExportButton_Click);
      }
      catch (Exception ex)
      {
        ApplicationError _ae = new ApplicationError("Page_Load", "", ex.Message, ex);
        this.Controls.Add(_ae.CreateMessage(at, true));
      }
    }
    /// <summary>
    /// Loads the state of the control.
    /// </summary>
    /// <param name="state">The state.</param>
    protected override void LoadControlState(object state)
    {
      if (state != null)
      {
        m_ControlState = (ControlState)state;
        m_ControlState.EntityHolder = GetEntities;
        m_StateMachineEngine.InitMahine(m_ControlState.InterfaceState);
      }
      else
      {
        m_ControlState = new ControlState(null, GetEntities);
        m_StateMachineEngine.InitMahine();
      }
    }
    /// <summary>
    /// Saves any server control state changes that have occurred since the time the page was posted back to the server.
    /// </summary>
    /// <returns>
    /// Returns the server control's current state. If there is no state associated with the control, this method returns null.
    /// </returns>
    protected override object SaveControlState()
    {
      return m_ControlState;
    }
    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnPreRender(EventArgs e)
    {
      SetEnabled(m_ControlState.SetEnabled);
      Show(m_ControlState.Invoice, m_ControlState.InvoiceContent, Batch);
      base.OnPreRender(e);
    }
    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Unload"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains event data.</param>
    protected override void OnUnload(EventArgs e)
    {
      m_DataContextManagement.Dispose();
      base.OnUnload(e);
    }
    #endregion

    #region StateManagement
    [Serializable]
    private class ControlState
    {
      #region state fields
      public string InvoiceID = String.Empty;
      public bool ReadOnly;
      public string InvoiceContentID = String.Empty;
      public string BatchID = String.Empty;
      public GenericStateMachineEngine.InterfaceState InterfaceState = GenericStateMachineEngine.InterfaceState.ViewState;
      public GenericStateMachineEngine.ControlsSet SetEnabled = 0;
      #endregion

      #region public
      internal void ClearInvoiceContent()
      {
        Batch = null;
        InvoiceContent = null;
      }
      internal GetEntitiesDelegate EntityHolder { set { _edc = value; } }
      internal InvoiceContent InvoiceContent
      {
        get
        {
          if (p_InvoiceContent == null)
            p_InvoiceContent = Element.FindAtIndex<InvoiceContent>(_edc().InvoiceContent, InvoiceContentID);
          if ((p_InvoiceContent != null) && (p_InvoiceContent.InvoiceIndex != Invoice))
            p_InvoiceContent = Invoice == null ? null : Invoice.InvoiceContent(_edc()).FirstOrDefault();
          return p_InvoiceContent;
        }
        set
        {
          p_InvoiceContent = value;
          InvoiceContentID = value == null ? String.Empty : value.Id.ToString();
        }
      }
      internal InvoiceLib Invoice
      {
        get
        {
          if (p_InvoiceLib == null)
            p_InvoiceLib = Element.FindAtIndex<InvoiceLib>(_edc().InvoiceLibrary, InvoiceID);
          return p_InvoiceLib;
        }
        set
        {
          p_InvoiceLib = value;
          InvoiceID = value == null ? String.Empty : value.Id.ToString();
        }
      }
      internal Batch Batch
      {
        get
        {
          if (p_Batch == null)
            p_Batch = Element.FindAtIndex<Batch>(_edc().Batch, BatchID);
          return p_Batch;
        }
        set
        {
          p_Batch = value;
          BatchID = value == null ? String.Empty : value.Id.ToString();
        }
      }
      internal WebsiteModel.Linq.Batch InvoiceBatch
      {
        get
        {
          return InvoiceContent == null ? null : InvoiceContent.InvoiceContent2BatchIndex;
        }
      }
      internal ControlState(ControlState _old, GetEntitiesDelegate edc)
      {
        _edc = edc;
        if (_old == null)
          return;
        InterfaceState = _old.InterfaceState;
      }
      #endregion

      #region private
      [NonSerialized]
      private GetEntitiesDelegate _edc = null;
      [NonSerialized]
      private InvoiceContent p_InvoiceContent = null;
      [NonSerialized]
      private InvoiceLib p_InvoiceLib = null;
      [NonSerialized]
      private Batch p_Batch = null;
      #endregion


    } //ControlState
    private class LocalStateMachineEngine : WEB.WebpartStateMachineEngine
    {
      #region constructor
      public LocalStateMachineEngine(ExportWebPartUserControl parent)
      {
        Parent = parent;
      }
      #endregion

      #region public
      internal void InitMahine(InterfaceState _ControlState)
      {
        Parent.m_ControlState.InterfaceState = _ControlState;
      }
      internal void InitMahine()
      {
        Parent.InitControlState();
        EnterState();
      }
      #endregion

      #region NewDataEventHandlers
      internal void NewDataEventHandler(object sender, BatchInterconnectionData e)
      {
        switch (CurrentMachineState)
        {
          case InterfaceState.EditState:
          case InterfaceState.NewState:
            Parent.SetInterconnectionData(e);
            break;
          case InterfaceState.ViewState:
          default:
            break;
        }
      }
      internal void NewDataEventHandler(object sender, InvoiceInterconnectionData e)
      {
        switch (CurrentMachineState)
        {
          case InterfaceState.ViewState:
            Parent.SetInterconnectionData(e);
            break;
          case InterfaceState.EditState:
          case InterfaceState.NewState:
          default:
            break;
        }
      }
      internal void NewDataEventHandler(object sender, InvoiceContentInterconnectionnData e)
      {
        switch (CurrentMachineState)
        {
          case InterfaceState.ViewState:
            Parent.SetInterconnectionData(e);
            break;
          case InterfaceState.EditState:
          case InterfaceState.NewState:
          default:
            break;
        }
      }
      #endregion

      #region GenericStateMachineEngine implementation
      protected override GenericStateMachineEngine.ActionResult Update()
      {
        try
        {
          List<string> _errors = new List<string>();
          double? _nq = Parent.m_InvoiceQuantityTextBox.TextBox2Double(_errors);
          if (_errors.Count > 0)
            return ActionResult.NotValidated(_errors[0]);
          if (_nq.HasValue && _nq.Value < 0)
            return ActionResult.NotValidated(String.Format(Resources.NegativeValueNotAllowed.GetLocalizedString(), Parent.m_InvoiceQuantityLabel.Text));
          Batch _batch = Parent.Batch;
          if (!_batch.Available(_nq.Value))
          {
            string _tmplt = Resources.NeBatchQuantityIsUnavailable.GetLocalizedString();
            return ActionResult.NotValidated(String.Format(CultureInfo.CurrentCulture, _tmplt, _batch.AvailableQuantity()));
          }
          InvoiceContent _ic = Parent.m_ControlState.InvoiceContent;
          _ic.Quantity = _nq;
          _ic.InvoiceContentStatus = InvoiceContentStatus.OK;
          if (_ic.InvoiceContent2BatchIndex != _batch)
          {
            _ic.InvoiceContent2BatchIndex = _batch;
            _ic.ProductType = _ic.InvoiceContent2BatchIndex.ProductType;
            _ic.Units = _ic.InvoiceContent2BatchIndex.ProductType.Value.Units();
            _ic.Quantity = _nq;
            _ic.CreateTitle();
          }
          Parent.m_DataContextManagement.DataContext.SubmitChanges();
        }
        catch (Exception ex)
        {
          return GenericStateMachineEngine.ActionResult.Exception(ex, "Update");
        }
        return GenericStateMachineEngine.ActionResult.Success;
      }
      protected override GenericStateMachineEngine.ActionResult Create()
      {
        try
        {
          List<string> _errors = new List<string>();
          double? _nq = Parent.m_InvoiceQuantityTextBox.TextBox2Double(_errors);
          if (_errors.Count > 0)
            return ActionResult.NotValidated(_errors[0]);
          if (_nq.HasValue && _nq.Value < 0)
            return ActionResult.NotValidated
              (String.Format(Resources.NegativeValueNotAllowed.GetLocalizedString(), Parent.m_InvoiceQuantityLabel.Text));
          Batch _batch = Parent.m_ControlState.Batch;
          InvoiceLib _invc = Parent.m_ControlState.Invoice;
          if (!_batch.Available(_nq.Value))
          {
            string _tmplt = Resources.QuantityIsUnavailable.GetLocalizedString();
            return ActionResult.NotValidated(String.Format(CultureInfo.CurrentCulture, _tmplt, _batch.AvailableQuantity()));
          }
          InvoiceContent _nic = new InvoiceContent()
          {
            InvoiceContent2BatchIndex = _batch,
            InvoiceIndex = _invc,
            SKUDescription = _batch.SKUIndex.Title,
            ProductType = _batch.ProductType,
            Quantity = _nq.Value,
            InvoiceContentStatus = InvoiceContentStatus.OK,
            Title = _batch.SKUIndex.Title,
            Units = _batch.ProductType.Value.Units()
          };
          Parent.m_DataContextManagement.DataContext.InvoiceContent.InsertOnSubmit(_nic);
          Parent.m_DataContextManagement.DataContext.SubmitChanges();
          _nic.CreateTitle();
          Parent.m_DataContextManagement.DataContext.SubmitChanges();
        }
        catch (Exception ex)
        {
          return GenericStateMachineEngine.ActionResult.Exception(ex, "Create");
        }
        return GenericStateMachineEngine.ActionResult.Success;
      }
      protected override GenericStateMachineEngine.ActionResult Delete()
      {
        try
        {
          InvoiceContent _invc = Parent.m_ControlState.InvoiceContent;
          Parent.m_ControlState.ClearInvoiceContent();
          Parent.m_DataContextManagement.DataContext.InvoiceContent.DeleteOnSubmit(_invc);
          Parent.m_DataContextManagement.DataContext.SubmitChanges();
        }
        catch (Exception ex)
        {
          return GenericStateMachineEngine.ActionResult.Exception(ex, "Delete");
        }
        return GenericStateMachineEngine.ActionResult.Success;
      }
      protected override void ClearUserInterface()
      {
        Parent.m_ControlState.ClearInvoiceContent();
      }
      protected override void SetEnabled(GenericStateMachineEngine.ControlsSet _buttons)
      {
        Parent.m_ControlState.SetEnabled = _buttons;
      }
      protected override void SMError(GenericStateMachineEngine.InterfaceEvent interfaceEvent)
      {
        ShowActionResult
          (ActionResult.Exception(new ApplicationError("SMError", CurrentMachineState.ToString(), "State machine internal error", null), "State machine internal error"));
        CurrentMachineState = InterfaceState.ViewState;
      }
      protected override void ShowActionResult(GenericStateMachineEngine.ActionResult _rslt)
      {
        Parent.Controls.Add(ControlExtensions.CreateMessage(_rslt.ActionException.Message));
        base.ShowActionResult(_rslt);
      }
      protected override GenericStateMachineEngine.InterfaceState CurrentMachineState
      {
        get
        {
          return Parent.m_ControlState.InterfaceState;
        }
        set
        {
          if (Parent.m_ControlState.InterfaceState == value)
            return;
          Parent.m_ControlState.InterfaceState = value;
          EnterState();
        }
      }
      protected override void EnterState()
      {
        switch (CurrentMachineState)
        {
          case InterfaceState.ViewState:
          case InterfaceState.EditState:
            Parent.m_EditBatchCheckBox.Checked = false;
            break;
          case InterfaceState.NewState:
            Parent.m_EditBatchCheckBox.Checked = true;
            break;
        }
        base.EnterState();
      }
      #endregion

      #region Event handlers
      internal void m_ExportButton_Click(object sender, EventArgs e)
      {
        switch (CurrentMachineState)
        {
          case InterfaceState.ViewState:
            ActionResult _resu = Parent.ClearThroughCustom();
            switch (_resu.LastActionResult)
            {
              case ActionResult.Result.Success:
                break;
              case ActionResult.Result.NotValidated:
                CurrentMachineState = InterfaceState.EditState;
                ShowActionResult(_resu);
                break;
              case ActionResult.Result.Exception:
              default:
                ShowActionResult(_resu);
                break;
            }
            break;
          case InterfaceState.EditState:
          case InterfaceState.NewState:
          default:
            SMError(InterfaceEvent.NewClick);
            break;
        }
      }
      #endregion

      #region private
      private ExportWebPartUserControl Parent { get; set; }
      #endregion

    } //LocalStateMachineEngine
    #endregion

    #region SetInterconnectionData
    private void SetInterconnectionData(BatchInterconnectionData e)
    {
      m_ControlState.BatchID = e.ID;
      m_BatchTextBox.Text = e.Title;
    }
    private void SetInterconnectionData(InvoiceInterconnectionData e)
    {
      try
      {
        m_ControlState.InvoiceID = e.ID;
        m_ControlState.ReadOnly = e.ReadOnly;
      }
      catch (Exception _ex)
      {
        ApplicationError _errr = new ApplicationError("SetInterconnectionData", "InvoiceInterconnectionData", _ex.Message, _ex);
        this.Controls.Add(_errr.CreateMessage(_errr.At, true));
      }
    }
    private void SetInterconnectionData(InvoiceContentInterconnectionnData e)
    {
      try
      {
        m_ControlState.InvoiceContentID = e.ID;
      }
      catch (Exception _ex)
      {
        ApplicationError _errr = new ApplicationError("SetInterconnectionData", "InvoiceContentInterconnectionnData", _ex.Message, _ex);
        this.Controls.Add(_errr.CreateMessage(_errr.At, true));
      }
    }

    #endregion

    #region private
    private delegate Entities GetEntitiesDelegate();
    private DataContextManagement<Entities> m_DataContextManagement = null;
    private LocalStateMachineEngine m_StateMachineEngine = null;
    private ControlState m_ControlState = null;
    private Entities GetEntities()
    {
      return m_DataContextManagement.DataContext;
    }
    private void SetEnabled(GenericStateMachineEngine.ControlsSet _set)
    {
      GenericStateMachineEngine.ControlsSet _allowed = m_ControlState.ReadOnly ? 0 : (GenericStateMachineEngine.ControlsSet)0xff;
      if (m_ControlState.Invoice == null)
        _allowed ^= GenericStateMachineEngine.ControlsSet.NewOn;
      if (m_ControlState.InvoiceContent == null)
        _allowed ^= GenericStateMachineEngine.ControlsSet.EditOn | GenericStateMachineEngine.ControlsSet.DeleteOn;
      _set &= _allowed;
      //Buttons
      m_EditButton.Enabled = (_set & GenericStateMachineEngine.ControlsSet.EditOn) != 0;
      m_CancelButton.Enabled = (_set & GenericStateMachineEngine.ControlsSet.CancelOn) != 0;
      m_NewButton.Enabled = (_set & GenericStateMachineEngine.ControlsSet.NewOn) != 0;
      m_SaveButton.Enabled = (_set & GenericStateMachineEngine.ControlsSet.SaveOn) != 0;
      m_DeleteButton.Enabled = (_set & GenericStateMachineEngine.ControlsSet.DeleteOn) != 0;
      //Local controls
      m_EditBatchCheckBox.Enabled = m_CancelButton.Enabled;
      m_InvoiceQuantityTextBox.Enabled = m_CancelButton.Enabled;
      m_ExportButton.Enabled = m_NewButton.Enabled && (m_ControlState.Invoice != null) && !m_ControlState.ReadOnly;
    }
    private GenericStateMachineEngine.ActionResult ClearThroughCustom()
    {
      try
      {
        if (m_ExportProcedureRadioButtonList.SelectedIndex < 0)
          return GenericStateMachineEngine.ActionResult.NotValidated("CustomsProcedureMustBeSelected".GetLocalizedString());
        switch (m_ExportProcedureRadioButtonList.SelectedValue)
        {
          case "Export":
            return Export();
          case "Revert":
            return GenericStateMachineEngine.ActionResult.Exception(new NotImplementedException("RevertFGTofreeCirculationIsNotImplementedYet".GetLocalizedString()), "ClearThroughCustom");
        }
      }
      catch (Exception ex)
      {
        return GenericStateMachineEngine.ActionResult.Exception(ex, "ClearThroughCustom");
      }
      return GenericStateMachineEngine.ActionResult.Success;
    }
    private GenericStateMachineEngine.ActionResult Export()
    {
      foreach (InvoiceContent item in m_ControlState.Invoice.InvoiceContent(m_DataContextManagement.DataContext))
      {
        //TODO ExportIsPossible - improve for many invoice content with the same FG batch groups by batch must be checked against possible export
        string _checkResult = item.ExportIsPossible(m_DataContextManagement.DataContext, item.Quantity);
        if (_checkResult.IsNullOrEmpty())
          continue;
        Controls.Add(ControlExtensions.CreateMessage(_checkResult));
        m_ControlState.InvoiceContent = item;
        string _frmt = "CannotProceedWithExportBecauseTheInvoiceItemContainsErrors".GetLocalizedString();
        return GenericStateMachineEngine.ActionResult.NotValidated(String.Format(_frmt, item.Title));
      }
      m_ControlState.Invoice.InvoiceLibraryStatus = true;
      Clearence _newClearance = Clearence.CreataClearence(m_DataContextManagement.DataContext, "FinishedGoodsExport", ClearenceProcedure._3151);
      string _masterDocumentName = _newClearance.FinishedGoodsExportFormFileName(m_DataContextManagement.DataContext);
      CigaretteExportFormCollection _cefc = FinishedGoodsFormFactory.GetFormContent
        (m_DataContextManagement.DataContext, m_ControlState.Invoice, _newClearance, _masterDocumentName, _newClearance.SADDocumentNumber, (x, y, z) => TraceEvent(x, y, z));
      int _sadConsignmentIdentifier = SPDocumentFactory.Prepare(SPContext.Current.Web, _cefc, _masterDocumentName);
      SADConsignment _sadConsignment = Element.GetAtIndex<SADConsignment>(m_DataContextManagement.DataContext.SADConsignment, _sadConsignmentIdentifier);
      _newClearance.SADConsignmentLibraryIndex = _sadConsignment;
      m_DataContextManagement.DataContext.SubmitChanges();
      return GenericStateMachineEngine.ActionResult.Success;
    }
    private void Show(InvoiceLib invoice, InvoiceContent invoiceContent, Batch batch)
    {
      m_InvoiceTextBox.Text = invoice == null ? String.Empty.NotAvailable() : invoice.Title;
      if (invoiceContent != null)
      {
        m_InvoiceContentTextBox.Text = invoiceContent.Title;
        m_InvoiceQuantityTextBox.Text = m_ControlState.InvoiceContent.Quantity.Value.ToString(CultureInfo.CurrentCulture);
      }
      else
      {
        m_InvoiceContentTextBox.Text = String.Empty.NotAvailable();
        m_InvoiceQuantityTextBox.Text = String.Empty;
      }
      m_BatchTextBox.Text = batch == null ? String.Empty.NotAvailable() : batch.Title;
    }
    private Batch Batch
    {
      get
      {
        return m_EditBatchCheckBox.Checked ? m_ControlState.Batch : m_ControlState.InvoiceBatch;
      }
    }
    private void InitControlState()
    {
      m_ControlState = new ControlState(null, GetEntities);
      m_ControlState.InterfaceState = GenericStateMachineEngine.InterfaceState.ViewState;
    }
    private static void TraceEvent(string message, int eventId, TraceSeverity severity)
    {
      WebsiteModelExtensions.TraceEvent(message, eventId, severity, WebsiteModelExtensions.LoggingCategories.Export);
    }
    #endregion

  }
}
