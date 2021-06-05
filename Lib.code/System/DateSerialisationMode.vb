Imports System
Imports System.ComponentModel

Namespace System

  <TypeConverter(GetType(EnumDisplayNameAttributeConverter))>
  Public Enum DateSerializingMode As Integer

    <EnumDisplayName("date only")>
    <EnumDisplayName("nur Datum", "DE")>
    DateOnly = 1

    <EnumDisplayName("time only")>
    <EnumDisplayName("nur Uhrzeit", "DE")>
    TimeOnly = 2

    <EnumDisplayName("date and time")>
    <EnumDisplayName("Datum und Uhrzeit", "DE")>
    DateAndTime = 3

  End Enum

End Namespace
