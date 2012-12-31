<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ms="urn:schemas-microsoft-com:xslt"
    xmlns:cas="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/BalanceSheetContent.xsd"
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
      <style type="text/css">
        td, li { font-size:9pt; }
        p  { font-size:11pt; }
        h3 { font-size:11pt; }
        th { font-size:11pt; }
        h2 { font-size:12pt; }
        h1 { font-size:14pt; text-align:center; }
      </style>
      <body>
        <xsl:apply-templates select="cas:BalanceSheetContent" />
      </body>
    </html>
  </xsl:template>
  <xsl:template match="cas:BalanceSheetContent">
    <table width="100%" border="0">
          <tr>
            <td align="left">
              Dokument: <xsl:value-of select="cas:DocumentNo"/>
            </td>
            <td align="right">
                Gostków Stary, <xsl:value-of select="ms:format-date(cas:DocumentDate, $FoarmatOfdate)"/>
            </td>
          </tr>
        </table>
        <h2>Control JSOX TAXID-2180-10 JTI Polska Sp. z o.o. </h2>
        <h2>Quantity of tobacco kilograms in IPR book and real stock.</h2>
        <h3>Situation at <xsl:value-of select="ms:format-date(cas:SituationAtDate, $FoarmatOfdate)"/></h3>
        <h3>Bilans na dzień <xsl:value-of select="ms:format-date(cas:SituationAtDate, $FoarmatOfdate)"/></h3>
          <xsl:apply-templates select="cas:IPRStock" />
        <table width="100%" border="0">
          <tr>
            <td align="left">
              .............................................<br/>
              Date
            </td>
            <td align="right">
              .............................................<br/>
              Verified by .............................................
            </td>
          </tr>
        </table>
  </xsl:template>
  <xsl:template match="cas:IPRStock">
    <table border="1">
          <tr align="center">
            <td colspan="3">
              &#160;
            </td>
            <td colspan="4">
              ZAPISY SYSTEMU IPR – ilości nierozliczone
            </td>
            <td colspan="4">
              STANY MAGAZYNOWE TYTONIU
            </td>
            <td>
              &#160;
            </td>
          </tr>
          <tr align="center">
            <th>OGL TYTONI</th>
            <th>SKU TYTONI</th>
            <th>BATCH TYTONI</th>
            <th>KSIEGA IPR SALDO (A)</th>
            <th>SH, ODPAD, PRZEPAŁ, TYTOŃ (B)</th>
            <th>PYŁ (C)</th>
            <th>DOSTĘPNY TYTOŃ (D=A-B-C)</th>
            <th>TYTONIE (E)</th>
            <th>PAPIEROSY (F)</th>
            <th>PRODUKCJA W TOKU (G)</th>
            <th>KRAJANKA (H)</th>
            <th>BILANS (D-E-F-G-H=0)</th>
          </tr>
            <xsl:apply-templates select="cas:IPRStockContent" />
        </table>
  </xsl:template>
  <xsl:template match="cas:IPRStockContent">
    <xsl:apply-templates select="cas:IPRList"/>
  </xsl:template>
  <xsl:template match="cas:IPRList">
    <xsl:apply-templates select="cas:IPRRow" />
    <tr>
      <td>
        SUMA
      </td>
      <td>
        &#160;
      </td>
      <td>
        &#160;
      </td>
      <td>
        <xsl:value-of select="format-number(cas:TotalIPRBook, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td>
        <xsl:value-of select="format-number(cas:TotalSHWasteOveruseCSNotStarted, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td>
        <xsl:value-of select="format-number(cas:TotalDustCSNotStarted, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td>
        <xsl:value-of select="format-number(cas:TotalTobaccoAvailable, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td>
        <xsl:value-of select="format-number(cas:TotalTobaccoInWarehouse, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td>
        <xsl:value-of select="format-number(cas:TotalTobaccoInCigarettesWarehouse, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td>
        <xsl:value-of select="format-number(cas:TotalTobaccoInCigarettesProduction, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td>
        <xsl:value-of select="format-number(cas:TotalTobaccoInCutfillerWarehouse, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td>
        <xsl:value-of select="format-number(cas:TotalBalance, $FoarmatOfFloat, 'pl')"/>
      </td>
    </tr>
  </xsl:template>
  <xsl:template match="cas:IPRRow">
    <tr>
      <td align="center">
        <xsl:value-of select="cas:EntryDocumentNo"/>
      </td>
      <td align="center">
        <xsl:value-of select="cas:SKU"/>
      </td>
      <td align="center">
        <xsl:value-of select="cas:Batch"/>
      </td>
      <td align="center">
        <xsl:value-of select="format-number(cas:IPRBook, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td align="center">
        <xsl:value-of select="format-number(cas:SHWasteOveruseCSNotStarted, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td align="center">
        <xsl:value-of select="format-number(cas:DustCSNotStarted, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td align="center">
        <xsl:value-of select="format-number(cas:TobaccoAvailable, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td align="center">
        <xsl:value-of select="format-number(cas:TobaccoInWarehouse, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td align="center">
        <xsl:value-of select="format-number(cas:TobaccoInCigarettesWarehouse, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td align="center">
        <xsl:value-of select="format-number(cas:TobaccoInCigarettesProduction, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td align="center">
        <xsl:value-of select="format-number(cas:TobaccoInCutfillerWarehouse, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td align="center">
        <xsl:value-of select="format-number(cas:Balance, $FoarmatOfFloat, 'pl')"/>
      </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>