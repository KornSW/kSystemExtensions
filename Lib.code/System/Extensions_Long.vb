Imports System.Runtime.CompilerServices
Imports System.ComponentModel
Imports System.Linq
Imports System.Text
Imports System
Imports System.Reflection
Imports System.Diagnostics

Namespace System

  Public Module ExtensionsForLong

#Region " Conversion "

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToInteger(value As Long) As Integer
      Return Convert.ToInt32(value)
    End Function

    Private _ScaleNames As String() = {" B", " KB", " MB", " GB", " TB", " PB", " EB"}
    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function ToByteSizeString(value As Long) As String
      Dim size As Double = value

      For i As Integer = 0 To _ScaleNames.Length - 1

        If (size < 1024 OrElse i = _ScaleNames.Length - 1) Then
          Return Math.Round(size, 1).ToString() & _ScaleNames(i)
        End If

        size = size / 1024
      Next

      Throw New NotImplementedException()
    End Function

#End Region

  End Module

End Namespace
