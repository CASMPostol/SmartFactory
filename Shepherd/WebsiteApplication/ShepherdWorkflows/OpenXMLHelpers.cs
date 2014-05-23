using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.SharePoint;

namespace CAS.SmartFactory.Shepherd.Workflows
{
  internal static class OpenXMLHelpers
  {
    internal static SPFile AddDocument2Collection(SPFile _template, SPFileCollection _dstCollection, string _fileName)
    {
      SPFile _docFile = default(SPFile);
      using (Stream _tmpStrm = _template.OpenBinaryStream())
      using (MemoryStream _docStrm = new MemoryStream())
      {
        byte[] _buff = new byte[_tmpStrm.Length + 200];
        int _leng = _tmpStrm.Read(_buff, 0, (int)_tmpStrm.Length);
        _docStrm.Write(_buff, 0, _leng);
        _docStrm.Position = 0;
        WordprocessingDocument _doc = WordprocessingDocument.Open(_docStrm, true);
        _doc.ChangeDocumentType(WordprocessingDocumentType.Document);
        _doc.Close();
        _docFile = _dstCollection.Add(_fileName, _docStrm, true);
        _docStrm.Flush();
        _docStrm.Close();
      }
      return _docFile;
    }
  }
}
