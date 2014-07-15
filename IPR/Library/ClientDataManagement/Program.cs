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
    static void Main(string[] args)
    {
      try
      {
        Activate180.Activate.Go(Properties.Settings.Default.URL, ProgressChanged);
        Archival.Archive.Go(Properties.Settings.Default.URL, ProgressChanged);
        Console.WriteLine("Finished without errors.");
      }
      catch (Exception ex)
      {
        Console.WriteLine("Program stoped by exception: ");
        Console.WriteLine(ex.ToString());
      }
      Console.WriteLine("Press enter to close the window");
      Console.ReadLine();
    }

    private static bool ProgressChanged(object sender, EntitiesChangedEventArgs e)
    {
      if (e == null)
      {
        throw new ArgumentNullException("e");
      }
      WriteDot(e.UserState);
      if (sender != null)
      {
        //Console.WriteLine( sender.ToString() );
        return true;
      }
      return true;
    }
    private static uint dotCounter = 0;
    private static void WriteDot(EntitiesChangedEventArgs.EntitiesState entitiesState)
    {
      if (entitiesState.UserState != null)
        if (entitiesState.UserState is String)
        {
          Console.WriteLine();
          Console.WriteLine((string)entitiesState.UserState);
          Console.WriteLine();
          dotCounter = 0;
          entitiesState.Entities.SubmitChanges();
          return;
        }
      dotCounter++;
      if (dotCounter % 10 == 0)
        Console.Write(".");
      if (dotCounter % 100 == 0)
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
