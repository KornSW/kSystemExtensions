Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.IO
Imports System.Linq
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Security
Imports System.Text
Imports System.Text.RegularExpressions

Namespace System

  Public Module ExtensionsForString

#Region " Conversion "

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToBoolean(value As String) As Boolean
      Return value.ToInteger().ToBoolean()
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToByte(value As String) As Byte
      Dim result As Byte = 0
      Byte.TryParse(value, result)
      Return result
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToInteger(value As String) As Integer
      Dim result As Integer = 0
      If (Not Integer.TryParse(value, result)) Then
        Select Case value.ToLower()
          Case "true", "wahr", (True).ToString().ToLower()
            result = 1
          Case "false", "falsch", (False).ToString().ToLower()
            result = 0
        End Select
      End If
      Return result
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToDecimal(value As String) As Decimal
      Dim result As Decimal = 0
      Decimal.TryParse(value, result)
      Return result
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToBytes(value As String, Optional enc As Encoding = Nothing) As Byte()
      If (enc Is Nothing) Then
        enc = Encoding.Default
      End If
      Return enc.GetBytes(value)
    End Function

#End Region

#Region " Compare & Match "

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function MatchesMaskOrRegex(stringToEvaluate As String, pattern As String, Optional ignoreCasing As Boolean = True) As Boolean
      If (pattern.StartsWith("^") AndAlso pattern.EndsWith("$")) Then
        Return MatchesRegex(stringToEvaluate, pattern, ignoreCasing)
      Else
        Return MatchesWildcardMask(stringToEvaluate, pattern, ignoreCasing)
      End If
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function MatchesRegex(stringToEvaluate As String, pattern As String, Optional ignoreCasing As Boolean = True) As Boolean
      If (ignoreCasing) Then
        Return Regex.IsMatch(stringToEvaluate, pattern, RegexOptions.IgnoreCase)
      Else
        Return Regex.IsMatch(stringToEvaluate, pattern)
      End If
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function MatchesWildcardMask(stringToEvaluate As String, pattern As String, Optional ignoreCasing As Boolean = True) As Boolean

      Dim indexOfDoubleDot = pattern.IndexOf("..", StringComparison.Ordinal)
      If (indexOfDoubleDot >= 0) Then
        For i = indexOfDoubleDot To pattern.Length - 1
          If (Not pattern(i) = "."c) Then
            Return False
          End If
        Next
      End If

      Dim normalizedPatternString As String = Regex.Replace(pattern, "\.+$", "")
      Dim endsWithDot As Boolean = (Not normalizedPatternString.Length = pattern.Length)
      Dim endCharCount As Integer = 0

      If (endsWithDot) Then
        Dim lastNonWildcardPosition = normalizedPatternString.Length - 1

        While lastNonWildcardPosition >= 0
          Dim currentChar = normalizedPatternString(lastNonWildcardPosition)
          If (currentChar = "*"c) Then
            endCharCount += Short.MaxValue
          ElseIf (currentChar = "?"c) Then
            endCharCount += 1
          Else
            Exit While
          End If
          lastNonWildcardPosition -= 1
        End While

        If (endCharCount > 0) Then
          normalizedPatternString = normalizedPatternString.Substring(0, lastNonWildcardPosition + 1)
        End If

      End If

      Dim endsWithWildcardDot As Boolean = endCharCount > 0
      Dim endsWithDotWildcardDot As Boolean = (endsWithWildcardDot AndAlso normalizedPatternString.EndsWith("."))

      If (endsWithDotWildcardDot) Then
        normalizedPatternString = normalizedPatternString.Substring(0, normalizedPatternString.Length - 1)
      End If

      normalizedPatternString = Regex.Replace(normalizedPatternString, "(?!^)(\.\*)+$", ".*")

      Dim escapedPatternString = Regex.Escape(normalizedPatternString)
      Dim prefix As String
      Dim suffix As String

      If (endsWithDotWildcardDot) Then
        prefix = "^" & escapedPatternString
        suffix = "(\.[^.]{0," & endCharCount & "})?$"
      ElseIf (endsWithWildcardDot) Then
        prefix = "^" & escapedPatternString
        suffix = "[^.]{0," & endCharCount & "}$"
      Else
        prefix = "^" & escapedPatternString
        suffix = "$"
      End If

      If (prefix.EndsWith("\.\*") AndAlso prefix.Length > 5) Then
        prefix = prefix.Substring(0, prefix.Length - 4)
        suffix = Convert.ToString("(\..*)?") & suffix
      End If

      Dim expressionString = prefix.Replace("\*", ".*").Replace("\?", "[^.]?") & suffix

      If (ignoreCasing) Then
        Return Regex.IsMatch(stringToEvaluate, expressionString, RegexOptions.IgnoreCase)
      Else
        Return Regex.IsMatch(stringToEvaluate, expressionString)
      End If

    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function CompareTo(strA As String, strB As String, ignoreCasing As Boolean) As Integer
      Return String.Compare(strA, strB, ignoreCasing)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function Equals(strA As String, strB As String, ignoreCasing As Boolean) As Boolean
      If (ignoreCasing) Then
        Return String.Equals(strA, strB, StringComparison.InvariantCultureIgnoreCase)
      Else
        Return String.Equals(strA, strB)
      End If
    End Function

    'HAT BUG!!!
    '<Extension(), EditorBrowsable(EditorBrowsableState.Always), Obsolete("Use ")>
    'Public Function MatchesToWcPattern(stringToEvaluate As String, wildcardPattern As String, Optional wildcardChar As Char = "*"c, Optional ignoreCasing As Boolean = True) As Boolean
    '  Dim patternParts As String()
    '  Dim currentPosition As Integer = 0

    '  If (ignoreCasing) Then
    '    wildcardPattern = wildcardPattern.ToLower()
    '    stringToEvaluate = stringToEvaluate.ToLower()
    '  End If

    '  patternParts = wildcardPattern.Split(wildcardChar)

    '  If (String.IsNullOrEmpty(wildcardPattern)) Then
    '    Return False
    '  End If

    '  If (Not String.IsNullOrEmpty(patternParts(0)) AndAlso Not stringToEvaluate.StartsWith(patternParts(0))) Then
    '    Return False
    '  End If

    '  For Each part As String In patternParts
    '    If (Not String.IsNullOrEmpty(part)) Then

    '      Dim foundIndex As Integer
    '      foundIndex = stringToEvaluate.Substring(currentPosition, stringToEvaluate.Length - currentPosition).IndexOf(part)
    '      If (foundIndex >= 0) Then
    '        currentPosition += foundIndex + part.Length
    '      Else
    '        Return False
    '      End If

    '    End If
    '  Next

    '  Return (currentPosition = stringToEvaluate.Length OrElse String.IsNullOrEmpty(patternParts(patternParts.Length - 1)))
    'End Function

    '<Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    'Public Function MatchesToRegex(stringToEvaluate As String, regexPattern As String, Optional ignoreCasing As Boolean = True) As Boolean
    '  If (ignoreCasing) Then
    '    Return Regex.IsMatch(stringToEvaluate, regexPattern, RegexOptions.IgnoreCase)
    '  Else
    '    Return Regex.IsMatch(stringToEvaluate, regexPattern)
    '  End If
    'End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function FilterByMaskOrRegex(value As IEnumerable(Of String), wildcardPattern As String, Optional ignoreCasing As Boolean = True) As IEnumerable(Of String)
      Return From item In value Where item.MatchesMaskOrRegex(wildcardPattern, ignoreCasing)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function FilterByWildcardMask(value As IEnumerable(Of String), wildcardPattern As String, Optional ignoreCasing As Boolean = True) As IEnumerable(Of String)
      Return From item In value Where item.MatchesWildcardMask(wildcardPattern, ignoreCasing)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function FilterByRegex(value As IEnumerable(Of String), regexPattern As String, Optional ignoreCasing As Boolean = True) As IEnumerable(Of String)
      Return From item In value Where item.MatchesRegex(regexPattern, ignoreCasing)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function CompileToRegex(regexPattern As String, ignoreCasing As Boolean) As Regex
      Dim regexFlags As RegexOptions

      If (ignoreCasing) Then
        regexFlags = RegexOptions.Compiled Or RegexOptions.IgnoreCase
      Else
        regexFlags = RegexOptions.Compiled
      End If

      Return New Regex(regexPattern, regexFlags)
    End Function

    'Private allCharsRegex As String = "[A-Za-z0-9ÄÖÜäöüß\]" & Regex.Escape("[)(}{,;.:_&!=-\/%""$+*?") & "]"
    'Private Function ConvertWildcardPatternToRegexPattern(wildCardPattern As String) As String

    '  wildCardPattern = wildCardPattern.Replace("%", "ANY")
    '  wildCardPattern = wildCardPattern.Replace("*", "ANY")
    '  wildCardPattern = wildCardPattern.Replace("+", "ONE_OR_MORE")
    '  wildCardPattern = wildCardPattern.Replace("?", "NONE_OR_ONE")

    '  wildCardPattern = Regex.Escape(wildCardPattern).Replace("]", "\]") '<BUG-FIX

    '  wildCardPattern = wildCardPattern.Replace("ANY", allCharsRegex & "*")
    '  wildCardPattern = wildCardPattern.Replace("ONE_OR_MORE", allCharsRegex & "+")
    '  wildCardPattern = wildCardPattern.Replace("NONE_OR_ONE", allCharsRegex & "?")

    '  Return "^" & wildCardPattern & "$"
    'End Function




    'Private Const RegexCharsToTerminate As String = "?+:()[].\^$*"
    'Private Const RegexTerminatorChar As Char = "\"c
    'Private Const RegexSuffixString As String = "^("
    'Private Const RegexPrefixString As String = ")$"

    '<Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    'Public Function MatchesWildcardPattern(input As String, wildcardPattern As String, Optional wildcardRegex As String = "([0-9]|[A_Z]|[a-z]|[ ])*", Optional wildcardChar As Char = "*"c) As Boolean
    '  Dim wildcardPatternParts As String()
    '  Dim regexPattern As String

    '  If (Not wildcardPattern.Contains(wildcardChar)) Then
    '    wildcardPattern = wildcardChar & wildcardPattern & wildcardChar
    '  End If
    '  wildcardPatternParts = wildcardPattern.Split(wildcardChar)

    '  For Each regexCharToTerminate As Char In RegexCharsToTerminate
    '    For i As Integer = 0 To (wildcardPatternParts.Length - 1)
    '      wildcardPatternParts(i) = wildcardPatternParts(i).Replace(regexCharToTerminate, RegexTerminatorChar & regexCharToTerminate)
    '    Next
    '  Next
    '  regexPattern = RegexSuffixString & String.Join(wildcardRegex, wildcardPatternParts) & RegexPrefixString

    '  Return input.MatchesRegex(regexPattern)
    'End Function

    '<Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    'Public Function MatchesRegex(input As String, regexPattern As String) As Boolean
    '  Return System.Text.RegularExpressions.Regex.IsMatch(input, regexPattern)
    'End Function

