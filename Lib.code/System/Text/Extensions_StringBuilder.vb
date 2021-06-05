Imports System.Runtime.CompilerServices
Imports System.ComponentModel
Imports System.Linq
Imports System.Text
Imports System
Imports System.Reflection
Imports System.Diagnostics

Namespace System.Text

  Public Module ExtensionsForStringBuilder

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub AppendLine(sb As StringBuilder, stringWithFormatPlaceholders As String, ParamArray args() As Object)
      sb.AppendLine(String.Format(stringWithFormatPlaceholders, args))
    End Sub

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub Append(sb As StringBuilder, stringWithFormatPlaceholders As String, ParamArray args() As Object)
      sb.Append(String.Format(stringWithFormatPlaceholders, args))
    End Sub

  End Module

End Namespace
