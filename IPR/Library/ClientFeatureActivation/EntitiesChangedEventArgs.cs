﻿//<summary>
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

using System.ComponentModel;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;

namespace CAS.SmartFactory.IPR.Client.FeatureActivation
{

  /// <summary>
  /// EntitiesChangedEventArgs class provides data for an event.
  /// </summary>
  public class EntitiesChangedEventArgs : ProgressChangedEventArgs
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="EntitiesChangedEventArgs"/> class.
    /// </summary>
    /// <param name="progressPercentage">The progress percentage.</param>
    /// <param name="userState">State of the user.</param>
    /// <param name="entities">The entities.</param>
    public EntitiesChangedEventArgs(int progressPercentage, object userState, Entities entities)
      : base(progressPercentage, new EntitiesStateInternal(userState, entities))
    { }
    /// <summary>
    /// Gets a unique user state.
    /// </summary>
    /// <returns>A unique <see cref="T:System.Object" /> indicating the user state.</returns>
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
      /// <summary>
      /// Gets the entities.
      /// </summary>
      /// <value>
      /// The entities.
      /// </value>
      public Entities Entities { get { return m_Entities; } }
      /// <summary>
      /// The m_ user state
      /// </summary>
      internal protected object m_UserState = null;
      /// <summary>
      /// The m_ entities
      /// </summary>
      internal protected Entities m_Entities = null;
    }
    //private
    private class EntitiesStateInternal : EntitiesState
    {
      /// <summary>
      /// Initializes a new instance of the <see cref="EntitiesState" /> class.
      /// </summary>
      /// <param name="userState">A unique user state.</param>
      /// <param name="entities">The entities.</param>
      internal EntitiesStateInternal(object userState, Entities entities)
      {
        m_UserState = userState;
        m_Entities = entities;
      }
    }

  } //EntitiesChangedEventArgs

}
