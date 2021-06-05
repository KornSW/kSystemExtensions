Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Web

Namespace System

  Public Module ExtensionsForNameValueCollection

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ParseQuery(extendee As Uri) As Dictionary(Of String, String)
      Dim urlParams = HttpUtility.ParseQueryString(extendee.Query).ToDictionary()
      Return urlParams
    End Function

  End Module

End Namespace
