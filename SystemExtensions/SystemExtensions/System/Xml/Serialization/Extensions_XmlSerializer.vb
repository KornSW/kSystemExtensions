Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Xml

Namespace System.Xml.Serialization

  Public Module ExtensionsForXmlSerializer

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function Serialize(serializer As XmlSerializer, input As Object) As String
      Using ms As New MemoryStream
        serializer.Serialize(ms, input)
        ms.Position = 0
        Using sr As New StreamReader(ms)
          Return sr.ReadToEnd()
        End Using
      End Using
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function Deserialize(serializer As XmlSerializer, xml As String) As Object
      Using ms As New MemoryStream
        Using sw As New StreamWriter(ms)
          sw.Write(xml)
          sw.Flush()
          ms.Position = 0
          Return serializer.Deserialize(ms)
        End Using
      End Using
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function Deserialize(Of TCastTo)(serializer As XmlSerializer, xml As String) As TCastTo
      Return DirectCast(serializer.Deserialize(xml), TCastTo)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function Deserialize(Of TCastTo)(serializer As XmlSerializer, xmlStream As Stream) As TCastTo
      Return DirectCast(serializer.Deserialize(xmlStream), TCastTo)
    End Function

  End Module

End Namespace
