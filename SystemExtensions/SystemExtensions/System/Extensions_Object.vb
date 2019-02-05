Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Xaml
Imports System.IO

Namespace System

  Public Module ExtensionsForObjects

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Sub TryDispose(Of T)(obj As T, Optional ignoreExceptions As Boolean = False)

      If (obj IsNot Nothing AndAlso TypeOf obj Is IDisposable) Then
        Try
          DirectCast(obj, IDisposable).Dispose()
        Catch ex As Exception When ignoreExceptions
        End Try
      End If

    End Sub

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function IfType(Of TBase, TDerived)(obj As TBase, thenDo As Action(Of TDerived)) As Boolean
      If (TypeOf obj Is TDerived) Then
        thenDo.Invoke(DirectCast(DirectCast(obj, Object), TDerived))
        Return True
      Else
        Return False
      End If
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToString(Of TType)(instance As TType, useTypeConverter As Boolean) As String

      If (instance Is Nothing) Then
        Return String.Empty
      End If

      If (useTypeConverter) Then
        Dim converterinstance As TypeConverter = instance.GetType().GetConverter()
        If (converterinstance IsNot Nothing) Then
          Dim result As String = converterinstance.ConvertToString(instance)
          If (result IsNot Nothing) Then
            Return result
          End If
        End If
      End If

      If (instance.GetType().IsEnum) Then
        Return instance.GetType().GetEnumName(instance)
      Else
        Return instance.ToString()
      End If

    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function IsList(extendee As Object) As Boolean
      Return extendee.GetType.IsList
    End Function

#Region " XAML "

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Sub FromXaml(Of T)(ByRef extendee As T, ByVal source As Stream)
      extendee = CType(XamlServices.Load(source), T)
    End Sub

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Sub ToXaml(Of T)(extendee As T, ByVal target As Stream)
      XamlServices.Save(target, extendee)
    End Sub

#End Region

  End Module

End Namespace
