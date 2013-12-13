<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ms="urn:schemas-microsoft-com:xslt"
    xmlns:cas="http://cas.eu/schemas/SmartFactory/CW/Interoperability/DocumentsFactory/RequestContent.xsd"
>
  <xsl:output method="html" indent="yes"/>
  <xsl:decimal-format name="pl" decimal-separator=',' grouping-separator='.' />
  <xsl:variable name="FoarmatOfFloat" >#####0,00</xsl:variable>
  <xsl:variable name="FoarmatOfdate" >dd-MM-yyyy</xsl:variable>
  <xsl:template match="/" >
    <html>
      <head>
        <title>
          Wniosek o całkowitą likwidacje zgłoszeń celnych
        </title>
        <style type="text/css">
          td, li { font-size:9pt; }
          p  { font-size:11pt; }
          h3 { font-size:11pt; }
          th { font-size:11pt; }
          h2 { font-size:12pt; text-align:center; }
          h1 { font-size:14pt; text-align:center; }
        </style>
      </head>
      <body>
        <xsl:apply-templates select="cas:RequestContent" />
      </body>
    </html>
  </xsl:template>
  <xsl:template match="cas:RequestContent">
    <!--RequestForAccountClearence-->
    <table border="0" width="100%">
      <tr>
        <td align="left">
          <p>
            Dokument:
            <b>
              <xsl:value-of select="cas:DocumentName" />
            </b>
          </p>
        </td>
        <td align="right">
          Gostków Stary, <xsl:value-of select="ms:format-date(cas:DocumentDate, $FoarmatOfdate)" />
        </td>
      </tr>
    </table>
    <p>
      <b>
        Urząd Celny II w Łodzi<br/>
        ul. Św. Teresy 106<br/>
        91-341 Łódź
      </b>
    </p>
    <h1>
      Wniosek
    </h1>
    <h2>
      Wnioskujemy o całkowitą likwidacje następujących zgłoszeń celnych:
    </h2>
    <h2>
      Skład Celny nr C-0042-01
    </h2>
    <table cellspacing="0" cellpadding="0" border="1" width="100%">
      <tr>
        <th>Lp.</th>
        <th colspan="2">Nr SAD wprowadzającego z dn.</th>
        <th colspan="2">Nr SAD wyprowadzającego z dn.</th>
      </tr>
      <tr>
        <td>
          1
        </td>
        <td>
          <xsl:value-of select="cas:IntroducingSADDocumentNo"/>
        </td>
        <td>
          <xsl:value-of select="ms:format-date(cas:IntroducingSADDocumentDate, $FoarmatOfdate)"/>
        </td>
        <td>
          <xsl:value-of select="cas:WithdrawalSADDcoumentNo"/>
        </td>
        <td>
          <xsl:value-of select="ms:format-date(cas:WithdrawalSADDocumentDate, $FoarmatOfdate)"/>
        </td>
      </tr>
    </table>
    <p>
      Załączniki:<br/>
      1. Karta rozliczeniowa konta S.C.<br/>
      2. Kopia SAD - ostatnie wyprowadzenie
    </p>
    <table border="0" width="100%">
      <tr>
        <td align="right" valign="bottom">
          <br/>
          <br/>
          <br/>
          <p>
            .............................................
          </p>
        </td>
      </tr>
      <tr>
        <td align="right">
          <p>
            Imię i Nazwisko
          </p>
        </td>
      </tr>
    </table>
    <!--AccountBallanceReport-->
    <table border="0" width="100%">
      <tr>
        <td align="left">
          <p>
            Dokument:
            <b>
              <xsl:value-of select="cas:DocumentName" />
            </b>
          </p>
        </td>
        <td align="right">
          Gostków Stary, <xsl:value-of select="ms:format-date(cas:DocumentDate, $FoarmatOfdate)" />
        </td>
      </tr>
    </table>
    <h1>
      KSIĘGA SKŁADU CELNEGO - WYPROWADZENIE
    </h1>
    <p>
      Numer Zezwolenia na korzystanie ze składu celnego <xsl:value-of select="cas:ConsentNo"/> z dnia <xsl:value-of select="cas:ConsentDate"/>
    </p>
    <table cellspacing="0" cellpading="0" border="1" width="100%">
      <tr>
        <td colspan="5">
          Numer ewidencji SAD<br/>
          <xsl:value-of select="cas:IntroducingSADDocumentNo"/>
        </td>
        <td colspan="6">
          Data zgłoszenia celnego<br/>
          <xsl:value-of select="ms:format-date(cas:IntroducingSADDocumentDate, $FoarmatOfdate)"/>
        </td>
        <td colspan="5">
          &#160;
        </td>
      </tr>
      <tr>
        <td>
          Lp.
        </td>
        <td colspan="4">
          Nazwa towaru
        </td>
        <td>
          Kod taryfy CN
        </td>
        <td>
          Pozycja SAD
        </td>
        <td>
          Ilość towaru
        </td>
        <td>
          Waga netto [kg]
        </td>
        <td>
          Waga brutto [kg]
        </td>
        <td>
          Ilość opakowań [szt]
        </td>
        <td>
          Wartość celna
        </td>
        <td>
          Waluta
        </td>
        <td>
          Cena
        </td>
        <td>
          PZ nr
        </td>
        <td>
          Faktura nr
        </td>
      </tr>
      <tr>
        <td>
          1
        </td>
        <td>
          Nazwa tytoniu
        </td>
        <td>
          Typ
        </td>
        <td>
          SKU
        </td>
        <td>
          Batch
        </td>
        <td>
          3
        </td>
        <td>
          4
        </td>
        <td>
          x
        </td>
        <td>
          5
        </td>
        <td>
          6
        </td>
        <td>
          &#160;
        </td>
        <td>
          7
        </td>
        <td>
          x
        </td>
        <td>
          8
        </td>
        <td>
          x
        </td>
        <td>
          10
        </td>
      </tr>
      <tr>
        <td>
          1
        </td>
        <td>
          <xsl:value-of select="cas:TobaccoName"/>
        </td>
        <td>
          <xsl:value-of select="cas:Grade"/>
        </td>
        <td>
          <xsl:value-of select="cas:SKU"/>
        </td>
        <td>
          <xsl:value-of select="cas:Batch"/>
        </td>
        <td>
          <xsl:value-of select="cas:CNTarrifCode"/>
        </td>
        <td>
          1
        </td>
        <td>
          <xsl:value-of select="format-number(cas:Quantity, $FoarmatOfFloat, 'pl')"/>
        </td>
        <td>
          <xsl:value-of select="format-number(cas:NetMass, $FoarmatOfFloat, 'pl')"/>
        </td>
        <td>
          <xsl:value-of select="format-number(cas:GrossMass, $FoarmatOfFloat, 'pl')"/>
        </td>
        <td>
          <xsl:value-of select="format-number(cas:PackageUnits, $FoarmatOfFloat, 'pl')"/>
        </td>
        <td>
          <xsl:value-of select="format-number(cas:Value, $FoarmatOfFloat, 'pl')"/>
        </td>
        <td>
          <xsl:value-of select="cas:Currency"/>
        </td>
        <td>
          <xsl:value-of select="format-number(cas:UnitPrice, $FoarmatOfFloat, 'pl')"/>
        </td>
        <td>
          <xsl:value-of select="cas:PzNo"/>
        </td>
        <td>
          <xsl:value-of select="cas:InvoiceNo"/>
        </td>
      </tr>
    </table>
  </xsl:template>
</xsl:stylesheet>