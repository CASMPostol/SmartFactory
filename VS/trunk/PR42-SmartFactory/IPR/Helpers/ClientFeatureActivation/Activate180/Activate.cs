using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;

namespace CAS.SmartFactory.IPR.Client.FeatureActivation.Activate180
{
  internal static class Activate
  {
    internal static void UpdateDisposals( WebsiteModel.Linq.Entities edc, Action<object, EntitiesChangedEventArgs> ProgressChanged )
    {
      foreach ( Disposal _dspx in edc.Disposal )
      {
        if ( _dspx.JSOXCustomsSummaryIndex != null )
          _dspx.JSOXReportID = _dspx.JSOXCustomsSummaryIndex.JSOXCustomsSummary2JSOXIndex.Id.Value;
        ProgressChanged( null, new EntitiesChangedEventArgs( 1, null, edc ) );
      }
    }
    internal static void IPRRecalculateClearedRecords( Entities entities, EntitiesChangedEventHandler progress )
    {
      List<Disposal> _dl = entities.Disposal.ToList<Disposal>();
      progress( null, new EntitiesChangedEventArgs( 1, null, entities ) );
      foreach ( IPR.WebsiteModel.Linq.IPR _iprX in entities.IPR )
        _iprX.RecalculateClearedRecords( entities, _dl, progress );
    }
  }
}
