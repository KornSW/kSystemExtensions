Imports System
Imports System.Linq
Imports System.ComponentModel

Namespace System.Collections.Generic

  Public Interface IQueryableSet(Of T)
    Inherits IQueryable(Of T)

    Sub Add(item As T)
    Sub Add(ParamArray items() As T)
    Sub Add(items As IEnumerable(Of T))

    Sub Remove(item As T)
    Sub Remove(ParamArray items() As T)
    Sub Remove(items As IEnumerable(Of T))

    <EditorBrowsable(EditorBrowsableState.Advanced)>
    Sub Clear()

    Function Count() As Integer

  End Interface

End Namespace
