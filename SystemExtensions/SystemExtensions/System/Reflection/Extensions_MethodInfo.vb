Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Diagnostics

Public Module ExtensionsForMethodInfo

  <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
  Public Function SignatureMatches(extendee As MethodInfo, ParamArray parameterTypeSignature() As Type) As Boolean
    Dim index As Integer = 0
    Dim parameters = extendee.GetParameters

    If (parameterTypeSignature Is Nothing) Then
      parameterTypeSignature = {}
    End If

    If (Not parameters.Length = parameterTypeSignature.Length) Then
      Return False
    End If

    For Each param In parameters
      If (Not param.ParameterType = parameterTypeSignature(index)) Then
        Return False
      End If
      index += 1
    Next

    Return True
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
  Public Function FilterByName(methodInfos As IEnumerable(Of MethodInfo), methodName As String, Optional ignoreCase As Boolean = True) As IEnumerable(Of MethodInfo)
    If (ignoreCase) Then
      Return From m In methodInfos Where m.Name.ToLower() = methodName.ToLower()
    Else
      Return From m In methodInfos Where m.Name = methodName
    End If
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetMethod(methodInfos As IEnumerable(Of MethodInfo), methodName As String, ParamArray parameterTypeSignature() As Type) As MethodInfo
    Return methodInfos.FilterByName(methodName, True).Where(Function(m) m.SignatureMatches(parameterTypeSignature)).SingleOrDefault()
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetDisplayName(extendee As MethodInfo) As String
    Dim displayNameAttr = extendee.GetCustomAttributes().OfType(Of DisplayNameAttribute)().FirstOrDefault()
    If (displayNameAttr Is Nothing) Then
      Return extendee.Name
    Else
      Return displayNameAttr.DisplayName
    End If
  End Function

End Module
