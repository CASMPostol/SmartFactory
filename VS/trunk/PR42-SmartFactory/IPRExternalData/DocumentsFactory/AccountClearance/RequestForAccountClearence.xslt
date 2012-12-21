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
          td, li { font-size:9pt; }
          p  { font-size:11pt; }
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
              <p>
                Dokument: 
                 <xsl:value-of select="cas:DocumentName" /><b><xsl:value-of select="cas:DocumentNo" /></b>
              </p>
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
      Wniosek nr <xsl:value-of select="cas:DocumentNo" /> o rozliczenie towaru objętego procedurą uszlachetniania czynnego
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
              Dokument: <xsl:value-of select="cas:DocumentName" /><b><xsl:value-of select="cas:DocumentNo" /></b>
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
        <td align="center">
          Lp.
        </td>
        <td align="center" colspan="4" valign="top">
          Nazwa towaru
        </td>
        <td align="center" valign="top">
          Kod taryfy PCN
        </td>
        <td align="center" valign="top">
          Faktura nr
        </td>
        <td align="center" valign="top">
          Pozycja SAD
        </td>
        <td align="center" valign="top">
          [typ cła] <br/>
          Przypadające cło
        </td>
        <td align="center" valign="top">
          [typ vat]<br/>
          Przypadający VAT
        </td>
        <td align="center" valign="top">
          Ilość netto
        </td>
        <td align="center" valign="top">
          Ilość brutto
        </td>
        <td align="center" valign="top">
          Kartony
        </td>
        <td align="center" valign="top">
          Norma (1-towar kompensacyjny właściwy - netto/brutto)
        </td>
      </tr>
      <tr>
        <td align="center" valign="top">
          &#160;
        </td>
        <td align="center" valign="top">
          Nazwa tytoniu
        </td>
        <td align="center" valign="top">
          Typ
        </td>
        <td align="center" valign="top">
          SKU
        </td>
        <td align="center" valign="top">
          Batch
        </td>
        <td align="center" valign="top">
          &#160;
        </td>
        <td>
          &#160;
        </td>
        <td align="center" valign="top">
          &#160;
        </td>
        <td align="center" valign="top">
          [PLN]
        </td>
        <td align="center" valign="top">
          [PLN]
        </td>
        <td align="center" valign="top">
          [kg]
        </td>
        <td align="center" valign="top">
          [kg]
        </td>
        <td>
          [kg]
        </td>
        <td align="center" valign="top">
          &#160;
        </td>
      </tr>
      <tr>
        <td valign="top">
          1
        </td>
        <td align="center">
          <xsl:value-of select="cas:TobaccoName"/>
        </td>
        <td align="center" valign="top">
          <xsl:value-of select="cas:Grade"/>
        </td>
        <td align="center" valign="top">
          <xsl:value-of select="cas:SKU"/>
        </td>
        <td align="center" valign="top">
          <xsl:value-of select="cas:Batch"/>
        </td>
        <td align="center" valign="top">
          <xsl:value-of select="cas:PCN"/>
        </td>
        <td align="center" valign="top">
          <xsl:value-of select="cas:InvoiceNo"/>
        </td>
        <td align="center" valign="top">
          1
        </td>
        <td align="center" valign="top">
          <xsl:value-of select="cas:DutyName"/>
        </td>
        <td align="center" valign="top">
          <xsl:value-of select="cas:VATName"/>
        </td>
        <td align="center" valign="top">
          <xsl:value-of select="format-number(cas:NetMass, $FoarmatOfFloat, 'pl')"/>
        </td>
        <td align="center" valign="top">
          <xsl:value-of select="format-number(cas:GrossMass, $FoarmatOfFloat, 'pl')"/>
        </td>
        <td align="center" valign="top">
          <xsl:value-of select="format-number(cas:Cartons, $FoarmatOfFloat, 'pl')"/>
        </td>
        <td align="center" valign="top">
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
        <td align="center" valign="top">
          Lp.
        </td>
        <td align="center" valign="top">
          Nr SAD wywozowego
        </td>
        <td align="center" valign="top" >
          Data
        </td>
        <td align="center" valign="top">
          Faktura
        </td>
        <td align="center" valign="top">
          Liczba towarów kompensa-cyjnych wywożonych
        </td>
        <td align="center" valign="top">
          Strata [kg]
        </td>
        <td align="center" valign="top">
          Ilość odpadów powstających przy wytworzeniu towarów kompensa-cyjnych
        </td>
        <td align="center" valign="top">
          Liczba odpadów wywożonych (w przeliczeniu na surowiec)
        </td>
        <td align="center" valign="top">
          Liczba zużytego surowca do rozliczenia
        </td>
        <td align="center" valign="top">
          Przypadająca kwota zwrotu cła lub zwolnienia zabezpieczenia
        </td>
        <td align="center" valign="top">
          Przypadająca kwota zwrotu VAT lub zwolnienie zabezpieczenia
        </td>
        <td align="center" valign="top">
          Suma Cła i VAT
        </td>
        <td align="center" valign="top">
          Liczba surowca pozostałego do rozliczenia
        </td>
        <td align="center" valign="top">
          Rodzaj procedury celnej
        </td>
        <td align="center" valign="top">
          Rodzaj wysyłanego towaru kompensa-cyjnego
        </td>
      </tr>
      <tr>
        <td valign="top">
          &#160;
        </td>
        <td valign="top">
          &#160;
        </td>
        <td valign="top" >
          &#160;
        </td>
        <td align="center" valign="top">
          &#160;
        </td>
        <td align="center" valign="top">
          [kg]
        </td>
        <td align="center" valign="top">
          [kg]
        </td>
        <td align="center" valign="top">
          [kg]
        </td>
        <td align="center" valign="top">
          [kg]
        </td>
        <td align="center" valign="top">
          [kg]
        </td>
        <td align="center" valign="top">
          [PLN]
        </td>
        <td align="center" valign="top">
          [PLN]
        </td>
        <td align="center" valign="top">
          [PLN]
        </td>
        <td align="center" valign="top">
          [kg]
        </td>
        <td valign="top">
          &#160;
        </td>
        <td valign="top">
          &#160;
        </td>
      </tr>
      <xsl:apply-templates select="cas:DisposalsColection" />
      <tr>
        <td colspan="4" valign="top">
          <p>
            <b>Razem</b>
          </p>
        </td>
        <td valign="top"  align="center">
          <p>
            <b>
              <xsl:value-of select="format-number(sum(cas:DisposalsColection/cas:DisposalsArray/cas:SettledQuantity), $FoarmatOfFloat, 'pl')"/>
            </b>
          </p>
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
        <td valign="top"  align="center">
          <p>
            <b>
              <xsl:value-of select="format-number(sum(cas:DisposalsColection/cas:DisposalsArray/cas:SettledQuantity)-cas:Cartons, $FoarmatOfFloat, 'pl')"/>
            </b>
          </p>
        </td>
        <td valign="top"  align="center">
          <p>
            <b>
              <xsl:value-of select="format-number(sum(cas:DisposalsColection/cas:DisposalsArray/cas:DutyPerSettledAmount), $FoarmatOfFloat, 'pl')"/>
            </b>
          </p>
        </td>
        <td valign="top"  align="center">
          <p>
            <b>
              <xsl:value-of select="format-number(sum(cas:DisposalsColection/cas:DisposalsArray/cas:VATPerSettledAmount), $FoarmatOfFloat, 'pl')"/>
            </b>
          </p>
        </td>
        <td valign="top"  align="center">
          <p>
            <b>
              <xsl:value-of select="format-number(sum(cas:DisposalsColection/cas:DisposalsArray/cas:DutyAndVAT), $FoarmatOfFloat, 'pl')"/>
            </b>
          </p>
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
      <td align="center">
        <xsl:value-of select="cas:No"/>
      </td>
      <td align="center">
        <xsl:value-of select="cas:SADDocumentNo"/>
      </td>
      <td align="center" nowrap="true">
        <xsl:value-of select="ms:format-date(cas:SADDate, $FoarmatOfdate)"/>
      </td>
      <td align="center">
        <xsl:value-of select="cas:InvoiceNo"/>
      </td>
      <td  align="center">
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
      <td align="center">
        <xsl:choose>
          <xsl:when test="cas:ProductCodeNumber='4819100000'">&#160;</xsl:when>
          <xsl:when test="not(cas:ProductCodeNumber='4819100000')"><xsl:value-of select="format-number(cas:SettledQuantity, $FoarmatOfFloat, 'pl')"/></xsl:when>
        </xsl:choose>  
      </td>
      <td align="center">
        <xsl:value-of select="format-number(cas:DutyPerSettledAmount, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td align="center">
        <xsl:value-of select="format-number(cas:VATPerSettledAmount, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td align="center">
        <xsl:value-of select="format-number(cas:DutyAndVAT, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td align="center">
        <xsl:value-of select="format-number(cas:RemainingQuantity, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td align="center">
        <xsl:value-of select="cas:CustomsProcedure"/>
      </td>
      <td align="center">
        <xsl:value-of select="cas:ProductCodeNumber"/>
      </td>
    </tr>
  </xsl:template>
  <xsl:template match="cas:PCNRecord">
      <xsl:apply-templates select="cas:ProductCodeNumberDesscription"/>
  </xsl:template>
  <xsl:template match="cas:ProductCodeNumberDesscription">
        <xsl:value-of select="cas:CodeNumber"/> - <xsl:value-of select="cas:Description"/>;&#160;
  </xsl:template>
</xsl:stylesheet>
