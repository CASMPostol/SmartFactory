//<summary>
//  Title   : EntitiesChangedEventArgs class
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;

namespace CAS.SmartFactory.IPR.Client.FeatureActivation
{

  /// <summary>
  /// EntitiesChangedEventArgs class provides data for an event.
  /// </summary>
  public class EntitiesChangedEventArgs : System.ComponentModel.ProgressChangedEventArgs
  {
    public EntitiesChangedEventArgs( int progressPercentage, object userState, Entities entities )
      : base( progressPercentage, new EntitiesStateInternal( userState, entities ) )
    {
      if ( entities == null )
        throw new ArgumentNullException( "entities" );
    }
    public new EntitiesState UserState { get { return (EntitiesState)base.UserState; } }
    /// <summary>
    /// Class retpresenting <see cref="Entities"/> state
    /// </summary>
    public abstract class EntitiesState
    {
      /// <summary>
      /// Gets a unique user state.
      /// </summary>
      /// <value>
      /// A unique System.Object indicating the user state.
      /// </value>
      public object UserState { get { return m_UserState; } }
      internal Entities Entities { get { return m_Entities; } }
      internal protected object m_UserState = null;
      internal protected Entities m_Entities = null;
    }
    //private
    private class EntitiesStateInternal: EntitiesState
    {
      /// <summary>
      /// Initializes a new instance of the <see cref="EntitiesState" /> class.
      /// </summary>
      /// <param name="userState">A unique user state.</param>
      /// <param name="entities">The entities.</param>
      internal EntitiesStateInternal( object userState, Entities entities )
      {
        m_UserState = userState;
        m_Entities = entities;
      }
    }

  } //EntitiesChangedEventArgs

}