#End Region

#Region " Cut & Trim "

    ''' <summary>
    ''' Determines whether the string instance ends with the specified suffix; otherwise, the suffix will be added to the string.
    ''' </summary>
    ''' <param name="extendee">The existing data type to be extended.</param>
    ''' <param name="suffix">The suffix for the string instance.</param>
    ''' <returns>A string instance including the suffix.</returns>
    ''' <remarks></remarks>
    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function EnsureEndsWith(ByVal extendee As String, ByVal suffix As String) As String
      If (Not String.IsNullOrEmpty(extendee)) Then
        If (extendee.EndsWith(suffix)) Then
          Return extendee
        End If
      End If
      Return extendee & suffix
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function CutBeginUntil(extendee As String, edge As String, removeEdge As Boolean) As String
      Dim edgeIndex As Integer = extendee.IndexOf(edge)
      If (edgeIndex < 0) Then
        Return extendee
      End If
      If (removeEdge) Then
        Return extendee.Substring(edgeIndex, extendee.Length - edgeIndex - edge.Length)
      Else
        Return extendee.Substring(edgeIndex, extendee.Length - edgeIndex)
      End If
    End Function

    ''' <summary>
    ''' Appends an 'NewLine' string to the original String
    ''' </summary>
    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function NewLine(ByRef extendee As String, Optional count As Integer = 1) As String
      Select Case count
        Case Is < 1 : Return extendee
        Case 1 : Return extendee & Environment.NewLine
        Case Else : Return extendee & Environment.NewLine.Duplicate(count)
      End Select
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function Duplicate(ByRef extendee As String, count As Integer) As String
      Select Case count
        Case Is < 1 : Return String.Empty
        Case 1 : Return extendee
        Case 2 : Return extendee & extendee
        Case 3 : Return extendee & extendee & extendee
        Case Else
          Dim sb As New StringBuilder
          For i As Integer = 1 To count
            sb.Append(extendee)
          Next
          Return sb.ToString()
      End Select
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function CutEndUntil(extendee As String, edge As String, removeEdge As Boolean) As String
      Dim edgeIndex As Integer = extendee.LastIndexOf(edge)
      If (edgeIndex < 0) Then
        Return extendee
      End If
      If (removeEdge) Then
        Return extendee.Substring(0, edgeIndex)
      Else
        Return extendee.Substring(0, edgeIndex + edge.Length)
      End If
    End Function

