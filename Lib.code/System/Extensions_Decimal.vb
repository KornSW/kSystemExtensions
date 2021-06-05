Imports System.Runtime.CompilerServices
Imports System.ComponentModel
Imports System.Linq
Imports System.Text
Imports System
Imports System.Reflection
Imports System.Diagnostics

Namespace System

  Public Module ExtensionsForDecimal

#Region " Conversion "

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToLong(value As Decimal) As Long
      Return Convert.ToInt64(Math.Round(value, 0))
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToDouble(value As Decimal) As Double
      Return Convert.ToDouble(value)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToInteger(value As Decimal) As Integer
      Return value.Round()
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function Round(value As Decimal) As Integer
      Return Convert.ToInt32(Math.Round(value, 0))
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function Round(value As Decimal, decimals As Integer) As Decimal
      Return Math.Round(value, decimals)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToString(value As Decimal, minimumLength As Integer, Optional fillChar As Char = "0"c) As String
      Return value.ToInteger.ToString(minimumLength, fillChar)
    End Function

#End Region

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsInsideRange(value As Decimal, start As Decimal, [end] As Decimal) As Boolean
      Return value >= start AndAlso value <= [end]
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsOuterRange(value As Decimal, start As Decimal, [end] As Decimal) As Boolean
      Return value < start OrElse value > [end]
    End Function

  End Module

End Namespace
