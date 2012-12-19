﻿<?xml version="1.0" encoding="utf-8"?>
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
          td, p, li { font-size:9pt; }
          h3 { font-size:11pt; }
          th { font-size:11pt; }
          h2 { font-size:12pt; }
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
              Dokument: <xsl:value-of select="cas:DocumentNo" />
            </td>
            <td align="right">
              Gostków Stary, <xsl:value-of select="ms:format-date(cas:DocumentDate, $FoarmatOfdate)" />
            </td>
          </tr>
        </table>
        <p>
          <b>Urząd Celny II w Łodzi<br/>
             ul. Św. Teresy 106<br/>
             91-341 Łódź
          </b>
        </p>
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
          <xsl:value-of select="cas:EntryDocumentNo"/> z dnia <xsl:value-of select="ms:format-date(cas:CustomsDebtDate, $FoarmatOfdate)"/>
        </td>
        <td>
          <xsl:value-of select="cas:SKU"/>
        </td>
        <td>
          <xsl:value-of select="cas:Batch"/>
        </td>
        <td align="right">
          <xsl:value-of select="format-number(cas:NetMass, $FoarmatOfFloat, 'pl')"/>
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
            NR <xsl:value-of select="cas:DocumentNo" />
          </b>
        </td>
      </tr>
    </table>
    <table border="0" width="100%">
          <tr>
            <td align="right" height="50" valign="bottom">
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
    <br/>
    <!--AccountBallanceReport-->
    <table border="0" width="100%">
          <tr>
            <td align="left">
              Dokument: <xsl:value-of select="cas:DocumentNo" />
            </td>
            <td align="right">
              Gostków Stary, <xsl:value-of select="ms:format-date(cas:DocumentDate, $FoarmatOfdate)" />
            </td>
          </tr>
        </table>
    <h1>KSIEGA USZLACHETNIANIA CZYNNEGO – PRZYWÓZ</h1>
    <p>Numer Zezwolenia <xsl:value-of select="cas:ConsentNo"/></p>
    <table border="1" cellspacing="0" cellpadding="0" width="100%">
      <tr>
        <td colspan="5">Numer ewidencji SAD</td>
        <td colspan="5">Data powstania długu celnego</td>
        <td colspan="6">Współczynnik produktywności od ok <xsl:value-of select="cas:ProductivityRateMin"/> % do ok <xsl:value-of select="cas:ProductivityRateMax"/> %</td>
      </tr>
      <tr>
        <td colspan="5">
          <xsl:value-of select="cas:EntryDocumentNo"/>
        </td>
        <td colspan="5">
          <xsl:value-of select="ms:format-date(cas:CustomsDebtDate, $FoarmatOfdate)"/>
        </td>
        <td colspan="6">
          Norma
        </td>
      </tr>
      <tr>
        <td>
          Lp.
        </td>
        <td colspan="4" valign="top">
          Nazwa towaru
        </td>
        <td valign="top">
          Kod taryfy PCN
        </td>
        <td valign="top">
          Faktura nr
        </td>
        <td valign="top">
          Pozycja SAD
        </td>
        <td valign="top">
          [typ cła] <br/>
          Przypadające cło [PLN]
        </td>
        <td valign="top" bgcolor="#ffff00">
          Przypadające cło na j. m.
        </td>
        <td valign="top">
          [typ vat]<br/>
          Przypadający VAT [PLN]
        </td>
        <td valign="top" bgcolor="#ffff00">
          Przypadający VAT na j. m.
        </td>
        <td valign="top">
          Ilość netto [kg]
        </td>
        <td valign="top">
          Ilość brutto [kg]
        </td>
        <td valign="top">
          Kartony [kg]
        </td>
        <td valign="top">
          Norma (1-towar kompensacyjny właściwy - netto/brutto)
        </td>
      </tr>
      <tr>
        <td valign="top">
          <b>1</b>
        </td>
        <td valign="top">
          Nazwa tytoniu
        </td>
        <td valign="top">
          Typ
        </td>
        <td valign="top">
          SKU
        </td>
        <td valign="top">
          Batch
        </td>
        <td valign="top">
          3
        </td>
        <td>
          &#160;
        </td>
        <td valign="top">
          4
        </td>
        <td valign="top">
          5
        </td>
        <td valign="top" bgcolor="#ffff00">
          6
        </td>
        <td valign="top">
          7
        </td>
        <td valign="top" bgcolor="#ffff00">
          8
        </td>
        <td valign="top">
          9
        </td>
        <td valign="top">
          10
        </td>
        <td>
          &#160;
        </td>
        <td valign="top">
          11
        </td>
      </tr>
      <tr>
        <td valign="top">
          1
        </td>
        <td>
          <xsl:value-of select="cas:TobaccoName"/>
        </td>
        <td valign="top">
          <xsl:value-of select="cas:Grade"/>
        </td>
        <td valign="top">
          <xsl:value-of select="cas:SKU"/>
        </td>
        <td valign="top">
          <xsl:value-of select="cas:Batch"/>
        </td>
        <td valign="top">
          <xsl:value-of select="cas:PCN"/>
        </td>
        <td valign="top">
          <xsl:value-of select="cas:InvoiceNo"/>
        </td>
        <td valign="top">
          1
        </td>
        <td valign="top">
          <xsl:value-of select="cas:DutyName"/>
        </td>
        <td valign="top" align="right" bgcolor="#ffff00">
          <xsl:value-of select="format-number(cas:DutyPerUnit, $FoarmatOfFloat, 'pl')"/>
        </td>
        <td valign="top">
          <xsl:value-of select="cas:VATName"/>
        </td>
        <td valign="top" align="right" bgcolor="#ffff00">
          <xsl:value-of select="format-number(cas:VATPerUnit, $FoarmatOfFloat, 'pl')"/>
        </td>
        <td valign="top" align="right">
          <xsl:value-of select="format-number(cas:NetMass, $FoarmatOfFloat, 'pl')"/>
        </td>
        <td valign="top" align="right">
          <xsl:value-of select="format-number(cas:GrossMass, $FoarmatOfFloat, 'pl')"/>
        </td>
        <td valign="top" align="right">
          <xsl:value-of select="format-number(cas:Cartons, $FoarmatOfFloat, 'pl')"/>
        </td>
        <td valign="top">
          [norma]
        </td>
      </tr>
    </table>
    <br/>
    <h1>
      KSIEGA USZLACHETNIANIA CZYNNEGO – WYWÓZ
    </h1>
    <table cellspacing="0" cellpadding="0" border="1" width="100%">
      <tr>
        <td colspan="4" valign="top">
          Numer zezwolenia<br/>
          <xsl:value-of select="cas:ConsentNo"/>
        </td>
        <td colspan="11" valign="top">
          Rodzaj towaru kompensacyjnego<br/>
          <xsl:apply-templates select="cas:PCNRecord" />
        </td>
      </tr>
      <tr>
        <td valign="top">
          Lp.
        </td>
        <td valign="top">
          Nr SAD wywozowego
        </td>
        <td valign="top" >
          Data
        </td>
        <td valign="top">
          Faktura
        </td>
        <td valign="top">
          Liczba towarów kompensacyjnych wywożonych [kg]
        </td>
        <td valign="top">
          Strata [kg]
        </td>
        <td valign="top">
          Ilość odpadów powstających przy wytworzeniu towarów kompensacyjnych [kg]
        </td>
        <td valign="top">
          Liczba odpadów wywożonych (w przeliczeniu na surowiec) [kg]
        </td>
        <td valign="top">
          Liczba zużytego surowca do rozliczenia [kg]
        </td>
        <td valign="top">
          Przypadająca kwota zwrotu cła lub zwolnienia zabezpieczenia [PLN]
        </td>
        <td valign="top">
          Przypadająca kwota zwrotu VAT lub zwolnienie zabezpieczenia [PLN]
        </td>
        <td valign="top">
          Suma Cła i VAT [PLN]
        </td>
        <td valign="top">
          Liczba surowca pozostałego do rozliczenia [kg]
        </td>
        <td valign="top">
          Rodzaj procedury celnej
        </td>
        <td valign="top">
          Rodzaj wysyłanego towaru kompensacyjnego
        </td>
      </tr>
      <xsl:apply-templates select="cas:DisposalsColection" />
      <tr>
        <td colspan="4" valign="top">
          Razem
        </td>
        <td valign="top" align="right">
          <xsl:value-of select="format-number(sum(cas:DisposalsColection/cas:DisposalsArray/cas:SettledQuantity), $FoarmatOfFloat, 'pl')"/>
        </td>
        <td valign="top">
          &#160;
        </td>
        <td valign="top">
          &#160;
        </td>
        <td valign="top">
          &#160;
        </td>
        <td valign="top" align="right">
          <xsl:value-of select="format-number(sum(cas:DisposalsColection/cas:DisposalsArray/cas:SettledQuantity)-cas:Cartons, $FoarmatOfFloat, 'pl')"/>
        </td>
        <td valign="top" align="right">
          <xsl:value-of select="format-number(sum(cas:DisposalsColection/cas:DisposalsArray/cas:DutyPerSettledAmount), $FoarmatOfFloat, 'pl')"/>
        </td>
        <td valign="top" align="right">
          <xsl:value-of select="format-number(sum(cas:DisposalsColection/cas:DisposalsArray/cas:VATPerSettledAmount), $FoarmatOfFloat, 'pl')"/>
        </td>
        <td valign="top" align="right">
          <xsl:value-of select="format-number(sum(cas:DisposalsColection/cas:DisposalsArray/cas:DutyAndVAT), $FoarmatOfFloat, 'pl')"/>
        </td>
        <td valign="top">
          &#160;
        </td>
        <td valign="top">
          &#160;
        </td>
        <td valign="top">
          &#160;
        </td>
      </tr>
    </table>
    <table border="0" width="100%">
          <tr>
            <td align="right" height="50" valign="bottom">
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
  </xsl:template>
  <xsl:template match="cas:DisposalsColection">
    <xsl:apply-templates select="cas:DisposalsArray"/>
  </xsl:template>
  <xsl:template match="cas:DisposalsArray">
    <tr>
      <td>
        <xsl:value-of select="cas:No"/>
      </td>
      <td>
        <xsl:value-of select="cas:SADDocumentNo"/>
      </td>
      <td>
        <xsl:value-of select="ms:format-date(cas:SADDate, $FoarmatOfdate)"/>
      </td>
      <td>
        <xsl:value-of select="cas:InvoiceNo"/>
      </td>
      <td align="right">
        <xsl:value-of select="format-number(cas:SettledQuantity, $FoarmatOfFloat, 'pl')"/>
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
      <td align="right">
        <xsl:choose>
          <xsl:when test="cas:ProductCodeNumber='4819100000'">&#160;</xsl:when>
          <xsl:when test="not(cas:ProductCodeNumber='4819100000')"><xsl:value-of select="format-number(cas:SettledQuantity, $FoarmatOfFloat, 'pl')"/></xsl:when>
        </xsl:choose>  
      </td>
      <td align="right">
        <xsl:value-of select="format-number(cas:DutyPerSettledAmount, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td align="right">
        <xsl:value-of select="format-number(cas:VATPerSettledAmount, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td align="right">
        <xsl:value-of select="format-number(cas:DutyAndVAT, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td align="right">
        <xsl:value-of select="format-number(cas:RemainingQuantity, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td>
        <xsl:value-of select="cas:CustomsProcedure"/>
      </td>
      <td>
        <xsl:value-of select="cas:ProductCodeNumber"/>
      </td>
    </tr>
  </xsl:template>
  <xsl:template match="cas:PCNRecord">
    <table border="0" cellspacing="0" cellpadding="0" align="left">
      <xsl:apply-templates select="cas:ProductCodeNumberDesscription"/>
    </table>
  </xsl:template>
  <xsl:template match="cas:ProductCodeNumberDesscription">
    <tr>
      <td>
        <xsl:value-of select="cas:CodeNumber"/> - <xsl:value-of select="cas:Description"/>
      </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>
