using System.Collections;
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
