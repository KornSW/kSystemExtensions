Imports System.Runtime.CompilerServices
Imports System.ComponentModel
Imports System.Linq
Imports System
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Text

Namespace System.Drawing

  Public Module Extensions

    Public Enum VerticalAlignment As Integer
      Top
      Middle
      Bottom
    End Enum

    Public Enum HorizontalAlignment As Integer
      Left
      Middle
      Right
    End Enum

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function ToImage(text As String, font As Font, foreColor As Color) As Image
      Return ToImage(text, font, foreColor, Color.Transparent)
    End Function


    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function ToImage(text As String, font As Font, foreColor As Color, backColor As Color) As Image
      If (String.IsNullOrWhiteSpace(text)) Then
        Return New Bitmap(font.Height, 1)
      End If
      Dim img As Bitmap
      Using g As Graphics = Graphics.FromImage(New Bitmap(font.Height, font.Height))
        With g.MeasureString(text, font)
          img = New Bitmap(CInt(.Width) + 1, CInt(.Height) + 1)
        End With
      End Using
      Dim brush As New SolidBrush(foreColor)
      Using g As Graphics = Graphics.FromImage(img)
        If (Not backColor = Color.Transparent) Then
          g.FillRectangle(New SolidBrush(backColor), 0, 0, img.Width, img.Height)
        End If

        g.DrawString(text, font, brush, 0, 0)
      End Using
      Return img
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function ToBase64String(image As Image) As String
      Return Convert.ToBase64String(image.GetBytes())
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function GetMimeName(format As ImageFormat) As String
      Return "image/" & format.ToString()
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function CalulateChildOffset(sourceInstance As Rectangle, childRect As Rectangle, hAlign As HorizontalAlignment, vAlign As VerticalAlignment) As Point
      Throw New NotImplementedException
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function CalulateChildOffset(sourceInstance As Size, childRect As Rectangle, hAlign As HorizontalAlignment, vAlign As VerticalAlignment) As Point
      Throw New NotImplementedException
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function CalulateChildOffset(sourceInstance As Rectangle, childSize As Size, hAlign As HorizontalAlignment, vAlign As VerticalAlignment) As Point
      Throw New NotImplementedException
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function CalulateChildOffset(sourceInstance As Size, childSize As Size, hAlign As HorizontalAlignment, vAlign As VerticalAlignment) As Point
      Throw New NotImplementedException
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function CalulateChildOffset(sourceInstance As Rectangle, childRect As Rectangle, percentFromLeft As Double, percentFromTop As Double) As Point
      Throw New NotImplementedException
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function CalulateChildOffset(sourceInstance As Size, childRect As Rectangle, percentFromLeft As Double, percentFromTop As Double) As Point
      Throw New NotImplementedException
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function CalulateChildOffset(sourceInstance As Rectangle, childSize As Size, percentFromLeft As Double, percentFromTop As Double) As Point
      Throw New NotImplementedException
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function CalulateChildOffset(sourceInstance As Size, childSize As Size, percentFromLeft As Double, percentFromTop As Double) As Point
      Throw New NotImplementedException
    End Function








    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function CalculateStringSize(f As Font, text As String) As SizeF
      If (_G Is Nothing) Then
        _G = Graphics.FromImage(New Bitmap(10, 10))
      End If
      Return _G.MeasureString(text, f)
    End Function
    Private _G As Graphics = Nothing



#Region " for Image Types "

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToIcon(sourceImage As Image) As Icon
      If (sourceImage Is Nothing) Then
        Return Nothing
      End If
      Dim bmp As New Bitmap(sourceImage)
      Return Icon.FromHandle(bmp.GetHicon)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToPngImage(sourceIcon As Icon) As Image
      If (sourceIcon Is Nothing) Then
        Return Nothing
      End If

      Throw New NotImplementedException

    End Function



#Region " Image Modifications "

    Public Enum RotationAngle As Integer
      Angle90Left = 270
      Angle90Right = 90
      Angle180 = 180
      Angle270Left = 90
      Angle270Right = 270
    End Enum

    Public Enum FlippingMode As Integer
      Horizontal = 1
      Vertical = 2
    End Enum

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetRotatedVersion(sourceInstance As System.Drawing.Image, angle As RotationAngle) As System.Drawing.Image
      Throw New NotImplementedException
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetFilppedVersion(sourceInstance As System.Drawing.Image, mode As FlippingMode) As System.Drawing.Image
      Throw New NotImplementedException
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetDisabledVersion(sourceInstance As System.Drawing.Image) As System.Drawing.Image
      Throw New NotImplementedException
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetOverlayedVersion(sourceInstance As System.Drawing.Image, overlayImage As System.Drawing.Image) As System.Drawing.Image
      Throw New NotImplementedException
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetTransparentVersion(sourceInstance As System.Drawing.Image, transparencyColor As System.Drawing.Color) As System.Drawing.Image
      Throw New NotImplementedException
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetOverlayedVersion(sourceInstance As System.Drawing.Image, overlayImage As System.Drawing.Image, overlayPosition As System.Drawing.Rectangle) As System.Drawing.Image
      Throw New NotImplementedException
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetResizedVersion(sourceInstance As System.Drawing.Image, maxWidth As Integer, maxHeight As Integer) As System.Drawing.Image
      Throw New NotImplementedException
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetResizedVersion(sourceInstance As System.Drawing.Image, maxWidth As Integer, maxHeight As Integer, minWidth As Integer, minHeight As Integer) As System.Drawing.Image
      Throw New NotImplementedException
    End Function

#End Region

#Region " Export To File "

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub SaveJpgFile(sourceInstance As System.Drawing.Image, fileName As String, qualityPercent As Integer)
      Throw New NotImplementedException
    End Sub

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub SavePngFile(sourceInstance As System.Drawing.Image, fileName As String, qualityPercent As Integer)
      Throw New NotImplementedException
    End Sub

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub SaveGifFile(sourceInstance As System.Drawing.Image, fileName As String, qualityPercent As Integer)
      Throw New NotImplementedException
    End Sub

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub SaveBmpFile(sourceInstance As System.Drawing.Image, fileName As String)
      Throw New NotImplementedException
    End Sub

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub SaveIcoFile(sourceInstance As System.Drawing.Image, fileName As String, qualityPercent As Integer)
      Throw New NotImplementedException
    End Sub

#End Region

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToByteArray(sourceInstance As System.Drawing.Image) As Byte()
      Throw New NotImplementedException
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub LoadEmbeddedFile(ByRef sourceInstance As System.Drawing.Image, assembly As System.Reflection.Assembly, defaultNamespace As String, fileName As String)
      Throw New NotImplementedException

    End Sub

#End Region

  End Module

End Namespace
