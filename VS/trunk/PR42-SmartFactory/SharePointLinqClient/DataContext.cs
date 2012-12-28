﻿using System;
using System.Collections;
using System.IO;

namespace Microsoft.SharePoint.Linq
{
  // Summary:
  //     Provides LINQ (Language Integrated Query) access to, and change tracking
  //     for, the lists and document libraries of a Windows SharePoint Services "14"
  //     Web site.
  public class DataContext: IDisposable
  {
    // Summary:
    //     Initializes a new instance of the Microsoft.SharePoint.Linq.DataContext class
    //     that provides access and change tracking for the specified Web site.
    //
    // Parameters:
    //   requestUrl:
    //     The URL of a Windows SharePoint Services "14" Web site.
    public DataContext( string requestUrl )  { throw new NotImplementedException(); } 

    // Summary:
    //     Gets a collection of objects that represent discrepancies between the current
    //     client value and the current database value of a field in a list item.
    //
    // Returns:
    //     A Microsoft.SharePoint.Linq.ChangeConflictCollection each of whose members
    //     represents a discrepancy.
    public ChangeConflictCollection ChangeConflicts { get; internal set; }
    //
    // Summary:
    //     Gets or sets a value indicating whether the LINQ to SharePoint provider should
    //     allow delay loading of Microsoft.SharePoint.Linq.EntityRef<TEntity> and Microsoft.SharePoint.Linq.EntitySet<TEntity>
    //     objects.
    //
    // Returns:
    //     true, if delayed loading should be used; false otherwise. The default is
    //     true.
    public bool DeferredLoadingEnabled { get; set; }
    //
    // Summary:
    //     Gets or sets an object that will write the Collaborative Application Markup
    //     Language (CAML) query that results from the translation of the LINQ query.
    //
    // Returns:
    //     A System.IO.TextWriter that can be called to write the CAML query.
    public TextWriter Log { get; set; }
    //
    // Summary:
    //     Gets or sets a value that indicates whether changes in objects are tracked.
    //
    // Returns:
    //     true, if changes are tracked; false otherwise.
    public bool ObjectTrackingEnabled { get; set; }
    //
    // Summary:
    //     Gets the full URL of the Web site whose data is represented by the Microsoft.SharePoint.Linq.DataContext
    //     object.
    //
    // Returns:
    //     A System.String that represents the full URL of the Web site.
    public string Web { get { throw new NotImplementedException(); } }

