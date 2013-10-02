﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using Microsoft.SharePoint.Client;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{
  /// <summary>
  ///  Provides client site LINQ (Language Integrated Query) access to, and change tracking for, the lists and document libraries of a Windows SharePoint Services "14" Web site.
  /// </summary>
  public class DataContext : IDisposable
  {

    #region public
    /// <summary>
    /// Initializes a new instance of the <see cref="DataContext" /> class.
    /// </summary>
    /// <param name="requestUrl">The URL of a Windows SharePoint Services "14" Web site that provides client site access and change tracking for the specified Web site..</param>
    public DataContext(string requestUrl)
    {
      // Open the current ClientContext
      m_ClientContext = new ClientContext(requestUrl);
      m_site = m_ClientContext.Site;
      m_ClientContext.Load<Site>(m_site);
      m_RootWeb = m_site.RootWeb;
      m_ClientContext.Load<Web>(m_RootWeb);
      this.ObjectTrackingEnabled = true;
    }
    /// <summary>
    /// Gets a collection of objects that represent discrepancies between the current client value and the current database value of a field in a list item.
    /// </summary>
    /// <value>
    ///  A Microsoft.SharePoint.Linq.ChangeConflictCollection each of whose members represents a discrepancy.
    /// </value>
    public ChangeConflictCollection ChangeConflicts { get; internal set; }
    /// <summary>
    /// Gets or sets a value indicating whether the LINQ to SharePoint provider should allow delay loading of Microsoft.SharePoint.Linq.EntityRef and Microsoft.SharePoint.Linq.EntitySet objects.
    /// </summary>
    /// <value>
    /// true if [deferred loading enabled]; otherwise, false.
    /// </value>
    public bool DeferredLoadingEnabled { get; set; }
    /// <summary>
    /// Gets or sets an object that will write the Collaborative Application Markup Language (CAML) query that results from the translation of the LINQ query.
    /// </summary>
    /// <value>
    /// A System.IO.TextWriter that can be called to write the CAML query.
    /// </value>
    public TextWriter Log { get; set; }
    /// <summary>
    /// Gets or sets a value that indicates whether changes in objects are tracked..
    /// </summary>
    /// <value>
    /// true, if changes are tracked; false otherwise.
    /// </value>
    public bool ObjectTrackingEnabled { get; set; }
    /// <summary>
    /// Gets the full URL of the Web site whose data is represented by the Microsoft.SharePoint.Linq.DataContext object.
    /// </summary>
    /// <value>
    /// A System.String that represents the full URL of the Web site.
    /// </value>
    public string Web { get { return m_ClientContext.Url; } }
    /// <summary>
    /// Returns an object that represents the specified list and is queryable by LINQ (Language Integrated Query).
    /// </summary>
    /// <typeparam name="T">The content type of the list items.</typeparam>
    /// <param name="listName">The name of the list.</param>
    /// <returns>An Microsoft.SharePoint.Linq.EntityList<TEntity> that represents the list.</returns>
    public virtual EntityList<T> GetList<T>(string listName)
       where T : class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
    {
      return GetList<T>(listName, CamlQuery.CreateAllItemsQuery());
    }
    internal virtual EntityList<T> GetList<T>(string listName, CamlQuery camlQuery)
       where T : class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
    {
      if (m_AllLists.ContainsKey(listName))
        return (EntityList<T>)m_AllLists[listName];
      EntityList<T> _ret = new EntityList<T>(this, listName);
      m_AllLists.Add(listName, _ret);
      return _ret;
    }
    /// <summary>
    /// Refreshes a collection of entities with the latest data from the content database according to the specified mode.
    /// </summary>
    /// <param name="mode">A value that specifies how to resolve differences between the current client values and the database values.</param>
    /// <param name="entities"> The entities that are refreshed.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void Refresh(RefreshMode mode, IEnumerable entities) { throw new NotImplementedException(); }
    /// <summary>
    /// Refreshes the specified entity with the latest data from the content database according to the specified mode.
    /// </summary>
    /// <param name="mode">A value that specifies how to resolve differences between the current client values and the database values.</param>
    /// <param name="entity">The object that is refreshed.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void Refresh(RefreshMode mode, object entity) { throw new NotImplementedException(); }
    /// <summary>
    /// Refreshes an array of entities with the latest data from the content database according to the specified mode.
    /// </summary>
    /// <param name="mode">A value that specifies how to resolve differences between the current client values and the database values.</param>
    /// <param name="entities">The entities that are refreshed.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void Refresh(RefreshMode mode, params object[] entities) { throw new NotImplementedException(); }
    /// <summary>
    /// Enables continued reading and writing to an Microsoft.SharePoint.Linq.EntityList even after it has been renamed.
    /// </summary>
    /// <typeparam name="T">The type of the list items.</typeparam>
    /// <param name="newListName">The new name of the list.</param>
    /// <param name="oldListName">The old name of the list.</param>
    public void RegisterList<T>(string newListName, string oldListName) { throw new NotImplementedException(); }
    /// <summary>
    /// Enables continued reading and writing to an Microsoft.SharePoint.Linq.EntityList even after it has been moved to another Web site.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="newListName">The new name of the list.</param>
    /// <param name="newWebUrl">The URL of the Web site to which the list was moved.</param>
    /// <param name="oldListName">The old name of the list.</param>
    public void RegisterList<T>(string newListName, string newWebUrl, string oldListName) { throw new NotImplementedException(); }
    /// <summary>
    /// Persists to the content database changes made by the current user to one
    /// or more lists using the specified failure mode; or, if a concurrency conflict
    /// is found, populates the Microsoft.SharePoint.Linq.DataContext.ChangeConflicts
    /// property.
    /// </summary>
    public void SubmitChanges()
    {
      foreach (EntityListData _elx in m_AllLists.Values)
        _elx.SubmitingChanges();
    }
    /// <summary>
    /// Persists to the content database changes made by the current user to one
    /// or more lists using the specified failure mode; or, if a concurrency conflict
    /// is found, populates the Microsoft.SharePoint.Linq.DataContext.ChangeConflicts
    /// property.
    /// </summary>
    /// <param name="failureMode">A value that specifies when a concurrency conflict should stop the attempt to persist changes and roll back any changes already made.</param>
    /// <exception cref="System.NotImplementedException">Microsoft.SharePoint.Linq.DataContext.ObjectTrackingEnabled is false- or
    ///     -At least one conflict in Microsoft.SharePoint.Linq.DataContext.ChangeConflicts
    ///     from the last time Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges
    ///     was called is not yet resolved.</exception>
    public void SubmitChanges(ConflictMode failureMode) { throw new NotImplementedException(); }
    /// <summary>
    /// Persists, to the content database, changes made by the current user to one
    /// or more lists using the specified failure mode and the specified indication
    /// of whether the versions of changed list items should be incremented; or,
    /// if a concurrency conflict is found, populates the Microsoft.SharePoint.Linq.DataContext.ChangeConflicts
    /// property.
    /// </summary>
    /// <param name="failureMode">A value that specifies when a concurrency conflict should stop the attempt
    /// to persist changes and roll back any changes already made.</param>
    /// <param name="systemUpdate">if set to true [system update].</param>
    /// <exception cref="System.NotImplementedException">Microsoft.SharePoint.Linq.DataContext.ObjectTrackingEnabled is false- or
    ///     -At least one conflict in Microsoft.SharePoint.Linq.DataContext.ChangeConflicts
    ///     from the last time Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges
    ///     was called is not yet resolved.</exception>
    public void SubmitChanges(ConflictMode failureMode, bool systemUpdate) { throw new NotImplementedException(); }
    #endregion

    #region internal
    internal void SubmitChanges(string listName)
    {
      if (m_AllLists.ContainsKey(listName))
        m_AllLists[listName].SubmitingChanges();
    }
    internal interface IRegister
    {
      void RegisterInContext(DataContext DataContext, AssociationAttribute AssociationAttribute);
    }
    internal interface IAssociationAttribute
    {
      FieldLookupValue Lookup { get; set; }
    }
    internal FieldLookupValue GetFieldLookupValue<TEntity>(string listName, TEntity entity)
      where TEntity : class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
    {
      return GetList<TEntity>(listName).GetFieldLookupValue(entity);
    }
    internal TEntity GetFieldLookupValue<TEntity>(string listName, FieldLookupValue fieldLookupValue)
      where TEntity : class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
    {
      return GetList<TEntity>(listName).GetFieldLookupValue(fieldLookupValue);
    }
    internal ClientContext m_ClientContext = default(ClientContext);
    internal Site m_site { get; set; }
    internal Web m_RootWeb { get; set; }
    internal void ExecuteQuery()
    {
      m_ClientContext.ExecuteQuery();
    }
    internal void ReloadNewListItem(ListItem _newListItem)
    {
      m_ClientContext.Load(_newListItem);
      m_ClientContext.ExecuteQuery();
    }
    #endregion

    #region IDisposing
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      // This object will be cleaned up by the Dispose method. 
      // Therefore, you should call GC.SupressFinalize to 
      // take this object off the finalization queue 
      // and prevent finalization code for this object 
      // from executing a second time.
      GC.SuppressFinalize(this);
    }

    //
    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.  Dispose(bool disposing) executes in two distinct scenarios. 
    /// If disposing equals true, the method has been called directly or indirectly by a user's code. Managed and unmanaged resources
    /// can be disposed. If disposing equals false, the method has been called by the runtime from inside the finalizer 
    /// and you should not reference other objects. Only unmanaged resources can be disposed. 
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      // Check to see if Dispose has already been called. 
      if (!this.disposed)
      {
        // If disposing equals true, dispose all managed 
        // and unmanaged resources. 
        if (disposing)
        {
          // Dispose managed resources.
          m_ClientContext.Dispose();
        }
        // Note disposing has been done.
        disposed = true;
      }
    }
    // Use C# destructor syntax for finalization code. 
    // This destructor will run only if the Dispose method 
    // does not get called. 
    // It gives your base class the opportunity to finalize. 
    // Do not provide destructors in types derived from this class.
    ~DataContext()
    {
      // Do not re-create Dispose clean-up code here. 
      // Calling Dispose(false) is optimal in terms of 
      // readability and maintainability.
      Dispose(false);
    }
    // Track whether Dispose has been called. 
    private bool disposed = false;
    #endregion

    #region private
    private Dictionary<string, EntityListData> m_AllLists = new Dictionary<string, EntityListData>();
    #endregion


  }
}