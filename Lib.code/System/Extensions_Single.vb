Imports System.Runtime.CompilerServices
Imports System.ComponentModel
Imports System.Linq
Imports System.Text
Imports System
Imports System.Reflection
Imports System.Diagnostics

Namespace System

  Public Module ExtensionsForSingle

#Region " Conversion "

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToLong(value As Single) As Long
      Return Convert.ToInt64(Math.Round(value, 0))
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToDecimal(value As Single) As Decimal
      Return Convert.ToDecimal(value)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToInteger(value As Single) As Integer
      Return value.Round()
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToDouble(value As Single) As Double
      Return Convert.ToDouble(value)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function Round(value As Single) As Integer
      Return Convert.ToInt32(Math.Round(value, 0))
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function Round(value As Single, decimals As Integer) As Single
      Return Math.Round(value, decimals).ToSingle()
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function Round(value As Single, decimals As Integer, mode As MidpointRounding) As Single
      Return Math.Round(value, decimals, mode).ToSingle()
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToString(value As Single, minimumLength As Integer, Optional fillChar As Char = "0"c) As String
      Return value.ToInteger.ToString(minimumLength, fillChar)
    End Function

#End Region

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsInsideRange(value As Single, start As Single, [end] As Single) As Boolean
      Return value >= start AndAlso value <= [end]
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsOuterRange(value As Single, start As Single, [end] As Single) As Boolean
      Return value < start OrElse value > [end]
    End Function

  End Module

End Namespace
