<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ms="urn:schemas-microsoft-com:xslt"
    xmlns:cas="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/BalanceSheetContent.xsd"
>
  <xsl:output method="html" indent="yes"/>
  <xsl:decimal-format name="pl" decimal-separator=',' grouping-separator='.' />
  <xsl:variable name="FormatOfFloat" >#####0,00</xsl:variable>
  <xsl:variable name="FormatOfdate" >dd-MM-yyyy</xsl:variable>
  <xsl:variable name="FormatOfdateMonthAndYear" >MM/yyyy</xsl:variable>
  <xsl:template match="/" >
    <html>
      <head>
        <title>Control JSOX TAXID-2180-10 JTI Polska Sp. z o.o.</title>
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
        <xsl:apply-templates select="cas:BalanceSheetContent" />
      </body>
    </html>
  </xsl:template>
  <xsl:template match="cas:BalanceSheetContent">
    <table width="100%" border="0">
          <tr>
            <td align="left">
              <font size="+0,25">
                Dokument: <b>
                  <xsl:value-of select="cas:DocumentNo"/>
                </b>
              </font>
            </td>
            <td align="right">
                Gostków Stary, <xsl:value-of select="ms:format-date(cas:DocumentDate, $FormatOfdate)"/>
            </td>
          </tr>
        </table>
        <h1>Control JSOX TAXID-2180-10 JTI Polska Sp. z o.o. </h1>
        <h1>Quantity of tobacco kilograms in IPR book and real stock.</h1>
        <h2>Situation at <xsl:value-of select="ms:format-date(cas:SituationAtDate, $FormatOfdate)"/></h2>
        <h2>Bilans na dzień <xsl:value-of select="ms:format-date(cas:SituationAtDate, $FormatOfdate)"/></h2>
          <xsl:apply-templates select="cas:BalanceBatch" />
        <table width="100%" border="0">
          <tr>
            <td align="left" height="50px">
              .............................................<br/>
              Date
            </td>
            <td align="right" height="50px">
              .............................................<br/>
              Verified by .............................................
            </td>
          </tr>
        </table>
    <br />
    <table width="100%" border="0">
      <tr>
        <td align="left">
          <font size="+0,25">
            Dokument: <b>
              <xsl:value-of select="cas:DocumentNo"/>
            </b>
          </font>
        </td>
        <td align="right">
          Gostków Stary, <xsl:value-of select="ms:format-date(cas:DocumentDate, $FormatOfdate)"/>
        </td>
      </tr>
    </table>
    <h1>Control JSOX TAXID-2180-10 JTI Polska Sp. z o.o.</h1>
    <h1>Export and Free Circulation in the IPR warehouse - information from IPR book</h1>
    <h2>Situation in <xsl:value-of select="ms:format-date(cas:StartDate, $FormatOfdateMonthAndYear)"/> (from <xsl:value-of select="ms:format-date(cas:StartDate, $FormatOfdate)"/> to <xsl:value-of select="ms:format-date(cas:EndDate, $FormatOfdate)"/>)</h2>
    <xsl:apply-templates select="cas:JSOX" />
    <table width="100%" border="0">
      <tr>
        <td colspan="2" align="right" height="50px">
          .............................................<br/>
          [Imię i nazwisko]
        </td>
      </tr>
      <tr>
        <td align="left" height="50px">
          .............................................<br/>
          Date
        </td>
        <td align="right" height="50px">
          .............................................<br/>
          Verified by .............................................
        </td>
      </tr>
    </table>
  </xsl:template>
  <xsl:template match="cas:BalanceBatch">
    <table border="1" width="100%">
          <tr align="center">
            <td colspan="3">
              &#160;
            </td>
            <td colspan="5">
              ZAPISY SYSTEMU IPR – ilości nierozliczone (kg)
            </td>
            <td colspan="4">
              STANY MAGAZYNOWE TYTONI (kg)
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
            <th>TYTOŃ W TRAKCIE ODPRAWY (B)</th>
            <th>SH, ODPAD, PRZEPAŁ, TYTOŃ (C)</th>
            <th>PYŁ (D)</th>
            <th>DOSTĘPNY TYTOŃ (E=A-B-C-D)</th>
            <th>TYTONIE (F)</th>
            <th>PAPIEROSY (G)</th>
            <th>PRODUKCJA W TOKU (H)</th>
            <th>KRAJANKA (I)</th>
            <th>BILANS (E-F-G-H-I=0)</th>
          </tr>
            <xsl:apply-templates select="cas:BalanceBatchContent" />
          <tr>
            <td colspan="3">
              SUMA
            </td>
            <td align="center">
              <xsl:value-of select="format-number(sum(cas:BalanceBatchContent/cas:TotalIPRBook), $FormatOfFloat, 'pl')"/>
            </td>
            <td align="center">
              <xsl:value-of select="format-number(sum(cas:BalanceBatchContent/cas:TotalTobaccoStarted), $FormatOfFloat, 'pl')"/>
            </td>
            <td align="center">
              <xsl:value-of select="format-number(sum(cas:BalanceBatchContent/cas:TotalSHWasteOveruseCSNotStarted), $FormatOfFloat, 'pl')"/>
            </td>
            <td align="center">
              <xsl:value-of select="format-number(sum(cas:BalanceBatchContent/cas:TotalDustCSNotStarted), $FormatOfFloat, 'pl')"/>
            </td>
            <td align="center">
              <xsl:value-of select="format-number(sum(cas:BalanceBatchContent/cas:TotalTobaccoAvailable), $FormatOfFloat, 'pl')"/>
            </td>
            <td align="center">
              <xsl:value-of select="format-number(sum(cas:BalanceBatchContent/cas:TotalTobaccoInWarehouse), $FormatOfFloat, 'pl')"/>
            </td>
            <td align="center">
              <xsl:value-of select="format-number(sum(cas:BalanceBatchContent/cas:TotalTobaccoInCigarettesWarehouse), $FormatOfFloat, 'pl')"/>
            </td>
            <td align="center">
              <xsl:value-of select="format-number(sum(cas:BalanceBatchContent/cas:TotalTobaccoInCigarettesProduction), $FormatOfFloat, 'pl')"/>
            </td>
            <td align="center">
              <xsl:value-of select="format-number(sum(cas:BalanceBatchContent/cas:TotalTobaccoInCutfillerWarehouse), $FormatOfFloat, 'pl')"/>
            </td>
            <td align="center">
              <xsl:value-of select="format-number(sum(cas:BalanceBatchContent/cas:TotalBalance), $FormatOfFloat, 'pl')"/>
            </td>
          </tr>
        </table>
  </xsl:template>
  <xsl:template match="cas:BalanceBatchContent">
    <xsl:apply-templates select="cas:BalanceIPR"/>
          <tr>
            <td nowrap="true" bgcolor="#CDCDCD">
              <b>SUMA CZĘŚCIOWA</b>
            </td>
            <td align="center" bgcolor="#CDCDCD">
              <b><xsl:value-of select="cas:BalanceIPR/cas:BalanceIPRContent/cas:SKU"/></b>
            </td>
            <td align="center" bgcolor="#CDCDCD">
              <b><xsl:value-of select="cas:BalanceIPR/cas:BalanceIPRContent/cas:Batch"/></b>
            </td>
            <td align="center" bgcolor="#CDCDCD">
              <b><xsl:value-of select="format-number(cas:TotalIPRBook, $FormatOfFloat, 'pl')"/></b>
            </td>
            <td align="center" bgcolor="#CDCDCD">
              <b><xsl:value-of select="format-number(cas:TotalTobaccoStarted, $FormatOfFloat, 'pl')"/></b>
            </td>
            <td align="center" bgcolor="#CDCDCD">
              <b><xsl:value-of select="format-number(cas:TotalSHWasteOveruseCSNotStarted, $FormatOfFloat, 'pl')"/></b>
            </td>
            <td align="center" bgcolor="#CDCDCD">
              <b><xsl:value-of select="format-number(cas:TotalDustCSNotStarted, $FormatOfFloat, 'pl')"/></b>
            </td>
            <td align="center" bgcolor="#CDCDCD">
              <b><xsl:value-of select="format-number(cas:TotalTobaccoAvailable, $FormatOfFloat, 'pl')"/></b>
            </td>
            <td align="center" bgcolor="#CDCDCD">
              <b><xsl:value-of select="format-number(cas:TotalTobaccoInWarehouse, $FormatOfFloat, 'pl')"/></b>
            </td>
            <td align="center" bgcolor="#CDCDCD">
              <b><xsl:value-of select="format-number(cas:TotalTobaccoInCigarettesWarehouse, $FormatOfFloat, 'pl')"/></b>
            </td>
            <td align="center" bgcolor="#CDCDCD">
              <b><xsl:value-of select="format-number(cas:TotalTobaccoInCigarettesProduction, $FormatOfFloat, 'pl')"/></b>
            </td>
            <td align="center" bgcolor="#CDCDCD">
              <b><xsl:value-of select="format-number(cas:TotalTobaccoInCutfillerWarehouse, $FormatOfFloat, 'pl')"/></b>
            </td>
            <td align="center" bgcolor="#CDCDCD">
              <b><xsl:value-of select="format-number(cas:TotalBalance, $FormatOfFloat, 'pl')"/></b>
            </td>
          </tr>
  </xsl:template>
  <xsl:template match="cas:BalanceIPR">
    <xsl:apply-templates select="cas:BalanceIPRContent" />
  </xsl:template>
  <xsl:template match="cas:BalanceIPRContent">
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
        <xsl:value-of select="format-number(cas:IPRBook, $FormatOfFloat, 'pl')"/>
      </td>
      <td align="center">
        <xsl:value-of select="format-number(cas:TobaccoStarted, $FormatOfFloat, 'pl')"/>
      </td>
      <td align="center">
        <xsl:value-of select="format-number(cas:SHWasteOveruseCSNotStarted, $FormatOfFloat, 'pl')"/>
      </td>
      <td align="center">
        <xsl:value-of select="format-number(cas:DustCSNotStarted, $FormatOfFloat, 'pl')"/>
      </td>
      <td align="center">
        <xsl:value-of select="format-number(cas:TobaccoAvailable, $FormatOfFloat, 'pl')"/>
      </td>
      <td align="center">
        &#160;
      </td>
      <td align="center">
        &#160;
      </td>
      <td align="center">
        &#160;
      </td>
      <td align="center">
        &#160;
      </td>
      <td align="center">
        &#160;
      </td>
    </tr>
  </xsl:template>
  <xsl:template match="cas:JSOX">
    <table border="1" width="100%">
      <tr align="center">
        <th>Export or Free Circulation SAD</th>
        <th>Introducing SAD</th>
        <th>Date SAD EX or FC</th>
        <th>Invoice No.</th>
        <th>Sad Consignment No</th>
        <th>Compensation Good Name</th>
        <th>Procedure</th>
        <th>Quantity - change procedures (kg)</th>
        <th>Balance (Introducing –Disposals) (kg)</th>
      </tr>
      <xsl:apply-templates select="cas:JSOXCustomsSummaryList"/>
    </table>
      <h3>JSOX summary</h3>
      <table border="0">
        <tr>
          <th colspan="2"></th>
          <th>Date</th>
          <th>Quantity (kg)</th>
        </tr>
        <tr>
          <td>A</td>
          <td>Previous month (Stock)</td>
          <td>
            [<xsl:value-of select="ms:format-date(cas:PreviousMonthDate, $FormatOfdate)"/>]
          </td>
          <td>
            <xsl:value-of select="format-number(cas:PreviousMonthQuantity, $FormatOfFloat, 'pl')"/>
          </td>
        </tr>
        <tr>
          <td>B</td>
          <td>Introducing</td>
          <td>
            [<xsl:value-of select="ms:format-date(cas:PreviousMonthDate, $FormatOfdate)"/> - <xsl:value-of select="ms:format-date(cas:BalanceDate, $FormatOfdate)"/>]
          </td>
          <td>
            <xsl:value-of select="format-number(cas:IntroducingQuantity, $FormatOfFloat, 'pl')"/>
          </td>
        </tr>
        <tr>
          <td>C</td>
          <td>Outbound</td>
          <td>
            [<xsl:value-of select="ms:format-date(cas:PreviousMonthDate, $FormatOfdate)"/> - <xsl:value-of select="ms:format-date(cas:BalanceDate, $FormatOfdate)"/>]
          </td>
          <td>
            <xsl:value-of select="format-number(cas:OutboundQuantity, $FormatOfFloat, 'pl')"/>
          </td>
        </tr>
        <tr>
          <td>D</td>
          <td>Balance (A+B-C)</td>
          <td>
            [<xsl:value-of select="ms:format-date(cas:BalanceDate, $FormatOfdate)"/>]
          </td>
          <td>
            <xsl:value-of select="format-number(cas:BalanceQuantity, $FormatOfFloat, 'pl')"/>
          </td>
        </tr>
        <tr>
          <td>E</td>
          <td>Situation at</td>
          <td>
            [<xsl:value-of select="ms:format-date(cas:SituationDate, $FormatOfdate)"/>]
          </td>
          <td>
            <xsl:value-of select="format-number(cas:SituationQuantity, $FormatOfFloat, 'pl')"/>
          </td>
        </tr>
        <tr>
          <td>
            <b>F</b>
          </td>
          <td>
            <b>Reassume (D-E)</b>
          </td>
          <td>
            &#160;
          </td>
          <td>
            <b>
              <xsl:value-of select="format-number(cas:ReassumeQuantity, $FormatOfFloat, 'pl')"/>
            </b>
          </td>
        </tr>
      </table>
  </xsl:template>
  <xsl:template match="cas:JSOXCustomsSummaryList">
    <xsl:apply-templates select="cas:JSOXCustomsSummaryOGLGroupArray" />
    <tr>
      <td colspan="2">
        Total
      </td>
      <td>
        &#160;
      </td>
      <td>
        <td>
          &#160;
        </td>
        &#160;
      </td>
      <td>
        &#160;
      </td>
      <td>
        &#160;
      </td>
      <td align="center">
        <xsl:value-of select="format-number(cas:SubtotalQuantity, $FormatOfFloat, 'pl')"/>
      </td>
      <td>
        &#160;
      </td>
    </tr>
  </xsl:template>
  <xsl:template match="cas:JSOXCustomsSummaryOGLGroupArray">
    <xsl:apply-templates select="cas:JSOXCustomsSummaryOGLGroup" />
  </xsl:template>
  <xsl:template match="cas:JSOXCustomsSummaryOGLGroup">
    <xsl:apply-templates select="cas:JSOXCustomsSummaryArray" />
    <tr>
      <td colspan="2" bgcolor="#CDCDCD">
        Subtotal - <xsl:value-of select="cas:JSOXCustomsSummaryArray/cas:JSOXCustomsSummary/cas:ExportOrFreeCirculationSAD"/>
      </td>
      <td bgcolor="#CDCDCD">
        &#160;
      </td>
      <td bgcolor="#CDCDCD">
        &#160;
      </td>
      <td bgcolor="#CDCDCD">
        &#160;
      </td>
      <td bgcolor="#CDCDCD">
        &#160;
      </td>
      <td bgcolor="#CDCDCD">
        &#160;
      </td>
      <td align="center" bgcolor="#CDCDCD">
        <xsl:value-of select="format-number(cas:SubtotalQuantity, $FormatOfFloat, 'pl')"/>
      </td>
      <td bgcolor="#CDCDCD">
        &#160;
      </td>
    </tr>
  </xsl:template>
  <xsl:template match="cas:JSOXCustomsSummaryArray">
    <xsl:apply-templates select="cas:JSOXCustomsSummary" />
  </xsl:template>
  <xsl:template match="cas:JSOXCustomsSummary">
    <tr>
      <td>
        <xsl:value-of select="cas:ExportOrFreeCirculationSAD"/>
      </td>
      <td>
        <xsl:value-of select="cas:EntryDocumentNo"/>
      </td>
      <td align="center" nowrap="true">
        <xsl:value-of select="ms:format-date(cas:SADDate, $FormatOfdate)"/>
      </td>
      <td align="center" nowrap="true">
        <xsl:value-of select="cas:InvoiceNo"/>
      </td>
      <td align="center">
        <xsl:value-of select="cas:SadConsignmentNo"/>
      </td>
      <td align="center" nowrap="true">
        <xsl:value-of select="cas:CompensationGood"/>
      </td>
      <td align="center">
        <xsl:value-of select="cas:Procedure"/>
      </td>
      <td align="center">
        <xsl:value-of select="format-number(cas:Quantity, $FormatOfFloat, 'pl')"/>
      </td>
      <td align="center">
        <xsl:value-of select="format-number(cas:Balance, $FormatOfFloat, 'pl')"/>
      </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>