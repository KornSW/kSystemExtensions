Imports System.IO
Imports System.Xml

Public Interface ISerializable

  Sub DeserializeFrom(source As XmlReader)

  Sub SerializeTo(target As XmlWriter)

End Interface
