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
Imports Microsoft.Win32
Imports System.Security

Public Module ExtensionsForRegistryKey

#Region " Dictionarys "

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Sub GetIntegers(key As RegistryKey, target As Dictionary(Of String, Integer))
    key.ForEachValue(
      Sub(name)
        Dim buffer = key.GetValue(name)
        If (TypeOf (buffer) Is Integer) Then
          target(name) = DirectCast(buffer, Integer)
        End If
      End Sub)
  End Sub

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Sub SetIntegers(key As RegistryKey, source As Dictionary(Of String, Integer))
    For Each keyName As String In source.Keys
      key.SetInteger(keyName, source(keyName))
    Next
  End Sub

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Sub GetStrings(key As RegistryKey, target As Dictionary(Of String, String))
    key.ForEachValue(
      Sub(name)
        Dim buffer = key.GetValue(name)
        If (TypeOf (buffer) Is String) Then
          target(name) = DirectCast(buffer, String)
        End If
      End Sub)
  End Sub

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Sub SetStrings(key As RegistryKey, source As Dictionary(Of String, String))
    For Each keyName As String In source.Keys
      key.SetString(keyName, source(keyName))
    Next
  End Sub

#End Region

#Region " Contains "

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function ContainsValue(key As RegistryKey, valueName As String) As Boolean
    Dim names As String() = key.GetValueNames()
    For Each name As String In names
      If (name.ToLower() = valueName.ToLower()) Then
        Return True
      End If
    Next
    Return False
  End Function

#End Region

#Region " ForEachValue "

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Sub ForEachValue(Of TValue)(parentKey As Microsoft.Win32.RegistryKey, action As Action(Of String, Microsoft.Win32.RegistryValueKind, Func(Of TValue, TValue), Action(Of TValue)))
    Dim valueNames As String() = parentKey.GetValueNames()
    For Each valueName In valueNames

      action.Invoke(
        valueName,
        parentKey.GetValueKind(valueName),
        Function([default])
          Return DirectCast(parentKey.GetValue(valueName, [default]), TValue)
        End Function,
        Sub(newValue)
          parentKey.SetValue(valueName, newValue)
        End Sub)

    Next
  End Sub

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Sub ForEachValue(key As RegistryKey, action As Action(Of String))
    Dim names As String() = key.GetValueNames()
    For Each name As String In names
      action.Invoke(name)
    Next
  End Sub

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Sub ForEachValue(key As RegistryKey, pattern As String, action As Action(Of String), Optional ignoreCasing As Boolean = True)
    Dim names As String() = key.GetValueNames()
    For Each name As String In names
      If (name.MatchesWildcardMask(pattern, ignoreCasing)) Then
        action.Invoke(name)
      End If
    Next
  End Sub

#End Region

#Region " ForEachSubkey "

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Sub ForEachSubkey(key As RegistryKey, action As Action(Of RegistryKey))
    Dim names As String() = key.GetSubKeyNames()
    For Each name As String In names
      Using subKey As RegistryKey = key.OpenSubKey(name)
        action.Invoke(subKey)
        subKey.Close()
      End Using
    Next
  End Sub

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Sub ForEachSubkey(key As RegistryKey, openWritable As Boolean, action As Action(Of RegistryKey))
    Dim names As String() = key.GetSubKeyNames()
    For Each name As String In names
      Using subKey As RegistryKey = key.OpenSubKey(name, openWritable)
        action.Invoke(subKey)
        subKey.Close()
      End Using
    Next
  End Sub

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Sub ForEachSubkey(key As RegistryKey, pattern As String, action As Action(Of RegistryKey), Optional ignoreCasing As Boolean = True)
    Dim names As String() = key.GetSubKeyNames()
    For Each name As String In names
      If (name.MatchesWildcardMask(pattern, ignoreCasing)) Then
        Using subKey As RegistryKey = key.OpenSubKey(name)
          action.Invoke(subKey)
          subKey.Close()
        End Using
      End If
    Next
  End Sub

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Sub ForEachSubkey(key As RegistryKey, pattern As String, openWritable As Boolean, action As Action(Of RegistryKey), Optional ignoreCasing As Boolean = True)
    Dim names As String() = key.GetSubKeyNames()
    For Each name As String In names
      If (name.MatchesWildcardMask(pattern, ignoreCasing)) Then
        Using subKey As RegistryKey = key.OpenSubKey(name, openWritable)
          action.Invoke(subKey)
          subKey.Close()
        End Using
      End If
    Next
  End Sub

