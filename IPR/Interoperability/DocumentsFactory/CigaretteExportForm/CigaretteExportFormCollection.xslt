<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ms="urn:schemas-microsoft-com:xslt"
    xmlns:cas="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/CigaretteExportForm.xsd"
>
  <xsl:output method="html" indent="yes"/>
  <xsl:decimal-format name="pl" decimal-separator=',' grouping-separator='.' />
  <xsl:variable name="FoarmatOfFloat" >###.##0,00</xsl:variable>
  <xsl:variable name="FoarmatOfFloatPrices" >###.##0,000</xsl:variable>
  <xsl:variable name="FoarmatOfdate" >dd-MM-yyyy</xsl:variable>
  <xsl:template match="/" >
    <html>
      <head>
        <title>Zbiorczy proces technologiczny produkcji papierosów na eksport</title>
        <style type="text/css">
          td, p, li { font-size:10pt; }
          h3 { font-size:11pt; }
          th { font-size:11pt; }
          h2 { font-size:12pt; }
          h1 { font-size:14pt; text-align:center; }
        </style>
      </head>
      <body>
        <table width="100%" border="0">
          <tr>
            <td>
              <p align="right">
                Gostków Stary, <xsl:value-of select="ms:format-date(cas:CigaretteExportFormCollection/cas:DocumentDate, $FoarmatOfdate)"/>
              </p>
            </td>
          </tr>
        </table>
        <xsl:apply-templates select="cas:CigaretteExportFormCollection"> </xsl:apply-templates>
        <table border="0" width="100%">
        <tr>
            <td align="right" valign="bottom">
              <br/><br/><br/>
              .............................................
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
  <xsl:template match="cas:CigaretteExportFormCollection">
    <h1>
      Procesy technologiczne (<xsl:value-of select="cas:NumberOfDocuments"/> x batch) dla papierosów z faktury nr <xsl:value-of select="cas:InvoiceNo"/>
    </h1>
    <p>
      Dokument zbiorczy nr: <xsl:value-of select="cas:DocumentNo" />
    </p>
    <xsl:apply-templates select="cas:CigaretteExportForms" />
  </xsl:template>
  <xsl:template match="cas:CigaretteExportForm">
    <div>
      <h2>
        Proces technologiczny produkcji papierosów na eksport
      </h2>
      <table border="1" cellspacing="0" cellpadding="0">
        <tr>
          <td>Dokument nr:</td>
          <td colspan="4">
            <xsl:value-of select="cas:DocumentNo"/>
          </td>
        </tr>
        <tr>
          <td>Format:</td>
          <td colspan="4">
            <xsl:value-of select="cas:ProductFormat"/>
          </td>
        </tr>
        <tr>
          <td>Batch:</td>
          <td colspan="4">
            <xsl:value-of select="cas:FinishedGoodBatch"/>
          </td>
        </tr>
        <tr>
          <td>Faktura:</td>
          <td colspan="4">
            <xsl:value-of select="../../cas:InvoiceNo"/>
          </td>
        </tr>
        <tr>
          <td >Procedura: </td>
          <td colspan="4">
            <xsl:value-of select="cas:CustomsProcedure"/>
          </td>
        </tr>
        <tr>
          <td>Nazwa papierosów: </td>
          <td>
            <xsl:value-of select="cas:FamilyDescription"/>
          </td>
          <td>
            <xsl:value-of select="cas:BrandDescription"/>
          </td>
          <td>
            <xsl:value-of select="cas:FinishedGoodSKU"/>
          </td>
          <td>
            <xsl:value-of select="cas:FinishedGoodSKUDescription"/>
          </td>
        </tr>
        <tr>
          <td>Ilość: </td>
          <td align="right" colspan="2">
            <xsl:value-of select="format-number((cas:FinishedGoodQantity)*1000, $FoarmatOfFloat, 'pl')"/>&#160;sztuk
          </td>
        </tr>
      </table>
      <h2>I.	Charakterystyka ogólna procesu technologicznego:</h2>
      <p>Przedstawiony niżej proces technologiczny obejmuje wykonanie papierosów z przeznaczeniem na eksport, z tytoni importowanych dla potrzeb firmy JTI i obejmuje dwa etapy:</p>
      <h3>1. Wykonanie krajanki tytoniowej z tytoni i żył tytoniowych importowanych objętych procedurą uszlachetniania czynnego wg zestawienia:</h3>
      <table border="1" cellspacing="0" cellpadding="0">
        <tr align="center" >
          <th>Lp.</th>
          <th>Nr SAD</th>
          <th>Data</th>
          <th>SKU tytoniu</th>
          <th>Batch tytoniu</th>
          <th>Cena</th>
          <th>Ilość–tytoń (kg)</th>
          <th>Likw.</th>
          <th>Wartość</th>
          <th>Waluta</th>
          <th>
            Cło<br/>(PLN)
          </th>
          <th>
            Vat<br/>(PLN)
          </th>
        </tr>
        <xsl:apply-templates select="cas:Ingredients/cas:IPRIngredient" />
        <tr>
          <td colspan="6" align="left">
            Ilość liści tytoniowych - Suma
          </td>
          <td align="center">
            <xsl:value-of select="format-number(cas:IPTMaterialQuantityTotal, $FoarmatOfFloat, 'pl')"/>
          </td>
          <td align="center" bgcolor="gray"> --- </td>
          <td colspan="4" align="right">
            <xsl:apply-templates select="cas:IPTDutyVatTotals" />
          </td>
        </tr>
      </table>
      <p>oraz</p>
      <table border="1" cellspacing="0" cellpadding="0">
        <tr align="center">
          <th >Pył</th>
          <th >Odpad</th>
          <th >SH Menthol</th>
          <th >Suma (kg)</th>
        </tr>
        <tr align="right" >
          <td align="center">
            <xsl:value-of select="format-number(cas:DustKg, $FoarmatOfFloat, 'pl')"/>
          </td>
          <td align="center">
            <xsl:value-of select="format-number(cas:WasteKg, $FoarmatOfFloat, 'pl')"/>
          </td>
          <td align="center">
            <xsl:value-of select="format-number(cas:SHMentholKg, $FoarmatOfFloat, 'pl')"/>
          </td>
          <td align="center">
            <xsl:value-of select="format-number(cas:IPRRestMaterialQantityTotal, $FoarmatOfFloat, 'pl')"/>
          </td>
        </tr>
      </table>
      <p>a także tytoni i żył nie objętych procedurą uszlachetniania czynnego wg zestawienia:</p>
      <table border="1" cellspacing="0" cellpadding="0">
        <tr >
          <th>SKU tytoniu</th>
          <th>Batch tytoniu</th>
          <th>Ilość [kg]</th>
        </tr>
        <xsl:apply-templates select="cas:Ingredients/cas:RegularIngredient" />
        <tr>
          <td colspan="2" align="left" >
            Suma
          </td>
          <td align="center">
            <xsl:value-of select="format-number(cas:RegularMaterialQuantityTotal, $FoarmatOfFloat, 'pl')"/>
          </td>
        </tr>
      </table>
      <p>
        Suma tytoni z zawieszeń i nie z zawieszeń: <xsl:value-of select="format-number(cas:TobaccoTotal, $FoarmatOfFloat, 'pl')"/> kg.
      </p>
      <h4>Opis procesu technologicznego:</h4>
      <p>W trakcie procesu technologicznego tytoń i żyły tytoniowe są poddane obróbce na linii przerobu tytoniu COMAS-GARBUIO w JTI Polska Sp. z o.o. w Wartkowicach, Zakład w Gostkowie Starym wg następującej instrukcji technologicznej:</p>
      <ol>
        <li>Rozlistkowanie tytoniu.</li>
        <li>Dozowanie dodatków tytoniowych i kondycjonowanie.</li>
        <li>Składowanie.</li>
        <li>Krojenie.</li>
        <li>Suszenie.</li>
        <li>Dozowanie dodatków tytoniowych.</li>
        <li>Aromatyzowanie i składowanie.</li>
        <li>Pakowanie.</li>
        <li>Przekazanie do produkcji papierosów.</li>
      </ol>
      <h3>
        2. Wykonanie i spakowanie papierosów z użyciem materiałów nie objętych procedurą uszlachetniania czynnego. 
      </h3>
      <p>Z krajanki papierosowej wymienionej w pkt. 1 oraz innych materiałów nietytoniowych uformowane zostają papierosy, potem spakowane w opakowania jednostkowe i zbiorcze na maszynach znajdujących się w JTI Polska Sp. z o.o. Zakład w Gostkowie Starym.</p>
      <h2>II.	Określenia parametrów wagowych obrabianego materiału</h2>
      <ol>
        <li>
          Na 1 mln. szt. papierosów format <xsl:value-of select="cas:ProductFormat"/> zużywa się w trakcie procesu produkcyjnego <xsl:value-of select="format-number(cas:CTFUsagePer1MFinishedGoodsMin, $FoarmatOfFloat, 'pl')"/> - <xsl:value-of select="format-number(cas:CTFUsagePer1MFinishedGoodsMax, $FoarmatOfFloat, 'pl')"/>  kg tytoniu. 
          <br/>
          Dodatkowo w trakcie procesu technologicznego powstają straty okreslone jako odpady. Skład odpadów:
          <ul>
          a.	pył tytoniowy<br/>
          b.	ścinki żył tytoniowych<br/>
          c.	pozostały odpad<br/>
          d.	papierosy aromatyzowane aromatem  mentolowym i owocowym nie spełniajace wymagań jakościowych (odrzut z maszyny).<br/>
          </ul>
          Dodatkowo przy produkcji 1 mln papierosów powstaje <xsl:value-of select="format-number(cas:WasteCoefficient * 100, $FoarmatOfFloat, 'pl')"/>% odpadów (zużycie tytoniu należy powiększyć o ten współczynnik)
        </li>
      </ol>
    </div>
  </xsl:template>
  <xsl:template match="cas:IPTDutyVatTotals">
    <table width="100%">
      <xsl:apply-templates select="cas:AmountOfMoney" />
    </table>
  </xsl:template>
  <xsl:template match="cas:AmountOfMoney">
    <tr>
      <td>
        <table width="100%">
          <xsl:apply-templates select="cas:ArrayOfTotals" />
        </table>
      </td>
      <td align="right">
        <xsl:value-of select="format-number(sum(cas:ArrayOfTotals/cas:IPRMaterialDutyTotal), $FoarmatOfFloat, 'pl')"/>
      </td>
      <td align="right">
        <xsl:value-of select="format-number(sum(cas:ArrayOfTotals/cas:IPRMaterialVATTotal), $FoarmatOfFloat, 'pl')"/>
      </td>
    </tr>
  </xsl:template>
  <xsl:template match="cas:ArrayOfTotals" >
    <tr align="right" >
      <td align="right">
        <xsl:value-of select="format-number(cas:IPRMaterialValueTotal, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td align="right">
        <xsl:value-of select="cas:Currency"/>
      </td>
    </tr>
  </xsl:template>   
  <xsl:template match="cas:IPRIngredient" >
    <tr align="right" >
      <td>
        <xsl:number/>
      </td>
      <td>
        <xsl:value-of select="cas:DocumentNoumber"/>
      </td>
      <td align="center" nowrap="true">
        <xsl:value-of select="ms:format-date(cas:Date, $FoarmatOfdate)"/>
      </td>
      <td align="center">
        <xsl:value-of select="cas:TobaccoSKU"/>
      </td>
      <td align="center">
        <xsl:value-of select="cas:TobaccoBatch"/>
      </td>
      <td align="center">
        <xsl:value-of select="format-number(cas:TobaccoUnitPrice, $FoarmatOfFloatPrices, 'pl')"/>
      </td>
      <td align="center">
        <xsl:value-of select="format-number(cas:TobaccoQuantity, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td align="center" nowrap="true">
        <xsl:choose>
          <xsl:when test="cas:ItemClearingType='PartialWindingUp'">l. część</xsl:when>
          <xsl:when test="cas:ItemClearingType='TotalWindingUp'">l. całk</xsl:when>
        </xsl:choose>
      </td>
      <td align="center">
        <xsl:value-of select="format-number(cas:TobaccoValue, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td align="center">
        <xsl:value-of select="cas:Currency"/>
      </td>
      <td align="center">
        <xsl:value-of select="format-number(cas:Duty, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td align="center">
        <xsl:value-of select="format-number(cas:VAT, $FoarmatOfFloat, 'pl')"/>
      </td>
    </tr>
  </xsl:template>
  <xsl:template match="cas:RegularIngredient">
    <tr align="center">
      <td>
        <xsl:value-of select="cas:TobaccoSKU"/>
      </td>
      <td>
        <xsl:value-of select="cas:TobaccoBatch"/>
      </td>
      <td align="center">
        <xsl:value-of select="format-number(cas:TobaccoQuantity, $FoarmatOfFloat, 'pl')" />
      </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>