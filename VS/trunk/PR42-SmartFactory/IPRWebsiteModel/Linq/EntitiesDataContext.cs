using System;
using System.Collections.Generic;
using CAS.SharePoint;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// Enumerated kinds of Disposal 
  /// </summary>
  public enum DisposalEnum { Dust, SHMenthol, Waste, OverusageInKg, Tobacco, TobaccoInCigaretess, Cartons };
  public partial class Entities
  {
    #region public
    /// <summary>
    /// Initializes a new instance of the <see cref="Entities" /> class.
    /// </summary>
    public Entities() : this( SPContext.Current.Web.Url ) { }
    /// <summary>
    /// Persists to the content database changes made by the current user to one or more lists using the specified failure mode;
    /// or, if a concurrency conflict is found, populates the <see cref="P:Microsoft.SharePoint.Linq.DataContext.ChangeConflicts"/> property.
    /// </summary>
    /// <param name="mode">Specifies how the list item changing system of the LINQ to SharePoint provider will respond when it 
    /// finds that a list item has been changed by another process since it was retrieved.
    /// </param>
    public void SubmitChangesSilently( RefreshMode mode )
    {
      try
      {
        SubmitChanges();
      }
      catch ( ChangeConflictException )
      {
        foreach ( ObjectChangeConflict changedListItem in this.ChangeConflicts )
          changedListItem.Resolve( mode );
        this.SubmitChanges();
      }
    }
    /// <summary>
    /// Resolves the change conflicts.
    /// </summary>
    /// <param name="_rsult">The _rsult.</param>
    /// <exception cref="System.ApplicationException"></exception>
    internal void ResolveChangeConflicts( ActionResult _rsult )
    {
      string _cp = "Starting";
      try
      {
        foreach ( ObjectChangeConflict _itx in this.ChangeConflicts )
        {
          _cp = "ObjectChangeConflict";
          string _tmp = String.Format( "Object: {0}", _itx.Object == null ? "null" : _itx.Object.ToString() );
          if ( _itx.MemberConflicts != null )
          {
            string _ft = ", Conflicts: Member.Name={0}; CurrentValue={1}; DatabaseValue={2}; OriginalValue={3}";
            String _chnges = String.Empty;
            foreach ( MemberChangeConflict _mid in _itx.MemberConflicts )
            {
              _chnges += String.Format( _ft,
                _mid.Member == null ? "null" : _mid.Member.Name,
                _mid.CurrentValue == null ? "null" : _mid.CurrentValue.ToString(),
                _mid.DatabaseValue == null ? "null" : _mid.DatabaseValue.ToString(),
                _mid.OriginalValue == null ? "null" : _mid.OriginalValue.ToString() );
            }
            _tmp += _chnges;
          }
          else
            _tmp += "; No member details";
          _rsult.AddMessage( "ResolveChangeConflicts at: " + _cp, _tmp );
          _cp = "AddMessage";
          _itx.Resolve( RefreshMode.KeepCurrentValues );
        } //foreach (ObjectChangeConflict
      }
      catch ( Exception ex )
      {
        string _frmt = "The current operation has been interrupted in ResolveChangeConflicts at {0} by error {1}.";
        throw new ApplicationException( String.Format( _frmt, _cp, ex.Message ) );
      }
    }
    /// <summary>
    /// String representing the requested procedure .
    /// </summary>
    /// <param name="procedureCode">The procedure code.</param>
    /// <returns></returns>
    internal static string RequestedProcedure( ClearenceProcedure? procedureCode )
    {
      switch ( procedureCode )
      {
        case ClearenceProcedure._3151:
        case ClearenceProcedure._3171:
          return "31";
        case ClearenceProcedure._4051:
        case ClearenceProcedure._4071:
          return "40";
        case ClearenceProcedure._5100:
        case ClearenceProcedure._5171:
          return "51";
        case ClearenceProcedure._7100:
        case ClearenceProcedure._7171:
          return "71";
      }
      return string.Empty.NotAvailable();
    }
    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <param name="procedureCode">The procedure code.</param>
    /// <returns>
    /// A <see cref="System.String" /> that represents procedure code.
    /// </returns>
    public static string ToString( ClearenceProcedure procedureCode )
    {
      string _value = String.Empty.NotAvailable();
      switch ( procedureCode )
      {
        case ClearenceProcedure._3151:
          _value = "3151";
          break;
        case ClearenceProcedure._3171:
          _value = "3171";
          break;
        case ClearenceProcedure._4051:
          _value = "4051";
          break;
        case ClearenceProcedure._4071:
          _value = "4071";
          break;
        case ClearenceProcedure._5100:
          _value = "5100";
          break;
        case ClearenceProcedure._5171:
          _value = "5171";
          break;
        case ClearenceProcedure._7100:
          _value = "7100";
          break;
        case ClearenceProcedure._7171:
          _value = "7171";
          break;
        default:
          break;
      }
      return _value;
    }
    /// <summary>
    /// ProductDescription
    /// </summary>
    public class ProductDescription
    {
      internal ProductType productType;
      internal bool IPRMaterial;
      internal SKUCommonPart skuLookup;
      internal ProductDescription( ProductType type, bool ipr, SKUCommonPart lookup )
      {
        productType = type;
        IPRMaterial = ipr;
        skuLookup = lookup;
      }
      internal ProductDescription( ProductType type, bool ipr )
        : this( type, ipr, null )
      { }
    }
    /// <summary>
    /// Gets the type of the product.
    /// </summary>
    /// <param name="sku">The sku.</param>
    /// <param name="location">The warehouse.</param>
    /// <returns></returns>
    public ProductDescription GetProductType( string sku, string location )
    {
      ProductDescription _ret = null;
      SKUCommonPart entity = SKUCommonPart.Find( this, sku );
      if ( entity != null )
        _ret = new ProductDescription( entity.ProductType.GetValueOrDefault( ProductType.Other ), entity.IPRMaterial.GetValueOrDefault( false ), entity );
      else
        _ret = GetProductType( location );
      return _ret;
    }
    /// <summary>
    /// Gets the type of the product.
    /// </summary>
    /// <param name="sku">The sku.</param>
    /// <param name="location">The warehouse.</param>
    /// <param name="isFinishedGood">if set to <c>true</c> it is finished good.</param>
    /// <returns></returns>
    /// <exception cref="InputDataValidationException">Cannot find finisched good in the SKU dictionary;Get Product Type</exception>
    public ProductDescription GetProductType( string sku, string location, bool isFinishedGood )
    {
      ProductDescription _ret = null;
      if ( isFinishedGood )
      {
        SKUCommonPart entity = SKUCommonPart.Find( this, sku );
        if ( entity == null )
        {
          List<String> _errors = new List<string>();
          string _mssg = String.Format( "Cannot find finisched good {0}", sku );
          _errors.Add( _mssg );
          throw new InputDataValidationException( "Cannot find finisched good in the SKU dictionary", "Get Product Type", _errors );
        }
        _ret = new ProductDescription( entity.ProductType.GetValueOrDefault( ProductType.Other ), entity.IPRMaterial.GetValueOrDefault( false ), entity );
      }
      else
        _ret = GetProductType( location );
      return _ret;
    }
    internal static DisposalStatus GetDisposalStatus( DisposalEnum status )
    {
      Linq.DisposalStatus _typeOfDisposal = default( Linq.DisposalStatus );
      switch ( status )
      {
        case DisposalEnum.Cartons:
          _typeOfDisposal = DisposalStatus.Cartons;
          break;
        case DisposalEnum.Dust:
          _typeOfDisposal = DisposalStatus.Dust;
          break;
        case DisposalEnum.SHMenthol:
          _typeOfDisposal = DisposalStatus.SHMenthol;
          break;
        case DisposalEnum.Waste:
          _typeOfDisposal = DisposalStatus.Waste;
          break;
        case DisposalEnum.OverusageInKg:
          _typeOfDisposal = DisposalStatus.Overuse;
          break;
        case DisposalEnum.TobaccoInCigaretess:
          _typeOfDisposal = DisposalStatus.TobaccoInCigaretes;
          break;
        case DisposalEnum.Tobacco:
          _typeOfDisposal = DisposalStatus.Tobacco;
          break;
      }
      return _typeOfDisposal;
    }
    public static string IPRLibraryName = "IPR Library";
    #endregion

    #region private
    private ProductDescription GetProductType( string location )
    {
      ProductDescription _ret = new ProductDescription( ProductType.Other, false );
      Warehouse wrh = Linq.Warehouse.Find( this, location );
      if ( wrh == null )
        return _ret;
      switch ( wrh.ProductType )
      {
        case ProductType.Tobacco:
          _ret = new ProductDescription( ProductType.Tobacco, false );
          break;
        case ProductType.IPRTobacco:
          _ret = new ProductDescription( ProductType.IPRTobacco, true );
          break;
        case ProductType.Invalid:
        case ProductType.Cutfiller:
        case ProductType.Cigarette:
        case ProductType.Other:
        default:
          break;
      }
      return _ret;
    }
    private const string m_Source = "Data Context";
    private const string m_WrongProductTypeMessage = "I cannot recognize product type of the stock entry SKU: {0} in location: {1}";
    #endregion

  }
}

