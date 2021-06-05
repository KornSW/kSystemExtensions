Imports System.Runtime.CompilerServices
Imports System.ComponentModel
Imports System.Linq
Imports System.Text
Imports System
Imports System.Reflection
Imports System.Diagnostics
Imports System.IO

Namespace System.IO

  Public Module ExtensionsForFileInfo

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ModifciationDateEquals(oneFile As FileInfo, anotherFile As FileInfo) As Boolean
      Return oneFile.LastWriteTime.Equals(anotherFile.LastWriteTime)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function EncodeMD5(file As FileInfo) As String
      '##################################
      '  TODO: IMPLEMENT THIS METHOD !!!
      Throw New NotImplementedException()
      '##################################
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function EncodeMD5(file As FileInfo, onlyFirstAndLastBytes As Integer) As String
      '##################################
      '  TODO: IMPLEMENT THIS METHOD !!!
      Throw New NotImplementedException()
      '##################################
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ContentEquals(oneFile As FileInfo, anotherFile As FileInfo) As Boolean
      '##################################
      '  TODO: IMPLEMENT THIS METHOD !!!
      Throw New NotImplementedException()
      '##################################
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ContentEquals(oneFile As FileInfo, anotherFile As FileInfo, onlyFirstAndLastBytes As Integer) As Boolean
      '##################################
      '  TODO: IMPLEMENT THIS METHOD !!!
      Throw New NotImplementedException()
      '##################################
    End Function


    '#Region " Thumbnails & Icons "

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Function GetExplorerIcon(sourceInstance As IO.FileInfo) As System.Drawing.Icon
    '      Throw New NotImplementedException
    '    End Function

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Function GetExplorerThumbnail(sourceInstance As IO.FileInfo) As System.Drawing.Image
    '      Throw New NotImplementedException
    '    End Function

    '#End Region

    '#Region " Compression "

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Sub CompressToZipFile(sourceInstance As IO.FileInfo(), destinationZipFileName As String)

    '    End Sub

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Sub CompressToZipFile(sourceInstance As IO.DirectoryInfo, destinationZipFileName As String, inculdeSubFolders As Boolean)

    '    End Sub

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Sub UncompressToFolder(sourceInstance As IO.FileInfo, destinationFolderName As String)

    '    End Sub

    '#End Region

    '#Region " Process Locks "

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Function GetFilesWithProcessLocks(sourceInstance As IO.DirectoryInfo) As IO.FileInfo()
    '      Throw New NotImplementedException
    '    End Function

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Function GetProcessLocks(sourceInstance As IO.FileInfo()) As System.Diagnostics.Process()
    '      Throw New NotImplementedException
    '    End Function

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Function GetProcessLocks(sourceInstance As IO.FileInfo) As System.Diagnostics.Process()
    '      Throw New NotImplementedException
    '    End Function

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Sub FreeProcessLocks(sourceInstance As IO.FileInfo())
    '      Throw New NotImplementedException
    '    End Sub

    '#End Region

  End Module

End Namespace
