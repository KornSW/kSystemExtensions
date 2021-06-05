Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.Drawing
Imports System.Collections.Generic
Imports System
Imports System.Linq

Namespace System

  Partial Public Class MathEx

    Private Sub New()
    End Sub

    ''' <summary>
    ''' Calculates the distance between two coordinates withing an n-dimentional space (example: use 3 dimensions R,G and B to calualate the analogy between two colors).
    ''' </summary>
    Public Shared Function DimensionDifference(ParamArray dimensions() As Tuple(Of Integer, Integer)) As Double

      Dim fullDifference As Double = 0
      Dim currentDimensionDifference As Double = 0

      If (dimensions.Count > 0) Then
        currentDimensionDifference = dimensions(0).Item1 - dimensions(0).Item2

        If (currentDimensionDifference < 0) Then
          currentDimensionDifference = currentDimensionDifference * -1
        End If

        fullDifference = currentDimensionDifference
        For dimension As Integer = 1 To (dimensions.Count - 1)
          currentDimensionDifference = dimensions(dimension).Item1 - dimensions(dimension).Item2

          If (currentDimensionDifference < 0) Then
            currentDimensionDifference = currentDimensionDifference * -1
          End If

          fullDifference = Math.Sqrt((fullDifference ^ 2) + (currentDimensionDifference ^ 2))
        Next

      End If

      Return fullDifference
    End Function

  End Class

End Namespace
