Imports System.Runtime.CompilerServices
Imports System.ComponentModel
Imports System.Linq
Imports System.Text
Imports System
Imports System.Reflection
Imports System.Diagnostics
Imports System.Security.Cryptography

Namespace System.Security

  'Public Module ExtensionsForMD5

  '  Private _CryptoMD5 As MD5CryptoServiceProvider = Nothing

  '  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  '  Public Function MD5(source As Byte(), Optional phrase As SecureString = Nothing) As HexString
  '    ' phrase???
  '    If (_CryptoMD5 Is Nothing) Then
  '      _CryptoMD5 = New MD5CryptoServiceProvider
  '    End If
  '    Return New HexString(_CryptoMD5.ComputeHash(source))
  '  End Function

  '  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  '  Public Function MD5(source As String, Optional phrase As SecureString = Nothing) As HexString
  '    source = phrase.MD5() & source
  '    Dim bytes() As Byte
  '    bytes = Encoding.ASCII.GetBytes(source)
  '    Return New HexString(_CryptoMD5.ComputeHash(bytes))
  '  End Function

  '  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  '  Public Function MD5(source As SecureString, Optional phrase As SecureString = Nothing) As HexString

  '    'source.ge()

  '    'phrase.
  '    '' phrase???
  '    'Dim bytes() As Byte
  '    'bytes = Encoding.ASCII.GetBytes(source)
  '    'Return New HexString(_CryptoMD5.ComputeHash(bytes))
  '    Throw New NotImplementedException
  '  End Function

  '  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  '  Public Function MD5(source As IO.FileInfo, Optional only4KHash As Boolean = False) As HexString



  '    Throw New NotImplementedException

  '  End Function

  'End Module

End Namespace
