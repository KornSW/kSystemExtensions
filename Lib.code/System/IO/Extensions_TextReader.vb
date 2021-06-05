Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Text

Namespace System.IO

  Public Module ExtensionsForTextReader

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function AllLines(source As TextReader) As IEnumerable(Of String)
      Return GetLineIterator(source)
    End Function
    Private Iterator Function GetLineIterator(source As TextReader) As IEnumerable(Of String)
      Dim currentLine As String
      currentLine = source.ReadLine
      While (currentLine IsNot Nothing)
        Yield currentLine
        currentLine = source.ReadLine
      End While
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function EndReached(extendee As TextReader) As Boolean
      Return extendee.Peek() < 0
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function PeekChar(extendee As TextReader) As Char
      Return Convert.ToChar(extendee.Peek())
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function ReadChar(extendee As TextReader) As Char
      Return Convert.ToChar(extendee.Read())
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Sub SkipUntil(extendee As TextReader, searchPattern As String)
      Dim patternIndex As Integer = 0
      Dim currentChar As Char

      Do Until extendee.EndReached
        currentChar = extendee.ReadChar()
        If (currentChar = searchPattern(patternIndex)) Then
          patternIndex += 1
          If (patternIndex = searchPattern.Length) Then
            Exit Sub
          End If
        Else
          patternIndex = 0
        End If
      Loop

    End Sub

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function ReadUntil(extendee As TextReader, searchPattern As String, removePattern As Boolean) As String
      Dim buffer As New StringBuilder
      Dim patternIndex As Integer = 0
      Dim currentChar As Char
      Dim patternFound As Boolean = False

      Do Until extendee.EndReached OrElse patternFound
        currentChar = extendee.ReadChar()
        buffer.Append(currentChar)
        If (currentChar = searchPattern(patternIndex)) Then
          patternIndex += 1
          If (patternIndex = searchPattern.Length) Then
            patternFound = True
          End If
        Else
          patternIndex = 0
        End If
      Loop

      If (patternFound AndAlso removePattern) Then
        Return buffer.ToString(0, buffer.Length - searchPattern.Length)
      Else
        Return buffer.ToString()
      End If

    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Iterator Function Tokenize(extendee As TextReader, tokenBegin As String, tokenEnd As String, Optional includeTokensOnly As Boolean = False) As IEnumerable(Of String)
      Dim inToken As Boolean = False
      Do Until extendee.EndReached
        If (inToken) Then
          Yield extendee.ReadUntil(tokenEnd, True)
          inToken = False
        Else
          Yield extendee.ReadUntil(tokenBegin, True)
          inToken = True
        End If
      Loop
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Sub Tokenize(extendee As TextReader, tokenBegin As String, tokenEnd As String, tokenReceiver As Action(Of String), Optional outerRangeReceiver As Action(Of String) = Nothing)
      Dim inToken As Boolean = False
      Dim tokens As New List(Of String)
      Do Until extendee.EndReached
        If (inToken) Then
          tokenReceiver.Invoke(extendee.ReadUntil(tokenEnd, True))
          inToken = False
        Else
          If (outerRangeReceiver IsNot Nothing) Then
            outerRangeReceiver.Invoke(extendee.ReadUntil(tokenBegin, True))
          Else
            extendee.SkipUntil(tokenBegin)
          End If

          inToken = True
        End If
      Loop
    End Sub

  End Module

End Namespace
