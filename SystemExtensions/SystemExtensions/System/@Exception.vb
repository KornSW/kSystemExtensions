Imports System.Runtime.CompilerServices
Imports System.ComponentModel
Imports System.Linq
Imports System.Text
Imports System
Imports System.Reflection
Imports System.Diagnostics

Namespace System

  Public Module ExtensionsForException

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToFullString(ex As Exception, Optional includeStacktrace As Boolean = True) As String
      Dim sb As New StringBuilder()
      ex.AppendTo(sb, includeStacktrace)
      Return sb.ToString()
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Sub AppendTo(ex As Exception, target As StringBuilder, Optional includeStacktrace As Boolean = True)
      If (ex Is Nothing) Then
        Exit Sub
      End If

      'typeinfo and message
      target.AppendLine($"Exception (Type: {ex.GetType().Namespace}.{ex.GetType().Name} ,HR: {ex.GetHResult})")
      target.AppendLine(ex.Message)

      'stacktrace
      If (includeStacktrace) Then
        target.AppendLine("StackTrace:")
        If (ex.StackTrace Is Nothing) Then
          target.AppendLine("[not available]")
        Else
          target.AppendLine(ex.StackTrace.Replace("   bei", "@" & Environment.NewLine & "call:").Replace(" in ", Environment.NewLine & "file: ").Replace(":Zeile", Environment.NewLine & "line:"))
        End If
      End If

      Try
        'specific details for well known exception types
        Select Case True

          Case TypeOf ex Is ArgumentException
            target.AppendLine($"ParamName: {DirectCast(ex, ArgumentException).ParamName}")

        End Select
      Catch
      End Try

      'inner exceptions
      If (ex.InnerException IsNot Nothing) Then
        target.AppendLine()
        target.AppendLine("################################################################################")
        target.AppendLine()
        target.Append("Inner ")
        ex.InnerException.AppendTo(target, includeStacktrace)
      End If

    End Sub

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function GetHResult(ByRef sourceInstance As Exception) As Integer
      Dim int As Integer = System.Runtime.InteropServices.Marshal.GetHRForException(sourceInstance)
      If (int < 0) Then
        Return int '- Int16.MinValue
      Else
        Return int
      End If
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Sub WriteToEventlog(ex As Exception, Optional sourceName As String = Nothing, Optional includeStacktrace As Boolean = True)
      If (sourceName.IsNullOrWhiteSpace()) Then
        sourceName = System.Diagnostics.Process.GetCurrentProcess().ProcessName.Replace(".vshost", String.Empty)
      End If
      System.Diagnostics.EventLog.WriteEntry(sourceName, ex.ToFullString(includeStacktrace), EventLogEntryType.Error)
    End Sub

  End Module

End Namespace