    // Summary:
    //     Releases all managed and unmanaged resources used by the Microsoft.SharePoint.Linq.DataContext
    //     object.
    public void Dispose() { throw new NotImplementedException(); }
    //
    // Summary:
    //     Releases all unmanaged resources used by the Microsoft.SharePoint.Linq.DataContext
    //     object and possibly also the managed resources as specified.
    //
    // Parameters:
    //   disposing:
    //     true to release both managed and unmanaged resources, false to release only
    //     unmanaged resources.
    protected virtual void Dispose( bool disposing ) { throw new NotImplementedException(); }
    //
    // Summary:
    //     Returns an object that represents the specified list and is queryable by
    //     LINQ (Language Integrated Query).
    //
    // Parameters:
    //   listName:
    //     The name of the list.
    //
    // Type parameters:
    //   T:
    //     The content type of the list items.
    //
    // Returns:
    //     An Microsoft.SharePoint.Linq.EntityList<TEntity> that represents the list.
    public virtual EntityList<T> GetList<T>( string listName ) { throw new NotImplementedException(); }
    //
    // Summary:
    //     Refreshes a collection of entities with the latest data from the content
    //     database according to the specified mode.
    //
    // Parameters:
    //   mode:
    //     A value that specifies how to resolve differences between the current client
    //     values and the database values.
    //
    //   entities:
    //     The entities that are refreshed.
    public void Refresh( RefreshMode mode, IEnumerable entities ) { throw new NotImplementedException(); }
    //
    // Summary:
    //     Refreshes the specified entity with the latest data from the content database
    //     according to the specified mode.
    //
    // Parameters:
    //   mode:
    //     A value that specifies how to resolve differences between the current client
    //     values and the database values.
    //
    //   entity:
    //     The object that is refreshed.
    public void Refresh( RefreshMode mode, object entity ) { throw new NotImplementedException(); }
    //
    // Summary:
    //     Refreshes an array of entities with the latest data from the content database
    //     according to the specified mode.
    //
    // Parameters:
    //   mode:
    //     A value that specifies how to resolve differences between the current client
    //     values and the database values.
    //
    //   entities:
    //     The entities that are refreshed.
    public void Refresh( RefreshMode mode, params object[] entities ) { throw new NotImplementedException(); }
    //
    // Summary:
    //     Enables continued reading and writing to an Microsoft.SharePoint.Linq.EntityList<TEntity>
    //     even after it has been renamed.
    //
    // Parameters:
    //   newListName:
    //     The new name of the list.
    //
    //   oldListName:
    //     The old name of the list.
    //
    // Type parameters:
    //   T:
    //     The type of the list items.
    public void RegisterList<T>( string newListName, string oldListName ) { throw new NotImplementedException(); }
    //
    // Summary:
    //     Enables continued reading and writing to an Microsoft.SharePoint.Linq.EntityList<TEntity>
    //     even after it has been moved to another Web site.
    //
    // Parameters:
    //   newListName:
    //     The new name of the list.
    //
    //   newWebUrl:
    //     The URL of the Web site to which the list was moved.
    //
    //   oldListName:
    //     The old name of the list.
    //
    // Type parameters:
    //   T:
    //     The type of the list items.
    public void RegisterList<T>( string newListName, string newWebUrl, string oldListName ) { throw new NotImplementedException(); }
    //
    // Summary:
    //     Persists to the content database changes made by the current user to one
    //     or more lists using the specified failure mode; or, if a concurrency conflict
    //     is found, populates the Microsoft.SharePoint.Linq.DataContext.ChangeConflicts
    //     property.
    //
    // Exceptions:
    //   System.InvalidOperationException:
    //     Microsoft.SharePoint.Linq.DataContext.ObjectTrackingEnabled is false- or
    //     -At least one conflict in Microsoft.SharePoint.Linq.DataContext.ChangeConflicts
    //     from the last time Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges
    //     was called is not yet resolved.
    //
    //   Microsoft.SharePoint.Linq.ChangeConflictException:
    //     There is a concurrency conflict.
    public void SubmitChanges() { throw new NotImplementedException(); }
    //
    // Summary:
    //     Persists to the content database changes made by the current user to one
    //     or more lists using the specified failure mode; or, if a concurrency conflict
    //     is found, populates the Microsoft.SharePoint.Linq.DataContext.ChangeConflicts
    //     property.
    //
    // Parameters:
    //   failureMode:
    //     A value that specifies when a concurrency conflict should stop the attempt
    //     to persist changes and roll back any changes already made.
    //
    // Exceptions:
    //   System.InvalidOperationException:
    //     Microsoft.SharePoint.Linq.DataContext.ObjectTrackingEnabled is false- or
    //     -At least one conflict in Microsoft.SharePoint.Linq.DataContext.ChangeConflicts
    //     from the last time Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges
    //     was called is not yet resolved.
    //
    //   Microsoft.SharePoint.Linq.ChangeConflictException:
    //     There is a concurrency conflict.
    public void SubmitChanges( ConflictMode failureMode ) { throw new NotImplementedException(); }
    //
    // Summary:
    //     Persists, to the content database, changes made by the current user to one
    //     or more lists using the specified failure mode and the specified indication
    //     of whether the versions of changed list items should be incremented; or,
    //     if a concurrency conflict is found, populates the Microsoft.SharePoint.Linq.DataContext.ChangeConflicts
    //     property.
    //
    // Parameters:
    //   failureMode:
    //     A value that specifies when a concurrency conflict should stop the attempt
    //     to persist changes and roll back any changes already made.
    //
    //   overwriteVersion:
    //     true to not increment the version of a changed item, false to increment it.
    //
    // Exceptions:
    //   System.InvalidOperationException:
    //     Microsoft.SharePoint.Linq.DataContext.ObjectTrackingEnabled is false- or
    //     -At least one conflict in Microsoft.SharePoint.Linq.DataContext.ChangeConflicts
    //     from the last time Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges
    //     was called is not yet resolved.
    //
    //   Microsoft.SharePoint.Linq.ChangeConflictException:
    //     There is a concurrency conflict.
    public void SubmitChanges( ConflictMode failureMode, bool systemUpdate ) { throw new NotImplementedException(); }
  }
}
