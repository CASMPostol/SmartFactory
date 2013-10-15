using System;
using System.ComponentModel;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{
  public class GetListAsyncCompletedEventArgs: AsyncCompletedEventArgs
  {
    public GetListAsyncCompletedEventArgs( object result, Exception e, bool canceled, object state )
      : base( e, canceled, state )
    {
      m_Result = result;
    }
    public EntityList<TEntity> Result<TEntity>()
       where TEntity: class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
    {
      return (EntityList<TEntity>)m_Result;
    }
    private object m_Result = null;
  }
}
