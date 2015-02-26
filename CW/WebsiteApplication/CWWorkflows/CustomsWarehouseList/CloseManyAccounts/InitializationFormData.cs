//_______________________________________________________________
//  Title   : InitializationFormData
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2015, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

namespace CAS.SmartFactory.CW.Workflows.CustomsWarehouseList.CloseManyAccounts
{
  [System.Runtime.Serialization.DataContract]
  public class InitializationFormData
  {
    
    [System.Runtime.Serialization.DataMember]
    public int[] AccountsArray
    {
      get { return b_AccountsArray; }
      set { b_AccountsArray = value; }
    }
    private int[] b_AccountsArray;

  }
}