#End Region

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function FixupLinebreaks(stringWithAnyLinebreaks As String) As String
      Return stringWithAnyLinebreaks.FixupLinebreaks(Environment.NewLine)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function FixupLinebreaks(stringWithAnyLinebreaks As String, targetLinebreak As String) As String
      Dim rdr As New StringReader(stringWithAnyLinebreaks)
      Dim sb As New StringBuilder(stringWithAnyLinebreaks.Length)
      Dim first As Boolean = True
      For Each line In rdr.AllLines()
        If (first) Then
          first = False
        Else
          sb.Append(targetLinebreak)
        End If
        sb.Append(line)
      Next
      Return sb.ToString()
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function BuildFormatString(stringWithPlaceholders As String, ParamArray placeholderKeys() As String) As String
      Dim sb As New StringBuilder
      sb.Append(stringWithPlaceholders)

      Dim index As Integer = 0
      For Each phk In placeholderKeys
        sb.Replace(phk, "{" & index.ToString() & "}")
        index += 1
      Next

      Return sb.ToString()
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function InsertLinebreaks(extendee As String, charcount As Integer) As String
      Dim sb As New StringBuilder
      For Each line In extendee.Lines
        While (line.Length > charcount)
          sb.AppendLine(line.Substring(0, charcount))
          line = line.Substring(charcount)
        End While
        sb.AppendLine(line)
      Next
      Return sb.ToString()
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function FormatAsKey(extendee As String, Optional blockSize As Integer = 5) As String
      Dim sb As New StringBuilder
      Dim currentBlockSize As Integer
      For i As Integer = 0 To extendee.Length - 1
        If (currentBlockSize = blockSize) Then
          sb.Append("-"c)
          currentBlockSize = 0
        End If
        sb.Append(extendee(i))
        currentBlockSize += 1
      Next
      Return sb.ToString().ToUpper()
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function TryParse(Of TTargetType)(sourceText As String, ByRef target As TTargetType) As Boolean
      Dim result As Object = Nothing
      If (GetType(TTargetType).TryParse(sourceText, result)) Then
        target = DirectCast(result, TTargetType)
        Return True
      Else
        Return False
      End If
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function TryParse(sourceText As String, targetType As Type, ByRef target As Object) As Boolean
      Return targetType.TryParse(sourceText, target)
    End Function

    ''' <summary>
    ''' If the source String is Empty or WhiteSpace, then Nothing will be returned. Otherwise the source String will be returned.
    ''' </summary>
    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function NothingIfEmpty(extendee As String) As String
      If String.IsNullOrWhiteSpace(extendee) Then
        Return Nothing
      Else
        Return extendee
      End If
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function SubStringBefore(extendee As String, searchString As String) As String
      Dim idx = extendee.IndexOf(searchString)
      If (idx >= 0) Then
        Return extendee.Substring(0, idx)
      Else
        Return extendee
      End If
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function SubStringAfter(extendee As String, searchString As String) As String
      Dim idx = extendee.IndexOf(searchString)
      If (idx >= 0) Then
        Return extendee.Substring(idx + 1, extendee.Length - idx - 1)
      Else
        Return String.Empty
      End If
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="str"></param>
    ''' <param name="length"></param>
    ''' <param name="cutter">The cutter will be prepended at the left size of the returned string, when the stric was cutted. use this to apply some dots for example ("...my string")</param>
    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function Right(str As String, length As Integer, Optional cutter As String = Nothing) As String
      Dim overFlow As Integer = str.Length - length
      If (overFlow > 0) Then
        If (cutter Is Nothing) Then
          Return str.Substring(overFlow, str.Length - overFlow)
        Else
          overFlow += cutter.Length
          Return cutter & str.Substring(overFlow, str.Length - overFlow)
        End If
      Else
        Return str
      End If
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsNullOrEmpty(anyString As String) As Boolean
      Return String.IsNullOrEmpty(anyString)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsNullOrWhiteSpace(anyString As String) As Boolean
      Return String.IsNullOrWhiteSpace(anyString)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsNotNullOrEmpty(anyString As String) As Boolean
      Return Not String.IsNullOrEmpty(anyString)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsNotNullOrWhiteSpace(anyString As String) As Boolean
      Return Not String.IsNullOrWhiteSpace(anyString)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToCharArray(str As String) As Char()
      Return str.ToArray()
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function FormatWith(stringWithPlaceholders As String, ParamArray values() As Object) As String
      Return String.Format(stringWithPlaceholders, values)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function MD5(value As String) As String
      Using md5Provider As New System.Security.Cryptography.MD5CryptoServiceProvider

        Dim stringBytes As Byte()
        Dim hashBytes As Byte()
        Dim tmp As String = ""
        Dim sb As New StringBuilder

        stringBytes = Encoding.ASCII.GetBytes(value)
        hashBytes = md5Provider.ComputeHash(stringBytes)

        For i As Integer = 0 To hashBytes.Length - 1
          tmp = Microsoft.VisualBasic.Hex(hashBytes(i))
          If (tmp.Length = 1) Then
            sb.Append("0")
          End If
          sb.Append(tmp)
        Next

        Return sb.ToString().ToLower()
      End Using

    End Function

#Region " Encode / Decode "



    '<Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    'Public Function DecodeHex(value As String, ByRef target As Integer) As Boolean
    '  '##################################
    '  '  TODO: IMPLEMENT THIS METHOD !!!
    '  Throw New NotImplementedException()
    '  '##################################
    'End Function

    '<Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    'Public Function DecodeHex(value As String, ByRef target As Byte) As Boolean
    '  '##################################
    '  '  TODO: IMPLEMENT THIS METHOD !!!
    '  Throw New NotImplementedException()
    '  '##################################
    'End Function

    '<Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    'Public Function DecodeHex(value As String, ByRef target As Byte()) As Boolean
    '  '##################################
    '  '  TODO: IMPLEMENT THIS METHOD !!!
    '  Throw New NotImplementedException()
    '  '##################################
    'End Function

    '<Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    'Public Function DecodeB64(value As String, ByRef target As Integer) As Boolean
    '  '##################################
    '  '  TODO: IMPLEMENT THIS METHOD !!!
    '  Throw New NotImplementedException()
    '  '##################################
    'End Function

    '<Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    'Public Function DecodeB64(value As String, ByRef target As Byte) As Boolean
    '  '##################################
    '  '  TODO: IMPLEMENT THIS METHOD !!!
    '  Throw New NotImplementedException()
    '  '##################################
    'End Function

    '<Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    'Public Function DecodeB64(value As String, ByRef target As Byte()) As Boolean
    '  '##################################
    '  '  TODO: IMPLEMENT THIS METHOD !!!
    '  Throw New NotImplementedException()
    '  '##################################
    'End Function

#End Region

#Region " Multifield Manipulation "

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function Join(value As IEnumerable(Of String), Optional separator As String = "") As String
      Dim sb As New StringBuilder
      Dim first As Boolean = True
      For Each s As String In value
        If (first) Then
          first = False
        Else
          sb.Append(separator)
        End If
        sb.Append(s)
      Next
      Return sb.ToString()
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function AllToLower(value As IEnumerable(Of String)) As IEnumerable(Of String)
      Return New EnumerableProxy(Of String, String)(value, Function(s) s.ToLower())
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function AllToUpper(value As IEnumerable(Of String)) As IEnumerable(Of String)
      Return New EnumerableProxy(Of String, String)(value, Function(s) s.ToUpper())
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function TrimAll(value As IEnumerable(Of String)) As IEnumerable(Of String)
      Return New EnumerableProxy(Of String, String)(value, Function(s) s.Trim())
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToInteger(value As IEnumerable(Of String)) As IEnumerable(Of Integer)
      Return value.TransformTo(Of Integer)(Function(s) Integer.Parse(s))
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function RemoveNullOrEmpty(value As IEnumerable(Of String)) As IEnumerable(Of String)
      Return From s As String In value Where Not String.IsNullOrEmpty(s)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function RemoveNullOrWhiteSpace(value As IEnumerable(Of String)) As IEnumerable(Of String)
      Return From s As String In value Where Not String.IsNullOrWhiteSpace(s)
    End Function

#End Region

#Region " Enumerable Lines "

    ''' <summary>
    ''' returns an ienumerable which allows to iterate over all lines of the string
    ''' </summary>
    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function Lines(ByRef extendee As String, Optional keepEmptyLines As Boolean = True) As IEnumerable(Of String)
      Return New StringLineEnumerable(extendee, keepEmptyLines)
    End Function

