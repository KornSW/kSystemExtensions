Imports System.Runtime.CompilerServices
Imports System.ComponentModel
Imports System.Linq
Imports System.Text
Imports System
Imports System.Reflection
Imports System.Diagnostics

Namespace System

  Public Module ExtensionsForChar

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function DecodeBase64(value As Char()) As Byte()
      Return Convert.FromBase64CharArray(value, 0, value.Length)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function DecodeHex(hexChars As Char()) As Byte()
      Dim alphabet As Char() = ("0123456789ABCDEF").ToCharArray()
      Dim bytes As Byte()
      Dim hiValue As Integer
      Dim loValue As Integer

      Dim hexGetter As Func(Of Char, Byte) =
        Function(letter As Char)
          If Char.IsLower(letter) Then
            letter = Char.ToUpper(letter)
          End If
          For i As Integer = 0 To 15
            If (letter = alphabet(i)) Then
              Return Convert.ToByte(i)
            End If
          Next
          Return 0
        End Function

      If ((hexChars.Length Mod 2) > 0) Then
        Throw New ArgumentException("The hexString must not have an uneven number of chars.")
      End If

      ReDim bytes((hexChars.Length \ 2) - 1)

      For i As Integer = 0 To (hexChars.Length - 2) Step 2
        hiValue = hexGetter.Invoke(hexChars(i)) * 16
        loValue = hexGetter.Invoke(hexChars(i + 1))
        bytes(i \ 2) = Convert.ToByte(hiValue + loValue)
      Next

      Return bytes
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToByteArray(value As Char(), encoding As Encoding) As Byte()
      Return encoding.GetBytes(value)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsIn(extendee As Char, ParamArray values() As Char) As Boolean
      For Each value As Char In values
        If (extendee.Equals(value)) Then
          Return True
        End If
      Next

      Return False
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsIn(extendee As Char, str As String) As Boolean
      For Each value As Char In str
        If (extendee.Equals(value)) Then
          Return True
        End If
      Next

      Return False
    End Function

  End Module

End Namespace
