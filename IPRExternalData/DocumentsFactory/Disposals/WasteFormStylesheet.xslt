<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ms="urn:schemas-microsoft-com:xslt"
    xmlns:cas="http://CAS.SmartFactory.xml.DocumentsFactory.Disposals/DocumentContent.xsd"
>
  <xsl:output method="html" indent="yes"/>
  <xsl:decimal-format name="pl" decimal-separator=',' grouping-separator='.' />
  <xsl:variable name="FoarmatOfFloat" >###.##0,00</xsl:variable>
  <xsl:variable name="FoarmatOfdate" >dd-MM-yyyy</xsl:variable>
  <xsl:template match="/" >
    <html>
      <head>
        <title>
          Zestawienie ilości odpadów tytoniowych powstałych przy produkcji papierosów
        </title>
        <style type="text/css">
          p  { font-size:12pt; }
          td { font-size:10pt; }
          th { font-size:11pt; }
          h2 { font-size:14pt; text-align:center; }
        </style>
      </head>
      <body>
        <table border="0" width="100%">
          <tr>
            <td align="left">
              Dokument: <xsl:value-of select="cas:DocumentContent/cas:DocumentNo"/>
            </td>
            <td align="right">
              Gostków Stary, <xsl:value-of select="ms:format-date(cas:DocumentContent/cas:DocumentDate, $FoarmatOfdate)"/>
            </td>
          </tr>
        </table>
        <xsl:apply-templates select="cas:DocumentContent" />
      </body>
    </html>
  </xsl:template>
  <xsl:template match="cas:DocumentContent">
        <h2>
          Zestawienie ilości odpadów tytoniowych powstałych przy produkcji papierosów wyprodukowanych w okresie od <xsl:value-of select="ms:format-date(cas:StartDate, $FoarmatOfdate)"/> do <xsl:value-of select="ms:format-date(cas:EndDate, $FoarmatOfdate)"/>
        </h2>
        <p>
          Procedura: <xsl:value-of select="cas:CustomProcedureCode" />
      </p>
        <table border="1" width="100%" cellspacing="0" cellpadding="0">
          <tr>
            <td colspan="6">
              Suma z pozostałe odpady %
            </td>
          </tr>
          <tr>
            <th>NR SAD</th>
            <th>Data</th>
            <th>SKU tytoniu</th>
            <th>Batch tytoniu</th>
            <th>Batch produktu gotowego</th>
            <th>Ilość w kg</th>
          </tr>
          <xsl:apply-templates select="cas:AccountDescription" />
          <tr>
            <td colspan="3">Suma końcowa</td>
            <td colspan="3" align="right">
              <xsl:value-of select="format-number(cas:TotalQuantity, $FoarmatOfFloat, 'pl')" />
            </td>
          </tr>
        </table>
        <table border="0" width="100%">
          <tr>
            <td align="right" height="50px" valign="bottom">
                .............................................
            </td>
          </tr>
          <tr>
            <td align="right">
                Imię i Nazwisko
            </td>
          </tr>
        </table>
  </xsl:template>
  <xsl:template match="cas:AccountDescription">
    <xsl:apply-templates select="cas:MaterialsOnOneAccount" />
  </xsl:template>
  <xsl:template match="cas:MaterialsOnOneAccount">
    <xsl:apply-templates select="cas:MaterialRecords" />
    <tr>
      <td colspan="6" align="right">
        <xsl:value-of select="format-number(cas:TotalQuantity, $FoarmatOfFloat, 'pl')"/>
      </td>
    </tr>
  </xsl:template>
  <xsl:template match="cas:MaterialRecords">
    <xsl:apply-templates select="cas:MaterialRecord" />
  </xsl:template>
  <xsl:template match="cas:MaterialRecord">
    <tr>
      <td>
        <xsl:value-of select="cas:CustomDocumentNo"/>
      </td>
      <td>
        <xsl:value-of select="ms:format-date(cas:Date, $FoarmatOfdate)"/>
      </td>
      <td>
        <xsl:value-of select="cas:MaterialSKU"/>
      </td>
      <td>
        <xsl:value-of select="cas:MaterialBatch"/>
      </td>
      <td>
        <xsl:value-of select="cas:FinishedGoodBatch"/>
      </td>
      <td align="right">
        <xsl:value-of select="format-number(cas:Qantity, $FoarmatOfFloat, 'pl')"/>
      </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>
