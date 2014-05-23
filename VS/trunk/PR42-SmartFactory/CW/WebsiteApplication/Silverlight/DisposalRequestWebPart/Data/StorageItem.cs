using System;
using System.Collections.Generic;
using System.Reflection;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{
  internal class StorageItem
  {
    internal static Type GetEnumValues( StorageItem _storage, Dictionary<string, string> _values, bool fildNameIsKey )
    {
      Type[] _types = _storage.Storage.FieldType.GetGenericArguments();
      if ( _types.Length != 1 )
        throw new ArgumentException( "Unexpected type in the AssignValues2Entity" );
      FieldInfo[] _fields = _types[ 0 ].GetFields();
      foreach ( FieldInfo _fld in _fields )
      {
        object[] _attrbts = _fld.GetCustomAttributes( false );
        foreach ( Attribute _attr in _attrbts )
        {
          ChoiceAttribute _ca = _attr as ChoiceAttribute;
          if ( _ca == null )
            continue;
          if ( fildNameIsKey )
            _values.Add( _fld.Name, _ca.Value );
          else
            _values.Add( _ca.Value, _fld.Name );
        }
      }
      return _types[ 0 ];
    }
    internal StorageItem( string propertyName, DataAttribute description, FieldInfo storage )
    {
      PropertyName = propertyName;
      Association = description is AssociationAttribute;
      Description = description;
      Storage = storage;
    }
    internal string PropertyName { get; private set; }
    internal bool Association { get; private set; }
    internal DataAttribute Description { get; private set; }
    internal FieldInfo Storage { get; private set; }
    internal bool IsLookup
    {
      get
      {
        if ( !Association )
          return false;
        return ( (AssociationAttribute)Description ).MultivalueType == AssociationType.Single;
      }
    }
  }
}
