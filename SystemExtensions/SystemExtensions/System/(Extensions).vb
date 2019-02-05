Imports System.Runtime.CompilerServices
Imports System.ComponentModel
Imports System.Linq
Imports System.Text
Imports System
Imports System.Reflection
Imports System.Diagnostics
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.IO
Imports System.Security.Cryptography
Imports System.Collections.Generic

Namespace System

  Public Module Extensions




    '#Region " Split & Cut "

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Function SplitLines(input As String) As String()




    '      Throw New NotImplementedException
    '    End Function

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Function SplitEx(sourceInstance As String, separatorString As String) As String()

    '      Throw New NotImplementedException
    '    End Function

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Function SplitEx(sourceInstance As String, separatorString As String, minimumFieldCount As Integer, Optional maximumFieldCount As Integer = -1) As String()

    '      Throw New NotImplementedException
    '    End Function

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Function SplitEx(sourceInstance As String, separatorString As String, removeEmptyFields As Boolean) As String()

    '      Throw New NotImplementedException
    '    End Function

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Function SplitEx(sourceInstance As String, separatorString As String, escapeChar As Char, enclosedEscaping As Boolean) As String()
    '      Throw New NotImplementedException

    '    End Function

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Function SplitEx(sourceInstance As String, separatorString As String, escapeChar As Char, enclosedEscaping As Boolean, minimumFieldCount As Integer, Optional maximumFieldCount As Integer = -1) As String()

    '      Throw New NotImplementedException
    '    End Function

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Function SplitEx(sourceInstance As String, separatorString As String, escapeChar As Char, enclosedEscaping As Boolean, removeEmptyFields As Boolean) As String()
    '      Throw New NotImplementedException

    '    End Function

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Function CutRight(ByRef sourceInstance As String, cutAt As Char, removeFromSourceString As Boolean) As String
    '      'original verändern
    '      Throw New NotImplementedException
    '    End Function

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Function CutLeft(ByRef sourceInstance As String, cutAt As Char, removeFromSourceString As Boolean) As String
    '      'original verändern 'abgeschnittenes zuückgeben
    '      Throw New NotImplementedException
    '    End Function

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Function LeftSubString(ByRef sourceInstance As String, startIndex As Integer, length As Integer) As String
    '      'ohne exception
    '      Throw New NotImplementedException
    '    End Function

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Function RightSubString(ByRef sourceInstance As String, startIndex As Integer, length As Integer) As String
    '      'ohne exception
    '      Throw New NotImplementedException
    '    End Function

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Function FindCharFromLeft(ByRef sourceInstance As String, find As Char) As Integer
    '      Throw New NotImplementedException
    '    End Function

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Function FindCharFromRight(ByRef sourceInstance As String, find As Char) As Integer
    '      Throw New NotImplementedException
    '    End Function

    '#End Region

    '#Region " Replace "

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Function ReplaceMultible(ByRef sourceInstance As String, replacement As String, ParamArray search() As String) As String

    '      Throw New NotImplementedException

    '    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function RemoveSubstring(ByRef sourceInstance As String, stringToRemove As String) As String
      Return sourceInstance.Replace(stringToRemove, String.Empty)
    End Function

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Function RemoveSubstring(ByRef sourceInstance As String, ParamArray stringsToRemove() As String) As String
    '      Throw New NotImplementedException
    '    End Function


    '#End Region


