Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq

Namespace System.Collections.Generic

  Public Class ListWithKey(Of TKeyType, TItem)
    Implements IList(Of TItem)

    Private _BaseList As New List(Of TItem)
    Private _KeySelector As Func(Of TItem, TKeyType)

    Public Sub New(keySelector As Func(Of TItem, TKeyType))
      _KeySelector = keySelector
    End Sub

    Public Function KeyOf(item As TItem) As TKeyType
      Return _KeySelector.Invoke(item)
    End Function

    Public Function IndexOfKey(key As TKeyType) As Integer
      Dim foundItem = (From item In _BaseList Where key.Equals(Me.KeyOf(item))).SingleOrDefault()
      If (foundItem Is Nothing) Then
        Return -1
      Else
        Return _BaseList.IndexOf(foundItem)
      End If
    End Function

    Public Function ContainsKey(key As TKeyType) As Boolean
      Return Me.IndexOfKey(key) > -1
    End Function

    Public Function Remove(item As TItem) As Boolean Implements ICollection(Of TItem).Remove
      Return _BaseList.Remove(item)
    End Function

    Public Function Remove(key As TKeyType) As Boolean
      If (Me.ContainsKey(key)) Then
        _BaseList.RemoveAt(Me.IndexOfKey(key))
        Return True
      Else
        Return False
        'Throw New ArgumentException(String.Format("Item with key '{0}' does not exist!", key))
      End If
    End Function

    Public Function IndexOf(item As TItem) As Integer Implements IList(Of TItem).IndexOf
      Return _BaseList.IndexOf(item)
    End Function

    Public Sub Insert(index As Integer, item As TItem) Implements IList(Of TItem).Insert
      If (Me.ContainsItem(item)) Then
        Throw New ArgumentException(String.Format("Item with key '{0}' already exists!", Me.KeyOf(item)))
      Else
        _BaseList.Insert(index, item)
      End If
    End Sub

    ''' <summary>
    ''' Returns Nothing, if not exist
    ''' </summary>
    Public Property ItemWithKey(key As TKeyType) As TItem
      Get
        If (Me.ContainsKey(key)) Then
          Return _BaseList(Me.IndexOfKey(key))
        Else
          Return Nothing
        End If
      End Get
      Set(value As TItem)
        If (Me.ContainsItem(value)) Then
          Dim index = Me.IndexOfKey(Me.KeyOf(value))
          _BaseList(index) = value
        Else
          _BaseList.Add(value)
        End If
      End Set
    End Property

    Public Sub AddOrReplace(item As TItem)
      Me.ItemWithKey(Me.KeyOf(item)) = item
    End Sub

    Public Sub Add(item As TItem) Implements ICollection(Of TItem).Add
      If (Me.ContainsItem(item)) Then
        Throw New ArgumentException(String.Format("Item with key '{0}' already exists!", Me.KeyOf(item)))
      End If
      _BaseList.Add(item)
    End Sub

    Public Sub Clear() Implements ICollection(Of TItem).Clear
      _BaseList.Clear()
    End Sub

    Public Function ContainsItem(item As TItem) As Boolean Implements ICollection(Of TItem).Contains
      Return Me.ContainsKey(Me.KeyOf(item))
    End Function

    Public Sub CopyTo(array() As TItem, arrayIndex As Integer) Implements ICollection(Of TItem).CopyTo
      _BaseList.CopyTo(array, arrayIndex)
    End Sub

    Public ReadOnly Property Count As Integer Implements ICollection(Of TItem).Count
      Get
        Return _BaseList.Count
      End Get
    End Property

    Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of TItem).IsReadOnly
      Get
        Return False
      End Get
    End Property

    Public Function GetEnumerator() As IEnumerator(Of TItem) Implements IEnumerable(Of TItem).GetEnumerator
      Return _BaseList.GetEnumerator
    End Function

    Private Function GetEnumeratorUntyped() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
      Return _BaseList.GetEnumerator
    End Function

    Default Public Property ItemAtIndex(index As Integer) As TItem Implements IList(Of TItem).Item
      Get
        Return _BaseList(index)
      End Get
      Set(value As TItem)
        _BaseList(index) = value
      End Set
    End Property

    Public Sub RemoveAt(index As Integer) Implements IList(Of TItem).RemoveAt
      _BaseList.RemoveAt(index)
    End Sub

  End Class

End Namespace
