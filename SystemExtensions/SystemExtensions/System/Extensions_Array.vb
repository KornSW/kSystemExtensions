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

  End Module

End Namespace