#Region " Linebreaks & Append "



    '<Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    'Public Function GetLinebreakFormat(ByRef sourceInstance As String) As LineBreakFormat
    '  Throw New NotImplementedException
    'End Function

    '<Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    'Public Sub ChangeLinebreakFormat(ByRef sourceInstance As String, newFormat As LineBreakFormat)
    '  Throw New NotImplementedException
    'End Sub

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function AppendLine(ByRef sourceInstance As String) As String
      Return (sourceInstance & Environment.NewLine)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function AppendLines(ByRef sourceInstance As String, count As Integer) As String
      Return (sourceInstance & (New String("°"c, count))).Replace("°", Environment.NewLine)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function AppendLine(ByRef sourceInstance As String, text As String) As String
      Return String.Format("{0}{1}{2}", sourceInstance, text, Environment.NewLine)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function AppendLine(ByRef sourceInstance As String, ParamArray text() As String) As String
      Dim SB As New System.Text.StringBuilder
      SB.Append(sourceInstance)
      For i As Integer = 0 To (text.Length - 1)
        If text(i).EndsWith("\n") Then
          SB.AppendLine()
        Else
          SB.Append(text(i))
        End If
      Next
      SB.AppendLine()
      Return SB.ToString()
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function Append(ByRef sourceInstance As String, text As String) As String
      Return (sourceInstance & text)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function Append(ByRef sourceInstance As String, ParamArray text() As String) As String
      Dim SB As New System.Text.StringBuilder
      SB.Append(sourceInstance)
      For i As Integer = 0 To (text.Length - 1)
        If text(i).EndsWith("\n") Then
          SB.AppendLine()
        Else
          SB.Append(text(i))
        End If
      Next
      Return SB.ToString()
    End Function

#End Region

