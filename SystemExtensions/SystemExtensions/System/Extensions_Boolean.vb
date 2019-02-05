Imports System.Runtime.CompilerServices
Imports System.ComponentModel
Imports System.Linq
Imports System.Text
Imports System
Imports System.Reflection
Imports System.Diagnostics

Namespace System

  Public Module ExtensionsForBoolean

#Region " Conversion "

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToLong(value As Boolean) As Long
      Return value.ToInteger().ToLong()
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToDouble(value As Boolean) As Double
      Return value.ToInteger().ToDouble()
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToInteger(value As Boolean) As Integer
      If (value) Then
        Return 1
      Else
        Return 0
      End If
    End Function

#End Region

#Region " Array Evaluation (AND/OR) "

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Function IsFalseAll(extendee() As Boolean) As Boolean
      For Each flag In extendee
        If (flag = True) Then
          Return False
        End If
      Next
      Return True
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Function IsTrueAll(extendee() As Boolean) As Boolean
      For Each flag In extendee
        If (flag = False) Then
          Return False
        End If
      Next
      Return True
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Function IsFalseAny(extendee() As Boolean) As Boolean
      For Each flag In extendee
        If (flag = False) Then
          Return True
        End If
      Next
      Return False
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Function IsTrueAny(extendee() As Boolean) As Boolean
      For Each flag In extendee
        If (flag = True) Then
          Return True
        End If
      Next
      Return False
    End Function

#End Region

#Region " Toggle "

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Sub Toggle(ByRef extendee As Boolean)
      extendee = Not extendee
    End Sub

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Sub Toggle(ByRef extendee As Boolean, onBecomeTrue As Action)
      extendee = Not extendee
      If (extendee) Then
        onBecomeTrue.Invoke()
      End If
    End Sub

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Sub Toggle(ByRef extendee As Boolean, onBecomeTrue As Action, onBecomeFalse As Action)
      extendee = Not extendee
      If (extendee) Then
        onBecomeTrue.Invoke()
      Else
        onBecomeFalse.Invoke()
      End If
    End Sub

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Sub Toggle(ByRef extendee As Boolean, changeHandler As Action(Of Boolean))
      extendee = Not extendee
      changeHandler.Invoke(extendee)
    End Sub

#End Region

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsTrue(target As Func(Of Boolean)) As Boolean
      Return target.Invoke()
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsFalse(target As Func(Of Boolean)) As Boolean
      Return Not target.Invoke()
    End Function

  End Module

End Namespace
