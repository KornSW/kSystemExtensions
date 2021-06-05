Imports System.Runtime.CompilerServices
Imports System.Collections.Generic
Imports System.Collections
Imports System.ComponentModel
Imports System
Imports System.Linq

Namespace System

  <Microsoft.VisualBasic.HideModuleName()>
  Public Module GlobalMethods

    Public Sub Iif(expression As Boolean, truePart As action, falsePart As action)
      If (expression) Then
        truePart.Invoke()
      Else
        falsePart.Invoke()
      End If
    End Sub

    Public Function Iif(Of T As Class)(expression As Boolean, truePart As T, falsePart As T) As T
      If (expression) Then
        Return truePart
      Else
        Return falsePart
      End If
    End Function

    Public Function CombineArrays(Of T)(array1 As T(), array2 As T()) As T()
      Dim combined As New List(Of T)

      For Each item As T In array1
        combined.Add(item)
      Next
      For Each item As T In array2
        combined.Add(item)
      Next

      Return combined.ToArray()
    End Function

    Public Function CombineArrays(Of T)(array1 As T(), array2 As T(), array3 As T()) As T()
      Dim combined As New List(Of T)

      For Each item As T In array1
        combined.Add(item)
      Next
      For Each item As T In array2
        combined.Add(item)
      Next

      Return combined.ToArray()
    End Function

  End Module

End Namespace
