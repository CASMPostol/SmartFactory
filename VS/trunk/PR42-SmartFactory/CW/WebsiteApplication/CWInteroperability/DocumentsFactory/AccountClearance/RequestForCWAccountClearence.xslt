<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ms="urn:schemas-microsoft-com:xslt"
    xmlns:cas="http://cas.eu/schemas/SmartFactory/CW/Interoperability/DocumentsFactory/RequestContent.xsd"
>
  <xsl:output method="html" indent="yes"/>
  <xsl:decimal-format name="pl" decimal-separator=',' grouping-separator='.' />
  <xsl:variable name="FoarmatOfFloat" >#####0,00</xsl:variable>
  <xsl:variable name="FoarmatOfFloatPrice" >#####0,000</xsl:variable>
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
        <td nowrap="true">
          <xsl:value-of select="ms:format-date(cas:IntroducingSADDocumentDate, $FoarmatOfdate)"/>
        </td>
        <td nowrap="true">
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
      KSIĘGA SKŁADU CELNEGO - WPROWADZENIE
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
        <td valign="middle" align="center">
          Lp.
        </td>
        <td valign="middle" align="center" colspan="4">
          Nazwa towaru
        </td>
        <td valign="middle" align="center">
          Kod taryfy CN
        </td>
        <td valign="middle" align="center">
          Pozycja SAD
        </td>
        <td valign="middle" align="center">
          Ilość towaru
        </td>
        <td valign="middle" align="center">
          Waga netto [kg]
        </td>
        <td valign="middle" align="center">
          Waga brutto [kg]
        </td>
        <td valign="middle" align="center">
          Ilość opakowań [szt]
        </td>
        <td valign="middle" align="center">
          Wartość celna
        </td>
        <td valign="middle" align="center">
          Waluta
        </td>
        <td valign="middle" align="center">
          Cena
        </td>
        <td valign="middle" align="center">
          PZ nr
        </td>
        <td valign="middle" align="center">
          Faktura nr
        </td>
      </tr>
      <tr>
        <td valign="middle" align="center">
          1
        </td>
        <td valign="middle" align="center">
          Nazwa tytoniu
        </td>
        <td valign="middle" align="center">
          Typ
        </td>
        <td valign="middle" align="center">
          SKU
        </td>
        <td valign="middle" align="center">
          Batch
        </td>
        <td valign="middle" align="center">
          3
        </td>
        <td valign="middle" align="center">
          4
        </td>
        <td valign="middle" align="center">
          5
        </td>
        <td valign="middle" align="center">
          6
        </td>
        <td valign="middle" align="center">
          7
        </td>
        <td valign="middle" align="center">
          8
        </td>
        <td valign="middle" align="center">
          9
        </td>
        <td valign="middle" align="center">
          10
        </td>
        <td valign="middle" align="center">
          11
        </td>
        <td valign="middle" align="center">
          12
        </td>
        <td valign="middle" align="center">
          13
        </td>
      </tr>
      <tr>
        <td valign="middle" align="center">
          1
        </td>
        <td valign="middle" align="center">
          <xsl:value-of select="cas:TobaccoName"/>
        </td>
        <td valign="middle" align="center">
          <xsl:value-of select="cas:Grade"/>
        </td>
        <td valign="middle" align="center">
          <xsl:value-of select="cas:SKU"/>
        </td>
        <td valign="middle" align="center">
          <xsl:value-of select="cas:Batch"/>
        </td>
        <td valign="middle" align="center">
          <xsl:value-of select="cas:CNTarrifCode"/>
        </td>
        <td valign="middle" align="center">
          1
        </td>
        <td valign="middle" align="center">
          <xsl:value-of select="format-number(cas:Quantity, $FoarmatOfFloat, 'pl')"/>
        </td>
        <td valign="middle" align="center">
          <xsl:value-of select="format-number(cas:NetMass, $FoarmatOfFloat, 'pl')"/>
        </td>
        <td valign="middle" align="center">
          <xsl:value-of select="format-number(cas:GrossMass, $FoarmatOfFloat, 'pl')"/>
        </td>
        <td valign="middle" align="center">
          <xsl:value-of select="format-number(cas:PackageUnits, $FoarmatOfFloat, 'pl')"/>
        </td>
        <td valign="middle" align="center">
          <xsl:value-of select="format-number(cas:Value, $FoarmatOfFloat, 'pl')"/>
        </td>
        <td valign="middle" align="center">
          <xsl:value-of select="cas:Currency"/>
        </td>
        <td valign="middle" align="center">
          <xsl:value-of select="format-number(cas:UnitPrice, $FoarmatOfFloatPrice, 'pl')"/>
        </td>
        <td valign="middle" align="center">
          <xsl:value-of select="cas:PzNo"/>
        </td>
        <td valign="middle" align="center">
          <xsl:value-of select="cas:InvoiceNo"/>
        </td>
      </tr>
    </table>
    <br/>
    <h1>
      KSIEGA SKŁADU CELNEGO - WYPROWADZENIE
    </h1>
    <table cellspacing="0" cellpadding="0" border="1" width="100%">
      <tr>
        <td colspan="3">
          Numer Zezwolenia na korzystanie ze składu celnego <xsl:value-of select="cas:ConsentNo"/>, dokument z dnia <xsl:value-of select="cas:ConsentDate"/>.
        </td>
        <td colspan="5" rowspan="3" align="center">
          <b>Towar odprawiony</b>
        </td>
        <td colspan="3" rowspan="3" align="center">
          <b>Towar wydany</b>
        </td>
        <td colspan="2" rowspan="3" align="center">
          <b>
            Towar pozostały na składzie
          </b>
        </td>
        <td rowspan="3">
          &#160;
        </td>
      </tr>
      <tr>
        <td colspan="3">
          Nazwa tytoniu <xsl:value-of select="cas:TobaccoName"/>
        </td>
      </tr>
      <tr>
        <td colspan="3">
          Typ <xsl:value-of select="cas:Grade"/>, SKU <xsl:value-of select="cas:SKU"/>, Batch <xsl:value-of select="cas:Batch"/>
        </td>
      </tr>
      <tr>
        <td valign="middle" align="center">
          Lp.
        </td>
        <td valign="middle" align="center">
          Nr zgłoszenia celnego o objęcie towaru inną procedurą celną
        </td>
        <td valign="middle" align="center">
          Data
        </td>
        <td valign="middle" align="center">
          Ilość towaru
        </td>
        <td valign="middle" align="center">
          Waga netto [kg]
        </td>
        <td valign="middle" align="center">
          Waga brutto [kg]
        </td>
        <td valign="middle" align="center">
          Wartość
        </td>
        <td valign="middle" align="center">
          Waluta
        </td>
        <td valign="middle" align="center">
          Waga netto [kg]
        </td>
        <td valign="middle" align="center">
          Ilość opakowań [szt]
        </td>
        <td valign="middle" align="center">
          WZ nr
        </td>
        <td valign="middle" align="center">
          Waga netto [kg]
        </td>
        <td valign="middle" align="center">
          Ilość opakowań [szt]
        </td>
        <td valign="middle" align="center">
          Kod CN
        </td>
      </tr>
      <tr>
        <td valign="middle" align="center">
          14
        </td>
        <td valign="middle" align="center">
          15
        </td>
        <td valign="middle" align="center">
          16
        </td>
        <td valign="middle" align="center">
          17
        </td>
        <td valign="middle" align="center">
          18
        </td>
        <td valign="middle" align="center">
          19
        </td>
        <td valign="middle" align="center">
          20
        </td>
        <td valign="middle" align="center">
          21
        </td>
        <td valign="middle" align="center">
          22
        </td>
        <td valign="middle" align="center">
          23
        </td>
        <td valign="middle" align="center">
          24
        </td>
        <td valign="middle" align="center">
          25
        </td>
        <td valign="middle" align="center">
          26
        </td>
        <td valign="middle" align="center">
          27
        </td>
      </tr>
      <xsl:apply-templates select="cas:DisposalsColection"/>
      <tr>
        <td colspan="3">
          <b valign="middle" align="center">
            RAZEM
          </b>
        </td>
        <td valign="middle" align="center">
          <b>
            <xsl:value-of select="format-number(sum(cas:DisposalsColection/cas:DisposalsArray/cas:SettledNetMass), $FoarmatOfFloat, 'pl')"/>
          </b>
        </td>
        <td valign="middle" align="center">
          <b>
            <xsl:value-of select="format-number(sum(cas:DisposalsColection/cas:DisposalsArray/cas:SettledNetMass), $FoarmatOfFloat, 'pl')"/>
          </b>
        </td>
        <td valign="middle" align="center">
          <b>
            <xsl:value-of select="format-number(sum(cas:DisposalsColection/cas:DisposalsArray/cas:SettledGrossMass), $FoarmatOfFloat, 'pl')"/>
          </b>
        </td>
        <td align="center">
          <b>
            <xsl:value-of select="format-number(sum(cas:DisposalsColection/cas:DisposalsArray/cas:TobaccoValue), $FoarmatOfFloat, 'pl')"/>
          </b>
        </td>
        <td align="center">
          <b>
            <xsl:value-of select="cas:DisposalsColection/cas:DisposalsArray/cas:Currency"/>
          </b>
        </td>
        <td valign="middle" align="center">
          <b>
            <xsl:value-of select="format-number(sum(cas:DisposalsColection/cas:DisposalsArray/cas:SettledNetMass), $FoarmatOfFloat, 'pl')"/>
          </b>
        </td>
        <td valign="middle" align="center">
          <b>
            <xsl:value-of select="format-number(sum(cas:DisposalsColection/cas:DisposalsArray/cas:PackageToClear), $FoarmatOfFloat, 'pl')"/>
          </b>
        </td>
        <td>
          &#160;
        </td>
        <td valign="middle" align="center">
          <b>
            &#160;
          </b>
        </td>
        <td valign="middle" align="center">
          <b>
            &#160;
          </b>
        </td>
        <td>
          &#160;
        </td>
      </tr>
    </table>
  </xsl:template>
  <xsl:template match="cas:DisposalsColection">
    <xsl:apply-templates select="cas:DisposalsArray"/>
  </xsl:template>
  <xsl:template match="cas:DisposalsArray">
    <tr>
      <td valign="middle" align="center">
        <xsl:choose>
          <xsl:when test="cas:No='0'">&#160;</xsl:when>
          <xsl:when test="not(cas:No='0')">
            <xsl:value-of select="cas:No"/>
          </xsl:when>
        </xsl:choose>
      </td>
      <td valign="middle" align="center">
        <xsl:value-of select="cas:SADDocumentNo"/>
      </td>
      <td valign="middle" align="center" nowrap="true">
        <xsl:value-of select="ms:format-date(cas:SADDate, $FoarmatOfdate)"/>
      </td>
      <td valign="middle" align="center">
        <xsl:value-of select="format-number(cas:SettledNetMass, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td valign="middle" align="center">
        <xsl:value-of select="format-number(cas:SettledNetMass, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td valign="middle" align="center">
        <xsl:value-of select="format-number(cas:SettledGrossMass, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td valign="middle" align="center">
        <xsl:value-of select="format-number(cas:TobaccoValue, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td valign="middle" align="center">
        <xsl:value-of select="cas:Currency"/>
      </td>
      <td valign="middle" align="center">
        <xsl:value-of select="format-number(cas:SettledNetMass, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td valign="middle" align="center">
        <xsl:value-of select="format-number(cas:PackageToClear, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td valign="middle" align="center">
        <xsl:value-of select="cas:WZ"/>
      </td>
      <td valign="middle" align="center">
        <xsl:choose>
          <xsl:when test="cas:RemainingQuantity='-1'">&#160;</xsl:when>
          <xsl:when test="not(cas:RemainingQuantity='-1')">
            <xsl:value-of select="format-number(cas:RemainingQuantity, $FoarmatOfFloat, 'pl')"/>
          </xsl:when>
        </xsl:choose>
      </td>
      <td valign="middle" align="center">
        <xsl:choose>
          <xsl:when test="cas:RemainingPackage='-1'">&#160;</xsl:when>
          <xsl:when test="not(cas:RemainingPackage='-1')">
            <xsl:value-of select="format-number(cas:RemainingPackage, $FoarmatOfFloat, 'pl')"/>
          </xsl:when>
        </xsl:choose>
      </td>
      <td valign="middle" align="center">
        <xsl:value-of select="cas:CNTarrifCode"/>
      </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>