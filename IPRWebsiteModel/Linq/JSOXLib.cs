using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SharePoint;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class JSOXLib
  {
    public void JSOXReport( Entities edc )
    {

      //this.BalanceDate = xzxzx;
      //this.BalanceQuantity = xzxzx;
      //Introducing
      decimal _introducingQuantity = 0;
      this.IntroducingDateEnd = DateTime.MinValue;
      this.IntroducingDateStart = DateTime.MaxValue;
      foreach ( IPR _iprx in IPR.GetAllNew4JSOX( edc ) )
      {
        _iprx.JSOXIndex = this;
        _introducingQuantity += _iprx.NetMassDec;
        this.IntroducingDateEnd = Max( _iprx.CustomsDebtDate.Value.Date, this.IntroducingDateEnd.Value );
        this.IntroducingDateStart = Min( _iprx.CustomsDebtDate.Value.Date, this.IntroducingDateEnd.Value );
      }
      this.IntroducingQuantity = Convert.ToDouble( _introducingQuantity );
      this.OutboundDateEnd = DateTime.MinValue;
      this.OutboundDateStart = DateTime.MaxValue;
      decimal _out = JSOXCustomsSummary.CreateEntries( edc, this );
      this.OutboundQuantity = Convert.ToDouble( _outboundQuantity );
    }

    private DateTime Min( DateTime dateTime1, DateTime dateTime2 )
    {
      return dateTime1 < dateTime2 ? dateTime1 : dateTime2;
    }
    private static DateTime Max( DateTime dateTime1, DateTime dateTime2 )
    {
      return dateTime1 > dateTime2 ? dateTime1 : dateTime2;
    }

  }
}
