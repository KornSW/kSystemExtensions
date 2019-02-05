Imports System
Imports System.Linq
Imports System.ComponentModel
Imports System.Collections.ObjectModel

Namespace System.Collections.Generic

  'Public Class ObservableSet(Of T)
  '  Inherits ObservableCollection(Of T)
  '  Implements IQueryableSet(Of T)

  '  Private Sub AddInternal(item As T) Implements IQueryableSet(Of T).Add
  '    MyBase.Add(item)
  '  End Sub

  '  Public Overloads Sub Add(ParamArray items() As T) Implements IQueryableSet(Of T).Add
  '    For Each item As T In items
  '      MyBase.Add(item)
  '    Next
  '  End Sub

  '  Public Overloads Sub Add(items As IEnumerable(Of T)) Implements IQueryableSet(Of T).Add
  '    For Each item As T In items
  '      MyBase.Add(item)
  '    Next
  '  End Sub

  '  Public Overloads Sub Remove(items As IEnumerable(Of T)) Implements IQueryableSet(Of T).Remove
  '    For Each item As T In items
  '      MyBase.Remove(item)
  '    Next
  '  End Sub

  '  Private Sub ClearInternal() Implements IQueryableSet(Of T).Clear
  '    MyBase.Clear()
  '  End Sub

  '  Private Function CountInternal() As Integer Implements IQueryableSet(Of T).Count
  '    Return MyBase.Count()
  '  End Function

  '  Private Sub RemoveInternal(item As T) Implements IQueryableSet(Of T).Remove
  '    MyBase.Remove(item)
  '  End Sub

  '  Public Overloads Sub Remove(ParamArray items() As T) Implements IQueryableSet(Of T).Remove
  '    For Each item As T In items
  '      MyBase.Remove(item)
  '    Next
  '  End Sub

  '  Protected ReadOnly Property ElementType As Type Implements IQueryable.ElementType
  '    Get
  '      Return Me.AsQueryable().ElementType
  '    End Get
  '  End Property

  '  Protected ReadOnly Property Expression As Expressions.Expression Implements IQueryable.Expression
  '    Get
  '      Return Me.AsQueryable().Expression
  '    End Get
  '  End Property

  '  Protected ReadOnly Property Provider As IQueryProvider Implements IQueryable.Provider
  '    Get
  '      Return Me.AsQueryable().Provider
  '    End Get
  '  End Property

  'End Class

End Namespace
