//<summary>
//  Title   : public interface IArchival
//  System  : Microsoft VisulaStudio 2013 / C#
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

namespace CAS.SmartFactory.IPR.Client.DataManagement.Linq
{
  /// <summary>
  /// The interface provides access to the Archival bit used for some list.
  /// </summary>
  public interface IArchival
  {
    /// <summary>
    /// Gets or sets the archival bit.
    /// </summary>
    /// <value>
    /// The archival bit.
    /// </value>
    System.Nullable<bool> Archival { get; set; }
  }
}
