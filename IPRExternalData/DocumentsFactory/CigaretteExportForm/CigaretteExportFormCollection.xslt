<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <!--<xsl:output method="html" indent="yes"/>

  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()"/>
    </xsl:copy>
  </xsl:template>-->

  <xsl:template match="/">
    <html>
      <head>
        <title>Proces technologiczny produkcji papierosów na eksport</title>
      </head>
      <body>
          <h1>Zbiorczy proces technologiczny produkcji papierosów na eksport</h1>
          <p>
            Dokument Nr: <xsl:value-of select="DocumentNo" />
          </p>
          <p>
            <xsl:value-of select="ProductFormat" />
          </p>
          <p>Text</p>
      </body>
    </html>
  </xsl:template>

</xsl:stylesheet>