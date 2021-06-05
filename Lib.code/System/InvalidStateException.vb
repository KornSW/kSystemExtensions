Imports System.Diagnostics

Namespace System

  Public Class InvalidStateException
    Inherits Exception
    Public Sub New(message As String)
      MyBase.New(message)
    End Sub

  End Class

End Namespace
