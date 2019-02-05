Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Text

Namespace System.Net.Sockets

  Public Module ExtensionsForTcpClient

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetRemoteEndpoint(extendee As TcpClient) As EndPoint
      Return extendee.Client.RemoteEndPoint
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetRemoteIpAddress(extendee As TcpClient) As IPAddress
      Dim ep As EndPoint = extendee.GetRemoteEndpoint()
      If (TypeOf (ep) Is IPEndPoint) Then
        Return DirectCast(ep, IPEndPoint).Address
      Else
        Return IPAddress.None
      End If
    End Function

  End Module

End Namespace
