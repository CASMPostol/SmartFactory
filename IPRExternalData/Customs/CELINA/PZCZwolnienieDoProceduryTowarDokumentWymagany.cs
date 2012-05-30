
namespace CAS.SmartFactory.xml.Customs.PZC
{
  public partial class PZCZwolnienieDoProceduryTowarDokumentWymagany: RequiredDocumentsDescription
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
