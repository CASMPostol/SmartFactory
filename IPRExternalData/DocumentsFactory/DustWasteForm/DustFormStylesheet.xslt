<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ms="urn:schemas-microsoft-com:xslt"
    xmlns:cas="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/DustWasteForm.xsd"
>
  <xsl:output method="html" indent="yes"/>
  <xsl:decimal-format name="pl" decimal-separator=',' grouping-separator='.' />
  <xsl:variable name="FoarmatOfFloat" >###.##0,00</xsl:variable>
  <xsl:variable name="FoarmatOfdate" >dd-MM-yyyy</xsl:variable>
  <xsl:template match="/" >
    <html>
      <head>
        <title>Control JSOX TAXID-2180-10 JTI Polska Sp. z o.o.</title>
      </head>
      <body>
        <table border="0">
          <tr>
            <td align="right">
              Gostków Stary, <xsl:value-of select="ms:format-date(cas:DocumentContent/cas:DocumentDate, $FoarmatOfdate)"/>
            </td>
          </tr>
          <tr>
            <td align="left">
              Dokument: <xsl:value-of select="cas:DocumentContent/cas:DocumentNo" />
            </td>
          </tr>
        </table>
        <xsl:apply-templates select="cas:DocumentContent" />
      </body>
    </html>
  </xsl:template>
  <xsl:template match="cas:DocumentContent">
    <h2>
      Zestawienie ilości pyłów i ścinków tytoniowych powstałych przy produkcji papierosów wyprodukowanych
      w okresie od <xsl:value-of select="ms:format-date(cas:StartDate, $FoarmatOfdate)"/> do <xsl:value-of select="ms:format-date(cas:EndDate, $FoarmatOfdate)"/>
    </h2>
    <p>
      Procedura: <xsl:value-of select="cas:CustomProcedureCode" />
    </p>
    <table border="1">
      <tr>
        <td colspan="3">
          Suma z pył + ścinki %
        </td>
        <td colspan="6">
        </td>
      </tr>
      <tr>
        <th>NR SAD</th>
        <th>Data</th>
        <th>SKU tytoniu</th>
        <th>Batch tytoniu</th>
        <th>Batch produktu gotowego</th>
        <th>Ilość w kg</th>
        <th>Cena jednostkowa</th>
        <th>Wartość</th>
        <th>Waluta</th>
      </tr>
      <xsl:apply-templates select="cas:AccountDescription" />
      <tr>
        <td colspan="3">Suma końcowa</td>
        <td align="right" colspan="6">
          <xsl:value-of select="cas:Total" />
        </td>
      </tr>
    </table>
    <table>
      <tr>
        <td>
          <p align="center">
            .............................................
          </p>
        </td>
      </tr>
      <tr>
        <td>
          <p align="center">
            Imię i Nazwisko
          </p>
        </td>
      </tr>
    </table>
  </xsl:template>
  <xsl:template match="cas:AccountDescription">
    <tr>
      <xsl:apply-templates select="cas:MaterialsOnOneAccount" />
    </tr>
  </xsl:template>
  <xsl:template match="cas:MaterialsOnOneAccount">
      <xsl:apply-templates select="cas:MaterialRecords" />
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
    <td>
      <xsl:value-of select="format-number(cas:Qantity, $FoarmatOfFloat, 'pl')"/>
    </td>
    <td>
      <xsl:value-of select="format-number(cas:UnitPrice, $FoarmatOfFloat, 'pl')"/>
    </td>
    <td>
      <xsl:value-of select="format-number(cas:TobaccoValue, $FoarmatOfFloat, 'pl')"/>
    </td>
    <td>
      <xsl:value-of select="cas:Currency"/>
    </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>
