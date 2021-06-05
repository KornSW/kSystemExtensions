Imports System
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Linq
Imports System.Reflection
Imports System.Resources

Namespace System.ComponentModel

  Public Class EnumResourceConverter
    Inherits ExtendedEnumConverterBase

    Private _ResourceManager As ResourceManager

    Public Sub New(ByVal type As System.Type)
      Me.New(type, type.Assembly.GetResourceManager())
    End Sub

    Public Sub New(ByVal type As System.Type, resourceMenager As ResourceManager)
      MyBase.New(type)
      _ResourceManager = resourceMenager
    End Sub

    Protected Overrides Function GetValueText(culture As CultureInfo, value As Object) As String
      Dim type As System.Type = value.GetType()
      Dim resourceName As String = String.Format("{0}_{1}", type.Name, value.ToString())
      Dim result As String = _ResourceManager.GetString(resourceName, culture)

      If (result Is Nothing) Then
        result = value.ToString()
      End If

      Return result
    End Function

  End Class

End Namespace
