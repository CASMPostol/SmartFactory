<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ms="urn:schemas-microsoft-com:xslt"
    xmlns:cas="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/CutfillerExportForm.xsd"
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
                Gostków Stary, <xsl:value-of select="ms:format-date(cas:CutfillerExportFormCollection/cas:DocumentDate, $FoarmatOfdate)"/>
            </td>
          </tr>
          <tr>
            <td align="left">
                Dokument:
            </td>
          </tr>
        </table>
        <h2>Control JSOX TAXID-2180-10 JTI Polska Sp. z o.o. </h2>
        <h2>Quantity of tobacco kilograms in IPR book and real stock.</h2>
        <h3>Situation at </h3>
        <h3>Bilans na dzień </h3>
        <table border="1">
          <tr align="center">
            <td colspan="3">
            </td>
            <td colspan="4">
              ZAPISY SYSTEMU IPR – ilości nierozliczone
            </td>
            <td colspan="4">
              STANY MAGAZYNOWE TYTONIU
            </td>
            <td></td>
          </tr>
          <tr align="center">
            <th>OGL TYTONI</th>
            <th>SKU TYTONI</th>
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
          <xsl:apply-templates select="cas:XX"> </xsl:apply-templates>
          <tr>
            <td>
              SUMA
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
        <table>
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