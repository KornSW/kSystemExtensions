Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Text

Namespace System

  Public Module ExtensionsGeneric

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function CastTo(Of TSource, T)(anyObj As TSource) As T
      Return DirectCast(DirectCast(anyObj, Object), T)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function TransformTo(Of TSource, TargetType)(value As IEnumerable(Of TSource), conversionMethod As Func(Of TSource, TargetType)) As IEnumerable(Of TargetType)
      Return New EnumerableProxy(Of TSource, TargetType)(value, conversionMethod)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function ToString(value As IEnumerable(Of Object)) As IEnumerable(Of String)
      Return New EnumerableProxy(Of Object, String)(value, Function(s) s.ToString())
    End Function

#Region " Cloning "

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function CloneBinary(Of T)(anyObj As T) As T
      If (anyObj Is Nothing) Then
        Return Nothing
      End If
      Dim formatter As IFormatter = New BinaryFormatter()
      Dim stream As Stream = New MemoryStream()
      Using stream
        formatter.Serialize(stream, anyObj)
        stream.Seek(0, SeekOrigin.Begin)
        Return DirectCast(formatter.Deserialize(stream), T)
      End Using
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function CloneValueMembers(Of T As {Class, New})(sourceObject As T) As T
      Dim targetObject As T = Activator.CreateInstance(Of T)()
      sourceObject.CopyValueMembersTo(targetObject)
      Return targetObject
    End Function

    Public Function CloneValueMembers(sourceObject As Object) As Object
      Dim targetObject As Object = Activator.CreateInstance(sourceObject.GetType())
      CopyValueMembers(sourceObject, targetObject)
      Return targetObject
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Sub CopyValueMembersTo(Of T As Class)(sourceObject As T, targetObject As T)
      CopyValueMembers(sourceObject, targetObject)
    End Sub

    Public Sub CopyValueMembers(sourceObject As Object, targetObject As Object)

      Dim sourceObjectProperties() As PropertyInfo = sourceObject.GetType().GetProperties()
      Dim targetObjectProperties() As PropertyInfo = targetObject.GetType().GetProperties()
      Dim matchingTargetObjectProperty As PropertyInfo
      Dim objectValue As Object

      For Each sourceObjectProperty As PropertyInfo In sourceObjectProperties

        If (sourceObjectProperty.CanRead AndAlso
            CopyAlowed(sourceObjectProperty.PropertyType) AndAlso
            sourceObjectProperty.GetIndexParameters().Count = 0) Then

          matchingTargetObjectProperty = Nothing
          For Each targetObjectProperty As PropertyInfo In sourceObjectProperties
            If (targetObjectProperty.Name = sourceObjectProperty.Name) Then
              matchingTargetObjectProperty = targetObjectProperty
              Exit For
            End If
          Next

          If (matchingTargetObjectProperty IsNot Nothing AndAlso
              matchingTargetObjectProperty.CanWrite AndAlso
              matchingTargetObjectProperty.GetIndexParameters().Count = 0 AndAlso
              matchingTargetObjectProperty.PropertyType = sourceObjectProperty.PropertyType) Then

            objectValue = sourceObjectProperty.GetValue(sourceObject, Nothing)
            CloneIfNessecary(objectValue)
            matchingTargetObjectProperty.SetValue(targetObject, objectValue, Nothing)
          End If

        End If

      Next

    End Sub

    Private Function CopyAlowed(type As Type) As Boolean

      If (type.IsValueType) Then
        Return True
      ElseIf (type.IsCollection) Then
        Return False
      End If

      Select Case type

        'non-valuetypes which are allowed to be copied
        Case GetType(String)
        Case GetType(Version)

        Case Else : Return False
      End Select

      Return True
    End Function

    Private Sub CloneIfNessecary(ByRef objectInstance As Object)
      If (objectInstance IsNot Nothing AndAlso Not objectInstance.GetType().IsValueType) Then
        Select Case objectInstance.GetType()

          'non-valuetypes which are allowed to be copied and requireing a manual clone operation
          Case GetType(Version) : objectInstance = DirectCast(objectInstance, Version).Clone()

        End Select
      End If
    End Sub

#End Region

#Region " Conversion "

    ''' <summary>
    ''' accepts a format pattern which will be resolved via reflection and can be used like this:
    ''' car.ToString("Car: {obj}{br}({AnyPropertyName}/{AnyFunctionName()})")
    ''' </summary>
    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToString(Of T)(anyObj As T, format As String) As String

      Dim foundPlaceHolders As New List(Of String)
      Dim contentString As New StringBuilder
      Dim placeHolderString As New StringBuilder
      Dim terminated As Boolean = False
      Dim inPlaceHolder As Boolean = False
      Dim targetType As Type

      For Each c As Char In format

        If (terminated) Then
          terminated = False
        Else

          Select Case c

            Case "/"c
              terminated = True
              Continue For

            Case "{"c
              inPlaceHolder = True
              Continue For

            Case "}"c
              If (inPlaceHolder) Then
                If (placeHolderString.Length > 0) Then
                  foundPlaceHolders.Add(placeHolderString.ToString())
                  placeHolderString.Clear()
                  contentString.Append("{" & (foundPlaceHolders.Count - 1).ToString() & "}")
                End If
                inPlaceHolder = False
              End If
              Continue For

          End Select

        End If

        If (inPlaceHolder) Then
          placeHolderString.Append(c)
        Else
          contentString.Append(c)
        End If

      Next

      targetType = anyObj.GetType()

      If (placeHolderString.Length > 0) Then
        foundPlaceHolders.Add(placeHolderString.ToString())
        placeHolderString.Clear()
        contentString.Append("{" & (foundPlaceHolders.Count - 1).ToString() & "}")
      End If

      For i As Integer = 0 To (foundPlaceHolders.Count - 1)
        If (foundPlaceHolders(i).ToLower() = "br") Then
          foundPlaceHolders(i) = Environment.NewLine
        ElseIf (foundPlaceHolders(i).ToLower() = "obj") Then
          foundPlaceHolders(i) = anyObj.ToString()
        ElseIf (foundPlaceHolders(i).EndsWith("()")) Then
          foundPlaceHolders(i) = targetType.GetMethod(foundPlaceHolders(i)).Invoke(anyObj, Nothing).ToString()
        Else
          foundPlaceHolders(i) = targetType.GetProperty(foundPlaceHolders(i)).GetValue(anyObj).ToString()
        End If
      Next

      Return String.Format(contentString.ToString(), foundPlaceHolders.ToArray())
    End Function

#End Region

  End Module

End Namespace
