<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ms="urn:schemas-microsoft-com:xslt"
    xmlns:cas="http://CAS.SmartFactory.xml.DocumentsFactory.BinCard/DocumentContent.xsd"
>
  <xsl:output method="html" indent="yes"/>
  <xsl:decimal-format name="pl" decimal-separator=',' grouping-separator='.' />
  <xsl:variable name="FoarmatOfFloat" >###.##0,00</xsl:variable>
  <xsl:variable name="FoarmatOfdate" >dd-MM-yyyy</xsl:variable>
  <xsl:template match="/" >
    <html>
      <head>
        <title>
          BinCard
        </title>
        <style type="text/css">
          p  { font-size:11pt; }
          td { font-size:10pt; }
          th { font-size:11pt; }
          h2 { font-size:14pt; text-align:center; }
        </style>
      </head>
      <body>
        <table border="1" width="100%" cellspacing="0" cellpadding="0">
          <tr>
            <td colspan="6">
              <h2>WPROWADZENIE</h2>
            </td>
          </tr>
          <tr>
            <td valign="top" colspan="2">
              <p>Nazwa tytoniu</p>
              <p>
                <xsl:value-of select="cas:DocumentContent/cas:TobaccoName" />
              </p>
              <table border="1" width="100%" cellspacing="0" cellpadding="0">
                <tr>
                  <td>
                    <p>Typ</p>
                    <p>
                      <xsl:value-of select="cas:DocumentContent/cas:TobaccoType" />
                    </p>
                  </td>
                  <td>
                    <p>SKU</p>
                    <p>
                      <xsl:value-of select="cas:DocumentContent/cas:SKU" />
                    </p>
                  </td>
                  <td>
                    <p>Batch</p>
                    <p>
                      <xsl:value-of select="cas:DocumentContent/cas:Batch" />
                    </p>
                  </td>
                </tr>
              </table>
            </td>
            <td valign="top">
              <p>Nr SAD</p>
              <p>
                <xsl:value-of select="cas:DocumentContent/cas:SAD" />
              </p>
              <p>
                z dnia <xsl:value-of select="ms:format-date(cas:DocumentContent/cas:SADDate, $FoarmatOfdate)"/>
              </p>
            </td>
            <td valign="top">
              <p>Nr PZ</p>
              <p>
                <xsl:value-of select="cas:DocumentContent/cas:PzNo" />
              </p>
            </td>
            <td valign="top" width="13%">
              <p>Waga netto [kg]</p>
              <p>
                <xsl:value-of select="format-number(cas:DocumentContent/cas:NetWeight, $FoarmatOfFloat, 'pl')" />
              </p>
            </td>
            <td valign="top" width="10%">
              <p>Ilość opakowań [szt]</p>
              <p>
                <xsl:value-of select="format-number(cas:DocumentContent/cas:PackageQuantity, $FoarmatOfFloat, 'pl')" />
              </p>
            </td>
          </tr>
          <tr>
            <td colspan="4">
              <h2>
                WYPROWADZENIE
              </h2>
            </td>
            <td colspan="2">
              <h2>STAN NA S.C.</h2>
            </td>
          </tr>
          <tr>
            <th width="13%">
              <p>Waga netto [kg]</p>
            </th>
            <th width="13%">
              <p>Ilość opakowań [szt]</p>
            </th>
            <th width="31%">
              <p>Nr SAD</p>
            </th>
            <th width="20%">
              <p>Data SAD</p>
            </th>
            <th width="13%">
              <p>Waga netto [kg]</p>
            </th>
            <th width="10%">
              <p>Ilość opakowań [szt]</p>
            </th>
          </tr>
          <tr>
            <td>
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
          </tr>
          <tr>
            <td>
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
          </tr>
          <tr>
            <td>
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
          </tr>
          <tr>
            <td>
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
          </tr>
          <tr>
            <td>
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
          </tr>
          <tr>
            <td>
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
          </tr>
          <tr>
            <td>
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
          </tr>
          <tr>
            <td>
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
          </tr>
          <tr>
            <td>
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
            <td height="30pt">
              &#160;<br /><br /><br />
            </td>
          </tr>
        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
