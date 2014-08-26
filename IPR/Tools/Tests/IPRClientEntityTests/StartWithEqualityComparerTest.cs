using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using CAS.SharePoint.Client.Linq2SP;

namespace CAS.SmartFactory.IPR.Client.DataManagement.Tests
{
  [TestClass]
  public class StartWithEqualityComparerTest
  {
    [TestMethod]
    public void DictionaryTest()
    {
      Dictionary<string, string> _dic = new Dictionary<string, string>(new StartWithEqualityComparer());
      _dic.Add(m_ShortString, m_ShortString);
      //_dic.Add(m_ShortString + m_ShortString, m_ShortString);
      _dic.Add(m_LongString, m_LongString);
      //_dic.Add(m_LongString, m_LongString);
      Assert.IsTrue(_dic.ContainsKey(m_LongString));
    }
    [TestMethod]
    public void StartWithEqualityComparerTestMethod()
    {
      StartWithEqualityComparer _comp = new StartWithEqualityComparer();
      Assert.IsTrue(_comp.Equals(m_ShortString, m_LongString));
      Assert.IsTrue(_comp.Equals(m_LongString, m_ShortString));

    }
    private string m_ShortString = "0x0101001A9445DBA29F4E1DB1813989F34483DF";
    private string m_LongString = "0x0101001A9445DBA29F4E1DB1813989F34483DF00C06B02B58FF1C4419F69CEA1D9064E66";

  }
}