#Region " Enumerable "

    Private Class StringLineEnumerable
      Implements IEnumerable(Of String)

      Private _FullString As String
      Private _KeepEmptyLines As Boolean

      Public Sub New(ByRef basedOn As String, keepEmptyLines As Boolean)
        _FullString = basedOn
        _KeepEmptyLines = keepEmptyLines
      End Sub

      Private Function CreateNewStringReader() As StringReader
        Return New StringReader(_FullString)
      End Function

      Public Function GetEnumerator() As IEnumerator(Of String) Implements IEnumerable(Of String).GetEnumerator
        Return New StringLineEnumerator(AddressOf Me.CreateNewStringReader, _KeepEmptyLines)
      End Function

      Public Function GetUntypedEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return New StringLineEnumerator(AddressOf Me.CreateNewStringReader, _KeepEmptyLines)
      End Function

#Region " Enumerator "

      Private Class StringLineEnumerator
        Implements IEnumerator(Of String)

        Public Delegate Function ReaderInitialisationMethod() As StringReader

        Private _CurrentLine As String = Nothing
        Private _Iterator As StringReader = Nothing
        Private _Initializer As ReaderInitialisationMethod
        Private _KeepEmptyLines As Boolean

        Public Sub New(initializer As ReaderInitialisationMethod, keepEmptyLines As Boolean)
          _Initializer = initializer
          _KeepEmptyLines = keepEmptyLines
        End Sub

        Public ReadOnly Property Current As String Implements IEnumerator(Of String).Current
          Get
            Return _CurrentLine
          End Get
        End Property

        Private ReadOnly Property UntypedCurrent As Object Implements IEnumerator.Current
          Get
            Return _CurrentLine
          End Get
        End Property

        Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
          If (_Iterator Is Nothing) Then
            Reset()
          End If

          _CurrentLine = _Iterator.ReadLine()

          If (Not _KeepEmptyLines) Then
            While (_CurrentLine IsNot Nothing AndAlso String.IsNullOrWhiteSpace(_CurrentLine))
              _CurrentLine = _Iterator.ReadLine()
            End While
          End If

          Return (_CurrentLine IsNot Nothing)
        End Function

        Public Sub Reset() Implements IEnumerator.Reset
          _Iterator = _Initializer.Invoke()
          _CurrentLine = Nothing
        End Sub

