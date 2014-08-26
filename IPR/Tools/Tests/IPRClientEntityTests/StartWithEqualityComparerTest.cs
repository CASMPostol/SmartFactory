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
      StartWithEqualityComparer _cmprr = new StartWithEqualityComparer();
      Dictionary<string, string> _dic = new Dictionary<string, string>(_cmprr);
      _dic.Add(m_ShortString, m_ShortString);
      _dic.Add(m_LongString, m_LongString);
      _cmprr.LockComparer();
      Assert.IsTrue(_dic.ContainsKey(m_ShortString));
      Assert.AreEqual<string>(m_ShortString, _dic[m_ShortString]);
      Assert.IsTrue(_dic.ContainsKey(m_LongString + m_ShortString));
      Assert.AreEqual<string>(m_LongString, _dic[m_LongString + m_ShortString]);
      Assert.IsTrue(_dic.ContainsKey(m_LongString + "dadadasasasad"));
      Assert.AreEqual<string>(m_LongString, _dic[m_LongString + "0988776iujhkjhkjhjk"]);
    }
    [TestMethod]
    public void StartWithEqualityComparerTestMethod()
    {
      StartWithEqualityComparer _comp = new StartWithEqualityComparer();
      _comp.GetHashCode(m_ShortString);
      _comp.LockComparer();
      Assert.IsTrue(_comp.Equals(m_ShortString, m_LongString));
      Assert.IsTrue(_comp.Equals(m_LongString, m_ShortString));

    }
    private string m_ShortString = "0x0101001A9445DBA29F4E1DB1813989F34483DF";
    private string m_LongString = "0x0101001A9445DBA29F4E1DB1813989F34483DF00C06B02B58FF1C4419F69CEA1D9064E66";

  }
}
