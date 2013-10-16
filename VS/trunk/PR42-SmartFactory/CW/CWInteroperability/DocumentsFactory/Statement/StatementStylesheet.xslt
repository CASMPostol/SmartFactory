<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ms="urn:schemas-microsoft-com:xslt"
    xmlns:cas="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/StatementContent.xsd"
>
  <xsl:output method="html" indent="yes"/>
  <xsl:decimal-format name="pl" decimal-separator=',' grouping-separator='.' />
  <xsl:variable name="FormatOfFloat" >#####0,00</xsl:variable>
  <xsl:variable name="FormatOfdate" >dd-MM-yyyy</xsl:variable>
  <xsl:variable name="FormatOfdateMonthAndYear" >MM/yyyy</xsl:variable>
  <xsl:template match="/" >
    <html>
      <head>
        <title>Wniosek o całkowitą likwidację zgłoszeń celnych</title>
        <style type="text/css">
          td, li { font-size:9pt; }
          p  { font-size:10pt; }
          h3 { font-size:10pt; }
          th { font-size:11pt; }
          h2 { font-size:11pt; text-align:center; }
          h1 { font-size:12pt; text-align:center; }
        </style>
      </head>
      <body>
        <xsl:apply-templates select="cas:StatementContent" />
      </body>
    </html>
  </xsl:template>
  <xsl:template match="cas:StatementContent">
    <table width="100%" border="0">
      <tr>
        <td align="left">
          <font size="+0,25">
            Urząd Celny II w Łodzi <br />
            ul. Św. Teresy 106 <br />
            91-341 Łódź
          </font>
        </td>
        <td align="right">
          Gostków Stary, <xsl:value-of select="ms:format-date(cas:DocumentDate, $FormatOfdate)"/>
        </td>
      </tr>
    </table>
    <h1>WNIOSEK</h1>
    <h2>Wnioskujemy o całkowitą likwidację następujących zgłoszeń celnych:</h2>
    <h2>Skład Celny nr C-0042-01</h2>
    <xsl:apply-templates select="cas:aa"/>
  </xsl:template>
</xsl:stylesheet>
