Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq

Namespace System.Collections.Generic

  Public Class SortableList(Of T)
    Implements IList(Of T)

    Private _BaseList As IList(Of T)

    Public Sub New(baseList As IList(Of T))
      _BaseList = baseList
    End Sub

    Public Property SortingDelegate As Func(Of IEnumerable(Of T), IEnumerable(Of T))

    Public Sub Add(item As T) Implements ICollection(Of T).Add
      DirectCast(_BaseList, ICollection(Of T)).Add(item)
    End Sub

    Public Sub Clear() Implements ICollection(Of T).Clear
      DirectCast(_BaseList, ICollection(Of T)).Clear()
    End Sub

    Public Function Contains(item As T) As Boolean Implements ICollection(Of T).Contains
      Return DirectCast(_BaseList, ICollection(Of T)).Contains(item)
    End Function

    Public Sub CopyTo(array() As T, arrayIndex As Integer) Implements ICollection(Of T).CopyTo
      DirectCast(_BaseList, ICollection(Of T)).CopyTo(array, arrayIndex)
    End Sub

    Public ReadOnly Property Count As Integer Implements ICollection(Of T).Count
      Get
        Return DirectCast(_BaseList, ICollection(Of T)).Count
      End Get
    End Property

    Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of T).IsReadOnly
      Get
        Return DirectCast(_BaseList, ICollection(Of T)).IsReadOnly
      End Get
    End Property

    Public Function Remove(item As T) As Boolean Implements ICollection(Of T).Remove
      Return DirectCast(_BaseList, ICollection(Of T)).Remove(item)
    End Function

    Public Function GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
      If (Me.SortingDelegate Is Nothing) Then
        Return _BaseList.GetEnumerator()
      Else
        Return Me.SortingDelegate.Invoke(_BaseList).GetEnumerator()
      End If
    End Function

    Public Function GetEnumeratorUntyped() As IEnumerator Implements IEnumerable.GetEnumerator
      Return Me.GetEnumerator
    End Function

    Public Function IndexOf(item As T) As Integer Implements IList(Of T).IndexOf
      Dim index As Integer = 0
      For Each itm In Me
        If (Equals(itm, item)) Then
          Return index
        End If
        index += 1
      Next
      Return -1
    End Function

    Public Sub Insert(index As Integer, item As T) Implements IList(Of T).Insert
      If (SortingDelegate Is Nothing) Then
        _BaseList.Insert(index, item)
      Else
        _BaseList.Add(item)
      End If
    End Sub

    Default Public Property Item(index As Integer) As T Implements IList(Of T).Item
      Get
        Return Me.Skip(index).First()
      End Get
      Set(value As T)
        Dim addressedItem = Me.Item(index)
        Dim indexInBase = _BaseList.IndexOf(addressedItem)
        _BaseList.Item(indexInBase) = value
      End Set
    End Property

    Public Sub RemoveAt(index As Integer) Implements IList(Of T).RemoveAt
      Me.Remove(Me.Item(index))
    End Sub

  End Class

End Namespace
