<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ms="urn:schemas-microsoft-com:xslt"
    xmlns:cas="http://cas.eu/schemas/SmartFactory/xml/DocumentsFactory/CutfillerExportForm.xsd"
>
  <xsl:output method="html" indent="yes"/>
  <xsl:decimal-format name="pl" decimal-separator=',' grouping-separator='.' />
  <xsl:variable name="FoarmatOfFloat" >###.##0,00</xsl:variable>
  <xsl:variable name="FoarmatOfdate" >dd-MM-yyyy</xsl:variable>
  <xsl:template match="/" >
    <html>
      <head>
        <title>Zbiorczy proces technologiczny produkcji krajanki na eksport</title>
      </head>
      <body>
        <table  >
          <tr>
            <td>
              <p align="right">
                Gostków Stary, <xsl:value-of select="ms:format-date(cas:CutfillerExportFormCollection/cas:DocumentDate, $FoarmatOfdate)"/>
              </p>
            </td>
          </tr>
          <tr>
            <td>
              <h1 align="left" >
                Zbiorczy proces technologiczny produkcji krajanki na eksport
              </h1>
            </td>
          </tr>
        </table>
        <xsl:apply-templates select="cas:CutfillerExportFormCollection"> </xsl:apply-templates>
      </body>
    </html>
  </xsl:template>
  <xsl:template match="cas:CutfillerExportFormCollection">
    <p>
      Dokument Zbiorczy Nr: <xsl:value-of select="cas:DocumentNo" />
    </p>
    <xsl:apply-templates select="cas:CutfillerExportForms" />
  </xsl:template>
  <xsl:template match="cas:CutfillerExportForm">
    <div>
      <h2>
        <xsl:number></xsl:number>. Proces technologiczny produkcji krajanki na eksport
      </h2>
      <table border="1">
        <tr>
          <td>Dokument:</td>
          <td colspan="2">

          </td>
        </tr>
        <tr>
          <td>Format:</td>
          <td colspan="2">

          </td>
        </tr>
        <tr>
          <td>Batch:</td>
          <td colspan="2">

          </td>
        </tr>
        <tr>
          <td>Faktura:</td>
          <td colspan="2">

          </td>
        </tr>
        <tr>
          <td>Procedura:</td>
          <td colspan="2">

          </td>
        </tr>
        <tr>
          <td>NAZWA KRAJANKI:</td>
          <td>

          </td>
          <td>

          </td>
        </tr>
        <tr>
          <td>Ilość:</td>
          <td colspan="2">
            kg.
          </td>
        </tr>
      </table>
      <h3>I.	Charakterystyka ogólna procesu technologicznego:</h3>
      <p>Przedstawiony niżej proces technologiczny obejmuje wykonanie krajanki z przeznaczeniem na eksport, z tytoni importowanych dla potrzeb firmy JTI i obejmuje dwa etapy:</p>
      <p>1.	Wykonanie krajanki tytoniowej z tytoni i żył tytoniowych importowanych objętych procedurą uszlachetniania czynnego wg zestawienia:</p>
      <table border="1">
        <tr align="center">
          <th>Lp.</th>
          <th>Nr SAD</th>
          <th>Data</th>
          <th>SKU tytoniu</th>
          <th>Batch tytoniu</th>
          <th>Cena</th>
          <th>Ilość - tytoń (kg)</th>
          <th>Likwidacja</th>
          <th>Wartość</th>
          <th>Waluta</th>
          <th>Cło (PLN)</th>
          <th>Vat (PLN)</th>
        </tr>
        <xsl:apply-templates select="cas:Ingredients/cas:RegularIngredient" />
        <tr>
          <td colspan="6">
            Ilość liści tytoniowych - Suma
          </td>
          <td>
            
          </td>
          <td>

          </td>
          <td>

          </td>
          <td>

          </td>
          <td>

          </td>
          <td>

          </td>
        </tr>
      </table>
      <p>oraz</p>
      <table border="1">
        <tr align="center">
          <th>Pył</th>
          <th>Odpad</th>
          <th>Suma (kg)</th>
        </tr>
        <tr>
          <td>
            
          </td>
          <td>

          </td>
          <td>

          </td>
        </tr>
      </table>
      <p>a także tytoni i żył nie objętych procedurą uszlachetniania czynnego wg zestawienia:</p>
      <table border="1">
        <tr align="center">
          <th>SKU tytoniu</th>
          <th>Batch tytoniu</th>
          <th>Ilość (kg)</th>
        </tr>
        <xsl:apply-templates select="cas:Ingredients/cas:RegularIngredient" />
        <tr>
          <td colspan="2">
            Suma
          </td>
          <td>
            
          </td>
        </tr>
      </table>
      <p>Suma tytoni z zawieszeń i nie z zawieszeń:[suma ilości z tabelek] kg.</p>
      <h3>Opis procesu technologicznego:</h3>
      <p>W trakcie procesu technologicznego tytoń jest poddany obróbce w linii przerobu tytoni w JTI Polska Sp. z o.o., Zakład w Gostkowie wg szczegółowej instrukcji technologicznej ustalonej z odbiorcą:</p>
      <ol>
        <li>Rozlistkowanie tytoni</li>
        <li>Kondycjonowanie</li>
        <li>Składowanie</li>
        <li>Krojenie tytoni</li>
        <li>Suszenie</li>
        <li>Odbiór krajanki w opakowania</li>
      </ol>
      <h3>II.	Określenia parametrów wagowych obrabianego materiału.</h3>
      <p>1.	Z wymienionego w pkt.1 materiału wsadowego poddanego obróbce w linii technologicznej uzyskuje się krajankę tytoniową o współczynniku produktywności [Wsp produktywnosci1] %-[Wsp produktywnosci2 ] % tzn. z 1000 kg tytoniu uzyskuje się [Ilosc krajanki min] -[Ilosc krajanki max ] kg (krajanki tytoniowej).</p>
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
