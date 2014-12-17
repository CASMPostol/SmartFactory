using CAS.SmartFactory.Shepherd.Client.Management.Services;
using Microsoft.Practices.Prism.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using System.Diagnostics;

namespace CAS.SmartFactory.Shepherd.Client.Management.Tests
{
  [TestClass]
  public class NamedTraceLoggerUnitTest
  {

    [TestMethod]
    public void NamedTraceLoggerCreationTestMethod()
    {
      Assert.IsNotNull(NamedTraceLogger.Logger);
      Assert.AreEqual<string>("CAS.SmartFactory.Shepherd.Client", NamedTraceLogger.Logger.Name);
      NamedTraceLogger.Logger.Close();
    }

    [TestMethod]
    public void NamedTraceLoggerTraceEventTestMethod()
    {
      //initialize
      Assert.IsNotNull(NamedTraceLogger.Logger);
      NamedTraceLogger.Logger.Listeners.Clear();
      MyTraceLisner _MyTraceLisner = new MyTraceLisner();
      NamedTraceLogger.Logger.Listeners.Add(_MyTraceLisner);
      NamedTraceLogger.Logger.Switch = new SourceSwitch("MySourceSwitch") { Level = SourceLevels.All };
      //Test it
      NamedTraceLogger.Logger.TraceEvent(TraceEventType.Critical, 0, m_Message);
      Assert.AreEqual<string>(@"CAS.SmartFactory.Shepherd.Client Critical: 0 : ", _MyTraceLisner.WriteMessage);
      Assert.AreEqual<string>(m_Message, _MyTraceLisner.WriteLineMessage);
      NamedTraceLogger.Logger.TraceProgressChange(new ProgressChangedEventArgs(0, m_Message));
      Assert.AreEqual<string>(@"CAS.SmartFactory.Shepherd.Client Information: 1 : ", _MyTraceLisner.WriteMessage);
      Assert.AreEqual<string>(m_Message, _MyTraceLisner.WriteLineMessage);
      NamedTraceLogger.Logger.Log(m_Message, Category.Debug, Priority.High);
      Assert.AreEqual<string>(@"CAS.SmartFactory.Shepherd.Client Critical: 0 : ", _MyTraceLisner.WriteMessage);
      Assert.IsTrue(_MyTraceLisner.WriteLineMessage.Contains("DEBUG: Test message. Priority: High. Time-stamp:"));
    }

    private class MyTraceLisner : TraceListener
    {
      public MyTraceLisner() { }
      public override void Write(string message)
      {
        WriteMessage = message;
      }
      public override void WriteLine(string message)
      {
        WriteLineMessage = message;
      }
      internal string WriteMessage { get; private set; }
      internal string WriteLineMessage { get; private set; }
    }
    private const string m_Message = "Test message";

  }
}
