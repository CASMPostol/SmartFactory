<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ms="urn:schemas-microsoft-com:xslt"
    xmlns:cas="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/AccountClearance.xsd"
>
  <xsl:output method="html" indent="yes"/>
  <xsl:decimal-format name="pl" decimal-separator=',' grouping-separator='.' />
  <xsl:variable name="FoarmatOfFloat" >###.##0,00</xsl:variable>
  <xsl:variable name="FoarmatOfdate" >dd-MM-yyyy</xsl:variable>
  <xsl:template match="/" >
    <html>
      <head>
        <title>
          Wniosek o rozliczenie towaru objętego procedurą uszlachetniania czynnego
        </title>
        <style type="text/css">
          td, p, li { font-size:10pt; }
          h3 { font-size:11pt; }
          th { font-size:11pt; }
          h2 { font-size:12pt; }
          h1 { font-size:14pt; text-align:center; }
        </style>
      </head>
      <body>
        <table border="0" width="100%">
          <tr>
            <td align="left">
              Dokument: <xsl:value-of select="cas:RequestContent/cas:DocumentNo"/>
            </td>
            <td align="right">
              Gostków Stary, <xsl:value-of select="ms:format-date(cas:RequestContent/cas:DocumentDate, $FoarmatOfdate)"/>
            </td>
          </tr>
        </table>
        <p>
          <b>Urząd Celny II w Łodzi<br/>
             ul. Św. Teresy 106<br/>
             91-341 Łódź
          </b>
        </p>
        <xsl:apply-templates select="cas:RequestContent" />
        <table border="0" width="100%">
          <tr>
            <td>
              <p align="right" height="50px" valign="bottom">
                .............................................
              </p>
            </td>
          </tr>
          <tr>
            <td>
              <p align="right">
                Imię i Nazwisko
              </p>
            </td>
          </tr>
        </table>
      </body>
    </html>
  </xsl:template>
  <xsl:template match="cas:RequestContent">
    <h1>
      Wniosek o rozliczenie towaru objętego procedurą uszlachetniania czynnego
    </h1>
    <table border="1" cellspacing="0" cellpadding="0" width="100%">
      <tr>
        <th>Nr SAD wprowadzającego</th>
        <th>SKU</th>
        <th>Batch</th>
        <th>Ilość w kg</th>
      </tr>
      <tr>
        <td>
          <xsl:value-of select="cas:EntryDocumentNo"/>
        </td>
        <td>
          <xsl:value-of select="cas:SKU"/>
        </td>
        <td>
          <xsl:value-of select="cas:Batch"/>
        </td>
        <td align="right">
          <xsl:value-of select="cas:NetMass"/>
        </td>
    </tr>
    </table>
    <br/>
    <table  border="0" cellspacing="0" cellpadding="0" width="100%">
      <tr>
        <td align="left"><xsl:value-of select="cas:ConsentNo"/> z dnia <xsl:value-of select="ms:format-date(cas:ConsentDate, $FoarmatOfdate)"/> ważne do dnia <xsl:value-of select="ms:format-date(cas:ValidToDate, $FoarmatOfdate)"/>
      </td>
        <td align="right">Procedura: [3151], [4051]</td>
      </tr>
    </table>
    <br/>
    <table border="1" cellspacing="0" cellpadding="0" width="100%">
      <tr>
        <th>Nr SAD wywozowego</th>
        <th>Data</th>
        <th>Ilość zużytego towaru w kg</th>
        <th>Ilość pozostałego towaru w kg</th>
      </tr>
      <tr>
        <td>
          &#160;
        </td>
        <td>
          &#160;
        </td>
        <td>
          &#160;
        </td>
        <td>
          &#160;
        </td>
      </tr>
      <tr>
        <td align="center" colspan="4">
          <b>
            WEDŁUG ZAŁĄCZONEGO WYDRUKU Z KSIĄŻKI U.CZ.
            NR [nr dokumentu]
          </b>
        </td>
      </tr>
    </table>
  </xsl:template>
</xsl:stylesheet>
