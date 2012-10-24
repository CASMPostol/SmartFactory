using System;
using System.Linq;
using CAS.SmartFactory.IPR;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Batch
  {
    #region public
    /// <summary>
    /// Gets the lookup.
    /// </summary>
    /// <param name="edc">The <see cref="Entities"/> instance.</param>
    /// <param name="index">The index of the entry we are lookin for.</param>
    /// <returns>The most recent <see cref="Batch"/> object.</returns>
    /// <exception cref="System.ArgumentNullException">The source is null.</exception>
    public static Batch FindLookup( Entities edc, string index )
    {
      try
      {
        return ( from batch in edc.Batch where batch.Batch0.Contains( index ) orderby batch.Identyfikator descending select batch ).FirstOrDefault();
      }
      catch ( Exception ex )
      {
        throw new IPRDataConsistencyException( m_Source, String.Format( m_LookupFailedMessage, index ), ex, "Cannot find batch" );
      }
    }
    /// <summary>
    /// Gets an existing <see cref="Batch"/> or create preliminary.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <param name="batch">The batch.</param>
    /// <returns></returns>
    public static Batch GetOrCreatePreliminary( Entities edc, string batch )
    {
      Batch newBatch = FindLookup( edc, batch );
      if ( newBatch == null )
      {
        newBatch = new Batch()
        {
          Batch0 = batch,
          BatchStatus = Linq.IPR.BatchStatus.Preliminary,
          Title = "Preliminary batch: " + batch,
          ProductType = Linq.IPR.ProductType.Invalid,
          FGQuantityAvailable = 0,
          FGQuantityBlocked = 0,
          FGQuantity = 0,
          FGQuantityPrevious = 0
        };
        edc.Batch.InsertOnSubmit( newBatch );
        Anons.WriteEntry( edc, m_Source, String.Format( m_LookupFailedAndAddedMessage, batch ) );
      }
      return newBatch;
    }
    
    #endregion

    #region private

    private const string m_Source = "Batch processing";
    private const string m_LookupFailedMessage = "I cannot recognize batch {0}.";
    private const string m_LookupFailedAndAddedMessage = "I cannot recognize batch {0} - added preliminary entry to the list that must be uploaded.";
    #endregion
  }
}
