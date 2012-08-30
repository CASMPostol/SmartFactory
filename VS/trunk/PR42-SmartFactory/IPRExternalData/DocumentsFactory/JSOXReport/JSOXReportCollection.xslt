<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ms="urn:schemas-microsoft-com:xslt"
    xmlns:cas="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/JSOXReport.xsd"
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
              Gostków Stary, <xsl:value-of select="ms:format-date(cas:JSOXReportCollection/cas:DocumentDate, $FoarmatOfdate)"/>
            </td>
          </tr>
          <tr>
            <td align="left">
              Dokument:
            </td>
          </tr>
        </table>
        <h2>Control JSOX TAXID-2180-10 JTI Polska Sp. z o.o.</h2>
        <h2>Export and Free Circulation in the IPR warehouse - information from IPR book</h2>
        <h3>Situation in [month] /2011 (from [date] to [date])</h3>
        <table border="1">
          <tr align="center">
            <th>Export or Free Circulation SAD</th>
            <th>Introducing SAD</th>
            <th>Date SAD EX or FC</th>
            <th>Invoice No.</th>
            <th>Compensation Good Name</th>
            <th>Procedure</th>
            <th>Quantity - change procedures (kg)</th>
            <th>Balance (Introducing –Disposals) (kg)</th>
          </tr>
          <xsl:apply-templates select="cas:XX"> </xsl:apply-templates>
          <tr>
            <td colspan="2">
              Total
            </td>
            <td>
              
            </td>
            <td>

            </td>
            <td>

            </td>
            <td>

            </td>
            <td>

            </td>
            <td>

            </td>
          </tr>
        </table>
        <h4>JSOX summary</h4>
        <table border="0">
          <tr>
            <th colspan="2"></th>
            <th>Date</th>
            <th>Quantity (kg)</th>
          </tr>
          <tr>
            <td>A</td>
            <td>Previous mounth (Stock)</td>
            <td>
              
            </td>
            <td>
              
            </td>
          </tr>
          <tr>
            <td>B</td>
            <td>Introducing</td>
            <td>
              
            </td>
            <td>
              
            </td>
          </tr>
          <tr>
            <td>C</td>
            <td>Outbound</td>
            <td>
              
            </td>
            <td>
              
            </td>
          </tr>
          <tr>
            <td>D</td>
            <td>Balance (A+B+C)</td>
            <td>
              
            </td>
            <td>
              
            </td>
          </tr>
          <tr>
            <td>E</td>
            <td>Situation at</td>
            <td>
              
            </td>
            <td>
              
            </td>
          </tr>
          <tr>
            <td>F</td>
            <td>Readdume (D-E)</td>
            <td>
              
            </td>
            <td>
              
            </td>
          </tr>
        </table>
        <table>
          <tr>
            <td align="right">
              .............................................</br>
            </td>
          </tr>
          <tr>
            <td align="left">
              .............................................</br>
              Date
            </td>
            <td align="right">
              .............................................</br>
              Verified by .............................................
            </td>
          </tr>
        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
