Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Text

Namespace System.Drawing

  Public Module ExtensionsForImage


    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function ToIcon(bytes As Byte()) As Icon
      Dim ms As New MemoryStream(bytes)
      Return New Icon(ms)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function GetBytes(image As Image) As Byte()
      Dim ms As New MemoryStream()
      image.Save(ms, ImageFormat.Png)
      Return ms.ToArray()
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function ToBase64String(image As Image) As String
      Return Convert.ToBase64String(image.GetBytes())
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function GetBytes(image As Image, format As ImageFormat) As Byte()
      If (image Is Nothing) Then
        Return Nothing
      End If
      Dim ms As New MemoryStream()
      image.Save(ms, format)
      Return ms.ToArray()
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function GetBase64ContentString(image As Image) As String
      Return image.GetBase64ContentString(ImageFormat.Png)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function GetBase64ContentString(image As Image, format As ImageFormat) As String
      Dim sb As New StringBuilder
      sb.Append("data:")
      sb.Append(format.GetMimeName())
      sb.Append(";base64,")
      sb.Append(image.GetBytes().ToBase64String())
      Return sb.ToString()
    End Function

  End Module

End Namespace
