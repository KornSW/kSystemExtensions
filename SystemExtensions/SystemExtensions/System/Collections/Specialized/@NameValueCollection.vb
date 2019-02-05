Imports System.Runtime.CompilerServices
Imports System.ComponentModel
Imports System.Linq
Imports System.Text
Imports System
Imports System.Reflection
Imports System.Diagnostics
Imports System.Collections.Generic

Namespace System.Collections.Specialized

  Public Module ExtensionsForNameValueCollection

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToDictionary(extendee As NameValueCollection) As Dictionary(Of String, String)
      Dim dict As New Dictionary(Of String, String)
      For Each k In extendee.AllKeys
        dict.Add(k, extendee.Item(k))
      Next
      Return dict
    End Function

  End Module

End Namespace
