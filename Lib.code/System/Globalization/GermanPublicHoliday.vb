Imports System
Imports System.ComponentModel
Imports System.Globalization
Imports System.Linq

Namespace System.Globalization

  <TypeConverter(GetType(EnumDisplayNameAttributeConverter))>
  Public Enum GermanPublicHoliday As Integer
    <EnumDisplayName("Neujahr")> Neujahr
    <EnumDisplayName("Heilige Drei Könige")> HeiligeDreiKönige
    <EnumDisplayName("Karfreitag")> Karfreitag
    <EnumDisplayName("Ostersonntag")> Ostersonntag
    <EnumDisplayName("Rosenmontag")> Rosenmontag
    <EnumDisplayName("Ostermontag")> Ostermontag
    <EnumDisplayName("Tag der Arbeit")> TagDerArbeit
    <EnumDisplayName("ChristiHimmelfahrt")> ChristiHimmelfahrt
    <EnumDisplayName("Pfingstsonntag")> Pfingstsonntag
    <EnumDisplayName("Pfingstmontag")> Pfingstmontag
    <EnumDisplayName("Fronleichnam")> Fronleichnam
    <EnumDisplayName("Mariä Himmelfahrt")> MariäHimmelfahrt
    <EnumDisplayName("Tag der deutschen Einheit")> TagDerDeutschenEinheit
    <EnumDisplayName("Allerheiligen")> Allerheiligen
    <EnumDisplayName("Buß und Bettag")> BussUndBettag
    <EnumDisplayName("Reformationstag")> Reformationstag
    <EnumDisplayName("1. Weihnachtsfeiertag")> ErsterWeihnachtsfeiertag
    <EnumDisplayName("2. Weihnachtsfeiertag")> ZweiterWeihnachtsfeiertag
    <EnumDisplayName("Valentinstag")> Valentinstag
    <EnumDisplayName("Halloween")> Halloween
    <EnumDisplayName("Vatertag")> Vatertag
    <EnumDisplayName("Muttertag")> Muttertag
    <EnumDisplayName("Martinstag")> Martinstag
    <EnumDisplayName("1. Advent")> ErsterAdvent
    <EnumDisplayName("2. Advent")> ZweiterAdvent
    <EnumDisplayName("3. Advent")> DritterAdvent
    <EnumDisplayName("4. Advent")> VierterAdvent
    <EnumDisplayName("Heilig Abend")> HeiligAbend
    <EnumDisplayName("Silvester")> Silvester
  End Enum

End Namespace