#End Region

#Region " DeleteAllValues "

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Sub DeleteAllValues(key As RegistryKey)
    For Each valueName As String In key.GetValueNames()
      key.DeleteValue(valueName)
    Next
  End Sub

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Sub DeleteAllValues(key As RegistryKey, pattern As String, Optional ignoreCasing As Boolean = True)
    For Each valueName As String In key.GetValueNames()
      If (valueName.MatchesWildcardMask(pattern, ignoreCasing)) Then
        key.DeleteValue(valueName)
      End If
    Next
  End Sub

#End Region

#Region " GetValue (Generic) "

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetValue(Of TValue)(parentKey As Microsoft.Win32.RegistryKey, name As String, [default] As TValue) As TValue
    Return DirectCast(parentKey.GetValue(name, [default]), TValue)
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetValue(Of TValue)(parentKey As Microsoft.Win32.RegistryKey, name As String) As TValue
    Return DirectCast(parentKey.GetValue(name), TValue)
  End Function

#End Region

#Region " String "

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetString(key As RegistryKey, name As String) As String
    Return key.GetString(name, String.Empty)
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetString(key As RegistryKey, name As String, defaultValue As String) As String
    Return key.GetValue(name, defaultValue).ToString()
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Sub SetString(key As RegistryKey, name As String, value As String)
    key.SetValue(name, value)
  End Sub

#End Region

#Region " ByteArray "

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetByteArray(key As RegistryKey, name As String) As Byte()
    '##################################
    '  TODO: IMPLEMENT THIS METHOD !!!
    Throw New NotImplementedException()
    '##################################
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetByteArray(key As RegistryKey, name As String, defaultValue As Byte()) As Byte()
    '##################################
    '  TODO: IMPLEMENT THIS METHOD !!!
    Throw New NotImplementedException()
    '##################################
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Sub SetByteArray(key As RegistryKey, name As String, value As Byte())
    '##################################
    '  TODO: IMPLEMENT THIS METHOD !!!
    Throw New NotImplementedException()
    '##################################
  End Sub

#End Region

#Region " SecureString "

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetSecureString(key As RegistryKey, name As String) As SecureString
    Dim encryptedString As String = key.GetString(name, Nothing)
    If (encryptedString Is Nothing) Then
      Return Nothing
    End If
    Dim s As New SecureString
    s.DecryptFrom(encryptedString, Assembly.GetCallingAssembly().GetFingerPrint(), True)
    Return s
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetSecureString(key As RegistryKey, name As String, defaultValue As SecureString) As SecureString
    Dim encryptedString As String = key.GetString(name, Nothing)
    If (encryptedString Is Nothing) Then
      Return defaultValue
    End If
    Dim s As New SecureString
    s.DecryptFrom(encryptedString, Assembly.GetCallingAssembly().GetFingerPrint(), True)
    Return s
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetSecureString(key As RegistryKey, name As String, defaultValue As SecureString, encKey As SecureString) As SecureString
    Dim encryptedString As String = key.GetString(name, Nothing)
    If (encryptedString Is Nothing) Then
      Return defaultValue
    End If
    Dim s As New SecureString
    s.DecryptFrom(encryptedString, encKey, True)
    Return s
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Sub SetSecureString(key As RegistryKey, name As String, value As SecureString)
    Dim encryptedString As String = String.Empty
    value.EncryptTo(encryptedString, Assembly.GetCallingAssembly().GetFingerPrint(), True)
    key.SetString(name, encryptedString)
  End Sub

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Sub SetSecureString(key As RegistryKey, name As String, value As SecureString, encKey As SecureString)
    Dim encryptedString As String = String.Empty
    value.EncryptTo(encryptedString, encKey, True)
    key.SetString(name, encryptedString)
  End Sub

