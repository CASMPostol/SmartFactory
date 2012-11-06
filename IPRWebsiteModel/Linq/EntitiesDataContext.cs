using System;
using CAS.SharePoint;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class Entities
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Entities" /> class.
    /// </summary>
    public Entities() : base( SPContext.Current.Web.Url ) { }
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
        {
          changedListItem.Resolve( mode );
        }
        this.SubmitChanges();
      }
    }
    /// <summary>
    /// Resolves the change conflicts.
    /// </summary>
    /// <param name="_rsult">The _rsult.</param>
    /// <exception cref="System.ApplicationException"></exception>
    public void ResolveChangeConflicts( ActionResult _rsult )
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
    internal class ProductDescription
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
    //Batch processing - product recognition improvement  http://cas_sp:11225/sites/awt/Lists/TaskList/DispForm.aspx?ID=3362
    internal ProductDescription GetProductType( string sku, string location )
    {
      SKUCommonPart entity = SKUCommonPart.Find( this, sku );
      if ( entity != null )
        return new ProductDescription( entity.ProductType.GetValueOrDefault( ProductType.Other ), entity.IPRMaterial.GetValueOrDefault( false ), entity );
      Warehouse wrh = Linq.Warehouse.Find( this, location );
      if ( wrh != null )
      {
        switch ( wrh.ProductType )
        {
          case ProductType.Tobacco:
            return new ProductDescription( ProductType.Tobacco, false );
          case ProductType.IPRTobacco:
            return new ProductDescription( ProductType.IPRTobacco, true );
          case ProductType.Invalid:
          case ProductType.Cutfiller:
          case ProductType.Cigarette:
          case ProductType.Other:
          default:
            break;
        }
      }
      return new ProductDescription( ProductType.Other, false );
    }
    private const string m_Source = "Data Context";
    private const string m_WrongProductTypeMessage = "I cannot recognize product type of the stock entry SKU: {0} in location: {1}";
  }
}

