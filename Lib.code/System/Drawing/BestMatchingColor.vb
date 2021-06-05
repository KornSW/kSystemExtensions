Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.Drawing
Imports System.Collections.Generic
Imports System
Imports System.Linq

Namespace System.Drawing

  Public Module ColorFindExtension

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function BestMatchingFrom(anyColor As Color, ParamArray possibleColors() As Color) As Color

      Dim bestColor As Color = anyColor
      Dim bestDifference As Double = Double.MaxValue
      Dim difference As Double = 0

      For Each possibleColor As Color In possibleColors

        difference = anyColor.Compare(possibleColor)

        If (bestDifference > difference) Then
          bestDifference = difference
          bestColor = possibleColor
        End If

      Next

      Return bestColor
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function Compare(anyColor As Color, colorToCompare As Color, Optional includeR As Boolean = True, Optional includeG As Boolean = True, Optional includeB As Boolean = True, Optional includeA As Boolean = False) As Double
      Dim dimensions As New List(Of Tuple(Of Integer, Integer))

      If (includeR) Then
        dimensions.Add(New Tuple(Of Integer, Integer)(CInt(anyColor.R), CInt(colorToCompare.R)))
      End If
      If (includeG) Then
        dimensions.Add(New Tuple(Of Integer, Integer)(CInt(anyColor.G), CInt(colorToCompare.G)))
      End If
      If (includeB) Then
        dimensions.Add(New Tuple(Of Integer, Integer)(CInt(anyColor.B), CInt(colorToCompare.B)))
      End If
      If (includeA) Then
        dimensions.Add(New Tuple(Of Integer, Integer)(CInt(anyColor.A), CInt(colorToCompare.A)))
      End If

      Return MathEx.DimensionDifference(dimensions.ToArray())
    End Function

  End Module

End Namespace
