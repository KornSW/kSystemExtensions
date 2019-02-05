Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Text

Namespace System.Drawing

  Public Module ExtensionsForByte

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function ToImage(bytes As Byte()) As Image
      Dim ms As New MemoryStream(bytes)
      Return Image.FromStream(ms)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function ToColor(bytes As Byte()) As Color
      Return Color.FromArgb(bytes(0), bytes(1), bytes(2))
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function ToColor(str As String) As Color
      If (str.StartsWith("#") And str.Length = 7) Then
        Return HexString.DecodeBytesFromHex(str.Substring(1)).ToColor()
      ElseIf str.Length > 4 AndAlso str.Length < 12 AndAlso str.Contains(",") Then
        Dim splt = str.Split(","c)
        Return Color.FromArgb(Byte.Parse(splt(0)), Byte.Parse(splt(1)), Byte.Parse(splt(2)))
      Else
        Return Color.FromName(str)
      End If
    End Function

  End Module

End Namespace
