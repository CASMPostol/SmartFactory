using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.SharePoint.Client;
using SPCList = Microsoft.SharePoint.Client.List;

namespace Microsoft.SharePoint.Linq
{
  // Summary:
  //     Represents a Windows SharePoint Services "14" list that can be queried with
  //     Language Integrated Query (LINQ).
  //
  // Type parameters:
  //   TEntity:
  //     The content type of the list items.
  public sealed class EntityList<TEntity>: IOrderedQueryable<TEntity>, IQueryable<TEntity>, IEnumerable<TEntity>, IOrderedQueryable, IQueryable, IEnumerable
    where TEntity: class, new()
  {
    private DataContext m_DataContext;
    private string m_ListName;
    private SPCList m_list = default( SPCList );
    private ListItemCollection m_allItems = default( ListItemCollection );
    private List<TEntity> m_allItemsEntities = new List<TEntity>();
    internal EntityList( DataContext dataContext, string listName )
    {
      this.m_DataContext = dataContext;
      this.m_ListName = listName;
      // Prepare a reference to the list of "DevLeap Contacts"
      m_list = dataContext.m_RootWeb.Lists.GetByTitle( listName );
      dataContext.m_ClientContext.Load( m_list );
      // Execute the prepared commands against the target ClientContext
      dataContext.m_ClientContext.ExecuteQuery();
      // Prepare a query for all items in the list
      CamlQuery query = new CamlQuery();
      query.ViewXml = "<View/>";
      m_allItems = m_list.GetItems( query );
      dataContext.m_ClientContext.Load( m_allItems );
      // Execute the prepared command against the target ClientContext
      dataContext.m_ClientContext.ExecuteQuery();
      foreach ( ListItem _ix in m_allItems )
      {
        TEntity _newEntity = new TEntity();
        m_allItemsEntities.Add( _newEntity );
        AssignValues( _newEntity, _newEntity.GetType(), name => _ix[ name ] );
      }
    }
    // Summary:
    //     Registers a disconnected or "detached" entity with the object tracking system
    //     of the Microsoft.SharePoint.Linq.DataContext object associated with the list.
    //
    // Parameters:
    //   entity:
    //     The entity that is registered.
    //
    // Exceptions:
    //   System.ArgumentNullException:
    //     entity is null.
    //
    //   System.InvalidOperationException:
    //     Object tracking is not enabled for the Microsoft.SharePoint.Linq.DataContext
    //     object.- or -entity is not of the same type as the list items.- or -entity
    //     has been deleted.- or -There is a problem with the internal ID of entity
    //     that is used by the object tracking system.
    public void Attach( TEntity entity ) { throw new NotImplementedException(); }
    //
    // Summary:
    //     Marks the specified entities for deletion on the next call of Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges.
    //
    // Parameters:
    //   entities:
    //     The entities to be marked for deletion.
    //
    // Exceptions:
    //   System.ArgumentNullException:
    //     At least one member of entities is null.
    //
    //   System.InvalidOperationException:
    //     Object tracking is not enabled for the Microsoft.SharePoint.Linq.DataContext
    //     object.- or -At least one member of entities is not of the same type as the
    //     list items.
    public void DeleteAllOnSubmit( IEnumerable<TEntity> entities ) { throw new NotImplementedException(); }
    //
    // Summary:
    //     Marks the specified entity for deletion on the next call of Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges.
    //
    // Parameters:
    //   entity:
    //     The entity to be marked for deletion.
    //
    // Exceptions:
    //   System.ArgumentNullException:
    //     entity is null.
    //
    //   System.InvalidOperationException:
    //     Object tracking is not enabled for the Microsoft.SharePoint.Linq.DataContext
    //     object.- or -entity is not of the same type as the list items.
    public void DeleteOnSubmit( TEntity entity ) { throw new NotImplementedException(); }
    //
    // Summary:
    //     Marks the specified entities for insertion into the list on the next call
    //     of Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges.
    //
    // Parameters:
    //   entities:
    //     The entities to be inserted.
    //
    // Exceptions:
    //   System.ArgumentNullException:
    //     At least one member of entities is null.
    //
    //   System.InvalidOperationException:
    //     Object tracking is not enabled for the Microsoft.SharePoint.Linq.DataContext
    //     object.- or -At least one member of entities is not of the same type as the
    //     list items.- or -At least one member of entities has been deleted.- or -At
    //     least one member of entities has been updated.- or -There is a problem with
    //     the internal ID of at least one member of entities that is used by the object
    //     tracking system.
    public void InsertAllOnSubmit( IEnumerable<TEntity> entities ) { throw new NotImplementedException(); }
    //
    // Summary:
    //     Marks the specified entity for insertion into the list on the next call of
    //     Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges.
    //
    // Parameters:
    //   entity:
    //     The entity to be marked for insertion.
    //
    // Exceptions:
    //   System.ArgumentNullException:
    //     entity is null.
    //
    //   System.InvalidOperationException:
    //     Object tracking is not enabled for the Microsoft.SharePoint.Linq.DataContext
    //     object.- or -entity is not of the same type as the list items.- or -entity
    //     has been deleted.- or -entity has been updated.- or -There is a problem with
    //     the internal ID of entity that is used by the object tracking system.
    public void InsertOnSubmit( TEntity entity )
    {
      if ( entity == null )
        throw new ArgumentNullException( "entity", "entity is null." );
      ListItem item = m_list.AddItem( new ListItemCreationInformation() );
      GetValues( entity, typeof( TEntity ), ( na, val ) => item[ na ] = val );
      item.Update();

    }
    //
    // Summary:
    //     Marks the specified entities to be put in the Recycle Bin on the next call
    //     of Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges.
    //
    // Parameters:
    //   entities:
    //     The entities to be recycled.
    //
    // Exceptions:
    //   System.ArgumentNullException:
    //     At least one member of entities is null.
    //
    //   System.InvalidOperationException:
    //     Object tracking is not enabled for the Microsoft.SharePoint.Linq.DataContext
    //     object.- or -At least one member of entities is not of the same type as the
    //     list items.
    public void RecycleAllOnSubmit( IEnumerable<TEntity> entities ) { throw new NotImplementedException(); }
    //
    // Summary:
    //     Marks the specified entity to be put in the Recycle Bin on the next call
    //     of Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges.
    //
    // Parameters:
    //   entity:
    //     The entity to be recycled.
    //
    // Exceptions:
    //   System.ArgumentNullException:
    //     entity is null.
    //
    //   System.InvalidOperationException:
    //     Object tracking is not enabled for the Microsoft.SharePoint.Linq.DataContext
    //     object.- or -entity is not of the same type as the list items.
    public void RecycleOnSubmit( TEntity entity ) { throw new NotImplementedException(); }
    //
    // Summary:
    //     Gets the subset of the Microsoft.SharePoint.Linq.EntityList<TEntity> that
    //     consists of all and only the list items that belong to a particular folder,
    //     with or without the items in subfolders.
    //
    // Parameters:
    //   listRelativePath:
    //     The list-relative path to the folder.
    //
    //   recursive:
    //     true to include items in subfolders; false to exclude them.
    //
    // Returns:
    //     An System.Linq.IQueryable<T> object that can be cast to Microsoft.SharePoint.Linq.EntityList<TEntity>.
    public IQueryable<TEntity> ScopeToFolder( string folderUrl, bool recursive ) { throw new NotImplementedException(); }

    #region IQueryable Members
    /// <summary>
    /// Gets the type of the element(s) that are returned when the expression tree associated with this instance of <see cref="T:System.Linq.IQueryable" /> is executed.
    /// </summary>
    /// <returns>A <see cref="T:System.Type" /> that represents the type of the element(s) that are returned when the expression tree associated with this object is executed.</returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public Type ElementType
    {
      get { throw new NotImplementedException(); }
    }
    /// <summary>
    /// Gets the expression tree that is associated with the instance of <see cref="T:System.Linq.IQueryable" />.
    /// </summary>
    /// <returns>The <see cref="T:System.Linq.Expressions.Expression" /> that is associated with this instance of <see cref="T:System.Linq.IQueryable" />.</returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public Expression Expression
    {
      get { throw new NotImplementedException(); }
    }
    /// <summary>
    /// Gets the query provider that is associated with this data source.
    /// </summary>
    /// <returns>The <see cref="T:System.Linq.IQueryProvider" /> that is associated with this data source.</returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public IQueryProvider Provider
    {
      get { throw new NotImplementedException(); }
    }
    #endregion

    #region IEnumerable Members
    //
    // Summary:
    //     Returns an enumerator that iterates through the Microsoft.SharePoint.Linq.EntityList<TEntity>.
    //
    // Returns:
    //     An System.Collections.Generic.IEnumerator<T> that can be used to iterate
    //     the list.
    public IEnumerator<TEntity> GetEnumerator() { return (IEnumerator<TEntity>)this.GetEnumerator(); }
    IEnumerator IEnumerable.GetEnumerator()
    {
      throw new NotImplementedException();
    }
    #endregion

    #region private
    private static void AssignValues( object _newEntity, Type type, Func<string, object> getValue )
    {
      Dictionary<string, MemberInfo> _mmbrs = GetMembers( type );
      foreach ( MemberInfo _ax in from _pidx in _mmbrs where _pidx.Value.MemberType == MemberTypes.Property select _pidx.Value )
      {
        foreach ( var _cax in _ax.GetCustomAttributes( false ) )
        {
          if ( _cax is ColumnAttribute )
          {
            ColumnAttribute _ca = _cax as ColumnAttribute;
            FieldInfo _strg = _mmbrs[ _ca.Storage ] as FieldInfo;
            object value = getValue( _ca.Name );
            _strg.SetValue( _newEntity, value );
          }
          else if ( _cax is AssociationAttribute )
          {
            AssociationAttribute _aa = _cax as AssociationAttribute;

          }
        }
      }
      if ( type.BaseType != typeof( Object ) )
        AssignValues( _newEntity, type.BaseType, getValue );
    }
    private static void GetValues( object entity, Type type, Method<string, object> assign )
    {
      Dictionary<string, MemberInfo> _mmbrs = GetMembers( type );
      foreach ( MemberInfo _ax in from _pidx in _mmbrs where _pidx.Value.MemberType == MemberTypes.Property select _pidx.Value )
        foreach ( var _cax in _ax.GetCustomAttributes( false ) )
          if ( _cax is ColumnAttribute )
          {
            ColumnAttribute _ca = _cax as ColumnAttribute;
            if ( _ca.ReadOnly )
              continue;
            FieldInfo _strg = _mmbrs[ _ca.Storage ] as FieldInfo;
            object value = _strg.GetValue( entity );
            assign( _ca.Name, value );
          }
          else if ( _cax is AssociationAttribute )
          {
            AssociationAttribute _aa = _cax as AssociationAttribute;
          }
      if ( type.BaseType != typeof( Object ) )
        GetValues( entity, type.BaseType, assign );

    }
    private static Dictionary<string, MemberInfo> GetMembers( Type type )
    {
      BindingFlags _flgs = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.GetField | BindingFlags.Public | BindingFlags.NonPublic;
      Dictionary<string, MemberInfo> _mmbrs = ( from _midx in type.GetMembers( _flgs )
                                                where _midx.MemberType == MemberTypes.Field || _midx.MemberType == MemberTypes.Property
                                                select _midx ).ToDictionary<MemberInfo, string>( _mi => _mi.Name );
      return _mmbrs;
    }
    private delegate void Method<T1, T2>( T1 arg1, T2 arg2 );
    #endregion
  }
}
