<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ms="urn:schemas-microsoft-com:xslt"
    xmlns:cas="http://CAS.SmartFactory.xml.DocumentsFactory.Disposals/DocumentContent.xsd"
>
  <xsl:output method="html" indent="yes"/>
  <xsl:decimal-format name="pl" decimal-separator=',' grouping-separator='.' />
  <xsl:variable name="FormatOfFloat" >###.##0,00</xsl:variable>
  <xsl:variable name="FormatOfFloatPrices" >###.##0,000</xsl:variable>
  <xsl:variable name="FormatOfdate" >dd-MM-yyyy</xsl:variable>
  <xsl:template match="/" >
    <html>
      <head>
        <title>
          Zestawienie zbiorcze - dopuszczenie do obrotu tytoni z systemu zawieszeń
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
              Gostków Stary, <xsl:value-of select="ms:format-date(cas:DocumentContent/cas:DocumentDate, $FormatOfdate)"/>
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
    <p>Procedura: <xsl:value-of select="cas:CustomProcedureCode"/>
  </p>
    <table border="1" width="100%" cellspacing="0" cellpadding="0">
      <tr>
        <th>Nr SAD</th>
        <th>Data</th>
        <th>SKU tytoniu</th>
        <th>Batch tytoniu</th>
        <th>Cena</th>
        <th>Ilość</th>
        <th>Wartość</th>
        <th>Waluta</th>
        <th>&#160;</th>
      </tr>
      <xsl:apply-templates select="cas:AccountDescription" />
      <tr>
        <td colspan="5">Suma</td>
        <td>
          <xsl:value-of select="cas:TotalQuantity"/>
        </td>
        <td>
          <xsl:value-of select="cas:TotalValue"/>
        </td>
        <td>
          &#160;
        </td>
        <td>
          &#160;
        </td>
      </tr>
    </table>
    <table width="100%" border="0">
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
      <td colspan="4">
        Suma częściowa
      </td>
      <td>
        &#160;
      </td>
      <td>
        <xsl:value-of select="format-number(cas:TotalQuantity, $FormatOfFloat, 'pl')"/>
      </td>
      <td>
          <xsl:value-of select="format-number(cas:TotalValue, $FormatOfFloat, 'pl')" />
      </td>
      <td>
        &#160;
      </td>
      <td>
        &#160;
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
        <xsl:value-of select="ms:format-date(cas:Date, $FormatOfdate)"/>
      </td>
      <td>
        <xsl:value-of select="cas:MaterialSKU"/>
      </td>
      <td>
        <xsl:value-of select="cas:MaterialBatch"/>
      </td>
      <td>
        <xsl:value-of select="format-number(cas:UnitPrice, $FormatOfFloatPrices, 'pl')"/>
      </td>
      <td>
        <xsl:value-of select="format-number(cas:Qantity, $FormatOfFloat, 'pl')"/>
      </td>
      <td>
        <xsl:value-of select="format-number(cas:TobaccoValue, $FormatOfFloat, 'pl')"/>
      </td>
      <td>
        <xsl:value-of select="cas:Currency"/>
      </td>
      <td>
        &#160;
      </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>