//<summary>
//  Title   : class GetListAsyncCompletedEventArgs
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
    public List<TEntity> Result<TEntity>()
       where TEntity: class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
    {
      return (List<TEntity>)m_Result;
    }
    private object m_Result = null;
  }
}
