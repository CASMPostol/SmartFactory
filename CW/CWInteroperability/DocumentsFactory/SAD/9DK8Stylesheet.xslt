<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ms="urn:schemas-microsoft-com:xslt"
    xmlns:sad="http://www.krakow.uc.gov.pl/Celina/CLN-XML/xsd/SADw2r0.xsd"
>
  <xsl:output method="html" indent="yes"/>
  <xsl:decimal-format name="pl" decimal-separator=',' grouping-separator='.' />
  <xsl:variable name="FoarmatOfFloat" >###.##0,00</xsl:variable>
  <xsl:variable name="FoarmatOfdate" >dd-MM-yyyy</xsl:variable>
  <xsl:template match="/" >
    <html>
      <head>
        <title>
          9DK8Template
        </title>
        <style type="text/css">
          p  { font-size:11pt; }
          td { font-size:10pt; }
          th { font-size:11pt; }
          h2 { font-size:14pt; text-align:center; }
        </style>
      </head>
      <body>
        <xsl:apply-templates select="sad:SAD" />
      </body>
    </html>
  </xsl:template>
  <xsl:template match="sad:SAD">
    <xsl:apply-templates select="sad:Zgloszenie" />
  </xsl:template>
  <xsl:template match="sad:Zgloszenie">
    <p>Dokument:
        <xsl:choose>
          <xsl:when test="sad:Towar/sad:DokumentWymagany/@Kod='9DK8'"><xsl:value-of select="sad:Towar/sad:DokumentWymagany/@Nr"/></xsl:when>
          <xsl:when test="not(sad:Towar/sad:DokumentWymagany/@Kod='9DK8')">&#160;</xsl:when>
        </xsl:choose>     
    </p>
        <p>Data utworzenia:
        <xsl:choose>
          <xsl:when test="sad:Towar/sad:DokumentWymagany/@Kod='9DK8'"><xsl:value-of select="sad:Towar/sad:DokumentWymagany/@Uwagi"/></xsl:when>
          <xsl:when test="not(sad:Towar/sad:DokumentWymagany/@Kod='9DK8')">&#160;</xsl:when>
        </xsl:choose>     
    </p>
    <p>
      <b>ZGŁOSZENIE:</b>
    </p>
    <p>
      Numer własny:<xsl:value-of select="@NrWlasny" /> /
      P1a:<xsl:value-of select="@P1a" /> /
      P1b:<xsl:value-of select="@P1b" /> /
      Liczba Pozycji:<xsl:value-of select="@LiczbaPozycji" /> /
      Kraj Wysylki:<xsl:value-of select="@KrajWysylki" /> /
      Kraj Przeznaczenia:<xsl:value-of select="@KrajPrzeznaczenia" /> /
      Kontenery:<xsl:value-of select="@Kontenery" /> /
      Rodzaj Transakcji:<xsl:value-of select="@RodzajTransakcji" /> /
      Masa Brutto:<xsl:value-of select="@MasaBrutto" />
    </p>
    <p>
      <xsl:apply-templates select="sad:Rodzaj" />
    </p>
    <p>
      <xsl:apply-templates select="sad:UC" />
    </p>
    <p>
      <xsl:apply-templates select="sad:Nadawca"/>
    </p>
    <p>
      <xsl:apply-templates select="sad:Odbiorca"/>
    </p>
    <p>
      <xsl:apply-templates select="sad:WarunkiDostawy" />
    </p>
    <p>
      <xsl:apply-templates select="sad:WartoscTowarow" />
    </p>
    <p>
      <xsl:apply-templates select="sad:Towar" />
    </p>
  </xsl:template>
  <xsl:template match="sad:Rodzaj">
    <p>
      <b>Rodzaj</b>
    </p>
    <p>
      Typ: <xsl:value-of select="@Typ"/> /
      Podtyp: <xsl:value-of select="@Podtyp"/> /
      Powiadomienie: <xsl:value-of select="@Powiadomienie"/>
    </p>
  </xsl:template>
  <xsl:template match="sad:UC">
    <p>
      <b>UC:</b>
    </p>
    <p>
      UCZgloszenia: <xsl:value-of select="@UCZgloszenia"/> /
      UCGraniczny: <xsl:value-of select="@UCGraniczny"/>
    </p>
    <xsl:apply-templates select="sad:Lokalizacja" />
    <xsl:apply-templates select="sad:SkladCelny" />
  </xsl:template>
  <xsl:template match="sad:Lokalizacja">
    <p>Lokalizacja</p>
    <p>
      Miejsce: <xsl:value-of select="@Miejsce"/>
    </p>
  </xsl:template>
  <xsl:template match="sad:SkladCelny">
    <p>Sklad Celny</p>
    <p>
      Typ: <xsl:value-of select="@Typ"/> /
      Miejsce: <xsl:value-of select="@Miejsce"/> /
      Kraj: <xsl:value-of select="@Kraj"/>
    </p>
  </xsl:template>
  <xsl:template match="sad:Nadawca">
    <p>
      <b>Nadawca</b>
    </p>
    <p>
      PozId: <xsl:value-of select="@PozId"/> /
      Nazwa: <xsl:value-of select="@Nazwa"/> /
      UlicaNumer: <xsl:value-of select="@UlicaNumer"/> /
      KodPocztowy: <xsl:value-of select="@KodPocztowy"/> /
      Miejscowosc: <xsl:value-of select="@Miejscowosc"/> /
      Kraj: <xsl:value-of select="@Kraj"/>
    </p>
  </xsl:template>
  <xsl:template match="sad:Odbiorca">
    <p>
      <b>Odbiorca</b>
    </p>
    <p>
      PozId: <xsl:value-of select="@PozId"/> /
      Nazwa: <xsl:value-of select="@Nazwa"/> /
      UlicaNumer: <xsl:value-of select="@UlicaNumer"/> /
      KodPocztowy: <xsl:value-of select="@KodPocztowy"/> /
      Miejscowosc: <xsl:value-of select="@Miejscowosc"/> /
      Kraj: <xsl:value-of select="@Kraj"/> /
      TIN: <xsl:value-of select="@TIN"/> /
      EORI: <xsl:value-of select="@EORI"/>
    </p>
  </xsl:template>
  <xsl:template match="sad:WarunkiDostawy">
    <p>
      <b>WarunkiDostawy</b>
    </p>
    <p>
      Kod: <xsl:value-of select="@Kod"/> /
      Miejsce: <xsl:value-of select="@Miejsce"/> /
      MiejsceKod: <xsl:value-of select="@MiejsceKod"/>
    </p>
  </xsl:template>
  <xsl:template match="sad:WartoscTowarow">
    <p>
      <b>WartoscTowarow</b>
    </p>
    <p>
      Wartosc: <xsl:value-of select="@Wartosc"/> /
      Waluta: <xsl:value-of select="@Waluta"/> /
      KursWaluty: <xsl:value-of select="@KursWaluty"/>
    </p>
  </xsl:template>
  <xsl:template match="sad:Towar">
    <p>
      <b>Towar</b>
    </p>
    <p>
      PozId: <xsl:value-of select="@PozId"/> /
      OpisTowaru: <xsl:value-of select="@OpisTowaru"/> /
      KodTowarowy: <xsl:value-of select="@KodTowarowy"/> /
      KodTaric: <xsl:value-of select="@KodTaric"/> /
      MasaBrutto: <xsl:value-of select="@MasaBrutto"/> /
      Procedura: <xsl:value-of select="@Procedura"/> /
      MasaNetto: <xsl:value-of select="@MasaNetto"/> /
    </p>
    <p>
      <xsl:apply-templates select="sad:IloscTowaru" />
    </p>
    <p>
      <xsl:apply-templates select="sad:Opakowanie" />
    </p>
    <p>
      <xsl:apply-templates select="sad:DokumentPoprzedni" />
    </p>
    <p>
      <xsl:apply-templates select="sad:DokumentWymagany" />
    </p>
    <p>
      <xsl:apply-templates select="sad:WartoscTowaru" />
    </p>
  </xsl:template>
  <xsl:template match="sad:IloscTowaru">
    <p>IloscTowaru</p>
    <p>
      PozId: <xsl:value-of select="@PozId"/> /
      Jm: <xsl:value-of select="@Jm"/> /
      Ilosc: <xsl:value-of select="@Ilosc"/>
    </p>
  </xsl:template>
  <xsl:template match="sad:Opakowanie">
    <p>Opakowanie</p>
    <p>
      PozId: <xsl:value-of select="@PozId"/> /
      LiczbaOpakowan: <xsl:value-of select="@LiczbaOpakowan"/>
    </p>
  </xsl:template>
  <xsl:template match="sad:DokumentPoprzedni">
    <p>DokumentPoprzedni</p>
    <p>
      PozId: <xsl:value-of select="@PozId"/> /
      Kategoria: <xsl:value-of select="@Kategoria"/> /
      Nr: <xsl:value-of select="@Nr"/> /
      NrCelina: <xsl:value-of select="@NrCelina"/>
    </p>
  </xsl:template>
  <xsl:template match="sad:DokumentWymagany">
    <p>DokumentWymagany</p>
    <p>
      PozId: <xsl:value-of select="@PozId"/> /
      Kod: <xsl:value-of select="@Kod"/> /
    </p>
  </xsl:template>
  <xsl:template match="sad:WartoscTowaru">
    <p>WartoscTowaru</p>
    <p>
      WartoscPozycji: <xsl:value-of select="@WartoscPozycji"/> /
      MetodaWartosciowania: <xsl:value-of select="@MetodaWartosciowania"/> /
      WartoscStatystyczna: <xsl:value-of select="@WartoscStatystyczna"/>
    </p>
    <p>
      <xsl:apply-templates select="sad:SzczegolyWartosci" />
    </p>
  </xsl:template>
  <xsl:template match="sad:SzczegolyWartosci">
    <p>SzczegolyWartosci</p>
    <p>
      PozId: <xsl:value-of select="@PozId"/> /
      Kod: <xsl:value-of select="@Kod"/> /
    </p>
  </xsl:template>
</xsl:stylesheet>
