<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ms="urn:schemas-microsoft-com:xslt"
    xmlns:cas="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/CigaretteExportForm.xsd"
>
  <xsl:output method="html" indent="yes"/>
  <xsl:variable name="FoarmatOfFloat" >### ##0.00</xsl:variable>
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
            <b>TBD</b>
          </td>
        </tr>
        <tr>
          <td>Faktura:</td>
          <td>
            <b>TBD</b>
          </td>
        </tr>
      </table>
      <table>
        <tr>
          <td>Procedura: </td>
          <td>
            TBD
          </td>
        </tr>
        <tr>
          <td>NAZWA PAPIEROSÓW: </td>
          <td>-- FamilyDescription --</td>
          <td>-- BrandDescription --</td>
          <td>-- Material --</td>
          <td>-- MaterialDescription  --</td>
        </tr>
        <tr>
          <td>Ilość: </td>
          <td align="right">-- Qantity --</td>
          <td align="left">-- Unit --</td>
        </tr>
      </table>
      <h3>I.	Charakterystyka ogólna procesu technologicznego:</h3>
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
          <td colspan="6">
            Ilość liści tytoniowych - Suma
          </td>
          <td>
            <b>TBD</b>
          </td>
          <td>
            -
          </td>
          <td>
            <b>TBD Wartość</b>
          </td>
          <td>
            -
          </td>
          <td>
            <b>TBD Cło</b>
          </td>
          <td>
            <b>TBD Cło</b>
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
            <xsl:value-of select="format-number(cas:DustKg, $FoarmatOfFloat)"/>
          </td>
          <td >
            <xsl:value-of select="format-number(cas:WasteKg, $FoarmatOfFloat)"/>
          </td>
          <td >
            <xsl:value-of select="format-number(cas:SHMentholKg, $FoarmatOfFloat)"/>
          </td>
          <td >
            TBD
          </td>
        </tr>
      </table>
      <p>a także tytoni i żył nie objętych procedurą uszlachetniania czynnego wg zestawienia:</p>
      <table border="1">
        <xsl:apply-templates select="cas:Ingredients/cas:RegularIngredient" />
      </table>
      <p>Suma tytoni z zawieszeń i nie z zawieszeń: [suma ilości z tabelek ] kg.</p>
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
      <p>
        2. Wykonanie i spakowanie papierosów za użytkiem materiałów nie objętych procedurą uszlachetniania czynnego.
      </p>
      <p>Z krajanki tytoniowej wymienionej w pkt. 1 oraz innych materiałów uformowany zostaje papieros oraz spakowany w opakowanie jednostkowe i zbiorcze na maszynach znajdujących się w JTI Polska Sp. z o.o., Zakład w Gostkowie.</p>
      <h3>II.	Określenia parametrów wagowych obrabianego materiału</h3>
      <ol>
        <li>
          Z wymienionego w pkt.1 materiału wsadowego poddanego obróbce w linii technologicznej uzyskuje się krajankę tytoniową o współczynniku produktywności [Wsp produktywnosci1] %-[Wsp produktywnosci2 ] % tzn. z 1000 kg tytoniu uzyskuje się [Ilosc krajanki min] -[Ilosc krajanki max ] kg (krajanki tytoniowej)
        </li>
        <li>
          Na 1 mln. szt. gotowych papierosów zużywa się w trakcie procesu produkcyjnego [Ilosc krajanki min] -[Ilosc krajanki max ] kg krajanki tytoniowej.
        </li>
        <li>
          Dodatkowo przy produkcji 1 mln papierosów powstaje [Procent odpadow ] % odpadów (zużycie tytoniu należy powiększyć o ten współczynnik)
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
        <xsl:value-of select="format-number(cas:TobaccoUnitPrice, $FoarmatOfFloat)"/>
      </td>
      <td>
        <xsl:value-of select="format-number(cas:TobaccoQuantity, $FoarmatOfFloat)"/>
      </td>
      <td>
        <xsl:choose>
          <xsl:when test="cas:ItemClearingType='PartialWindingUp'">Częśćiowa</xsl:when>
          <xsl:when test="cas:ItemClearingType='TotalWindingUp'">Całkowita</xsl:when>
        </xsl:choose>
      </td>
      <td>
        <xsl:value-of select="format-number(cas:TobaccoValue, $FoarmatOfFloat)"/>
      </td>
      <td>
        <xsl:value-of select="cas:Currency"/>
      </td>
      <td>
        <xsl:value-of select="format-number(cas:Duty, $FoarmatOfFloat)"/>
      </td>
      <td>
        <xsl:value-of select="format-number(cas:VAT, $FoarmatOfFloat)"/>
      </td>
    </tr>
  </xsl:template>
  <xsl:template match="cas:RegularIngredient">
    <tr>
      <td>
        <p>
          <xsl:number/> RegularIngredient
        </p>
      </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>