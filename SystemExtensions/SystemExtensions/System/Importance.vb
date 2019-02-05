
Imports System
Imports System.ComponentModel

Namespace System

  '<TypeConverter(GetType(EnumDisplayNameAttributeConverter))>
  Public Enum Importance As Integer

    '<EnumDisplayName("Low")>
    '<EnumDisplayName("Niedrig", "DE")>
    Low = 0

    '<EnumDisplayName("Normal")>
    '<EnumDisplayName("Normal", "DE")>
    Normal = 1

    '<EnumDisplayName("High")>
    '<EnumDisplayName("Hoch", "DE")>
    High = 2

  End Enum

End Namespace
