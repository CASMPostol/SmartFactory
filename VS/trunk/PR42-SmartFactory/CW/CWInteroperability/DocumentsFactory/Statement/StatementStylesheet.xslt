<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ms="urn:schemas-microsoft-com:xslt"
    xmlns:cas="http://cas.eu/schemas/SmartFactory/CW/Interoperability/DocumentsFactory/StatementContent.xsd"
>
  <xsl:output method="html" indent="yes"/>
  <xsl:decimal-format name="pl" decimal-separator=',' grouping-separator='.' />
  <xsl:variable name="FormatOfFloat" >#####0,00</xsl:variable>
  <xsl:variable name="FormatOfdate" >dd-MM-yyyy</xsl:variable>
  <xsl:variable name="FormatOfdateMonthAndYear" >MM/yyyy</xsl:variable>
  <xsl:template match="/" >
    <html>
      <head>
        <title>Zestawienie należności</title>
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
    <table width="100%" border="1">
      <tr>
        <td align="left" colspan="3">
          Data: <xsl:value-of select="ms:format-date(cas:DocumentDate, $FormatOfdate)"/>
        </td>
        <td align="center" colspan="3">
          <xsl:value-of select="cas:CustomsProcedure"/>
        </td>
        <td align="center" colspan="3">
          &#160;
        </td>
      </tr>
      <tr>
        <td align="center" colspan="6">
          ZGŁASZAJĄCY: Agencja Celna MA-AN PLAEOC360000100003
        </td>
        <td align="center" colspan="3">
          NALEŻNOŚCI:
        </td>
      </tr>
      <tr>
        <th>Lp.</th>
        <th>nr syst.</th>
        <th>ident. systemowy</th>
        <th>nr ewidencji</th>
        <th>POD</th>
        <th>PZC</th>
        <th>cło</th>
        <th>VAT</th>
        <th>SUMA</th>
      </tr>
      <xsl:apply-templates select="cas:StatementOfDuties" />
      <tr>
        <td colspan="6">
          &#160;
        </td>
        <td align="center">
          <b>
            <xsl:value-of select="format-number(sum(cas:StatementOfDuties/cas:Statement/cas:DutyPerSettledAmount), $FormatOfFloat, 'pl')"/>
          </b>
        </td>
        <td align="center">
          <b>
            <xsl:value-of select="format-number(sum(cas:StatementOfDuties/cas:Statement/cas:VATPerSettledAmount), $FormatOfFloat, 'pl')"/>
          </b>
        </td>
        <td align="center">
          <b>
            <xsl:value-of select="format-number(sum(cas:StatementOfDuties/cas:Statement/cas:DutyAndVAT), $FormatOfFloat, 'pl')"/>
          </b>
        </td>
      </tr>
    </table>
  </xsl:template>
  <xsl:template match="cas:StatementOfDuties">
    <xsl:apply-templates select="cas:Statement" />
  </xsl:template>
  <xsl:template match="cas:Statement">
    <tr>
      <td align="center">
        <xsl:value-of select="cas:No"/>
      </td>
      <td align="center">
        <xsl:value-of select="cas:ReferenceNumber"/>
      </td>
      <td>
        &#160;
      </td>
      <td align="center">
        <xsl:value-of select="cas:SADDocumentNo"/>
      </td>
      <td>
        &#160;
      </td>
      <td>
        &#160;
      </td>
      <td align="center">
        <xsl:value-of select="format-number(cas:DutyPerSettledAmount, $FormatOfFloat, 'pl')"/>
      </td>
      <td align="center">
        <xsl:value-of select="format-number(cas:VATPerSettledAmount, $FormatOfFloat, 'pl')"/>
      </td>
      <td align="center">
        <xsl:value-of select="format-number(cas:DutyAndVAT, $FormatOfFloat, 'pl')"/>
      </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>
