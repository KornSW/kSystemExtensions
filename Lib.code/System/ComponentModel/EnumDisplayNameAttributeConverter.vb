Imports System
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Linq

Namespace System.ComponentModel

  Public Class EnumDisplayNameAttributeConverter
    Inherits ExtendedEnumConverterBase

    Public Sub New(ByVal type As System.Type)
      MyBase.New(type)
    End Sub

    Protected Overrides Function GetValueText(culture As CultureInfo, value As Object) As String

      Dim enumType As System.Type = value.GetType()
      Dim valueText As String = Nothing
      Dim fallbackValue As String = Nothing
      Dim valueName As String = [Enum].GetName(enumType, value)
      Dim memInfo = enumType.GetMember(valueName)
      Dim attributes = memInfo(0).GetCustomAttributes(GetType(EnumDisplayNameAttribute), False)

      For Each attrib As EnumDisplayNameAttribute In attributes.OfType(Of EnumDisplayNameAttribute)()

        If (fallbackValue Is Nothing) Then
          fallbackValue = attrib.DisplayName
        End If

        If (attrib.TwoLetterISOLanguageName.ToLower() = "en") Then
          fallbackValue = attrib.DisplayName
        End If

        If (attrib.TwoLetterISOLanguageName.ToLower() = culture.TwoLetterISOLanguageName.ToLower()) Then
          valueText = attrib.DisplayName
          Exit For
        End If

      Next

      If (fallbackValue Is Nothing) Then
        fallbackValue = valueName.Replace("_", " ")
      End If

      If (valueText Is Nothing) Then
        valueText = fallbackValue
      End If

      Return valueText
    End Function

  End Class

End Namespace
