//<summary>
//  Title   : Name of Application
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
          ProgressChanged( null, new EntitiesChangedEventArgs( 1, "Activate.IPRRecalculateClearedRecords", edc ) );
          //Activate180.Activate.IPRRecalculateClearedRecords( edc, ProgressChanged );
          edc.SubmitChanges();
          Activate180.Activate.ResetArchival(edc, ProgressChanged);
          ProgressChanged( null, new EntitiesChangedEventArgs( 1, "Archive.Go", edc ) );
          Archival.Archive.Go( edc, ProgressChanged );
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
    private static void ProgressChanged( object sender, EntitiesChangedEventArgs e )
    {
      if ( e == null )
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
      if (entitiesState.UserState != null)
        if ( entitiesState.UserState is String )
        {
          Console.WriteLine();
          Console.WriteLine( (string)entitiesState.UserState );
          Console.WriteLine();
          dotCounter = 0;
          entitiesState.Entities.SubmitChanges();
          return;
        }
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
