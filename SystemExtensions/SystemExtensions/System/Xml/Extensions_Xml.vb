Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Text

Namespace System.Xml

  Public Module ExtensionsForXml

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ChildElements(node As XmlNode) As IEnumerable(Of XmlElement)
      Return node.ChildNodes.OfType(Of XmlNode).Where(Function(n) n.NodeType = XmlNodeType.Element).OfType(Of XmlElement)()
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function WithTagName(
      elements As IEnumerable(Of XmlElement),
      tagName As String,
      Optional caseSensitve As Boolean = False) As IEnumerable(Of XmlElement)

      If (caseSensitve) Then
        Return From e In elements Where e.Name = tagName
      Else
        tagName = tagName.ToLower()
        Return From e In elements Where e.Name.ToLower() = tagName
      End If
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ChildElements(node As XmlNode, name As String) As IEnumerable(Of XmlElement)
      Return node.ChildNodes.OfType(Of XmlNode).Where(
        Function(n) n.NodeType = XmlNodeType.Element).OfType(Of XmlElement).Where(Function(e) e.Name.ToLower() = name.ToLower())
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function AppendChildElement(node As XmlNode, newElementName As String) As XmlElement
      Dim newElement As XmlElement
      If (TypeOf (node) Is XmlDocument) Then
        newElement = DirectCast(node, XmlDocument).CreateElement(newElementName)
      Else
        newElement = node.OwnerDocument.CreateElement(newElementName)
      End If

      node.AppendChild(newElement)
      Return newElement
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function AppendChildElement(element As XmlElement, newElementName As String) As XmlElement
      Dim newElement As XmlElement = element.OwnerDocument.CreateElement(newElementName)
      element.AppendChild(newElement)
      Return newElement
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetCData(e As XmlElement) As String
      If (e.FirstChild Is Nothing) Then
        Return Nothing
      End If
      If (TypeOf e.FirstChild Is XmlCDataSection) Then
        Return DirectCast(e.FirstChild, XmlCDataSection).Data
      Else
        Return Nothing
      End If
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub SetAttributeValue(node As XmlNode, attributeName As String, newValue As String)
      Dim attrib = node.Attributes.ItemOf(attributeName)
      If (attrib Is Nothing) Then
        attrib = node.Attributes.Append(node.OwnerDocument.CreateAttribute(attributeName))
      End If
      attrib.Value = newValue
    End Sub

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetAttributeValue(node As XmlNode, attributeName As String, Optional defaultValue As String = Nothing) As String
      Dim attrib = node.Attributes.ItemOf(attributeName)
      If (attrib Is Nothing) Then
        Return defaultValue
      End If
      Return attrib.Value
    End Function


    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ReadXmlDocument(str As Stream) As XmlDocument
      Dim doc As New XmlDocument
      Dim settings As New XmlReaderSettings

      settings.IgnoreComments = True
      settings.IgnoreProcessingInstructions = True
      settings.IgnoreWhitespace = True

      Using reader = XmlReader.Create(str, settings)
        Dim lastNode As XmlNode
        While Not reader.EOF
          lastNode = doc.ReadNode(reader)
          If (Not lastNode.NodeType = XmlNodeType.XmlDeclaration) Then
            doc.AppendChild(lastNode)
          End If
        End While
        Return doc
      End Using

    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ReadXmlElement(str As Stream) As XmlElement
      Return str.ReadXmlDocument().RootElement()
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function RootElement(doc As XmlDocument) As XmlElement
      If (doc Is Nothing) Then
        Return Nothing
      End If
      Return doc.ChildElements().FirstOrDefault()
    End Function

  End Module

End Namespace
