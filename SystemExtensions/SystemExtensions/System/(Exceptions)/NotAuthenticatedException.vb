Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Runtime.CompilerServices

Namespace System

  Public Class NotAuthenticatedException
    Inherits Exception

    <EditorBrowsable(EditorBrowsableState.Never)>
    Private Const _DefaultMessage = "Authentication Required!"

    Public Sub New()
      MyBase.New(_DefaultMessage)
    End Sub

    Public Sub New(message As String)
      MyBase.New(message)
    End Sub

    Public Sub New(innerException As Exception)
      MyBase.New(_DefaultMessage, innerException)
    End Sub

    Public Sub New(message As String, innerException As Exception)
      MyBase.New(message, innerException)
    End Sub

  End Class

End Namespace
