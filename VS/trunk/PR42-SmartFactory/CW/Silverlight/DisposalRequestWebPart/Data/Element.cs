//<summary>
//  Title   : partial class Element 
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
      
namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{
  public partial class Element : ITrackEntityState, ITrackOriginalValues, System.ComponentModel.INotifyPropertyChanged, System.ComponentModel.INotifyPropertyChanging
  {

    #region private
    private System.Nullable<int> _id;
    private System.Nullable<int> _version;
    private string _path;
    private EntityState _entityState;
    private System.Collections.Generic.IDictionary<string, object> _originalValues;
    private string _title;
    #endregion

    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate();
    partial void OnCreated();
    #endregion

    #region ITrackEntityState
    EntityState ITrackEntityState.EntityState
    {
      get
      {
        return this._entityState;
      }
      set
      {
        if ( ( value != this._entityState ) )
        {
          this._entityState = value;
        }
      }
    }
    #endregion

    #region ITrackOriginalValues
    System.Collections.Generic.IDictionary<string, object> ITrackOriginalValues.OriginalValues
    {
      get
      {
        if ( ( null == this._originalValues ) )
        {
          this._originalValues = new System.Collections.Generic.Dictionary<string, object>();
        }
        return this._originalValues;
      }
    }
    #endregion

    #region ctor
    public Element()
    {
      this.OnCreated();
    }
    #endregion

    #region public
    [ColumnAttribute( Name = "ID", Storage = "_id", ReadOnly = true, FieldType = "Counter" )]
    public System.Nullable<int> Id
    {
      get
      {
        return this._id;
      }
      set
      {
        if ( ( value != this._id ) )
        {
          this.OnPropertyChanging( "Id", this._id );
          this._id = value;
          this.OnPropertyChanged( "Id" );
        }
      }
    }
    [ColumnAttribute( Name = "owshiddenversion", Storage = "_version", ReadOnly = true, FieldType = "Integer" )]
    public System.Nullable<int> Version
    {
      get
      {
        return this._version;
      }
      set
      {
        if ( ( value != this._version ) )
        {
          this.OnPropertyChanging( "Version", this._version );
          this._version = value;
          this.OnPropertyChanged( "Version" );
        }
      }
    }
    [ColumnAttribute( Name = "FileDirRef", Storage = "_path", ReadOnly = true, FieldType = "Lookup", IsLookupValue = true )]
    public string Path
    {
      get
      {
        return this._path;
      }
      set
      {
        if ( ( value != this._path ) )
        {
          this.OnPropertyChanging( "Path", this._path );
          this._path = value;
          this.OnPropertyChanged( "Path" );
        }
      }
    }
    [ColumnAttribute( Name = "Title", Storage = "_title", Required = true, FieldType = "Text" )]
    public string Title
    {
      get
      {
        return this._title;
      }
      set
      {
        if ( ( value != this._title ) )
        {
          this.OnPropertyChanging( "Title", this._title );
          this._title = value;
          this.OnPropertyChanged( "Title" );
        }
      }
    }
    public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
    #endregion

    #region protected 
    protected virtual void OnPropertyChanged( string propertyName )
    {
      if ( ( null != this.PropertyChanged ) )
      {
        this.PropertyChanged( this, new System.ComponentModel.PropertyChangedEventArgs( propertyName ) );
      }
    }
    protected virtual void OnPropertyChanging( string propertyName, object value )
    {
      if ( ( null == this._originalValues ) )
      {
        this._originalValues = new System.Collections.Generic.Dictionary<string, object>();
      }
      if ( ( false == this._originalValues.ContainsKey( propertyName ) ) )
      {
        this._originalValues.Add( propertyName, value );
      }
      if ( ( null != this.PropertyChanging ) )
      {
        this.PropertyChanging( this, new System.ComponentModel.PropertyChangingEventArgs( propertyName ) );
      }
    }
    #endregion

  }
}
