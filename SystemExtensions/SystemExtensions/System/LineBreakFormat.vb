Imports System
Imports System.ComponentModel

Namespace System.Text

  <TypeConverter(GetType(EnumDisplayNameAttributeConverter))>
  Public Enum LineBreakFormat
    <EnumDisplayName("Windows (CR+LF)")> Windows_CrLf = 1310
    <EnumDisplayName("Unix (LF)")> Unix_Lf = 10
    <EnumDisplayName("Mac (CR)")> Mac_Cr = 13
  End Enum

End Namespace
