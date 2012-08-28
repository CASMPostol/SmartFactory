﻿<?xml version="1.0" encoding="utf-8"?>
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
        <h2>
          Zestawienie ilości pyłów i ścinków tytoniowych powstałych przy produkcji papierosów wyprodukowanych
          w okresie od [Data] do [Data]
        </h2>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
