using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;

namespace CAS.SmartFactory.IPR.Client.FeatureActivation
{
  class Program
  {
    static void Main( string[] args )
    {
      try
      {
        uint _i = 0;
        using ( Entities edc = new Entities( Properties.Settings.Default.URL ) )
        {
          foreach ( Disposal _dspx in edc.Disposal )
          {
            _i++;
            if ( _dspx.JSOXCustomsSummaryIndex != null )
              _dspx.JSOXReportID = _dspx.JSOXCustomsSummaryIndex.JSOXCustomsSummary2JSOXIndex.Id.Value;
            if ( _i % 10 == 0 )
              Console.Write( "." );
            if ( _i % 100 == 0 )
            {
              Console.WriteLine();
              edc.SubmitChanges();
            }
          }
          edc.SubmitChanges();
        }
        Console.WriteLine( "Finished without errors." );
      }
      catch ( Exception ex )
      {
        Console.WriteLine( "Program stoped by exception: " );
        Console.WriteLine( ex.ToString() );
      }
      Console.WriteLine( "Press enter to close the window" );
      Console.ReadLine();
    }
  }
}
