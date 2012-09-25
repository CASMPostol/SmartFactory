﻿<?xml version="1.0" encoding="utf-8"?>
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
      </head>
      <body>
        <table  >
          <tr>
            <td>
              <p align="right">
                Gostków Stary, <xsl:value-of select="ms:format-date(cas:CigaretteExportFormCollection/cas:DocumentDate, $FoarmatOfdate)"/>
              </p>
            </td>
          </tr>
        </table>
        <xsl:apply-templates select="cas:CigaretteExportFormCollection"> </xsl:apply-templates>
      </body>
    </html>
  </xsl:template>
  <xsl:template match="cas:CigaretteExportFormCollection">
    <h1>
      Procesy technologiczne (sztuk: <xsl:value-of select="cas:NumberOfDocuments"/>) dla papierosów z faktury nr <xsl:value-of select="cas:InvoiceNo"/>
    </h1>
    <p>
      Dokument zbiorczy nr: <xsl:value-of select="cas:DocumentNo" />
    </p>
    <xsl:apply-templates select="cas:CigaretteExportForms" />
  </xsl:template>
  <xsl:template match="cas:CigaretteExportForm">
    <div>
      <h2>
        <xsl:number></xsl:number>. Proces technologiczny produkcji papierosów na eksport
      </h2>
      <table border="1">
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
            <xsl:value-of select="format-number(cas:FinishedGoodQantity, $FoarmatOfFloat, 'pl')"/>
          </td>
          <td align="left">
            <xsl:value-of select="cas:FinishedGoodUnit"/>
          </td>
        </tr>
      </table>
      <h3>I.	Charakterystyka ogólna procesu technologicznego:</h3>
      <p>Przedstawiony niżej proces technologiczny obejmuje wykonanie papierosów z przeznaczeniem na eksport, z tytoni importowanych dla potrzeb firmy JTI i obejmuje dwa etapy:</p>
      <h4>1. Wykonanie krajanki tytoniowej z tytoni i żył tytoniowych importowanych objętych procedurą uszlachetniania czynnego wg zestawienia:</h4>
      <table border="1">
        <tr align="center" >
          <th>Lp.</th>
          <th>Nr SAD</th>
          <th>Data</th>
          <th>SKU tytoniu</th>
          <th>Batch tytoniu</th>
          <th>Cena</th>
          <th>Ilość–tytoń (kg)</th>
          <th>Likwidacja</th>
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
          <td align="right">
            <xsl:value-of select="format-number(cas:IPTMaterialQuantityTotal, $FoarmatOfFloat, 'pl')"/>
          </td>
          <td align="center" bgcolor="gray"> --- </td>
          <td colspan="4" align="right">
            <xsl:apply-templates select="cas:IPTDutyVatTotals" />
          </td>
        </tr>
      </table>
      <p>oraz</p>
      <table border="1">
        <tr align="center">
          <th >Pył</th>
          <th >Odpad</th>
          <th >SH Menthol</th>
          <th >Suma (kg)</th>
        </tr>
        <tr align="right" >
          <td >
            <xsl:value-of select="format-number(cas:DustKg, $FoarmatOfFloat, 'pl')"/>
          </td>
          <td >
            <xsl:value-of select="format-number(cas:WasteKg, $FoarmatOfFloat, 'pl')"/>
          </td>
          <td >
            <xsl:value-of select="format-number(cas:SHMentholKg, $FoarmatOfFloat, 'pl')"/>
          </td>
          <td >
            <xsl:value-of select="format-number(cas:IPRRestMaterialQantityTotal, $FoarmatOfFloat, 'pl')"/>
          </td>
        </tr>
      </table>
      <p>a także tytoni i żył nie objętych procedurą uszlachetniania czynnego wg zestawienia:</p>
      <table border="1">
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
          <td align="right">
            <xsl:value-of select="format-number(cas:RegularMaterialQuantityTotal, $FoarmatOfFloat, 'pl')"/>
          </td>
        </tr>
      </table>
      <p>
        Suma tytoni z zawieszeń i nie z zawieszeń: <xsl:value-of select="format-number(cas:TobaccoTotal, $FoarmatOfFloat, 'pl')"/> kg.
      </p>
      <h4>Opis procesu technologicznego:</h4>
      <p>W trakcie procesu technologicznego tytoń jest poddany obróbce w linii przerobu tytoni w JTI Polska Sp. z o.o., Zakład w Gostkowie wg szczegółowej instrukcji technologicznej ustalonej z odbiorcą:</p>
      <ol>
        <li>Rozlistkowanie tytoni</li>
        <li>Kondycjonowanie</li>
        <li>Składowanie</li>
        <li>Krojenie tytoni</li>
        <li>Suszenie</li>
        <li>Odbiór krajanki w opakowania</li>
      </ol>
      <h4>
        2. Wykonanie i spakowanie papierosów za użytkiem materiałów nie objętych procedurą uszlachetniania czynnego.
      </h4>
      <p>Z krajanki tytoniowej wymienionej w pkt. 1 oraz innych materiałów uformowany zostaje papieros oraz spakowany w opakowanie jednostkowe i zbiorcze na maszynach znajdujących się w JTI Polska Sp. z o.o., Zakład w Gostkowie.</p>
      <h2>II.	Określenia parametrów wagowych obrabianego materiału</h2>
      <ol>
        <li>
          Z wymienionego w pkt.1 materiału wsadowego poddanego obróbce w linii technologicznej uzyskuje się krajankę tytoniową o współczynniku produktywności
          <xsl:value-of select="format-number(cas:CTFUsagePerUnitMin, $FoarmatOfFloat, 'pl')"/>%-<xsl:value-of select="format-number(cas:CTFUsagePerUnitMax, $FoarmatOfFloat, 'pl')"/>%
          tzn. z 1000 kg tytoniu uzyskuje się
          <xsl:value-of select="format-number(cas:CTFUsagePerUnitMin * 1000, $FoarmatOfFloat, 'pl')"/>-
          <xsl:value-of select="format-number(cas:CTFUsagePerUnitMax * 1000, $FoarmatOfFloat, 'pl')"/>kg (krajanki tytoniowej)
        </li>
        <li>
          Na 1 mln. szt. gotowych papierosów zużywa się w trakcie procesu produkcyjnego
          <xsl:value-of select="format-number(cas:CTFUsagePer1MFinishedGoodsMin, $FoarmatOfFloat, 'pl')"/>-
          <xsl:value-of select="format-number(cas:CTFUsagePer1MFinishedGoodsMax, $FoarmatOfFloat, 'pl')"/> kg krajanki tytoniowej.
        </li>
        <li>
          Dodatkowo przy produkcji 1 mln papierosów powstaje <xsl:value-of select="format-number(cas:WasteCoefficient * 100, $FoarmatOfFloat, 'pl')"/>% odpadów (zużycie tytoniu należy powiększyć o ten współczynnik)
        </li>
      </ol>
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
    </div>
  </xsl:template>
  <xsl:template match="cas:IPTDutyVatTotals">
    <table width="100%">
      <xsl:apply-templates select="cas:AmountOfMoney" />
    </table>
  </xsl:template>
  <xsl:template match="cas:AmountOfMoney">
    <xsl:apply-templates select="cas:ArrayOfTotals" />
  </xsl:template>
  <xsl:template match="cas:ArrayOfTotals" >
    <tr align="right" >
      <td align="right">
        <xsl:value-of select="format-number(cas:IPRMaterialValueTotal, $FoarmatOfFloatPrices, 'pl')"/>
      </td>
      <td align="right">
        <xsl:value-of select="cas:Currency"/>
      </td>
      <td align="right">
        <xsl:value-of select="format-number(cas:IPRMaterialDutyTotal, $FoarmatOfFloatPrices, 'pl')"/>
      </td>
      <td align="right">
        <xsl:value-of select="format-number(cas:IPRMaterialVATTotal, $FoarmatOfFloatPrices, 'pl')"/>
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
      <td>
        <xsl:value-of select="ms:format-date(cas:Date, $FoarmatOfdate)"/>
      </td>
      <td>
        <xsl:value-of select="cas:TobaccoSKU"/>
      </td>
      <td>
        <xsl:value-of select="cas:TobaccoBatch"/>
      </td>
      <td>
        <xsl:value-of select="format-number(cas:TobaccoUnitPrice, $FoarmatOfFloatPrices, 'pl')"/>
      </td>
      <td>
        <xsl:value-of select="format-number(cas:TobaccoQuantity, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td>
        <xsl:choose>
          <xsl:when test="cas:ItemClearingType='PartialWindingUp'">L. częściowa</xsl:when>
          <xsl:when test="cas:ItemClearingType='TotalWindingUp'">L. całkowita</xsl:when>
        </xsl:choose>
      </td>
      <td>
        <xsl:value-of select="format-number(cas:TobaccoValue, $FoarmatOfFloatPrices, 'pl')"/>
      </td>
      <td>
        <xsl:value-of select="cas:Currency"/>
      </td>
      <td>
        <xsl:value-of select="format-number(cas:Duty, $FoarmatOfFloatPrices, 'pl')"/>
      </td>
      <td>
        <xsl:value-of select="format-number(cas:VAT, $FoarmatOfFloatPrices, 'pl')"/>
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
      <td align="right">
        <xsl:value-of select="format-number(cas:TobaccoQuantity, $FoarmatOfFloat, 'pl')" />
      </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>