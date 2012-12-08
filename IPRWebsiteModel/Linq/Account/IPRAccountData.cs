using System;
using System.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq.Account
{
  
  /// <summary>
  /// Invard Processing Account Record Data
  /// </summary>
  public class IPRAccountData: AccountData
  {
    
    #region ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="IPRAccountData" /> class.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <param name="good">The good.</param>
    /// <param name="messageType">Type of the _message.</param>
    public IPRAccountData( Entities edc, SADGood good, MessageType messageType )
      : base( edc, good, messageType )
    {
      AnalizeDutyAndVAT( good );
    }
    #endregion

    #region CW Data
    internal double CartonsMass { get; private set; }
    internal double Duty { get; private set; }
    internal string DutyName { get; private set; }
    internal double DutyPerUnit { get; private set; }
    internal string VATName { get; private set; }
    internal double VAT { get; private set; }
    internal double VATPerUnit { get; private set; }
    #endregion

    #region private
    private void AnalizeDutyAndVAT( SADGood good )
    {
      string _at = "Started";
      try
      {
        Duty = 0;
        VAT = 0;
        DutyName = string.Empty;
        VATName = string.Empty;
        foreach ( SADDuties _duty in good.SADDuties )
        {
          _at = "switch " + _duty.DutyType;
          switch ( _duty.DutyType )
          {
            //Duty
            case "A10":
            case "A00":
            case "A20":
              Duty += _duty.Amount.Value;
              _at = "DutyName";
              DutyName += String.Format( "{0}={1:F2}; ", _duty.DutyType, _duty.Amount.Value );
              break;
            //VAT
            case "B00":
            case "B10":
            case "B20":
              VAT += _duty.Amount.Value;
              _at = "VATName";
              VATName += String.Format( "{0}={1:F2}; ", _duty.DutyType, _duty.Amount.Value );
              break;
            default:
              break;
          }
        }
        _at = "DutyPerUnit";
        DutyPerUnit = Duty / NetMass;
        _at = "VATPerUnit";
        VATPerUnit = VAT / NetMass;
      }
      catch ( Exception _ex )
      {
        string _src = String.Format( "AnalizeDutyAndVAT error at {0}", _at );
        throw new IPRDataConsistencyException( _src, _ex.Message, _ex, _src );
      }
    }
    /// <summary>
    /// Analizes the good.
    /// </summary>
    /// <param name="good">The good.</param>
    /// <param name="_messageType">Type of the _message.</param>
    protected internal override void AnalizeGood( SADGood good, MessageType _messageType )
    {
      base.AnalizeGood( good, _messageType );
      SADPackage _packagex = good.SADPackage.First();
      if ( _packagex.Package.ToUpper().Contains( "CT" ) )
        CartonsMass = GrossMass - NetMass;
      else
        CartonsMass = 0;
    }
    protected internal override Consent.CustomsProcess Process
    {
      get { return Consent.CustomsProcess.cw; }
    }
    #endregion

  }

}
