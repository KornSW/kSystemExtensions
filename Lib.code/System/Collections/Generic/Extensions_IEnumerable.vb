Imports System.Runtime.CompilerServices
Imports System.ComponentModel
Imports System.Linq
Imports System.Text
Imports System
Imports System.Reflection
Imports System.Diagnostics

Namespace System.Collections.Generic

  Public Module ExtensionsForEnumerable

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function None(Of T)(source As IEnumerable(Of T)) As Boolean
      Return Not source.AsQueryable().Any()
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Sub CopyTo(Of T)(source As IEnumerable(Of T), target As IList(Of T), Optional skipExisting As Boolean = False)
      For Each item In source
        If (skipExisting = False OrElse Not target.Contains(item)) Then
          target.Add(item)
        End If
      Next
    End Sub

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function To2dObjectArray(Of T)(input As IEnumerable(Of T())) As T(,)
      Return ExtensionsForArray.To2dObjectArray(Of T)(input.ToArray())
    End Function

  End Module

End Namespace
