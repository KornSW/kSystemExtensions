Imports System.Runtime.CompilerServices
Imports System.ComponentModel
Imports System.Linq
Imports System.Text
Imports System
Imports System.Reflection
Imports System.Diagnostics

Namespace System.Diagnostics

  Public Module ExtensionsForStackTrace

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function EntryPoint(extendee As StackTrace) As StackFrame
      Dim allFrames = extendee.GetFrames()
      For i As Integer = (allFrames.Length - 1) To 0 Step -1
        Dim assName = allFrames(i).Assembly.GetName().Name.ToLower()
        If (Not assName.StartsWith("mscorlib") AndAlso Not assName.StartsWith("microsoft")) Then
          Return allFrames(i)
        End If
      Next
      Return extendee.CurrentPosition()
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function CurrentPosition(extendee As StackTrace) As StackFrame
      Return extendee.GetFrames().First()
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function Assembly(extendee As StackFrame) As Assembly
      Return extendee.GetMethod().DeclaringType.Assembly
    End Function

  End Module

End Namespace
