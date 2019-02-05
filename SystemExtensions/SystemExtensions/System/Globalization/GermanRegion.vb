Imports System
Imports System.ComponentModel
Imports System.Globalization
Imports System.Linq

Namespace System.Globalization

  <TypeConverter(GetType(EnumDisplayNameAttributeConverter))>
  Public Enum GermanRegion
    <EnumDisplayName("Baden-Württemberg")> BadenWürttemberg
    <EnumDisplayName("Bayern")> Bayern
    <EnumDisplayName("Berlin")> Berlin
    <EnumDisplayName("Brandenburg")> Brandenburg
    <EnumDisplayName("Bremen")> Bremen
    <EnumDisplayName("Hamburg")> Hamburg
    <EnumDisplayName("Hessen")> Hessen
    <EnumDisplayName("Mecklenburg-Vorpommern")> MecklenburgVorpommern
    <EnumDisplayName("Niedersachsen")> Niedersachsen
    <EnumDisplayName("Nordrhein-Westfalen")> NordrheinWestfalen
    <EnumDisplayName("Rheinland-Pfalz")> RheinlandPfalz
    <EnumDisplayName("Saarland")> Saarland
    <EnumDisplayName("Sachsen")> Sachsen
    <EnumDisplayName("Sachsen-Anhalt")> SachsenAnhalt
    <EnumDisplayName("Schleswig-Holstein")> SchleswigHolstein
    <EnumDisplayName("Thüringen")> Thüringen
  End Enum

End Namespace
