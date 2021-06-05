Imports System.Runtime.CompilerServices
Imports System.ComponentModel
Imports System.Linq
Imports System.Text
Imports System
Imports System.Reflection
Imports System.Diagnostics
Imports System.IO

Namespace System.IO

  Public Module ExtensionsForDirectoryInfo

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetFilesSafe(extendee As DirectoryInfo) As FileInfo()
      Return GetFilesSafe(extendee, "*")
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetFilesSafe(extendee As DirectoryInfo, searchPattern As String) As FileInfo()
      Return GetFilesSafe(extendee, searchPattern, SearchOption.TopDirectoryOnly)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetFilesSafe(extendee As DirectoryInfo, searchPattern As String, searchOption As SearchOption) As FileInfo()
      Dim results As New List(Of FileInfo)

      Try
        For Each file In extendee.GetFiles(searchPattern)
          results.Add(file)
        Next
      Catch ex As UnauthorizedAccessException
        'FUCK THE FILESYSTEM!!! there is no performant way to
        'avoid from this exception by some pre-evaluation...
      End Try

      If (searchOption = SearchOption.AllDirectories) Then
        For Each subDir In extendee.GetDirectoriesSafe("*", searchOption)
          Try
            For Each file In subDir.GetFiles(searchPattern)
              results.Add(file)
            Next
          Catch ex As UnauthorizedAccessException
            'FUCK THE FILESYSTEM!!! there is no performant way to
            'avoid from this exception by some pre-evaluation...
          End Try
        Next
      End If

      Return results.ToArray()
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetDirectoriesSafe(extendee As DirectoryInfo) As DirectoryInfo()
      Return GetDirectoriesSafe(extendee, "*")
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetDirectoriesSafe(extendee As DirectoryInfo, searchPattern As String) As DirectoryInfo()
      Return GetDirectoriesSafe(extendee, searchPattern, SearchOption.TopDirectoryOnly)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetDirectoriesSafe(extendee As DirectoryInfo, searchPattern As String, searchOption As SearchOption) As DirectoryInfo()
      Dim results As New List(Of DirectoryInfo)

      Dim singleLevelEvaluator As Action(Of DirectoryInfo) = (
      Sub(currentDir As DirectoryInfo)
        Try
          For Each subDir In currentDir.GetDirectories(searchPattern)
            results.Add(subDir)
            If (searchOption = SearchOption.AllDirectories) Then
              singleLevelEvaluator.Invoke(subDir)
            End If
          Next
        Catch ex As UnauthorizedAccessException
          'FUCK THE FILESYSTEM!!! there is no performant way to
          'avoid from this exception by some pre-evaluation...
        End Try
      End Sub
    )

      singleLevelEvaluator.Invoke(extendee)

      Return results.ToArray()
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="extendee"></param>
    ''' <param name="searchPattern">a path, RELATIVE to the extendee(can be .\**\*.txt)</param>
    ''' <returns></returns>
    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetFilesWithPathPattern(extendee As DirectoryInfo, searchPattern As String) As FileInfo()

      Dim fullPatternPath As String
      If (Path.IsPathRooted(searchPattern)) Then
        fullPatternPath = searchPattern
      Else
        fullPatternPath = Path.Combine(extendee.FullName, searchPattern)
      End If

      fullPatternPath = fullPatternPath.Replace("*", "%S~").Replace("?", "%Q~")
      fullPatternPath = Path.GetFullPath(fullPatternPath).Replace("%S~", "*").Replace("%Q~", "?")

      Dim pathTokens As String() = fullPatternPath.Split(Path.DirectorySeparatorChar)
      Dim directoryTokens As String() = pathTokens.SubArray(0, pathTokens.Length - 1)
      Dim fileToken As String = pathTokens(pathTokens.Length - 1)
      Dim foundFiles As New List(Of FileInfo)

      Dim recursiveCrawler As Action(Of Integer, DirectoryInfo) = (
      Sub(currentTokenIndex As Integer, currentDir As DirectoryInfo)

        If (currentTokenIndex >= (directoryTokens.Length - 1)) Then
          currentDir.GetFilesSafe(fileToken).CopyTo(foundFiles, True)
          Exit Sub
        End If

        Dim nextTokenIndex = currentTokenIndex + 1
        Dim subDirToken As String = directoryTokens(nextTokenIndex)
        Dim searchOption As SearchOption = SearchOption.TopDirectoryOnly

        If (subDirToken = "**") Then
          'begin recursion (but also keeping the outstanding tokens)
          searchOption = SearchOption.AllDirectories
          subDirToken = "*"

          'but first process the outstanding tokens for the current level ("./") because ** also includes this
          recursiveCrawler.Invoke(nextTokenIndex, currentDir)
        End If

        Dim subDirs = currentDir.GetDirectoriesSafe(subDirToken, searchOption) 'exact or masks
        For Each subDir In subDirs
          recursiveCrawler.Invoke(nextTokenIndex, subDir)
        Next

      End Sub
    )

      Dim drive As New DirectoryInfo(directoryTokens(0) + Path.DirectorySeparatorChar)
      recursiveCrawler.Invoke(0, drive) 'TRIGGER RECURSION

      Return foundFiles.ToArray()
    End Function

  End Module

End Namespace
