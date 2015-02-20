<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ms="urn:schemas-microsoft-com:xslt"
    xmlns:cas="http://www.cas.eu/SmartFactory.Customs.Messages.CELINA.SAD.xsd"
    xmlns:sad="http://www.krakow.uc.gov.pl/Celina/CLN-XML/xsd/SADw2r0.xsd">
  <xsl:output method="html" indent="yes"/>
  <xsl:decimal-format name="pl" decimal-separator=',' grouping-separator='.' />
  <xsl:variable name="FoarmatOfFloat" >#####0,00</xsl:variable>
  <xsl:variable name="FoarmatOfdate" >dd-MM-yyyy</xsl:variable>
  <xsl:template match="/" >
    <html>
      <head>
        <title>
          SAD Collection
        </title>
        <style type="text/css">
          p  { font-size:11pt; }
          td { font-size:10pt; }
          th { font-size:11pt; }
          h2 { font-size:14pt; text-align:center; }
        </style>
      </head>
      <body>
        <xsl:apply-templates select="cas:SADCollection" />
      </body>
    </html>
  </xsl:template>
  <xsl:template match="cas:SADCollection">
    <xsl:apply-templates select="cas:ListOfSAD" />
  </xsl:template>
  <xsl:template match="cas:ListOfSAD">
    <xsl:apply-templates select="cas:SAD" />
  </xsl:template>
  <xsl:template match="cas:SAD">
    <xsl:apply-templates select="sad:Zgloszenie" />
  </xsl:template>
  <xsl:template match="sad:Zgloszenie">
    <table cellspacing="0" cellpadding="0" border="0" align="center" width="100%">
      <tr>
        <td align="left">
          <p>
            Dokument:
            <xsl:choose>
              <xsl:when test="sad:Towar/sad:DokumentWymagany/@Kod='9DK8'">
                <xsl:value-of select="sad:Towar/sad:DokumentWymagany/@Nr"/>
              </xsl:when>
              <xsl:when test="not(sad:Towar/sad:DokumentWymagany/@Kod='9DK8')">&#160;</xsl:when>
            </xsl:choose>
          </p>
        </td>
        <td align="right">
          <p>
            Gostków Stary,
            <xsl:value-of select="sad:MiejsceData/@Data"/>
          </p>
        </td>
      </tr>
    </table>
    <p>
      <xsl:apply-templates select="sad:Nadawca"/>
    </p>
    <p>
      <xsl:apply-templates select="sad:Odbiorca"/>
    </p>
    <h2>ZGŁOSZENIE:</h2>
    <p>
      <xsl:apply-templates select="sad:WartoscTowarow" />
    </p>
    <p>
      <xsl:apply-templates select="sad:Towar" />
    </p>
  </xsl:template>
  <xsl:template match="sad:Nadawca">
    <table width="35%" cellspaing="10" cellpadding="10" border="1">
      <tr>
        <td>
          <p>
            <b>Nadawca</b>
          </p>
          <p>
            <xsl:value-of select="@Nazwa"/>
            <br />
            <xsl:value-of select="@UlicaNumer"/>
            <br />
            <xsl:value-of select="@KodPocztowy"/>&#160;<xsl:value-of select="@Miejscowosc"/>
            <br />
            <xsl:choose>
              <xsl:when test="@Kraj='CH'">
                SZWAJCARIA
              </xsl:when>
              <xsl:when test="not(@Kraj='CH')">&#160;</xsl:when>
            </xsl:choose>
          </p>
        </td>
      </tr>
    </table>
  </xsl:template>
  <xsl:template match="sad:Odbiorca">
    <table width="35%" cellspaing="10" cellpadding="10" border="1">
      <tr>
        <td>
          <p>
            <b>Odbiorca</b>
          </p>
          <p>
            <xsl:value-of select="@Nazwa"/><br />
            <xsl:value-of select="@UlicaNumer"/><br />
            <xsl:value-of select="@KodPocztowy"/>&#160;<xsl:value-of select="@Miejscowosc"/><br />
            <xsl:choose>
              <xsl:when test="@Kraj='PL'">
                POLSKA
              </xsl:when>
              <xsl:when test="not(@Kraj='PL')">&#160;</xsl:when>
            </xsl:choose><br />
            TIN: <xsl:value-of select="@TIN"/><br />
            EORI: <xsl:value-of select="@EORI"/>
          </p>
        </td>
      </tr>
    </table>
  </xsl:template>
  <xsl:template match="sad:WartoscTowarow">
    <p>
      <b>Wartość towarów</b>
    </p>
    <table cellspacing="0" cellpadding="0" border="1" width="100%">
      <tr>
        <th>Wartość</th>
        <th>Waluta</th>
        <th>Kurs waluty</th>
      </tr>
      <tr>
        <td>
          <xsl:value-of select="@Wartosc"/>
        </td>
        <td>
          <xsl:value-of select="@Waluta"/>
        </td>
        <td>
          <xsl:value-of select="@KursWaluty"/>
        </td>
      </tr>
    </table>
  </xsl:template>
  <xsl:template match="sad:Towar">
    <p>
      <b>Lista pozycji towarowych</b>
    </p>
    <table cellspacing="0" cellpadding="0" border="1" width="100%">
      <tr>
        <th>Poz</th>
        <th>Opis</th>
        <th>Kod towarowy</th>
        <th>Wartość pozycji</th>
        <th>Wartość statystyczna</th>
        <th>Kod taric</th>
        <th>Masa brutto</th>
        <th>Procedura</th>
      </tr>
      <tr>
        <td valign="top" align="center">
          <xsl:value-of select="@PozId"/>
        </td>
        <td valign="top" align="center">
          <xsl:value-of select="@OpisTowaru"/>
        </td>
        <td valign="top" align="center">
          <xsl:value-of select="@KodTowarowy"/>
        </td>
        <td valign="top" align="center">
          <xsl:value-of select="sad:WartoscTowaru/@WartoscPozycji"/>
        </td>
        <td valign="top" align="center">
          <xsl:value-of select="sad:WartoscTowaru/@WartoscStatystyczna"/>
        </td>
        <td valign="top" align="center">
          <xsl:value-of select="@KodTaric"/>
        </td>
        <td valign="top" align="center">
          <xsl:value-of select="@MasaBrutto"/>
        </td>
        <td valign="top" align="center">
          <xsl:value-of select="@Procedura"/>
        </td>
      </tr>
    </table>
    <p>
      <b>Szczegóły dotyczące ilości</b>
    </p>
    <table cellspacing="0" cellpadding="0" border="1" width="100%">
      <tr>
        <th>Ilość towaru</th>
        <th>Jednostka</th>
        <th>Liczba opakowań</th>
      </tr>
      <tr>
        <td>
          <xsl:value-of select="sad:IloscTowaru/@Ilosc"/>
        </td>
        <td>
          <xsl:value-of select="sad:IloscTowaru/@Jm"/>
        </td>
        <td>
          <xsl:value-of select="sad:Opakowanie/@LiczbaOpakowan"/>
        </td>
      </tr>
    </table>
    <p>
      <b>Dokument poprzedni</b>
    </p>
    <table cellspacing="0" cellpadding="0" border="1" width="100%">
      <tr>
        <th>Poz.</th>
        <th>Nr</th>
      </tr>
      <xsl:apply-templates select="sad:DokumentPoprzedni" />
    </table>
    <p>
      <b>Dokument wymagany</b>
    </p>
    <table cellspacing="0" cellpadding="0" border="1" width="100%">
      <xsl:apply-templates select="sad:DokumentWymagany" />
    </table>
  </xsl:template>
  <xsl:template match="sad:DokumentWymagany">
    <tr>
      <td>
        <xsl:choose>
          <xsl:when test="@Kod='9DK8'">
            <center>
              <b>Kod</b>
            </center>
          </xsl:when>
          <xsl:when test="not(@Kod='9DK8')">
            <xsl:value-of select="@Kod"/>
          </xsl:when>
        </xsl:choose>
      </td>
      <td>
        <xsl:choose>
          <xsl:when test="@Kod='9DK8'">
            <center>
              <b>Nr</b>
            </center>
          </xsl:when>
          <xsl:when test="not(@Kod='9DK8')">
            <xsl:value-of select="@Nr"/>
          </xsl:when>
        </xsl:choose>
      </td>
      <td>
        <xsl:choose>
          <xsl:when test="@Kod='9DK8'">
            <center>
              <b>Uwagi</b>
            </center>
          </xsl:when>
          <xsl:when test="not(@Kod='9DK8')">
            <xsl:value-of select="@Uwagi"/>
          </xsl:when>
        </xsl:choose>
      </td>
    </tr>
  </xsl:template>
  <xsl:template match="sad:DokumentPoprzedni">
    <tr>
      <td>
        <xsl:value-of select="@PozId"/>
      </td>
      <td>
        <xsl:value-of select="@Nr"/>
      </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>
