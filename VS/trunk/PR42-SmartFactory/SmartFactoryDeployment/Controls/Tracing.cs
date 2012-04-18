using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CAS.Lib.RTLib.Processes;

namespace CAS.SmartFactory.Deployment.Controls
{
  /// <summary>
  /// The class provides the tracing functionality. It is singleton.
  /// </summary>
  public partial class Tracing : Component
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Tracing"/> class.
    /// </summary>
    public Tracing()
    {
      InitializeComponent();
      if (TraceEvent == null)
        TraceEvent = new TraceEvent("SharePoint.Deployment");
    }
    internal static TraceEvent TraceEvent { get; private set; }
    internal TraceEvent Trace { get { return Tracing.TraceEvent; } }
    /// <summary>
    /// Initializes a new instance of the <see cref="Tracing"/> class.
    /// </summary>
    /// <param name="container">The container.</param>
    public Tracing(IContainer container)
      : this()
    {
      container.Add(this);
    }
    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
        if (TraceEvent != null)
          TraceEvent.TraceEventClose();
        TraceEvent = null;
      }
      base.Dispose(disposing);
    }
  }
}
