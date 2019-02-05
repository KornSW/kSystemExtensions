Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Diagnostics

Public Module ExtensionsForMethodBase

  <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
  Public Sub WriteToTrace(extendee As MethodBase, Optional currentPositionInfo As String = "")
    If (String.IsNullOrEmpty(currentPositionInfo)) Then
      Trace.WriteLine(String.Format("{0}{1}", extendee.ReflectedType.Name, extendee.Name))
    Else
      Trace.WriteLine(String.Format("{0}{1} @ {2}", extendee.ReflectedType.Name, extendee.Name, currentPositionInfo))
    End If
  End Sub

End Module
