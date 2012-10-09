<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ms="urn:schemas-microsoft-com:xslt"
    xmlns:cas="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/DustWasteForm.xsd"
    xmlns:cas2="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory" 
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
        <table border="1">
          <tr>
            <td colspan="3">
              Suma z pozostałe odpady %
            </td>
            <td colspan="3"></td>
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
            <td colsapn="3"></td>
          </tr>
        </table>
        <table border="0" width="100%">
          <tr>
            <td align="right">
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
    <xsl:apply-templates select="cas2:MaterialRecords" />
    <tr>
      <td colspan="6" align="right">
        <xsl:value-of select="format-number(cas2:Total, $FoarmatOfFloat, 'pl')"/>
      </td>
    </tr>
  </xsl:template>
  <xsl:template match="cas2:MaterialRecords">
    <xsl:apply-templates select="cas2:MaterialRecord" />
  </xsl:template>
  <xsl:template match="cas2:MaterialRecord">
    <tr>
      <td>
        <xsl:value-of select="cas2:CustomDocumentNo"/>
      </td>
      <td>
        <xsl:value-of select="ms:format-date(cas2:Date, $FoarmatOfdate)"/>
      </td>
      <td>
        <xsl:value-of select="cas2:MaterialSKU"/>
      </td>
      <td>
        <xsl:value-of select="cas2:MaterialBatch"/>
      </td>
      <td>
        <xsl:value-of select="cas2:FinishedGoodBatch"/>
      </td>
      <td>
        <xsl:value-of select="format-number(cas2:Qantity, $FoarmatOfFloat, 'pl')"/>
      </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>
