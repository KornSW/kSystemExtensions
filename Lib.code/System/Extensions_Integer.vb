Imports System.Runtime.CompilerServices
Imports System.ComponentModel
Imports System.Linq
Imports System.Text
Imports System
Imports System.Reflection
Imports System.Diagnostics

Namespace System

  Public Module ExtensionsForInteger

#Region " Conversion "

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToLong(value As Integer) As Long
      Return Convert.ToInt64(value)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToChar(value As Integer) As Char
      Return Convert.ToChar(value)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToDouble(value As Integer) As Double
      Return Convert.ToDouble(value)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToBoolean(value As Integer) As Boolean
      Return (value > 0)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToString(value As Integer, minimumLength As Integer, Optional fillChar As Char = "0"c) As String
      Dim digitCount As Integer = value.DigitCount
      If (digitCount >= minimumLength) Then
        Return value.ToString()
      Else
        Return (New String(fillChar, minimumLength - digitCount))
      End If
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function ToByteSizeString(value As Integer) As String
      Return value.ToLong().ToByteSizeString()
    End Function

#End Region

#Region " Encode / Decode "

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function EncodeHex(value As Integer) As String
      '##################################
      '  TODO: IMPLEMENT THIS METHOD !!!
      Throw New NotImplementedException()
      '##################################
    End Function

#End Region

#Region " Misc "

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function DigitCount(value As Integer) As Integer
      Dim numOfDigits As Integer = 1
      Do Until (10 ^ numOfDigits) > value
        numOfDigits += 1
      Loop
      Return numOfDigits
    End Function

#End Region


    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsInsideRange(value As Integer, start As Integer, [end] As Integer) As Boolean
      Return value >= start AndAlso value <= [end]
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsOuterRange(value As Integer, start As Integer, [end] As Integer) As Boolean
      Return value < start OrElse value > [end]
    End Function

  End Module

End Namespace
