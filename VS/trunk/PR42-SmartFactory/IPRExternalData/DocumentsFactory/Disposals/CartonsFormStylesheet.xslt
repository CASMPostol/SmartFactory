<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ms="urn:schemas-microsoft-com:xslt"
    xmlns:cas="http://CAS.SmartFactory.xml.DocumentsFactory.Disposals/DocumentContent.xsd"
>
  <xsl:output method="html" indent="yes"/>
  <xsl:decimal-format name="pl" decimal-separator=',' grouping-separator='.' />
  <xsl:variable name="FoarmatOfFloat" >###.##0,00</xsl:variable>
  <xsl:variable name="FoarmatOfdate" >dd-MM-yyyy</xsl:variable>
  <xsl:template match="/" >
    <html>
      <head>
        <title>
          Kartony dopuszczenie do obrotu
        </title>
        <style type="text/css">
          td, p { font-size:10px; }
          th { font-size:12px; }
          h2 { font-size:14px; text-align:center; }
        </style>
      </head>
      <body>
        <table border="0" width="100%">
          <tr>
            <td align="left">
              Dokument: <xsl:value-of select="cas:DocumentContent/cas:DocumentNo"/>
            </td>
            <td align="right">
              Gostków Stary, <xsl:value-of select="ms:format-date(cas:DocumentContent/cas:DocumentDate, $FoarmatOfdate)"/>
            </td>
          </tr>
        </table>
        <xsl:apply-templates select="cas:DocumentContent" />
      </body>
    </html>
  </xsl:template>
 <xsl:template match="cas:DocumentContent">
   <h2>Kartony</h2>
   <h2>
     dopuszczenie do obrotu za okres od <xsl:value-of select="ms:format-date(cas:StartDate, $FoarmatOfdate)"/> do <xsl:value-of select="ms:format-date(cas:EndDate, $FoarmatOfdate)"/>
   </h2>
   <p>
     Procedura: <xsl:value-of select="cas:CustomProcedureCode" />
   </p>
   <table border="1" width="100%">
     <tr>
       <th>NR SAD</th>
       <th>Data</th>
       <th>Ilość w kg</th>
     </tr>
     <xsl:apply-templates select="cas:AccountDescription" />
     <tr>
       <td colspan="2">Suma końcowa</td>
       <td align="right">
         <xsl:value-of select="format-number(cas:Total, $FoarmatOfFloat, 'pl')" />
       </td>
     </tr>
   </table>
   <table width="100%" border="0">
     <tr>
       <td align="right">
         .............................................
       </td>
     </tr>
     <tr>
       <td align="right">
         Imię i Nazwisko
       </td>
     </tr>
   </table>
 </xsl:template>
 <xsl:template match="cas:AccountDescription">
   <xsl:apply-templates select="cas:MaterialsOnOneAccount" />
 </xsl:template>
 <xsl:template match="cas:MaterialsOnOneAccount">
   <xsl:apply-templates select="cas:MaterialRecords" />
   <tr>
     <td colspan="2">
       Suma częściowa
     </td>
     <td align="right">
       <xsl:value-of select="format-number(cas:Total, $FoarmatOfFloat, 'pl')"/>
     </td>
   </tr>
 </xsl:template>
 <xsl:template match="cas:MaterialRecords">
   <xsl:apply-templates select="cas:MaterialRecord" />
 </xsl:template>
 <xsl:template match="cas:MaterialRecord">
   <tr>
     <td>
       <xsl:value-of select="cas:CustomDocumentNo"/>
     </td>
     <td>
       <xsl:value-of select="ms:format-date(cas:Date, $FoarmatOfdate)"/>
     </td>
     <td>
       <xsl:value-of select="format-number(cas:Qantity, $FoarmatOfFloat, 'pl')"/>
     </td>
   </tr>
 </xsl:template>
</xsl:stylesheet>
