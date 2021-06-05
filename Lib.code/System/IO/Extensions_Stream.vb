Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Security.Cryptography
Imports System.Text
Imports System.Security

Namespace System.IO

  Public Module ExtensionsForStream

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ReadAllBytes(source As Stream) As Byte()
      If (source.CanSeek) Then
        source.Seek(0, SeekOrigin.Begin)
      End If
      Using sr As New BinaryReader(source)
        Return sr.ReadBytes(CInt(source.Length))
      End Using
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ReadAllText(source As Stream) As String
      If (source.CanSeek) Then
        source.Seek(0, SeekOrigin.Begin)
      End If
      Using sr As New StreamReader(source)
        Return sr.ReadToEnd()
      End Using
    End Function

  End Module

End Namespace
