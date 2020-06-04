Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Web

Namespace System.Threading

  Public Module ExtensionsForCancellationToken

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub WaitForCancellationRequested(extendee As CancellationToken, pollingIntervalMs As Integer)
      Do Until extendee.IsCancellationRequested
        Thread.Sleep(pollingIntervalMs)
      Loop
    End Sub

  End Module

End Namespace
