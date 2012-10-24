using System.Collections.Generic;
using CAS.SmartFactory.xml.Dictionaries;

namespace CAS.SmartFactory.Linq.IPR
{
  internal partial class CutfillerCoefficientExtension
  {
    #region public
    internal static void ImportData( ConfigurationCutfillerCoefficientItem[] configuration, Entities edc )
    {
      List<CutfillerCoefficient> list = new List<CutfillerCoefficient>();
      foreach ( ConfigurationCutfillerCoefficientItem item in configuration )
      {
        CutfillerCoefficient cc = new CutfillerCoefficient
        {
          CFTProductivityRateMax = item.CFTProductivityRateMax,
          CFTProductivityRateMin = item.CFTProductivityRateMin
        };
        list.Add( cc );
      };
      edc.CutfillerCoefficient.InsertAllOnSubmit( list );
    }
    #endregion
  }
}
