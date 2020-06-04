Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Text

Namespace System

  Public Module ExtensionsForArray

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function SubArray(Of T)(extendee As T(), startIndex As Integer) As T()
      If (startIndex = 0) Then
        Return extendee
      Else
        Return SubArray(Of T)(extendee, startIndex, extendee.Length - startIndex)
      End If
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function SubArray(Of T)(extendee As T(), startIndex As Integer, length As Integer) As T()

      If (length < 1) Then
        Return {}
      ElseIf (startIndex = 0 AndAlso length = extendee.Length) Then
        Return extendee
      End If

      Dim newArray(length - 1) As T
      For i As Integer = 0 To length - 1
        newArray(i) = extendee(i + startIndex)
      Next

      Return newArray
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToUntypedList(extendee As Array) As IList
      Dim listType As Type = extendee.GetType().GetItemType().MakeListType()
      Dim listInstance As IList = listType.Activate(Of IList)()
      For Each item In extendee
        listInstance.Add(item)
      Next
      Return listInstance
    End Function

    ''' <summary>
    ''' Returns the new index
    ''' </summary>
    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GrowOne(Of T)(ByRef extendee As T()) As Integer
      Dim l = extendee.ToList()
      l.Add(Nothing)
      extendee = l.ToArray()
      Return (extendee.Length - 1)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub Resize(Of T)(ByRef extendee As T(), newLength As Integer, Optional factory As Func(Of T) = Nothing)
      Dim l = extendee.ToList()
      If (l.Count > newLength) Then
        extendee = l.Take(newLength).ToArray()
        Exit Sub
      End If
      If (factory = Nothing) Then
        factory = Function() Nothing
      End If

      While (l.Count < newLength)
        l.Add(factory.Invoke())
      End While
      extendee = l.ToArray()
    End Sub

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub RemoveAt(Of T)(ByRef extendee As T(), index As Integer)
      Dim l = extendee.ToList()
      l.RemoveAt(index)
      extendee = l.ToArray()
    End Sub

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function To2dObjectArray(Of T)(input As T()()) As T(,)

      Dim dimension1Size As Integer = input.Length
      Dim dimension2Size As Integer = 0

      For i1 As Integer = 0 To input.GetUpperBound(0)
        If (dimension2Size < input(i1).Length) Then
          dimension2Size = input(i1).Length
        End If
      Next

      If (dimension1Size > 0) Then
        dimension1Size -= 1
      End If

      If (dimension2Size > 0) Then
        dimension2Size -= 1
      End If

      Dim result(dimension1Size, dimension2Size) As T
      For i1 As Integer = 0 To dimension1Size
        For i2 As Integer = 0 To dimension2Size
          result(i1, i2) = input(i1)(i2)
        Next
      Next

      Return result
    End Function

  End Module

End Namespace
