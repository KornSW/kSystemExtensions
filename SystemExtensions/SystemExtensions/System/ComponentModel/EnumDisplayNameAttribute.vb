Imports System
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Linq
Imports System.Diagnostics

Namespace System.ComponentModel

  <AttributeUsage(AttributeTargets.Field, Allowmultiple:=True, Inherited:=False)>
  Public Class EnumDisplayNameAttribute
    Inherits Attribute

#Region " Constructors "
    Public Sub New(displayName As String)
      Me.New(displayName, "EN")
    End Sub

    Public Sub New(displayName As String, twoLetterISOLanguageName As String)
      MyBase.New()
      _TwoLetterISOLanguageName = twoLetterISOLanguageName
      _DisplayName = displayName
    End Sub

#End Region

#Region " Properties "

    <DebuggerBrowsable(DebuggerBrowsableState.Never)>
    Private _TwoLetterISOLanguageName As String

    Public ReadOnly Property TwoLetterISOLanguageName As String
      Get
        Return _TwoLetterISOLanguageName
      End Get
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)>
    Private _DisplayName As String

    Public ReadOnly Property DisplayName As String
      Get
        Return _DisplayName
      End Get
    End Property

#End Region

#Region " ToString "

    Public Overrides Function ToString() As String
      Return Me.DisplayName
    End Function

#End Region

  End Class

End Namespace
