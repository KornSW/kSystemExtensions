Option Explicit On
Option Strict On

Imports System
Imports System.Drawing
Imports System.Collections.Generic
Imports System.Runtime.CompilerServices
Imports System.ComponentModel

Namespace System.Drawing

  Public Module CirclePaintingExtension

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function PositionArround(centerPoint As System.Drawing.Point, radius As Decimal, degrees As Decimal) As System.Drawing.Point
      Dim x As Decimal
      Dim y As Decimal
      If (radius < 0) Then
        degrees = degrees + 180
        radius = radius * -1
      End If
      degrees = degrees.NormalizeAngle()
      Dim radian As Decimal = CDec((degrees - 90) * (Math.PI / 180))
      x = CDec(Math.Cos(radian) * radius)
      y = CDec(Math.Sin(radian) * radius)
      Return New System.Drawing.Point(centerPoint.X + CInt(x), centerPoint.Y + CInt(y))
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function Rotate(oldBitmap As System.Drawing.Image, degrees As Decimal) As Bitmap
      Dim newBitmap = New Bitmap(oldBitmap.Width, oldBitmap.Height)
      Using g = Graphics.FromImage(newBitmap)
        'g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        g.TranslateTransform(CSng(oldBitmap.Width / 2), CSng(oldBitmap.Height / 2))
        g.RotateTransform(degrees)
        g.TranslateTransform(-CSng(oldBitmap.Width / 2), -CSng(oldBitmap.Height / 2))
        g.DrawImage(oldBitmap, New Point(0, 0))
        Return newBitmap
      End Using
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function DeepClone(img As Image) As Image
      Dim newImg = New Bitmap(img.Width, img.Height)
      Using g = Graphics.FromImage(newImg)
        g.DrawImage(img, New Point(0, 0))
        Return newImg
      End Using
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function AngleRelatedTo(currentPoint As System.Drawing.Point, centerPoint As System.Drawing.Point) As Decimal
      Return currentPoint.AngleRelatedTo(centerPoint.X, centerPoint.Y)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function AngleRelatedTo(currentPoint As System.Drawing.Point, centerX As Integer, centerY As Integer) As Decimal
      Dim radius As Integer = 0
      Return AngleRelatedTo(currentPoint, centerX, centerY, radius)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function AngleRelatedTo(currentPoint As Point, centerX As Integer, centerY As Integer, ByRef radius As Integer) As Decimal
      Dim angle As Decimal
      Dim xDiff As Decimal = centerX - currentPoint.X
      Dim yDiff As Decimal = centerY - currentPoint.Y
      angle = CDec(Math.Atan2(yDiff, xDiff)) * CDec(180 / Math.PI)
      angle = (angle - 90).NormalizeAngle
      ' Debug.WriteLine(angle)
      Return angle
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Private Function NormalizeAngle(angle As Decimal) As Decimal
      While (angle > 360)
        angle -= 360
      End While
      While (angle < 0)
        angle += 360
      End While
      Return angle
    End Function


    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetRange(sourceImage As Image, range As Rectangle) As Image
      Dim targetImage = New Bitmap(range.Width, range.Height)
      Using g As Graphics = Graphics.FromImage(targetImage)
        g.DrawImage(sourceImage, New Rectangle(0, 0, targetImage.Width, targetImage.Height), range, GraphicsUnit.Pixel)
        Return targetImage
      End Using
    End Function


    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsInCircle(pointToEvaluate As Point, centerPoint As Point, deadRadiusPx As Integer) As Boolean
      Return False

      '    (Math.Pow(x1-x2,2)+Math.Pow(y1-y2,2)) < d*d;

      'Noticed that the preferred one does not call Pow at all for speed resons, and the second one, probably slower, as well does not call Math.Sqrt,always for performance reasons. Maybe such optimization are premature in your case, but they are useful if that code has to be executed a lot of times.

      'Of course you are talking in meters and I supposed point coordinates are expressed in meters too.



    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function Fold(oldBitmap As System.Drawing.Image, radius As Decimal) As Bitmap
      Dim newBitmap = New Bitmap(oldBitmap.Width, oldBitmap.Height)
      Using g = Graphics.FromImage(newBitmap)


        Dim toplineRadius As Decimal = radius + CDec((oldBitmap.Height / 2))
        Dim corridorWidthDegrees As Decimal = CDec(360 * (oldBitmap.Width / (toplineRadius * 2 * Math.PI)))
        Dim drgreesPerPixel As Decimal = corridorWidthDegrees / oldBitmap.Width
        Dim offsetToPictureMid As Integer
        Dim virtualCenterPoint As New Point(CInt(oldBitmap.Width / 2), CInt(toplineRadius))
        Dim columnSpecificRotationAngle As Decimal
        For pixelColumn As Integer = 0 To (oldBitmap.Width - 1)
          offsetToPictureMid = CInt((pixelColumn - (oldBitmap.Width / 2)))
          columnSpecificRotationAngle = offsetToPictureMid * drgreesPerPixel
          Using columnImage As Image = oldBitmap.GetRange(New Rectangle(pixelColumn, 0, 1, oldBitmap.Height))
            'Using rotatedImage As Image = columnImage.Rotate(columnSpecificRotationAngle)
            Dim rotatedImage As Image = columnImage

            'g.TranslateTransform(CSng(oldBitmap.Width / 2), CSng(oldBitmap.Height / 2))
            'g.TranslateTransform(pixelColumn , -CSng(oldBitmap.Height / 2))
            'g.TranslateTransform(pixelColumn, 0)
            'g.TranslateTransform(pixelColumn, toplineRadius)
            g.TranslateTransform(pixelColumn, CSng(oldBitmap.Height))

            g.RotateTransform(columnSpecificRotationAngle) '.NormalizeAngle())

            'g.TranslateTransform(-CSng(oldBitmap.Width / 2), -CSng(oldBitmap.Height / 2))
            'g.TranslateTransform(-pixelColumn, -CSng(oldBitmap.Height / 2))
            'g.TranslateTransform(-pixelColumn, -0)
            'g.TranslateTransform(pixelColumn, -toplineRadius)
            g.TranslateTransform(pixelColumn, -CSng(oldBitmap.Height))

            g.DrawImage(rotatedImage, virtualCenterPoint.PositionArround(CInt(toplineRadius), columnSpecificRotationAngle))
            g.ResetTransform()

            'End Using
          End Using
        Next

        Return newBitmap
      End Using
    End Function

  End Module

End Namespace
