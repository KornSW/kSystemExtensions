Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Text

Namespace System.Net

  Public Module ExtensionsForIpAddress

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ResolveDnsName(extendee As IPAddress) As String
      Return Dns.GetHostEntry(extendee).HostName
    End Function

  End Module

End Namespace
