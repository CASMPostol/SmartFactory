using System;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;

namespace CAS.SmartFactory.IPR.Client.FeatureActivation
{
  class Program
  {
    static void Main( string[] args )
    {
      try
      {
        using ( Entities edc = new Entities( Properties.Settings.Default.URL ) )
        {
          Activate180.Activate.UpdateDisposals( edc, ProgressChanged );
          edc.SubmitChanges();
          Activate180.Activate.Go( edc, ProgressChanged );
          edc.SubmitChanges();
          Archival.Archive.Go( edc, ProgressChanged );
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
    private static void ProgressChanged( object sender, EntitiesChangedEventArgs e )
    {
      if ( e != null )
      {
        throw new ArgumentNullException( "e" );
      }
      WriteDot( e.UserState );
      if ( sender != null )
      {
        //Console.WriteLine( sender.ToString() );
        return;
      }
    }
    private static uint dotCounter = 0;
    private static void WriteDot( EntitiesChangedEventArgs.EntitiesState entitiesState )
    {
      dotCounter++;
      if ( dotCounter % 10 == 0 )
        Console.Write( "." );
      if ( dotCounter % 100 == 0 )
      {
        Console.WriteLine();
        entitiesState.Entities.SubmitChanges();
      }
    }
    private static void WriteLine()
    {
      dotCounter = 0;
      Console.WriteLine();
    }
  }
}