#Region " LevenshteinDistance / ColognePhonetic / Soundex "

    ''' <summary>
    ''' Computes the 'ColognePhonetic' (=Kölner Phonetik) of a string
    ''' </summary>
    ''' <remarks>http://de.wikipedia.org/wiki/K%C3%B6lner_Phonetik</remarks>
    <Extension()>
    Public Function ColognePhonetic(word As String) As String

      Dim ubound As Integer
      Dim nextChar As Char
      Dim currentChar As Char
      Dim lastChar As Char
      Dim buffer As New List(Of Integer)
      Dim sb As New StringBuilder

      word = word.ToLower()
      ubound = (word.Length - 1)

      For index As Integer = 0 To ubound

        currentChar = word(index)

        If (index < ubound) Then
          nextChar = word(index + 1)
        Else
          nextChar = " "c
        End If

        If (index > 0) Then
          lastChar = word(index - 1)
        Else
          lastChar = " "c
        End If

        Select Case currentChar
          Case "a"c, "e"c, "i"c, "j"c, "o"c, "u"c, "y"c, "ä"c, "ö"c, "ü"c
            buffer.Add(0)
          Case "h"c
            'nichts
          Case "b"c
            buffer.Add(1)
          Case "p"c
            If (nextChar = "h"c) Then
              buffer.Add(3)
            Else
              buffer.Add(1)
            End If
          Case "d"c, "t"c
            If ((nextChar = "c"c OrElse nextChar = "s"c OrElse nextChar = "z"c)) Then
              buffer.Add(8)
            Else
              buffer.Add(2)
            End If
          Case "f"c, "v"c, "w"c
            buffer.Add(3)
          Case "g"c, "k"c, "q"c
            buffer.Add(4)
          Case "c"c
            If (index = 0) Then
              If (nextChar = "a"c OrElse nextChar = "h"c OrElse nextChar = "k"c OrElse nextChar = "l"c OrElse nextChar = "o"c OrElse nextChar = "q"c OrElse nextChar = "r"c OrElse nextChar = "u"c OrElse nextChar = "x"c) Then
                buffer.Add(4)
              Else
                buffer.Add(8)
              End If
            ElseIf (lastChar = "s"c OrElse lastChar = "z"c) Then
              buffer.Add(8)
            ElseIf (nextChar = "a"c OrElse nextChar = "h"c OrElse nextChar = "k"c OrElse nextChar = "o"c OrElse nextChar = "q"c OrElse nextChar = "u"c OrElse nextChar = "x"c) Then
              buffer.Add(4)
            Else
              buffer.Add(8)
            End If
          Case "x"c
            If ((lastChar = "c"c OrElse lastChar = "k"c OrElse lastChar = "q"c)) Then
              buffer.Add(8)
            Else
              buffer.Add(4)
              buffer.Add(8)
            End If
          Case "l"c
            buffer.Add(5)
          Case "m"c, "n"c
            buffer.Add(6)
          Case "r"c
            buffer.Add(7)
          Case "s"c, "z"c, "ß"c
            buffer.Add(8)
        End Select

      Next

      'delete all following codes
      Dim lastCode As Integer = 9
      For index As Integer = (buffer.Count - 1) To 0 Step -1
        If (buffer(index) = lastCode) Then
          buffer.RemoveAt(index)
        Else
          lastCode = buffer(index)
        End If
      Next

      'delete all zeroes except the first
      For index As Integer = (buffer.Count - 1) To 1 Step -1
        If (buffer(index) = 0) Then
          buffer.RemoveAt(index)
        End If
      Next

      'write out
      For index As Integer = 0 To (buffer.Count - 1)
        sb.Append(buffer(index))
      Next

      Return sb.ToString()
    End Function

    ''' <summary>
    ''' Computes the 'LevenshteinDistance' between two strings
    ''' </summary>
    <Extension()>
    Public Function LevenshteinDistance(ByVal sourceString As String, ByVal anotherString As String) As Integer

      Dim n As Integer = sourceString.Length
      Dim m As Integer = anotherString.Length

      Dim d(n + 1, m + 1) As Integer

      If n = 0 Then
        Return m
      End If

      If m = 0 Then
        Return n
      End If

      Dim i As Integer
      Dim j As Integer

      For i = 0 To n
        d(i, 0) = i
      Next

      For j = 0 To m
        d(0, j) = j
      Next

      For i = 1 To n
        For j = 1 To m

          Dim cost As Integer

          If anotherString(j - 1) = sourceString(i - 1) Then
            cost = 0
          Else
            cost = 1
          End If

          d(i, j) = Math.Min(Math.Min(d(i - 1, j) + 1, d(i, j - 1) + 1), d(i - 1, j - 1) + cost)

        Next

      Next

      Return d(n, m)
    End Function

#End Region


    '    ''' <summary>
    '    ''' accepts a format pattern which will be resolved via reflection and can be used like this:
    '    ''' car.ToString("Car: {obj}{br}({AnyPropertyName}/{AnyFunctionName()})")
    '    ''' </summary>
    '    <Extension()>
    '    Public Function ToString(Of T)(obj As T, format As String) As String

    '      'Sample:
    '      '  Dim ha As New System.Net.IPAddress({12, 34, 56, 78})
    '      '  Dim s As String = ha.ToString("IP {obj}{br}(IsIPv6LinkLocal=""{IsIPv6LinkLocal}"")")

    '      Dim foundPlaceHolders As New List(Of String)
    '      Dim contentString As New StringBuilder
    '      Dim placeHolderString As New StringBuilder
    '      Dim terminated As Boolean = False
    '      Dim inPlaceHolder As Boolean = False
    '      Dim targetType As Type

    '      If (obj Is Nothing) Then
    '        Return "{NULL}"
    '      End If

    '      For Each c As Char In format

    '        If (terminated) Then
    '          terminated = False
    '        Else

    '          Select Case c

    '            Case "/"c
    '              terminated = True
    '              Continue For

    '            Case "{"c
    '              inPlaceHolder = True
    '              Continue For

    '            Case "}"c
    '              If (inPlaceHolder) Then
    '                If (placeHolderString.Length > 0) Then
    '                  foundPlaceHolders.Add(placeHolderString.ToString())
    '                  placeHolderString.Clear()
    '                  contentString.Append("{" & (foundPlaceHolders.Count - 1).ToString() & "}")
    '                End If
    '                inPlaceHolder = False
    '              End If
    '              Continue For

    '          End Select

    '        End If

    '        If (inPlaceHolder) Then
    '          placeHolderString.Append(c)
    '        Else
    '          contentString.Append(c)
    '        End If

    '      Next

    '      targetType = obj.GetType()

    '      If (placeHolderString.Length > 0) Then
    '        foundPlaceHolders.Add(placeHolderString.ToString())
    '        placeHolderString.Clear()
    '        contentString.Append("{" & (foundPlaceHolders.Count - 1).ToString() & "}")
    '      End If

    '      For i As Integer = 0 To (foundPlaceHolders.Count - 1)
    '        If (foundPlaceHolders(i).ToLower() = "br") Then
    '          foundPlaceHolders(i) = Environment.NewLine
    '        ElseIf (foundPlaceHolders(i).ToLower() = "obj") Then
    '          foundPlaceHolders(i) = obj.ToString()
    '        ElseIf (foundPlaceHolders(i).EndsWith("()")) Then
    '          foundPlaceHolders(i) = targetType.GetMethod(foundPlaceHolders(i)).Invoke(obj, Nothing).ToString()
    '        Else
    '          foundPlaceHolders(i) = targetType.GetProperty(foundPlaceHolders(i)).GetValue(obj).ToString()
    '        End If
    '      Next

    '      Return String.Format(contentString.ToString(), foundPlaceHolders.ToArray())
    '    End Function

    '#End Region

































    '#Region " for STRING Array "

    '#Region " Array Trimming "

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Function WithoutEmptyFields(sourceInstance As String()) As String()

    '      Throw New NotImplementedException
    '    End Function

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Function WithoutFirstField(sourceInstance As String()) As String()

    '      Throw New NotImplementedException
    '    End Function

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Function WithoutLastField(sourceInstance As String()) As String()

    '      Throw New NotImplementedException
    '    End Function

    '#End Region

    '#End Region

    '#Region " for BYTE Array "

    '#Region " MD5 & 3DES "

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Function Encrypt3DES(sourceInstance As Byte(), key As String) As Byte()

    '      Throw New NotImplementedException
    '    End Function

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Function Encrypt3DES(sourceInstance As Byte(), key As System.Security.SecureString) As Byte()
    '      Throw New NotImplementedException

    '    End Function

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Function Decrypt3DES(sourceInstance As Byte(), key As String) As Byte()

    '      Throw New NotImplementedException
    '    End Function

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Function Decrypt3DES(sourceInstance As Byte(), key As System.Security.SecureString) As Byte()

    '      Throw New NotImplementedException
    '    End Function


    '#End Region

    '#Region " Encoding & Hex "

    '    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    '    Public Function GetHexString(sourceInstance As Byte()) As String
    '    '      Throw New NotImplementedException
    '    '    End Function

    '    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    '    Public Function ToCharArray(sourceInstance As Byte(), encoding As System.Text.Encoding) As Char()
    '    '      Throw New NotImplementedException
    '    '    End Function

    '    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    '    Public Function Encode(sourceInstance As Byte()) As String
    '    '      Throw New NotImplementedException
    '    '    End Function

    '    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    '    Public Function Encode(sourceInstance As Byte(), encoding As System.Text.Encoding) As String
    '    '      Throw New NotImplementedException
    '    '    End Function

    '#End Region

    '#Region " File Operations "

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Sub LoadEmbeddedFile(ByRef sourceInstance As Byte(), assembly As System.Reflection.Assembly, defaultNamespace As String, fileName As String)



    '    End Sub

    '#End Region

    '#Region " Drawing "

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Function ToImage(sourceInstance As Byte()) As System.Drawing.Image
    '      Throw New NotImplementedException
    '    End Function

    '#End Region

    '#End Region

    '#Region " for Char Array "

    '#Region " Type Conversion "

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Function ToByteArray(sourceInstance As Char()) As Byte()
    '      Throw New NotImplementedException
    '    End Function

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Function ToByteArray(sourceInstance As Char(), encoding As System.Text.Encoding) As Byte()
    '      Throw New NotImplementedException
    '    End Function

    '#End Region

    '#Region " Encoding & Hex "

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Function GetHexString(sourceInstance As Char()) As String
    '      Throw New NotImplementedException
    '    End Function

    '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    Public Function GetHexString(sourceInstance As Char(), encoding As System.Text.Encoding) As String
    '      Throw New NotImplementedException
    '    End Function

    '#End Region

    '#End Region

    '#Region " for Integer "

    '    '<Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    'Public Function SplitToDateTime(sourceInstance As Integer, mode As DateSerializingMode) As DateTime
    '    '  Throw New NotImplementedException
    '    'End Function

    '    '<Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    'Public Function GetHexString(sourceInstance As Integer) As String
    '    '  Throw New NotImplementedException
    '    'End Function

    '#End Region

    '#Region " for Byte "



    '    '<Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    'Public Function GetHexString(sourceInstance As Byte) As String
    '    '  Throw New NotImplementedException
    '    'End Function

    '#End Region

    '#Region " for Exception "

    '#Region " Logging "

    '    '<Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    'Public Function GetWinErrorNumber(ByRef sourceInstance As Exception) As Integer
    '    '  Return System.Runtime.InteropServices.Marshal.GetHRForException(sourceInstance)
    '    'End Function

    '    '<Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    'Public Sub WriteToEventlog(ByRef sourceInstance As Exception)
    '    '  WriteToEventlog(sourceInstance, 0, New String() {})
    '    'End Sub

    '    '<Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    'Public Sub WriteToEventlog(ByRef sourceInstance As Exception, errorNumber As Integer)
    '    '  WriteToEventlog(sourceInstance, errorNumber, New String() {})
    '    'End Sub

    '    '<Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    'Public Sub WriteToEventlog(ByRef sourceInstance As Exception, ParamArray additionalInfoLines() As String)
    '    '  WriteToEventlog(sourceInstance, 0, additionalInfoLines)
    '    'End Sub

    '    '<Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    'Public Sub WriteToEventlog(ByRef sourceInstance As Exception, eventID As Integer, ParamArray additionalInfoLines() As String)
    '    '  Dim SB As New System.Text.StringBuilder
    '    '  Dim ProductName As String
    '    '  ExToString(sourceInstance, SB, additionalInfoLines)
    '    '  Try
    '    '    Debug.WriteLine("LOG EXCEPTION:")
    '    '    Debug.WriteLine(SB.ToString())
    '    '    ProductName = Assembly.GetEntryAssembly().GetAssemblyProduct()
    '    '    System.Diagnostics.EventLog.WriteEntry(ProductName, SB.ToString(), EventLogEntryType.Error, eventID)
    '    '  Catch
    '    '  End Try
    '    'End Sub

    '    'Private Sub ExToString(ex As Exception, SB As System.Text.StringBuilder, ParamArray additionalInfoLines() As String)
    '    '  SB.Append(ex.GetType().Name)
    '    '  SB.Append(": ")
    '    '  SB.AppendLine(ex.Message)
    '    '  If (additionalInfoLines IsNot Nothing) Then
    '    '    If (additionalInfoLines.Length > 0) Then
    '    '      SB.AppendLine("Details:")
    '    '      For Each additionalInfoLine As String In additionalInfoLines
    '    '        SB.AppendLine(additionalInfoLine)
    '    '      Next
    '    '    End If
    '    '  End If
    '    '  SB.Append("StackTrace: ")
    '    '  SB.AppendLine(ex.StackTrace)
    '    '  If (ex.InnerException IsNot Nothing) Then
    '    '    SB.AppendLine("/// inner exception ///")
    '    '    ExToString(ex.InnerException, SB)
    '    '  End If
    '    'End Sub

    '#End Region

    '#Region " Catchblock Bypass "

    '    'Private ThrowUp As Exception = Nothing

    '    'TODO: referenz zu winforms auflösen

    '    '<Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    'Public Function AskForCatch(Of dialogType As {New, Windows.Forms.Form, IExceptionDisplayDialog})(ByRef sourceInstance As Exception, Optional additionalInfo As String = "", Optional ByRef dialogResult As DialogResult = Nothing) As Boolean
    '    '  If ThrowUp IsNot Nothing Then
    '    '    If ThrowUp Is sourceInstance Then
    '    '      Return False
    '    '    End If
    '    '  End If
    '    '  Dim dialogInstance As New dialogType
    '    '  dialogInstance.ExceptionToDisplay = sourceInstance
    '    '  dialogInstance.AdditionalInfo = additionalInfo
    '    '  dialogResult = dialogInstance.ShowDialog()
    '    '  If (dialogInstance.ThowToDebugger) Then
    '    '    ThrowUp = sourceInstance
    '    '    Return False
    '    '  Else
    '    '    Return True
    '    '  End If
    '    'End Function

    '#End Region

    '#End Region



    '    '<Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    'Public Sub WindowMessage(sourceInstance As Process, wMessage As Integer)
    '    '  Throw New NotImplementedException
    '    'End Sub

    '    '<Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    '    'Public Sub WindowMessage(sourceInstance As Process, wMessage As Integer, lMessage As Integer)
    '    '  Throw New NotImplementedException
    '    'End Sub

  End Module

End Namespace
