using System;
using System.Collections.Generic;
using System.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class PCNCode
  {
    /// <summary>
    /// Adds the or get.
    /// </summary>
    /// <param name="_edc">The _edc.</param>
    /// <param name="productCodeNumber">The _code.</param>
    /// <returns></returns>
    //TODO PCN Code list - change the title http://cas_sp:11225/sites/awt/Lists/TaskList/DispForm.aspx?ID=3340
    public static PCNCode AddOrGet( Entities _edc, ClearenceProcedure procedureCode, string productCodeNumber, string title )
    {
      PCNCode _pcncode = ( from _pcnx in _edc.PCNCode where productCodeNumber.Trim().Contains( _pcnx.ProductCodeNumber ) select _pcnx ).FirstOrDefault();
      if ( _pcncode == null )
      {
        _pcncode = new PCNCode()
          {
            //TODO PCNCode - auto generation value of the column CompensationGood http://cas_sp:11225/sites/awt/Lists/TaskList/DispForm.aspx?ID=3341
            CompensationGood = "Tytoń",
            Procedure = "TBD",
            ProductCodeNumber = productCodeNumber,
            Title = String.Format( "{0} {1}", "Tytoń", productCodeNumber )
          };
        _edc.PCNCode.InsertOnSubmit( _pcncode );
      }
      return _pcncode;
    }
    internal static PCNCode Find( Entities edc, ClearenceProcedure procedureCode, string productCodeNumber )
    {
      PCNCode _pcn = ( from _pcnx in edc.PCNCode
                       where _pcnx.Procedure.Contains( Entities.RequestedProcedure( procedureCode ) ) && _pcnx.ProductCodeNumber.Contains( productCodeNumber )
                       select _pcnx ).FirstOrDefault();
      return _pcn;
    }

  }
}
