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

  Public Module ExtensionsForByte

#Region " Conversion "

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToLong(value As Byte) As Long
      Return Convert.ToInt64(value)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToDouble(value As Byte) As Double
      Return Convert.ToDouble(value)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToInteger(value As Byte) As Integer
      Return Convert.ToInt32(value)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToString(value As Byte, minimumLength As Integer, Optional fillChar As Char = "0"c) As String
      Return value.ToInteger.ToString(minimumLength, fillChar)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToChar(value As Byte) As Char
      Return Encoding.ASCII.GetChars(New Byte() {value})(0)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToString(value As Byte(), Optional enc As Encoding = Nothing) As String
      If (enc Is Nothing) Then
        enc = Encoding.Default
      End If
      Return enc.GetString(value)
    End Function

#End Region

#Region " Encode / Decode "

    '<Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    'Public Function EncodeHex(value As Byte) As String
    '  '##################################
    '  '  TODO: IMPLEMENT THIS METHOD !!!
    '  Throw New NotImplementedException()
    '  '##################################
    'End Function

    '<Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    'Public Function EncodeHex(value As Byte()) As String
    '  '##################################
    '  '  TODO: IMPLEMENT THIS METHOD !!!
    '  Throw New NotImplementedException()
    '  '##################################
    'End Function

    '<Extension(), EditorBrowsable(EditorBrowsableState.Never), Obsolete("Use ToBase64String", True)>
    'Public Function EncodeB64(value As Byte) As String
    '  Return Convert.ToBase64String({value})
    'End Function



    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToCharArray(value As Byte(), encoding As Encoding) As Char()
      Return encoding.GetChars(value)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function EncodeBase64(value As Byte()) As Char()
      Return Convert.ToBase64String(value).ToCharArray()
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function EncodeHex(value As Byte()) As Char()
      Dim sb As New StringBuilder
      For Each b In value
        sb.Append(b.ToString("X2"))
      Next
      Return sb.ToString().ToCharArray()
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToBase64String(bytes As Byte()) As String
      Return Convert.ToBase64String(bytes)
    End Function




    '<Extension(), EditorBrowsable(EditorBrowsableState.Never), Obsolete("Use ToBase64String", True)>
    'Public Function EncodeB64(value As Byte()) As String
    '  Return Convert.ToBase64String(value)
    'End Function

    '<Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    'Public Function EncodeMD5(value As Byte) As String
    '  '##################################
    '  '  TODO: IMPLEMENT THIS METHOD !!!
    '  Throw New NotImplementedException()
    '  '##################################
    'End Function

    '<Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    'Public Function EncodeMD5(value As Byte()) As String
    '  '##################################
    '  '  TODO: IMPLEMENT THIS METHOD !!!
    '  Throw New NotImplementedException()
    '  '##################################
    'End Function

#End Region

  End Module

End Namespace
