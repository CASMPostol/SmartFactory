//<summary>
//  Title   : public static class Linq
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

using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.SharePoint.Client;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{
  public static class Linq
  {
    public static IEnumerable<TEntity> Filter<TEntity>( this EntityList<TEntity> entity, CamlQuery query )
          where TEntity: class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
    {
      entity.Query = query;
      return entity;
    }
  }
}
