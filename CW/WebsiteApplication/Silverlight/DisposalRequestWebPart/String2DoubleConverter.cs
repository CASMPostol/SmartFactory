//<summary>
//  Title   : class Double2StringConverter
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Windows.Data;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart
{
  /// <summary>
  /// Double to string Converter
  /// </summary>
  
  public class Double2StringConverter: IValueConverter
  {

    #region IValueConverter Members
    public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
    {
      double _doubleValue = (double)value;
      if ( typeof( String ) != targetType )
        throw new ArgumentOutOfRangeException( String.Format( "targetType", "Wrong target type {0} but expected String", targetType.Name ) );
      return _doubleValue.ToString("F2", culture );
    }

    public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
    {
      string _stringValue = (String)value;
      if ( typeof( Double ) != targetType )
        throw new ArgumentOutOfRangeException( String.Format( "targetType", "Wrong target type {0} but expected Double", targetType.Name ) );
      return Double.Parse( _stringValue, culture );
    }
    #endregion

  }

}