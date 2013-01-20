
namespace CAS.SmartFactory.xml.Customs.SAD
{
  public partial class SADZgloszenieTowarDokumentWymagany: RequiredDocumentsDescription
  {
    public override string GetCode()
    {
      return this.Kod;
    }
    public override string GetNumber()
    {
      return this.Nr;
    }
  }
}
