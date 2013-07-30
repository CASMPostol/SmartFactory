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
    /// <param name="productCodeNumber">The product code number.</param>
    /// <param name="title">The title.</param>
    /// <returns></returns>
    public static PCNCode AddOrGet( Entities entities, string productCodeNumber, string title )
    {
      PCNCode _pcncode = Find( entities, false, productCodeNumber );
      if ( _pcncode == null )
      {
        _pcncode = new PCNCode()
          {
            //TODO PCNCode - auto generation value of the column CompensationGood http://cas_sp:11225/sites/awt/Lists/TaskList/DispForm.aspx?ID=3341
            CompensationGood = "Tytoń",
            Disposal = false,
            ProductCodeNumber = productCodeNumber,
            Title = title
          };
        entities.PCNCode.InsertOnSubmit( _pcncode );
        entities.SubmitChanges();
      }
      return _pcncode;
    }
    /// <summary>
    /// Finds the specified entities.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="disposal">if set to <c>true</c> if disposal is looked through.</param>
    /// <param name="productCodeNumber">The product code number.</param>
    /// <returns></returns>
    internal static PCNCode Find( Entities entities, bool disposal, string productCodeNumber )
    {
      return ( from _pcnx in entities.PCNCode
               where _pcnx.ProductCodeNumber.Contains( productCodeNumber ) && _pcnx.Disposal.Value == disposal
               select _pcnx ).FirstOrDefault();
    }
  }
}
