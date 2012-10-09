<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ms="urn:schemas-microsoft-com:xslt"
    xmlns:cas="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/TobaccoFreeCirculationForm.xsd"
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
          Zestawienie zbiorcze - dopuszczenie do obrotu tytoni z systemu zawieszeń
        </title>
      <style type="text/css">
          td, p { font-size:10px; }
          th { font-size:12px; }
          h2 { font-size:14px; text-align:center; }
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
      Zestawienie zbiorcze - dopuszczenie do obrotu tytoni z systemu zawieszeń
    </h2>
    <p>Z uwagi na zmiany planu produkcyjnego jesteśmy zmuszeni zmienić status celny niżej wymienionych tytoni.</p>
    <p>Procedura <xsl:value-of select="cas:CustomProcedureCode"/>
  </p>
    <table border="1">
      <tr>
        <th>Nr SAD</th>
        <th>Data</th>
        <th>SKU tytoniu</th>
        <th>Batch tytoniu</th>
        <th>Cena</th>
        <th>Ilość</th>
        <th>Wartość</th>
        <th>Waluta</th>
      </tr>
      <xsl:apply-templates select="cas:AccountDescription" />
      <tr>
        <td colspan="5">Suma</td>
        <td>
          <xsl:value-of select="cas:Total"/>
        </td>
        <td>

        </td>
        <td>

        </td>
      </tr>
    </table>
    <table width="100%" border="0">
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
    <xsl:apply-templates select="cas:MaterialRecord" />
  </xsl:template>
  <xsl:template match="cas:MaterialRecord">
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
        <xsl:value-of select="format-number(cas2:UnitPrice, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td>
        <xsl:value-of select="format-number(cas2:Qantity, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td>
        <xsl:value-of select="format-number(cas2:TobaccoValue, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td>
        <xsl:value-of select="cas2:Currency"/>
      </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>