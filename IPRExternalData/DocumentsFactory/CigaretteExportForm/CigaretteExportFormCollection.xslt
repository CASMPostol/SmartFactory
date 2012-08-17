<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ms="urn:schemas-microsoft-com:xslt"
    xmlns:cas="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/CigaretteExportForm.xsd"
>
  <xsl:output method="html" indent="yes"/>
  <xsl:decimal-format name="pl" decimal-separator=',' grouping-separator='.' />
  <xsl:variable name="FoarmatOfFloat" >###.##0,00</xsl:variable>
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
                Gostków Stary, TBD: Date
              </p>
            </td>
          </tr>
          <tr>
            <td>
              <h1 align="left" >
                Zbiorczy proces technologiczny produkcji papierosów na eksport
              </h1>
            </td>
          </tr>
        </table>
        <xsl:apply-templates select="cas:CigaretteExportFormCollection"> </xsl:apply-templates>
      </body>
    </html>
  </xsl:template>
  <xsl:template match="cas:CigaretteExportFormCollection">
    <p>
      Dokument Zbiorczy Nr: <xsl:value-of select="cas:DocumentNo" />
    </p>
    <xsl:apply-templates select="cas:CigaretteExportForms" />
  </xsl:template>
  <xsl:template match="cas:CigaretteExportForm">
    <div>
      <h2>
        <xsl:number></xsl:number>. Proces technologiczny produkcji papierosów na eksport
      </h2>
      <table>
        <tr>
          <td>Dokument nr:</td>
          <td>
            <xsl:value-of select="cas:DocumentNo"/>
          </td>
        </tr>
        <tr>
          <td>Format:</td>
          <td>
            <xsl:value-of select="cas:ProductFormat"/>
          </td>
        </tr>
        <tr>
          <td>Batch:</td>
          <td>
            <xsl:value-of select="cas:FinishedGoodBatch"/>
          </td>
        </tr>
        <tr>
          <td>Faktura:</td>
          <td>
            <xsl:value-of select="cas:TBDInvoiceNo"/>
          </td>
        </tr>
        <tr>
          <td>Procedura: </td>
          <td>
            <xsl:value-of select="cas:CustomsProcedure"/>
          </td>
        </tr>
        <tr>
          <td>NAZWA PAPIEROSÓW: </td>
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
          <td align="right">
            <xsl:value-of select="format-number(cas:FinishedGoodQantity, $FoarmatOfFloat, 'pl')"/>
          </td>
          <td align="left">
            <xsl:value-of select="cas:FinishedGoodUnit"/>
          </td>
        </tr>
      </table>
      <h3>I.	Charakterystyka ogólna procesu technologicznego:</h3>
      <h4>a. Receptura procesu technologicznego:</h4>
      <p>Przedstawiony niżej proces technologiczny obejmuje wykonanie papierosów z przeznaczeniem na eksport, z tytoni importowanych dla potrzeb firmy JTI i obejmuje dwa etapy:</p>
      1. Wykonanie krajanki tytoniowej z tytoni i żył tytoniowych importowanych objętych procedurą uszlachetniania czynnego wg zestawienia:
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
          <td align ="center">
            ---
          </td>
          <td align="right">
            <xsl:value-of select="format-number(cas:IPTMaterialValueTotal, $FoarmatOfFloat, 'pl')"/>
          </td>
          <td align ="center">
            ---
          </td>
          <td align="right">
            <xsl:value-of select="format-number(cas:IPTMaterialDutyTotal, $FoarmatOfFloat, 'pl')"/>
          </td>
          <td align="right">
            <xsl:value-of select="format-number(cas:IPTMaterialVATTotal, $FoarmatOfFloat, 'pl')"/>
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
          <td>
            <xsl:value-of select="format-number(cas:TobaccoTotal, $FoarmatOfFloat, 'pl')"/>
          </td>
        </tr>       
      </table>
      <p>Suma tytoni z zawieszeń i nie z zawieszeń: <xsl:value-of select="format-number(cas:RegularMaterialQuantityTotal, $FoarmatOfFloat, 'pl')"/> kg.</p>
      <h3>b. Opis procesu technologicznego:</h3>
      <p>W trakcie procesu technologicznego tytoń jest poddany obróbce w linii przerobu tytoni w JTI Polska Sp. z o.o., Zakład w Gostkowie wg szczegółowej instrukcji technologicznej ustalonej z odbiorcą:</p>
      <ol>
        <li>Rozlistkowanie tytoni</li>
        <li>Kondycjonowanie</li>
        <li>Składowanie</li>
        <li>Krojenie tytoni</li>
        <li>Suszenie</li>
        <li>Odbiór krajanki w opakowania</li>
      </ol>
      <p>
        2. Wykonanie i spakowanie papierosów za użytkiem materiałów nie objętych procedurą uszlachetniania czynnego.
      </p>
      <p>Z krajanki tytoniowej wymienionej w pkt. 1 oraz innych materiałów uformowany zostaje papieros oraz spakowany w opakowanie jednostkowe i zbiorcze na maszynach znajdujących się w JTI Polska Sp. z o.o., Zakład w Gostkowie.</p>
      <h4>II.	Określenia parametrów wagowych obrabianego materiału</h4>
      <ol>
        <li>
          Z wymienionego w pkt.1 materiału wsadowego poddanego obróbce w linii technologicznej uzyskuje się krajankę tytoniową o współczynniku produktywności
          <xsl:value-of select="format-number(cas:CTFUsageMin, $FoarmatOfFloat, 'pl')"/>%-<xsl:value-of select="format-number(cas:CTFUsageMax, $FoarmatOfFloat, 'pl')"/>%
          tzn. z 1000 kg tytoniu uzyskuje się <xsl:value-of select="format-number(cas:CTFUsagePerUnitMin, $FoarmatOfFloat, 'pl')"/>-<xsl:value-of select="format-number(cas:CTFUsagePerUnitMax, $FoarmatOfFloat, 'pl')"/> kg (krajanki tytoniowej)
        </li>
        <li>
          Na 1 mln. szt. gotowych papierosów zużywa się w trakcie procesu produkcyjnego 
          <xsl:value-of select="format-number(cas:CTFUsagePer1MFinishedGoodsMin, $FoarmatOfFloat, 'pl')"/>-
          <xsl:value-of select="format-number(cas:CTFUsagePer1MFinishedGoodsMax, $FoarmatOfFloat, 'pl')"/> kg krajanki tytoniowej.
        </li>
        <li>
          Dodatkowo przy produkcji 1 mln papierosów powstaje <xsl:value-of select="format-number(cas:CTFUsagePer1MFinishedGoodsMax, $FoarmatOfFloat, 'pl')"/>% odpadów (zużycie tytoniu należy powiększyć o ten współczynnik)
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
        <xsl:value-of select="format-number(cas:TobaccoUnitPrice, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td>
        <xsl:value-of select="format-number(cas:TobaccoQuantity, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td>
        <xsl:choose>
          <xsl:when test="cas:ItemClearingType='PartialWindingUp'">Częśćiowa</xsl:when>
          <xsl:when test="cas:ItemClearingType='TotalWindingUp'">Całkowita</xsl:when>
        </xsl:choose>
      </td>
      <td>
        <xsl:value-of select="format-number(cas:TobaccoValue, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td>
        <xsl:value-of select="cas:Currency"/>
      </td>
      <td>
        <xsl:value-of select="format-number(cas:Duty, $FoarmatOfFloat, 'pl')"/>
      </td>
      <td>
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
      <td align="right">
        <xsl:value-of select="format-number(cas:TobaccoQuantity, $FoarmatOfFloat, 'pl')" />
      </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>