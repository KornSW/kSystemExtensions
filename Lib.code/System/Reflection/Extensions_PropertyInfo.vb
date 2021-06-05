Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Diagnostics

Namespace System.Reflection

  Public Module ExtensionsForPropertyInfo

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function IsList(extendee As PropertyInfo) As Boolean
      Return extendee.PropertyType.IsList
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function IsClass(extendee As PropertyInfo) As Boolean
      If (extendee.PropertyType Is GetType(String)) Then
        Return False
      End If
      Return (extendee.PropertyType.IsClass) AndAlso (Not extendee.PropertyType.FullName.StartsWith("System"))
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function DisplayName(extendee As PropertyInfo) As String
      If (extendee.IsDefined(GetType(DisplayNameAttribute), False)) Then
        Dim attribute As DisplayNameAttribute =
          CType(extendee.GetCustomAttributes(GetType(DisplayNameAttribute), False)(0), DisplayNameAttribute)

        Return attribute.DisplayName
      End If

      Return extendee.Name
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function GetNullableType(extendee As PropertyInfo) As PropertyInfo
      If (extendee.IsNullableType) Then
        Return extendee.PropertyType.GetProperty("Value")
      End If

      Return extendee
    End Function

    ''' <summary>
    ''' Returns wether the given type is a collection or not.
    ''' </summary>
    ''' <param name="extendee">The existing propertyinfo to be extended.</param>
    <Extension()>
    Public Function IsCollection(extendee As PropertyInfo) As Boolean
      Return ((extendee IsNot Nothing) AndAlso (extendee.PropertyType.IsCollection))
    End Function

    ''' <summary>
    ''' Returns wether the given type is a nullable type or not.
    ''' </summary>
    ''' <param name="extendee">The existing type to be extended.</param>
    ''' <returns>True, if the given type is a nullable type, otherwise false.</returns>
    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function IsNullableType(extendee As PropertyInfo) As Boolean
      If ((extendee.PropertyType.IsGenericType) AndAlso (extendee.PropertyType.GetGenericTypeDefinition Is GetType(Nullable(Of )))) Then
        Return True
      End If

      Return False
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetDisplayName(extendee As PropertyInfo) As String
      Dim displayNameAttr = extendee.GetCustomAttributes().OfType(Of DisplayNameAttribute)().FirstOrDefault()
      If (displayNameAttr Is Nothing) Then
        Return extendee.Name
      Else
        Return displayNameAttr.DisplayName
      End If
    End Function

  End Module

End Namespace
