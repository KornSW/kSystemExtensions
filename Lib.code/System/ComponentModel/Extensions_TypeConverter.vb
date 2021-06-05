Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Collections.Generic

Namespace System.ComponentModel

  Public Module ExtensionsForTypeConverter

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ConvertToString(converter As TypeConverter, instance As Object) As String
      If (converter IsNot Nothing AndAlso converter.CanConvertTo(GetType(System.String))) Then
        Return converter.ConvertTo(instance, GetType(System.String)).ToString()
      Else
        Return Nothing
      End If
    End Function

  End Module

End Namespace
