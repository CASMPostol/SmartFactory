using System;
using System.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// PCNCode
  /// </summary>
  public partial class PCNCode
  {
    /// <summary>
    /// Adds the or get.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="procedureCode">The procedure code.</param>
    /// <param name="productCodeNumber">The product code number.</param>
    /// <param name="title">The title.</param>
    /// <returns></returns>
    public static PCNCode AddOrGet( Entities entities, ClearenceProcedure procedureCode, string productCodeNumber, string title )
    {
      PCNCode _pcncode = Find( entities, procedureCode, productCodeNumber );
      if ( _pcncode == null )
      {
        _pcncode = new PCNCode()
          {
            //TODO PCNCode - auto generation value of the column CompensationGood http://cas_sp:11225/sites/awt/Lists/TaskList/DispForm.aspx?ID=3341
            CompensationGood = "Tytoń",
            Procedure = Entities.RequestedProcedure( procedureCode ),
            ProductCodeNumber = productCodeNumber,
            Title = title
          };
        entities.PCNCode.InsertOnSubmit( _pcncode );
      }
      return _pcncode;
    }
    /// <summary>
    /// Finds the specified entities.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="procedureCode">The procedure code.</param>
    /// <param name="productCodeNumber">The product code number.</param>
    /// <returns></returns>
    internal static PCNCode Find( Entities entities, ClearenceProcedure procedureCode, string productCodeNumber )
    {
      string _requestedProcedure = Entities.RequestedProcedure( procedureCode );
      PCNCode _pcn = ( from _pcnx in entities.PCNCode
                       where String.IsNullOrEmpty( _pcnx.Procedure ) || _pcnx.Procedure.Contains( _requestedProcedure ) &&
                             _pcnx.ProductCodeNumber.Contains( productCodeNumber )
                       select _pcnx ).FirstOrDefault();
      return _pcn;
    }
  }
}
