<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ms="urn:schemas-microsoft-com:xslt"
    xmlns:cas="http://CAS.SmartFactory.xml.DocumentsFactory.Disposals/DutyReliefAccount.xsd"
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
              Dokument: <xsl:value-of select="cas:DocumentContent/cas:DocumentNo"/>
            </td>
            <td align="right">
              Gostków Stary, <xsl:value-of select="ms:format-date(cas:DocumentContent/cas:DocumentDate, $FoarmatOfdate)"/>
            </td>
          </tr>
        </table>
        <p>
          <b>Urząd Celny II w Łodzi<br/>
             ul. Św. Teresy 106<br/>
             91-341 Łódź
          </b>
        </p>
        <xsl:apply-templates select="cas:DocumentContent" />
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
  <xsl:template match="cas:DocumentContent">
    <h1>
      Wniosek o rozliczenie towaru objętego procedurą uszlachetniania czynnego
    </h1>
    <table  border="1" cellspacing="0" cellpadding="0">
      <tr>
        <th>Nr SAD wprowadzającego</th>
        <th>SKU</th>
        <th>Batch</th>
        <th>Ilość w kg</th>
      </tr>
      <xsl:apply-templates select="cas:XX" />
    </table>
    <table  border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left">[nr dokumentu] z dnia [Data] ważne do dnia [Data]</td>
        <td align="right">Procedura: [3151], [4051]</td>
      </tr>
    </table>
    <table border="1" cellspacing="0" cellpadding="0">
      <tr>
        <th>Nr SAD wywozowego</th>
        <th>Data</th>
        <th>Ilość zużytego towaru w kg</th>
        <th>Ilość pozostałego towaru w kg</th>
      </tr>
      <xsl:apply-templates select="cas:XX2" />
      <tr>
        <td colspan="4">
          <b>
            WEDŁUG ZAŁĄCZONEGO WYDRUKU Z KSIĄŻKI U.CZ.
            NR [nr dokumentu]
          </b>
        </td>
      </tr>
    </table>
  </xsl:template>
</xsl:stylesheet>
