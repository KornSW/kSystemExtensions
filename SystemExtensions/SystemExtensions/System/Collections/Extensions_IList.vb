Imports System.Runtime.CompilerServices
Imports System.ComponentModel
Imports System.Linq
Imports System.Text
Imports System
Imports System.Reflection
Imports System.Diagnostics

Namespace System.Collections

  Public Module ExtensionsForIList

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function ToUntypedArray(list As IList) As Array
      Dim a = Array.CreateInstance(list.GetType().GetItemType(), list.Count)
      For i As Integer = 0 To a.Length - 1
        a.SetValue(list.Item(i), i)
        'a(i) = list.Item(i)
      Next
      Return a
    End Function

  End Module

End Namespace
