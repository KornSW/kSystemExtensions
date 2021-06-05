Imports System

Namespace System.Collections.Generic

  Public Class GenericKeyDictionary(Of TValue)
    Inherits Dictionary(Of Type, TValue)

    Public Overridable Overloads Sub Add(Of T)(value As TValue)
      MyBase.Add(GetType(T), value)
    End Sub

    Public Overridable Overloads Function ContainsKey(Of T)() As Boolean
      Return MyBase.ContainsKey(GetType(T))
    End Function

    Public Overridable Function GetItem(Of T)() As TValue
      Return MyBase.Item(GetType(T))
    End Function

    Public Overridable Sub SetItem(Of T)(value As TValue)
      MyBase.Item(GetType(T)) = value
    End Sub

    Public Overridable Overloads Function Remove(Of T)() As Boolean
      Return MyBase.Remove(GetType(T))
    End Function

  End Class

  Public Class GenericKeyDictionary
    Inherits Dictionary(Of Type, Object)

    Public Overridable Overloads Sub Add(Of T)(value As T)
      MyBase.Add(GetType(T), value)
    End Sub

    Public Overridable Overloads Function ContainsKey(Of T)() As Boolean
      Return MyBase.ContainsKey(GetType(T))
    End Function

    Public Overridable Function GetItem(Of T)() As T
      Return DirectCast(MyBase.Item(GetType(T)), T)
    End Function

    Public Overridable Sub SetItem(Of T)(value As T)
      MyBase.Item(GetType(T)) = value
    End Sub

    Public Overridable Overloads Function Remove(Of T)() As Boolean
      Return MyBase.Remove(GetType(T))
    End Function

  End Class

End Namespace
