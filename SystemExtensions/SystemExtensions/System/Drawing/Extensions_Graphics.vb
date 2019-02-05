Imports System.Runtime.CompilerServices
Imports System.ComponentModel
Imports System.Linq
Imports System
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging

Namespace System.Drawing

  Public Module ExtensionsForGraphics

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub DrawRoundedRectangle(g As Graphics, pen As Pen, radius As Integer, rect As Rectangle)
      g.DrawRoundedRectangle(pen, radius, rect.X, rect.Y, rect.Width, rect.Height)
    End Sub

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub DrawRoundedRectangle(g As Graphics, pen As Pen, radius As Integer, x As Integer, y As Integer, width As Integer, height As Integer)
      g.DrawRoundedRectangle(pen, radius, CSng(x), CSng(y), CSng(width), CSng(height))
    End Sub

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub DrawRoundedRectangle(g As Graphics, pen As Pen, radius As Integer, x As Single, y As Single, width As Single, height As Single)

      Dim dm As Integer = radius * 2
      Dim topEdge = y + pen.Width / 2
      Dim bottomEdge = y + height - pen.Width / 2
      Dim leftEdge = x + pen.Width / 2
      Dim rightEdge = x + width - pen.Width / 2

      g.DrawLine(pen,
        leftEdge + radius - 1, topEdge,
        rightEdge - radius + 1, topEdge)
      g.DrawLine(pen,
        leftEdge + radius - 1, bottomEdge,
        rightEdge - radius + 1, bottomEdge)
      g.DrawLine(pen,
       leftEdge, topEdge + radius - 1,
       leftEdge, bottomEdge - radius + 1)
      g.DrawLine(pen,
        rightEdge, topEdge + radius - 1,
        rightEdge, bottomEdge - radius + 1)

      g.DrawArc(pen, rightEdge - dm, bottomEdge - dm, dm, dm, 0, 90)
      g.DrawArc(pen, leftEdge - 1, bottomEdge - dm, dm, dm, 90, 90)
      g.DrawArc(pen, leftEdge - 1, topEdge - 1, dm, dm, 180, 90)
      g.DrawArc(pen, rightEdge - dm, topEdge - 1, dm, dm, 270, 90)

    End Sub

  End Module

End Namespace
