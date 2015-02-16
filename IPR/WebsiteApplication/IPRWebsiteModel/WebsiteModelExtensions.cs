//<summary>
//  Title   : WebsiteModelExtensions
//  System  : Microsoft VisulaStudio 2013 / C#
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

using CAS.SharePoint.Logging;
using Microsoft.SharePoint.Administration;
using System;

namespace CAS.SmartFactory.IPR.WebsiteModel
{

  /// <summary>
  /// Website Model Extensions
  /// </summary>
  public static class WebsiteModelExtensions
  {

    #region public
    /// <summary>
    /// Rounds a double-precision floating-point value to a specified number of fractional digits or returns the default value.
    /// </summary>
    /// <param name="value"> A  <see cref="double"/> number to be rounded.</param>
    /// <returns></returns>
    public static decimal Round2Double(this double value)
    {
      return Convert.ToDecimal(Math.Round(value, 2));
    }
    /// <summary>
    /// Rounds a double-precision floating-point value to a specified number of fractional digits or returns the default value.
    /// </summary>
    /// <param name="value"> A  <see cref="double"/> number to be rounded.</param>
    /// <returns></returns>
    public static decimal Round2DecimalOrDefault(this double? value)
    {
      return Convert.ToDecimal(Math.Round(value.GetValueOrDefault(), 2));
    }
    /// <summary>
    /// Rounds the mass.
    /// </summary>
    /// <param name="value">The value to be rounded.</param>
    /// <returns>The <paramref name="value"/> rounded; the number of fractional digits in the return value is 2.</returns>
    public static double Round2Decimals(this double value)
    {
      return Math.Round(value, 2);
    }
    /// <summary>
    /// Rounds the mass.
    /// </summary>
    /// <param name="value">The value to be rounded.</param>
    /// <returns>The <paramref name="value"/> rounded; the number of fractional digits in the return value is 2.</returns>
    public static decimal Round2Decimals(this decimal value)
    {
      return Math.Round(value, 2);
    }
    /// <summary>
    ///   Converts the value of the specified decimal number to an equivalent double-precision floating-point 
    ///   number and rounds it to 2 fractional digits in the return value.
    /// </summary>
    /// <param name="value">The value to be converted.</param>
    /// <returns>The <paramref name="value"/> converted and rounded; the number of fractional digits in the return value is 2.</returns>
    public static double Convert2Double2Decimals(this decimal value)
    {
      return Math.Round(Convert.ToDouble(value), 2);
    }
    /// <summary>
    /// Rounds the mass upper.
    /// </summary>
    /// <param name="value">The value to be rounded.</param>
    /// <returns>The <paramref name="value"/> rounded; the number of fractional digits in the return value is 2.</returns>
    public static double RoundMassUpper(this double value)
    {
      return Math.Round(value + 0.005, 2);
    }
    /// <summary>
    /// Rounds the currency.
    /// </summary>
    /// <param name="value">The value to be rounded.</param>
    /// <returns>The <paramref name="value"/> rounded; the number of fractional digits in the return value is 2.</returns>
    public static double RoundCurrency(this double value)
    {
      return Math.Round(value, 3);
    }
    /// <summary>
    /// Enum LoggingCategories - registered set of categories.
    /// </summary>
    public enum LoggingCategories
    {
      /// <summary>
      /// The feature activation action
      /// </summary>
      FeatureActivation,
      /// <summary>
      /// The close account action
      /// </summary>
      SADProcessing,
      /// <summary>
      /// The report creation action
      /// </summary>
      ReportCreation,
      /// <summary>
      /// The ipr closing action
      /// </summary>
      IPRClosing,
      /// <summary>
      /// The batch processing action
      /// </summary>
      BatchProcessing,
      /// <summary>
      /// The export user action
      /// </summary>
      Export,
      /// <summary>
      /// The clearance user action
      /// </summary>
      Clearance
    }
    /// <summary>
    /// Writes a diagnostic message into the trace log, with specified Microsoft.SharePoint.Administration.TraceSeverity. 
    /// Don't use in sandbox.
    /// </summary>
    /// <param name="message">The message to write into the log.</param>
    /// <param name="eventId">The eventId that corresponds to the event.</param>
    /// <param name="severity">The severity of the trace.</param>
    /// <param name="category">The category to write the message to.</param>
    public static void TraceEvent(string message, int eventId, TraceSeverity severity, LoggingCategories category)
    {
      NamedTraceLogger.Logger.TraceToDeveloper(message, eventId, severity, string.Format("{0}/{1}", LoggingArea, category));
    }
    internal static void RegisterLoggerSource()
    {
      NamedTraceLogger.RegisterLoggerSource(LoggingArea, Enum.GetNames(typeof(LoggingCategories)));
    }
    internal static void UnregisterLoggerSource()
    {
      NamedTraceLogger.UnregisterLoggerSource(LoggingArea);
    }
    #endregion

    #region private
    private static string m_LoggingArea;
    private static string LoggingArea
    {
      get
      {
        if (String.IsNullOrEmpty(m_LoggingArea))
          m_LoggingArea = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        return m_LoggingArea;
      }
    }
    //The number of fractional digits in the return value.
    private const int m_MassFractionalDigits = 2;
    #endregion

  }
}
