Imports System.Runtime.CompilerServices
Imports System.ComponentModel
Imports System.Linq
Imports System.Text
Imports System
Imports System.Reflection
Imports System.Diagnostics

Namespace System.Collections.Generic

  Public Module ExtensionsForList

    '<Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    'Public Function ContainsItemWithKey(Of TItem As IHasKey)(source As List(Of TItem), key As Object) As Boolean
    '  Return (From item As TItem In source Where item.Equals(key)).Any()
    'End Function

    '<Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    'Public Function GetItemWithKey(Of TItem As IHasKey)(source As List(Of TItem), key As Object) As TItem
    '  Return (From item As TItem In source Where item.Equals(key)).SingleOrDefault()
    'End Function

    '<Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    'Public Function GetIndexOfItemWithKey(Of TItem As IHasKey)(source As List(Of TItem), key As Object) As Integer
    '  For i As Integer = 0 To source.Count - 1
    '    If (source(i).Key.Equals(key)) Then
    '      Return i
    '    End If
    '  Next
    '  Return -1
    'End Function

    '<Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    'Public Function SetItemWithKey(Of TItem As IHasKey)(source As List(Of TItem), item As TItem) As TItem
    '  Dim index As Integer = GetIndexOfItemWithKey(Of TItem)(source, item.Key)
    '  If (index < 0) Then
    '    source.Add(item)
    '  Else
    '    source(index) = item
    '  End If
    'End Function

    '<Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    'Public Function RemoveItemWithKey(Of TItem As IHasKey)(source As List(Of TItem), key As Object) As TItem
    '  Dim index As Integer = GetIndexOfItemWithKey(Of TItem)(source, key)
    '  If (index < 0) Then
    '    source.RemoveAt(index)
    '  End If
    'End Function

  End Module

End Namespace
