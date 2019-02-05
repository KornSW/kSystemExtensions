Imports System.Runtime.CompilerServices
Imports System.ComponentModel
Imports System.Linq
Imports System
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging

Namespace System.Drawing

  Public Module ExtensionsForFont

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function WithSize(baseFont As Font, size As Single, Optional unit As GraphicsUnit = GraphicsUnit.Pixel) As Font
      Return New Font(baseFont.FontFamily, size, baseFont.Style, baseFont.Unit)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function WithStyle(baseFont As Font, style As FontStyle) As Font
      Return New Font(baseFont.FontFamily, baseFont.Size, style, baseFont.Unit)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function WithStyle(baseFont As Font, bold As Boolean, underline As Boolean, Optional strikeout As Boolean = False) As Font
      Dim expression As FontStyle = FontStyle.Regular
      If (bold) Then
        expression = expression Or FontStyle.Bold
      End If
      If (underline) Then
        expression = expression Or FontStyle.Underline
      End If
      If (strikeout) Then
        expression = expression Or FontStyle.Strikeout
      End If
      Return baseFont.WithStyle(expression)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function WithBold(baseFont As Font) As Font
      Return New Font(baseFont.FontFamily, baseFont.Size, baseFont.Style Or FontStyle.Bold, baseFont.Unit)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function WithItalic(baseFont As Font) As Font
      Return New Font(baseFont.FontFamily, baseFont.Size, baseFont.Style Or FontStyle.Italic, baseFont.Unit)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function WithUnderline(baseFont As Font) As Font
      Return New Font(baseFont.FontFamily, baseFont.Size, baseFont.Style Or FontStyle.Underline, baseFont.Unit)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function WithStrikeout(baseFont As Font) As Font
      Return New Font(baseFont.FontFamily, baseFont.Size, baseFont.Style Or FontStyle.Strikeout, baseFont.Unit)
    End Function

  End Module

End Namespace
