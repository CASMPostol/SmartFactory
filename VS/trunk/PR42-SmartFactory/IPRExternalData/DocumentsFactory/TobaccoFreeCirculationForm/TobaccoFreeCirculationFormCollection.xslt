<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ms="urn:schemas-microsoft-com:xslt"
    xmlns:cas="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/TobaccoFreeCirculationForm.xsd"
>
  <xsl:output method="html" indent="yes"/>
  <xsl:decimal-format name="pl" decimal-separator=',' grouping-separator='.' />
  <xsl:variable name="FoarmatOfFloat" >###.##0,00</xsl:variable>
  <xsl:variable name="FoarmatOfdate" >dd-MM-yyyy</xsl:variable>
  <xsl:template match="/" >
    <html>
      <head>
        <title>
          Zestawienie zbiorcze - dopuszczenie do obrotu tytoni z systemu zawieszeń</title>
      </head>
      <body>
        <table border="0">
          <tr>
            <td align="right">
              Gostków Stary, <xsl:value-of select="ms:format-date(cas:TobaccoFreeCirculationFormCollection/cas:DocumentDate, $FoarmatOfdate)"/>
            </td>
          </tr>
          <tr>
            <td align="left">
              Dokument:
            </td>
          </tr>
        </table>
        <h2>
          Zestawienie zbiorcze - dopuszczenie do obrotu tytoni z systemu zawieszeń
        </h2>
        <p>Z uwagi na zmiany planu produkcyjnego jesteśmy zmuszeni zmienić status celny niżej wymienionych tytoni.</p>
        <p>Procedura</p>
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
          <xsl:apply-templates select="cas:XX" />
          <tr>
            <td colspan="5">Suma</td>
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
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>