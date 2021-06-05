Imports System.Runtime.CompilerServices
Imports System.ComponentModel
Imports System.Linq
Imports System.Text
Imports System
Imports System.Reflection
Imports System.Diagnostics

Namespace System

  Public Module ExtensionsForDouble

#Region " Conversion "

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToLong(value As Double) As Long
      Return Convert.ToInt64(Math.Round(value, 0))
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToDecimal(value As Decimal) As Decimal
      Return Convert.ToDecimal(value)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToInteger(value As Double) As Integer
      Return value.Round()
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToSingle(value As Double) As Single
      Return Convert.ToSingle(value)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function Round(value As Double) As Integer
      Return Convert.ToInt32(Math.Round(value, 0))
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function Round(value As Double, decimals As Integer) As Double
      Return Math.Round(value, decimals)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function Round(value As Double, decimals As Integer, mode As MidpointRounding) As Double
      Return Math.Round(value, decimals, mode)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToString(value As Double, minimumLength As Integer, Optional fillChar As Char = "0"c) As String
      Return value.ToInteger.ToString(minimumLength, fillChar)
    End Function

#End Region

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsInsideRange(value As Double, start As Double, [end] As Double) As Boolean
      Return value >= start AndAlso value <= [end]
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsOuterRange(value As Double, start As Double, [end] As Double) As Boolean
      Return value < start OrElse value > [end]
    End Function

  End Module

End Namespace
