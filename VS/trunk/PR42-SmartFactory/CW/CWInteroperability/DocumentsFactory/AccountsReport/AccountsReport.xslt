<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
    xmlns:ms="urn:schemas-microsoft-com:xslt"
    xmlns:cas="http://cas.eu/schemas/SmartFactory/CW/Interoperability/DocumentsFactory/AccountsReport.xsd"
>
  <xsl:output method="html" indent="yes"/>
  <xsl:decimal-format name="pl" decimal-separator=',' grouping-separator='.' />
  <xsl:variable name="FormatOfFloat" >#####0,00</xsl:variable>
  <xsl:variable name="FormatOfDate" >dd-MM-yyyy</xsl:variable>
  <xsl:template match="/" >
    <html>
      <head>
        <title>
          Zestawienie ilości towarów na składzie celnym nr C-0042-01
        </title>
        <style type="text/css">
          td, li { font-size:9pt; }
          p  { font-size:11pt; }
          h3 { font-size:11pt; }
          th { font-size:11pt; }
          h2 { font-size:12pt; text-align:center; }
          h1 { font-size:14pt; text-align:center; }
        </style>
      </head>
      <body>
        <xsl:apply-templates select="AccountsReportContentWithStylesheet" />
      </body>
    </html>
  </xsl:template>
  <xsl:template match="AccountsReportContentWithStylesheet">
    <!--AccountsReport-->
    <table border="0" width="100%">
      <tr>
        <td align="left">
          <p>
            Dokument:
            <b>
              <xsl:value-of select="cas:DocumentName" />
            </b>
          </p>
        </td>
        <td align="right">
          Gostków Stary, <xsl:value-of select="ms:format-date(cas:DocumentDate, FormatOfDate)" />
        </td>
      </tr>
    </table>
    <h1>
      Zestawienie ilości towarów na składzie celnym nr C-0042-01
    </h1>
    <h2>
      Zgodnie z pozwoleniem na korzystanie z procedury składu celnego nr <xsl:apply-templates select="cas:ConsentsCollection" />
    </h2>
    <h2>
      Stan na dzień <xsl:value-of select="ms:format-date(cas:ReportDate, FormatOfDate)"/>
    </h2>
    <table border="0" width="100%">
      <tr>
        <td align="right">
          Oddział Celny II w Łodzi<br/>
          Składy Celne
        </td>
      </tr>
    </table>
    <table cellspacing="0" cellpadding="0" border="1" width="100%">
      <tr>
        <th>Lp.</th>
        <th>Nr SAD-u wprowadzenia</th>
        <th>Data</th>
        <th>Nazwa tytoniu</th>
        <th>Typ</th>
        <th>SKU</th>
        <th>Batch</th>
        <th>Kod CN</th>
        <th>Waga netto [kg]</th>
        <th>Wartość towaru</th>
        <th>Waluta</th>
      </tr>
      <xsl:apply-templates select="cas:AccountsColection" />
    </table>
  </xsl:template>
  <xsl:template match="cas:ConsentsCollection">
    <xsl:apply-templates select="cas:ConsentsArray" />
  </xsl:template>
  <xsl:template match="cas:ConsentsArray">
    <xsl:value-of select="cas:Consent"/> z dnia <xsl:value-of select="ms:format-date(cas:ConsentDate, $FormatOfDate)"/>&#160;
  </xsl:template>
  <xsl:template match="cas:AccountsColection">
    <xsl:apply-templates select="cas:AccountsArray" />
  </xsl:template>
  <xsl:template match="cas:AccountsArray">
    <xsl:apply-templates select="cas:AccountsDetails" />
    <tr>
      <td align="center" colspan="6">
        <b>RAZEM TYTONI [kg]</b>
      </td>
      <td>
        &#160;
      </td>
      <td>
        &#160;
      </td>
      <td align="center">
        <b>
          <xsl:value-of select="format-number(cas:TotalNetMass, $FormatOfFloat, 'pl')"/>
        </b>
      </td>
      <td align="center">
        <b>
          <xsl:value-of select="format-number(cas:TotalValue, $FormatOfFloat, 'pl')"/>
        </b>
      </td>
      <td align="center">
        <b>
          <xsl:value-of select="cas:TotalCurrency"/>
        </b>
      </td>
    </tr>
  </xsl:template>
  <xsl:template match="cas:AccountsDetails">
    <xsl:apply-templates select="cas:DetailsOfOneAccount" />
  </xsl:template>
  <xsl:template match="cas:DetailsOfOneAccount">
    <tr>
      <td align="center">
        <xsl:value-of select="cas:No"/>
      </td>
      <td align="center">
        <xsl:value-of select="cas:DocumentNo"/>
      </td>
      <td align="center" nowrap="true">
        <xsl:value-of select="ms:format-date(cas:CustomsDebtDate, $FormatOfDate)"/>
      </td>
      <td align="center">
        <xsl:value-of select="cas:TobaccoName"/>
      </td>
      <td align="center">
        <xsl:value-of select="cas:Grade"/>
      </td>
      <td align="center">
        <xsl:value-of select="cas:SKU"/>
      </td>
      <td align="center">
        <xsl:value-of select="cas:Batch"/>
      </td>
      <td align="center">
        <xsl:value-of select="cas:CNTarrifCode"/>
      </td>
      <td align="center">
        <xsl:value-of select="format-number(cas:NetMass, $FormatOfFloat, 'pl')"/>
      </td>
      <td align="center">
        <xsl:value-of select="format-number(cas:Value, $FormatOfFloat, 'pl')"/>
      </td>
      <td align="center">
        <xsl:value-of select="cas:Currency"/>
      </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>