#End Region

#Region " Decimal "

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetDecimal(key As RegistryKey, name As String) As Decimal
    Return key.GetDecimal(name, 0)
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetDecimal(key As RegistryKey, name As String, defaultValue As Decimal) As Decimal
    Dim ret As String = key.GetString(name, Nothing)
    If (ret Is Nothing) Then
      Return defaultValue
    Else
      Return ret.ToDecimal()
    End If
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Sub SetDecimal(key As RegistryKey, name As String, value As Decimal)
    key.SetString(name, value.ToString())
  End Sub

#End Region

#Region " Integer "

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetInteger(key As RegistryKey, name As String) As Integer
    Return key.GetInteger(name, 0)
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetInteger(key As RegistryKey, name As String, defaultValue As Integer) As Integer
    Return DirectCast(key.GetValue(name, defaultValue), Integer)
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Sub SetInteger(key As RegistryKey, name As String, value As Integer)
    key.SetValue(name, value)
  End Sub

#End Region

#Region " Boolean "

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetBoolean(key As RegistryKey, name As String) As Boolean
    Return key.GetBoolean(name, False)
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetBoolean(key As RegistryKey, name As String, defaultValue As Boolean) As Boolean
    Dim ret As String = key.GetValue(name, Nothing).ToString()
    If (ret Is Nothing) Then
      Return defaultValue
    Else
      Return ret.ToBoolean()
    End If
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Sub SetBoolean(key As RegistryKey, name As String, value As Boolean)
    key.SetString(name, value.ToInteger().ToString())
  End Sub

#End Region

#Region " Guid "

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetGuid(key As RegistryKey, name As String) As Guid
    Return key.GetGuid(name, Guid.Empty)
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetGuid(key As RegistryKey, name As String, defaultValue As Guid) As Guid
    '##################################
    '  TODO: IMPLEMENT THIS METHOD !!!
    Throw New NotImplementedException()
    '##################################
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Sub SetGuid(key As RegistryKey, name As String, value As Guid)
    '##################################
    '  TODO: IMPLEMENT THIS METHOD !!!
    Throw New NotImplementedException()
    '##################################
  End Sub

#End Region

#Region " Version "

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetVersion(key As RegistryKey, name As String) As Version
    Return key.GetVersion(name, New Version(0, 0, 0, 0))
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetVersion(key As RegistryKey, name As String, defaultValue As Version) As Version
    '##################################
    '  TODO: IMPLEMENT THIS METHOD !!!
    Throw New NotImplementedException()
    '##################################
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Sub SetVersion(key As RegistryKey, name As String, value As Version)
    '##################################
    '  TODO: IMPLEMENT THIS METHOD !!!
    Throw New NotImplementedException()
    '##################################
  End Sub

#End Region

#Region " Date / DateTime "

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetDate(key As RegistryKey, name As String) As Date
    '##################################
    '  TODO: IMPLEMENT THIS METHOD !!!
    Throw New NotImplementedException()
    '##################################
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetDate(key As RegistryKey, name As String, defaultValue As Date) As Date
    '##################################
    '  TODO: IMPLEMENT THIS METHOD !!!
    Throw New NotImplementedException()
    '##################################
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Sub SetDate(key As RegistryKey, name As String, value As Date)
    '##################################
    '  TODO: IMPLEMENT THIS METHOD !!!
    Throw New NotImplementedException()
    '##################################
  End Sub

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetDateTime(key As RegistryKey, name As String) As DateTime
    '##################################
    '  TODO: IMPLEMENT THIS METHOD !!!
    Throw New NotImplementedException()
    '##################################
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetDateTime(key As RegistryKey, name As String, defaultValue As Date) As DateTime
    '##################################
    '  TODO: IMPLEMENT THIS METHOD !!!
    Throw New NotImplementedException()
    '##################################
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Sub SetDateTime(key As RegistryKey, name As String, value As DateTime)
    '##################################
    '  TODO: IMPLEMENT THIS METHOD !!!
    Throw New NotImplementedException()
    '##################################
  End Sub

#End Region

End Module