#Region " Dispose "

        <EditorBrowsable(EditorBrowsableState.Never), DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _DisposedValue As Boolean

        <EditorBrowsable(EditorBrowsableState.Advanced)>
        Protected Overridable Sub Dispose(disposing As Boolean)
          If (Not _DisposedValue) Then
            If (disposing) Then
              'MANAGED
            End If
            'UNMANAGED
          End If
          _DisposedValue = True
        End Sub

        <EditorBrowsable(EditorBrowsableState.Advanced)>
        Public Sub Dispose() Implements IDisposable.Dispose
          Me.Dispose(True)
          GC.SuppressFinalize(Me)
        End Sub

#End Region

      End Class

#End Region

    End Class

#End Region

#End Region

#Region " Split To Enumerable "

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function Split(extendee As String, separator As String) As IEnumerable(Of String)
      Return New StringSplitEnumerable(extendee, separator)
    End Function

#Region " Enumerable "

    Private Class StringSplitEnumerable
      Implements IEnumerable(Of String)

      Private _FullString As String
      Private _Separator As String

      Public Sub New(ByRef basedOn As String, ByRef separator As String)
        _FullString = basedOn
        _Separator = separator
      End Sub

      Public Function GetEnumerator() As IEnumerator(Of String) Implements IEnumerable(Of String).GetEnumerator
        Return New StringLineEnumerator(_FullString, _Separator)
      End Function

      Public Function GetEnumeratorUntyped() As IEnumerator Implements IEnumerable.GetEnumerator
        Return New StringLineEnumerator(_FullString, _Separator)
      End Function

