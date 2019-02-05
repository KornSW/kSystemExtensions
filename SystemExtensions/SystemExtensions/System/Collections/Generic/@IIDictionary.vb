Imports System.Runtime.CompilerServices
Imports System.ComponentModel
Imports System.Linq
Imports System.Text
Imports System
Imports System.Reflection
Imports System.Diagnostics
Imports System.Collections.Specialized

Namespace System.Collections.Generic

  Public Module ExtensionsForIDictionary

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ContainsKey(Of T)(dictionary As IDictionary(Of String, T), key As String, ignoreCase As Boolean) As Boolean
      If (ignoreCase) Then
        For Each k In dictionary.Keys
          If (String.Compare(k, key, True) = 0) Then
            Return True
          End If
        Next
        Return False
      Else
        Return dictionary.ContainsKey(key)
      End If
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetItem(Of TItemType)(dictionary As IDictionary(Of String, TItemType), key As String, ignoreCase As Boolean) As TItemType
      If (ignoreCase) Then
        For Each k In dictionary.Keys
          If (String.Compare(k, key, True) = 0) Then
            Return dictionary.Item(k)
          End If
        Next
      End If
      Return dictionary.Item(key)
    End Function

    '''' <summary>
    '''' Gets a value form the dictionary and casts it to the requested type (available for all dictionaries with TValue=Object)
    '''' </summary>
    '<Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    'Public Function GetItem(Of TKey, TItemType)(dictionary As IDictionary(Of TKey, Object), key As TKey) As TItemType
    '  Return DirectCast(dictionary.Item(key), TItemType)
    'End Function


    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function TryGetItem(Of TItemType)(dictionary As IDictionary(Of String, TItemType), key As String, ignoreCase As Boolean, ByRef foundItem As TItemType) As Boolean
      If (dictionary.ContainsKey(key, ignoreCase)) Then
        foundItem = dictionary.GetItem(key, ignoreCase)
        Return True
      End If
      Return False
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function TryGetItem(Of TKey, TItemType)(dictionary As IDictionary(Of TKey, TItemType), key As TKey, ByRef foundItem As TItemType) As Boolean
      If (dictionary.ContainsKey(key)) Then
        foundItem = dictionary(key)
        Return True
      End If
      Return False
    End Function



    ''' <summary>
    ''' Gets a value form the dictionary and casts it to the requested type (available for all dictionaries with TValue=Object)
    ''' </summary>
    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetItem(Of TKey, TItemType)(dictionary As IDictionary(Of TKey, Object), key As TKey) As TItemType
      Return DirectCast(dictionary.Item(key), TItemType)
    End Function

    ''' <summary>
    ''' Gets a value form the dictionary and casts it to the requested type (available for all dictionaries with TValue=Object)
    ''' </summary>
    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetItem(Of TItemType)(dictionary As IDictionary(Of String, Object)) As TItemType
      Return DirectCast(dictionary.Item(GetType(TItemType).GetNameWithNamespace()), TItemType)
    End Function

    ''' <summary>
    ''' Gets a value form the dictionary and casts it to the requested type (available for all dictionaries with TValue=Object)
    ''' </summary>
    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetOrCreateItem(Of TItemType As New)(dictionary As IDictionary(Of String, Object)) As TItemType
      Dim key = GetType(TItemType).GetNameWithNamespace()
      If (Not dictionary.ContainsKey(key)) Then
        dictionary.Add(key, New TItemType)
      End If
      Return DirectCast(dictionary.Item(key), TItemType)
    End Function

    ''' <summary>
    ''' Gets a value form the dictionary and casts it to the requested type (available for all dictionaries with TValue=Object)
    ''' </summary>
    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetItem(Of TItemType)(dictionary As IDictionary(Of Type, Object)) As TItemType
      Return DirectCast(dictionary.Item(GetType(TItemType)), TItemType)
    End Function

    ''' <summary>
    ''' Gets a value form the dictionary and casts it to the requested type (available for all dictionaries with TValue=Object)
    ''' </summary>
    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetOrCreateItem(Of TItemType As New)(dictionary As IDictionary(Of Type, Object)) As TItemType
      Dim key = GetType(TItemType)
      If (Not dictionary.ContainsKey(key)) Then
        dictionary.Add(key, New TItemType)
      End If
      Return DirectCast(dictionary.Item(key), TItemType)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToNameValueCollection(extendee As Dictionary(Of String, String)) As NameValueCollection
      Dim dict As New NameValueCollection
      For Each k In extendee.Keys
        dict.Add(k, extendee.Item(k))
      Next
      Return dict
    End Function

  End Module

End Namespace
