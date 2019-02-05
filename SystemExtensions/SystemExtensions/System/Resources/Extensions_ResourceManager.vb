Imports System.Runtime.CompilerServices
Imports System.ComponentModel
Imports System.Linq
Imports System.Text
Imports System
Imports System.Reflection
Imports System.Diagnostics
Imports System.Collections.Generic
Imports System.Resources

Namespace System

  Public Module ExtensionsForResourceManager

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function GetStringFailsafe(rm As ResourceManager, name As String) As String
      Return rm.GetStringFailsafe(name, name)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function GetStringFailsafe(rm As ResourceManager, name As String, [default] As String) As String
      Dim result As String = rm.GetString(name)
      If (result Is Nothing) Then
        result = [default]
      End If
      Return result
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Sub Resolve(rm As ResourceManager, ByRef name As String)
      name = rm.GetStringFailsafe(name, name)
    End Sub

  End Module

End Namespace