#Region " Enumerator "

      Private Class StringLineEnumerator
        Implements IEnumerator(Of String)

        Private _Separator As String
        Private _SeparatorLength As Integer
        Private _CurrentPart As String = Nothing
        Private _LastIndex As Integer = 0
        Private _EndReached As Boolean = False
        Private _FullString As String

        Public Sub New(ByRef basedOn As String, separator As String)
          _FullString = basedOn
          _Separator = separator
          _SeparatorLength = _Separator.Length
        End Sub

        Public ReadOnly Property Current As String Implements IEnumerator(Of String).Current
          Get
            Return _CurrentPart
          End Get
        End Property

        Private ReadOnly Property UntypedCurrent As Object Implements IEnumerator.Current
          Get
            Return _CurrentPart
          End Get
        End Property

        Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
          Dim foundIndex As Integer

          If (_EndReached) Then
            Return False
          Else
            foundIndex = _FullString.IndexOf(_Separator, _LastIndex)
          End If

          If (foundIndex < 0) Then
            _EndReached = True
          Else
            _CurrentPart = _FullString.Substring(_LastIndex, foundIndex - _LastIndex)
            _LastIndex = foundIndex + _SeparatorLength
          End If

          Return True
        End Function

        Public Sub Reset() Implements IEnumerator.Reset
          _LastIndex = 0
          _EndReached = False
          _CurrentPart = Nothing
        End Sub

