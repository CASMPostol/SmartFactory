using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class Batch
  {
    internal static Batch GetLookup(EntitiesDataContext edc, string index)
    {
      Batch newBatch = null;
      var cb =
         from batch in edc.Batch where batch.Batch0.Contains(index) orderby batch.Identyfikator descending select batch;
      if (cb.Count<Batch>() == 0)
      {
        newBatch = new Batch()
        {
          Batch0 = index,
          BatchStatus = Entities.BatchStatus.Preliminary,
          Tytuł = index
        };
        edc.Batch.InsertOnSubmit(newBatch);
      }
      else
        newBatch = cb.First();
      return newBatch;
    }
    internal Disposal[] GetDisposals(EntitiesDataContext edc)
    {
      var disposals =
          from idx in edc.Disposal
          where this.Identyfikator == idx.BatchLookup.Identyfikator
          select idx;
      return disposals.ToArray<Disposal>();
    }
  }
}
