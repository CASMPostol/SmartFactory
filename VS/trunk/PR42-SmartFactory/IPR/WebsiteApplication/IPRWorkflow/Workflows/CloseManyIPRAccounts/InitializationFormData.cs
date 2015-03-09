//_______________________________________________________________
//  Title   : InitializationFormData
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate: 2015-02-26 12:14:26 +0100 (Cz, 26 lut 2015) $
//  $Rev: 11417 $
//  $LastChangedBy: mpostol $
//  $URL: svn://svnserver.hq.cas.com.pl/VS/trunk/PR42-SmartFactory/CW/WebsiteApplication/CWWorkflows/CustomsWarehouseList/CloseManyAccounts/InitializationFormData.cs $
//  $Id: InitializationFormData.cs 11417 2015-02-26 11:14:26Z mpostol $
//
//  Copyright (C) 2015, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

namespace CAS.SmartFactory.IPR.Workflows.CloseManyIPRAccounts
{
  /// <summary>
  /// Class InitializationFormData - <see cref="System.Runtime.Serialization.DataContract"/> containing information entered using the form.
  /// </summary>
  [System.Runtime.Serialization.DataContract]
  public class InitializationFormData
  {

    /// <summary>
    /// Gets or sets the accounts identifiers array.
    /// </summary>
    /// <value>The accounts array.</value>
    [System.Runtime.Serialization.DataMember]
    public int[] AccountsArray
    {
      get { return b_AccountsArray; }
      set { b_AccountsArray = value; }
    }
    private int[] b_AccountsArray;

  }
}