#Region " Dispose "

        <EditorBrowsable(EditorBrowsableState.Never), DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _DisposedValue As Boolean

        <EditorBrowsable(EditorBrowsableState.Advanced)>
        Protected Overridable Sub Dispose(disposing As Boolean)
          If (Not _DisposedValue) Then
            If (disposing) Then
              'MANAGED
            End If
            'UNMANAGED
          End If
          _DisposedValue = True
        End Sub

        <EditorBrowsable(EditorBrowsableState.Advanced)>
        Public Sub Dispose() Implements IDisposable.Dispose
          Me.Dispose(True)
          GC.SuppressFinalize(Me)
        End Sub

#End Region

      End Class

#End Region

    End Class

#End Region

#End Region

#Region " Invoking Action(Of String)"

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub InvokeNewLine(target As Action(Of String))
      target.Invoke(Environment.NewLine)
    End Sub

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub InvokeNewLine(target As Action(Of String), param As String)
      target.Invoke(param & Environment.NewLine)
    End Sub

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub InvokeNewLine(target As Action(Of String), format As String, ParamArray values() As Object)
      target.Invoke(String.Format(format, values) & Environment.NewLine)
    End Sub

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub Invoke(target As Action(Of String), format As String, ParamArray values() As Object)
      target.Invoke(String.Format(format, values))
    End Sub

#End Region

    ''' <summary>
    ''' Makes the string orderable by included numbers. instead of ["X1_K", "X10_K", "X2_K"] (wrong order) we will get ["X001_K", "X002_K", "X010_K"] (correct order) when specifing 'numberlength'=3.
    ''' </summary><param name="numberlength">should at least as long as the max possible length of numbers in the input string</param>
    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function ToNumericOrderable(input As String, Optional numberlength As Integer = 7) As String
      If (String.IsNullOrWhiteSpace(input)) Then
        Return input
      End If
      Dim separated As String() = SeparateNumericBlocks(input)
      Dim numberLengthFormatstring As String = New String("0"c, numberlength)
      For i As Integer = 0 To separated.Length - 1
        If (separated(i).Length > 0 AndAlso Char.IsNumber(separated(i)(0))) Then
          separated(i) = Integer.Parse(separated(i)).ToString(numberLengthFormatstring)
        End If
      Next
      Return String.Join("", separated)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function SeparateNumericBlocks(input As String) As String()

      If (String.IsNullOrEmpty(input)) Then
        Return {String.Empty}
      End If

      Dim result(0) As String
      Dim currentIndex As Integer = 0
      Dim currentlyAlpha As Boolean = Not Char.IsNumber(input(0))

      For Each c In input
        Dim isAlpha = Not Char.IsNumber(c)
        If (Not currentlyAlpha = isAlpha) Then
          currentlyAlpha = isAlpha
          currentIndex += 1
          ReDim Preserve result(currentIndex)
        End If
        result(currentIndex) += c
      Next

      Return result
    End Function

  End Module

End Namespace
