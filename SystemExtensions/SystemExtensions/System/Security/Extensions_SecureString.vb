Option Strict On
Option Explicit On

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.IO
Imports System.Linq
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Security
Imports System.Text
Imports System.Security.Cryptography

Namespace System.Security

  Public Module ExtensionsForSecureString

#Region " Compare "

    ''' <summary>
    ''' Compares a SecureString to another
    ''' </summary>
    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function CompareTo(oneSecureString As SecureString, anotherSecureString As SecureString) As Boolean
      Return GetMD5Hash(oneSecureString) = GetMD5Hash(anotherSecureString)
    End Function

    ''' <summary>
    ''' Compares a SecureString to another
    ''' </summary>
    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function CompareTo(oneSecureString As SecureString, anotherString As String) As Boolean
      Return GetMD5Hash(oneSecureString) = GetMD5Hash(anotherString)
    End Function

#End Region

#Region " Initialize "

    ''' <summary>
    ''' Appends a String to the end of the current SecureString
    ''' </summary>
    ''' <remarks >THIS IS AN UNSAFE METHOD!</remarks>
    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Sub AppendString(secureString As SecureString, ByRef sourceString As String)
      secureString.AppendBytes(Encoding.Default.GetBytes(sourceString))
    End Sub

    ''' <summary>
    ''' Appends a multiple Characters to the end of the current SecureString
    ''' </summary>
    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Sub AppendChars(secureString As SecureString, ParamArray chars() As Char)
      For i As Integer = 0 To chars.Length - 1
        secureString.AppendChar(chars(i))
        chars(i) = Char.MaxValue
      Next
    End Sub

    ''' <summary>
    ''' Appends a multiple Bytes to the end of the current SecureString
    ''' </summary>
    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Sub AppendBytes(secureString As SecureString, ParamArray bytes() As Byte)
      Dim chars As Char() = System.Text.Encoding.Default.GetChars(bytes)
      For i As Integer = 0 To chars.Length - 1
        secureString.AppendChar(chars(i))
        chars(i) = Char.MaxValue
      Next
    End Sub

#End Region

#Region " Extract "

    ''' <summary>
    ''' Extracts the content of the current SecureString to an normal insecure String variable.
    ''' </summary>
    ''' <remarks >THIS IS AN UNSAFE METHOD!</remarks>
    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Sub ExtractTo(secureString As SecureString, ByRef targetString As String)
      Dim unmanagedStringPointer As IntPtr = IntPtr.Zero
      Try
        unmanagedStringPointer = Marshal.SecureStringToGlobalAllocUnicode(secureString)
        targetString = Marshal.PtrToStringUni(unmanagedStringPointer)
      Finally
        Marshal.ZeroFreeGlobalAllocUnicode(unmanagedStringPointer)
      End Try
    End Sub

    ''' <summary>
    ''' Extracts the content of the current SecureString to an normal insecure String variable.
    ''' </summary>
    ''' <remarks >THIS IS AN UNSAFE METHOD!</remarks>
    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function GetExtractedString(secureString As SecureString) As String
      Dim targetString As String = Nothing
      secureString.ExtractTo(targetString)
      Return targetString
    End Function

#End Region

#Region " 3DES "

    ''' <summary>
    ''' Encrypts the Content of the SecureString using 3DES and writes the encrypted String into the target String variable.
    ''' </summary>
    ''' <param name="secureString">Source SecureString Handle</param>
    ''' <param name="encryptionKey">The 3DES Key which will be used to encrypt the data. If not specified a Key will be automatically generated using the Fingerprint of the Assembly which contains the calling Method.</param>
    ''' <param name="useHex">If set to False, the encrypted String will be encoded in 'Base64' format instead of 'Hex'</param>
    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetEncryptedString(secureString As SecureString, Optional encryptionKey As SecureString = Nothing, Optional useHex As Boolean = True) As String
      Dim encStr As String = Nothing
      secureString.EncryptTo(encStr, encryptionKey, useHex)
      Return encStr
    End Function

    ''' <summary>
    ''' Encrypts the Content of the SecureString using 3DES and writes the encrypted String into the target String variable.
    ''' </summary>
    ''' <param name="secureString">Source SecureString Handle</param>
    ''' <param name="targetString">Target String variable to which the encrpted String will be written</param>
    ''' <param name="encryptionKey">The 3DES Key which will be used to encrypt the data. If not specified a Key will be automatically generated using the Fingerprint of the Assembly which contains the calling Method.</param>
    ''' <param name="useHex">If set to False, the encrypted String will be encoded in 'Base64' format instead of 'Hex'</param>
    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub EncryptTo(secureString As SecureString, ByRef targetString As String, Optional encryptionKey As SecureString = Nothing, Optional useHex As Boolean = True)

      Dim output() As Byte = {}
      Dim key(23) As Byte
      Dim iv(7) As Byte
      Dim streamLen As Integer

      Using provider3DES As New TripleDESCryptoServiceProvider
        Try

          TrippleDesEncryptor.Generate3DesInitVector(iv)
          If (encryptionKey Is Nothing OrElse encryptionKey.Length < 1) Then
            To3DesKey(GetAssemblyFingerPrint(Reflection.Assembly.GetCallingAssembly()), key)
          Else
            To3DesKey(encryptionKey, key)
          End If

          Using encryptTransform As ICryptoTransform = provider3DES.CreateEncryptor(key, iv)
            Using memStream As MemoryStream = New MemoryStream
              Using cryptStream As CryptoStream = New CryptoStream(memStream, encryptTransform, CryptoStreamMode.Write)

                secureString.WriteToStream(cryptStream)
                Try
                  cryptStream.FlushFinalBlock()
                Catch
                End Try

                memStream.Position = 0
                streamLen = Convert.ToInt32(memStream.Length)
                ReDim output(streamLen - 1)
                memStream.Read(output, 0, streamLen)

                memStream.Close()
                cryptStream.Close()

                If (useHex) Then
                  targetString = EncodeHex(output)
                Else
                  targetString = EncodeB64(output)
                End If

              End Using
            End Using
          End Using

        Finally
          OverwriteArray(output)
          OverwriteArray(key)
          OverwriteArray(iv)
        End Try
      End Using
    End Sub

    ''' <summary>
    ''' Decrypts a encrypted String using 3DES and writes the Content into the SecureString.
    ''' </summary>
    ''' <param name="secureString">Target SecureString Handle</param>
    ''' <param name="encryptedSourceString">Source String variable from which the encrpted String will be read</param>
    ''' <param name="decryptionKey">The 3DES Key which will be used to decrypt the data. If not specified a Key will be automatically generated using the Fingerprint of the Assembly which contains the calling Method.</param>
    ''' <param name="useHex">If set to False, the encrypted String will be expected in 'Base64' format instead of 'Hex'</param>
    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub DecryptFrom(secureString As SecureString, encryptedSourceString As String, Optional decryptionKey As SecureString = Nothing, Optional useHex As Boolean = True)

      Dim input() As Byte = {}
      Dim key(23) As Byte
      Dim iv(7) As Byte
      Dim buff() As Byte = {}

      Using provider3DES As New TripleDESCryptoServiceProvider
        Try

          If (useHex) Then
            input = DecodeHex(encryptedSourceString)
          Else
            input = DecodeB64(encryptedSourceString)
          End If

          TrippleDesEncryptor.Generate3DesInitVector(iv)
          If (decryptionKey Is Nothing OrElse decryptionKey.Length < 1) Then
            To3DesKey(GetAssemblyFingerPrint(Reflection.Assembly.GetCallingAssembly()), key)
          Else
            To3DesKey(decryptionKey, key)
          End If

          ReDim buff(input.Length - 1)

          Using decryptTransform As ICryptoTransform = provider3DES.CreateDecryptor(key, iv)
            Using memStream As MemoryStream = New MemoryStream(input.Length)
              Using cryptStream As CryptoStream = New CryptoStream(memStream, decryptTransform, CryptoStreamMode.Write)

                cryptStream.Write(input, 0, input.Length)
                Try
                  cryptStream.FlushFinalBlock()
                Catch
                End Try

                memStream.Position = 0
                memStream.Read(buff, 0, buff.Length)

                secureString.Clear()
                Dim chars = Encoding.Default.GetChars(buff)
                For i As Integer = 0 To chars.Length - 1
                  secureString.AppendChar(chars(i))
                  chars(i) = Char.MaxValue
                Next

                memStream.Close()
                cryptStream.Close()

              End Using
            End Using
          End Using

        Finally
          OverwriteArray(buff)
          OverwriteArray(input)
          OverwriteArray(key)
          OverwriteArray(iv)
        End Try

      End Using
    End Sub

    ''' <summary>
    ''' Copies bytewise from Source to Target until the exact lenght of the target key is filled.
    ''' If the source key is not long enough, it will be uses multiple times
    ''' </summary>
    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Sub To3DesKey(sourceKey As SecureString, ByRef targetKey As Byte())
      TrippleDesEncryptor.Normalize3DesKey(sourceKey, targetKey)
    End Sub

#End Region

#Region " MD5 "

    ''' <summary>
    ''' Generates the MD5 Hash of the Content contained in the
    ''' SecureString without creating any unsafe String handles
    ''' </summary>
    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetMD5Hash(secureString As SecureString) As String
      Dim result As Byte() = {}

      Using MD5 As New MD5CryptoServiceProvider
        Try
          secureString.WriteTo(Sub(buf, off, cnt)
                                 result = MD5.ComputeHash(buf, off, cnt)
                               End Sub)

          Return EncodeHex(result)

        Finally
          OverwriteArray(result)
        End Try
      End Using

    End Function

    ''' <summary>
    ''' Generates the MD5 Hash of a String without
    ''' creating any additional String handles.
    ''' </summary>
    Public Function GetMD5Hash(ByRef planeString As String) As String

      Dim MD5 As New MD5CryptoServiceProvider
      Dim result As Byte() = {}
      Dim planeBytes As Byte() = Nothing

      Try

        planeBytes = Encoding.ASCII.GetBytes(planeString)
        result = MD5.ComputeHash(planeBytes)

        Return EncodeHex(result)

      Finally
        For i As Integer = 0 To (result.Length - 1)
          result(i) = 0
        Next
        MD5.Dispose()
        MD5 = Nothing
        OverwriteArray(planeBytes)
      End Try

    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub Append(extendee As SecureString, another As SecureString)
      another.WriteTo(Sub(buf, off, cnt)
                        For Each c As Char In System.Text.Encoding.Unicode.GetChars(buf)
                          extendee.AppendChar(c)
                        Next
                      End Sub)
    End Sub

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub GetMD5Hash(extendee As SecureString, appendTo As SecureString)
      Dim result As Byte() = {}

      Using MD5 As New MD5CryptoServiceProvider
        Try
          extendee.WriteTo(Sub(buf, off, cnt)
                             result = MD5.ComputeHash(buf, off, cnt)
                           End Sub)
          For Each b As Byte In result
            For Each c As Char In String.Format("{0:x2}", b)
              appendTo.AppendChar(c)
            Next
          Next
        Finally
          OverwriteArray(result)
        End Try
      End Using
    End Sub

#End Region

#Region " Stream IO "

    <EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Delegate Sub SecureByteReceiver(buffer As Byte(), offset As Integer, count As Integer)

    Private _LockObj As New Object

    ''' <summary>
    ''' Pushes the Content of an SecureString to a Bytewise processing method
    ''' without creating any unsafe String handles
    ''' </summary>
    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Sub WriteTo(secureString As SecureString, receiver As SecureByteReceiver)
      SyncLock _LockObj
        Dim len As Integer
        Dim unmanagedByteArray As IntPtr = IntPtr.Zero
        Dim byteArray() As Byte = {}

        len = secureString.Length
        If (len > 0) Then
          Try
            Try

              ReDim byteArray(len - 1)
              unmanagedByteArray = Marshal.SecureStringToGlobalAllocAnsi(secureString)
              Marshal.Copy(unmanagedByteArray, byteArray, 0, len)

            Finally
              'Marshal.ZeroFreeGlobalAllocUnicode(unmanagedByteArray)
              Marshal.ZeroFreeGlobalAllocAnsi(unmanagedByteArray)
            End Try

            receiver.Invoke(byteArray, 0, len)

          Finally
            OverwriteArray(byteArray)
          End Try

        End If
      End SyncLock
    End Sub

    ''' <summary>
    ''' Writes the Content of an SecureString to an Stream
    ''' without creating any unsafe String handles
    ''' </summary>
    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Sub WriteToStream(secureString As SecureString, targetStream As Stream)
      secureString.WriteTo(AddressOf targetStream.Write)
      targetStream.Flush()
    End Sub

    ''' <summary>
    ''' Reads the Content from an Stream and writes it to an SecureString
    ''' without creating any unsafe String handles
    ''' </summary>
    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Sub ReadFromStream(secureString As SecureString, sourceStream As Stream)
      Dim sr As New BinaryReader(sourceStream)
      Dim currentChar As Char

      secureString.Clear()
      Try

        If (sourceStream.Length > 0) Then
          Do
            currentChar = sr.ReadChar()
            secureString.AppendChar(currentChar)
          Loop Until (sourceStream.Position = sourceStream.Length)
        End If

      Finally
        currentChar = Char.MaxValue
        currentChar = Nothing
      End Try

    End Sub

#End Region

#Region " HEX & B64 Encoding "

    ''' <summary>
    ''' Encodes an Byte Array to an Hexadecimal String
    ''' </summary>
    Private Function EncodeHex(bytes() As Byte) As String
      Dim hex As New StringBuilder(bytes.Length * 2)

      For Each b As Byte In bytes
        hex.AppendFormat("{0:x2}", b)
      Next

      Return hex.ToString()
    End Function

    ''' <summary>
    ''' Decodes an Byte Array from an Hexadecimal String
    ''' </summary>
    Private Function DecodeHex(hexString As String) As Byte()
      Dim len As Integer = hexString.Length
      Dim bytes(Convert.ToInt32((len / 2) - 1)) As Byte

      For i As Integer = 0 To len - 1 Step 2
        bytes(Convert.ToInt32(i / 2)) = Convert.ToByte(hexString.Substring(i, 2), 16)
      Next

      Return bytes

    End Function

    ''' <summary>
    ''' Encodes an Byte Array to an Base64 String
    ''' </summary>
    Private Function EncodeB64(bytes() As Byte) As String
      Return Convert.ToBase64String(bytes)
    End Function

    ''' <summary>
    ''' Decodes an Byte Array from an Base64 String
    ''' </summary>
    Private Function DecodeB64(b64String As String) As Byte()
      Return Convert.FromBase64String(b64String)
    End Function

#End Region

#Region " Helpers "

    ''' <summary>
    ''' Overwrited all fields of an Byte Array with zeroes
    ''' </summary>
    Private Sub OverwriteArray(ByRef array As Byte())
      For i As Integer = 0 To array.Length - 1
        array(i) = 0
      Next
      array = Nothing
    End Sub

    ''' <summary>
    ''' Generates an unique Fingerprint for an Assembly using the PublicKeyToken
    ''' </summary>
    Private Function GetAssemblyFingerPrint(assembly As Reflection.Assembly) As SecureString
      Dim fingerPrint As New SecureString
      Dim basePharse As New SecureString
      basePharse.AppendBytes(18, 10, 84)

      Try

        Dim pk() As Byte = assembly.GetName().GetPublicKeyToken().Reverse().ToArray()
        If (pk IsNot Nothing) Then
          basePharse.AppendBytes(pk)

          fingerPrint.AppendBytes(Encoding.ASCII.GetBytes(basePharse.GetMD5Hash()))
          Return fingerPrint

        End If

      Catch
      End Try

      basePharse.AppendBytes(Encoding.ASCII.GetBytes(assembly.GetName.Name))

      fingerPrint.AppendBytes(Encoding.ASCII.GetBytes(basePharse.GetMD5Hash()))
      Return fingerPrint
    End Function

#End Region

  End Module

  '  Public Module ExtensionsForSecureString

  '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  '    Public Function MD5(extendee As SecureString, Optional phrase As SecureString = Nothing) As HexString

  '      Throw New NotImplementedException

  '    End Function

  '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  '    Public Function GetLength(extendee As SecureString) As Integer

  '      Throw New NotImplementedException

  '    End Function

  '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  '    Public Function Unlock(extendee As SecureString) As String

  '      Throw New NotImplementedException

  '    End Function

  '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  '    Public Function Clone(extendee As SecureString) As SecureString

  '      Throw New NotImplementedException

  '    End Function

  '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  '    Public Function CompareTo(extendee As SecureString, otherString As SecureString) As Boolean
  '      Dim phrase As SecureString = Guid.NewGuid().ToString().ToSecureString()
  '      Return extendee.MD5(phrase) = otherString.MD5(phrase)
  '    End Function

  '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  '    Public Function CompareTo(extendee As SecureString, otherString As String) As Boolean
  '      Dim phrase As SecureString = Guid.NewGuid().ToString().ToSecureString()
  '      Return extendee.MD5(phrase) = otherString.MD5(phrase)
  '    End Function

  '    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  '    Public Function EditVia(extendee As SecureString, editMethod As Action(Of SecureStringEditor)) As SecureString
  '      Dim newString As SecureString = extendee.Clone()
  '      Using editor As New SecureStringEditor(newString)
  '        editMethod.Invoke(editor)
  '      End Using
  '      Return newString
  '    End Function

  '  End Module

  '  <CLSCompliant(False)>
  '  Public NotInheritable Class SecureStringEditor
  '    Implements IDisposable
  '    Implements IEnumerable(Of Char)

  '    ' from http://netvignettes.wordpress.com/2011/05/09/how-to-use-securestring/

  '    Private _secureString As SecureString
  '    Private _gcHandle As GCHandle

  '    Friend Sub New(secureString As SecureString)
  '      _secureString = secureString
  '      Me.Initialize()
  '    End Sub

  '    Public Value As String

  '#If Not DEBUG Then
  '        <DebuggerHidden>
  '#End If
  '    Private Sub Initialize()

  '      ''Unsafe ???

  '      ''We are about to create an unencrypted version of our sensitive string and store it in memory. 
  '      ''Don't let anyone (GC) make a copy. 
  '      ''To do this, create a new gc handle so we can "pin" the memory. 
  '      ''The gc handle will be pinned and later, we will put info in this string. 
  '      'Dim _gcHandle As New GCHandle()

  '      ''insecurePointer will be temporarily used to access the SecureString 
  '      'Dim insecurePointer As IntPtr = IntPtr.Zero

  '      '  Dim code as        System.Runtime.CompilerServices.RuntimeHelpers.TryCode  = delegate
  '      '          { 
  '      ''create a new string of appropriate length that is filled with 0's 
  '      '              Value = new string((char)0, _secureString.Length); 
  '      ''Even though we are in the ExecuteCodeWithGuaranteedCleanup, processing can be interupted. 
  '      ''We need to make sure nothing happens between when memory is allocated and 
  '      ''when _gcHandle has been assigned the value. Otherwise, we can't cleanup later. 
  '      ''PrepareConstrainedRegions is better than a try/catch. Not even a threadexception will interupt this processing. 
  '      ''A CER is not the same as ExecuteCodeWithGuaranteedCleanup. A CER does not have a cleanup. 

  '      '              Action alloc = delegate { _gcHandle = GCHandle.Alloc(Value, GCHandleType.Pinned); }; 
  '      '              alloc.ExecuteInConstrainedRegion(); 

  '      ''Even though we are in the ExecuteCodeWithGuaranteedCleanup, processing can be interupted. 
  '      ''We need to make sure nothing happens between when memory is allocated and 
  '      ''when insecurePointer has been assigned the value. Otherwise, we can't cleanup later. 
  '      ''PrepareConstrainedRegions is better than a try/catch. Not even a threadexception will interupt this processing. 
  '      ''A CER is not the same as ExecuteCodeWithGuaranteedCleanup. A CER does not have a cleanup. 
  '      '              Action toBSTR = delegate { insecurePointer = Marshal.SecureStringToBSTR(_secureString); }; 
  '      '              toBSTR.ExecuteInConstrainedRegion(); 

  '      ''get a pointer to our new "pinned" string 
  '      '              var value = (char*)_gcHandle.AddrOfPinnedObject(); 
  '      ''get a pointer to the unencrypted string 
  '      '              var charPointer = (char*)insecurePointer; 
  '      ''copy 
  '      '              for (int i = 0; i < _secureString.Length; i++) 
  '      '              { 
  '      '                  value[i] = charPointer[i]; 
  '      '              } 
  '      '          }; 



  '      '      Dim cleanup as    System.Runtime.CompilerServices.RuntimeHelpers.CleanupCode  = delegate
  '      '          { 
  '      '  'insecurePointer was temporarily used to access the securestring 
  '      '  'set the string to all 0's and then clean it up. this is important. 
  '      '  'this prevents sniffers from seeing the sensitive info as it is cleaned up. 
  '      '              if (insecurePointer != IntPtr.Zero) 
  '      '              { 
  '      '                  Marshal.ZeroFreeBSTR(insecurePointer); 
  '      '              } 
  '      '          }; 

  '      '    'Better than a try/catch. Not even a threadexception will bypass the cleanup code 
  '      '    RuntimeHelpers.ExecuteCodeWithGuaranteedCleanup(code, cleanup, null)

  '      '    'Unsafe ende ???
  '    End Sub

  '#If Not DEBUG Then
  '        <DebuggerHidden>
  '#End If
  '    Public Sub Dispose() Implements IDisposable.Dispose

  '      '      'Unsafe ???

  '      '      'we have created an insecurestring 
  '      '      If (_gcHandle.IsAllocated) Then

  '      '        'get the address of our gchandle and set all chars to 0's 
  '      '                    var insecurePointer = (char*)_gcHandle.AddrOfPinnedObject(); 
  '      '                    for (int i = 0; i < _secureString.Length; i++) 
  '      '                    { 
  '      '                        insecurePointer[i] = (char)0; 
  '      '                    } 
  '      '#If DEBUG Then
  '      '                    var disposed = "¡DISPOSED¡"; 
  '      '                    disposed = disposed.Substring(0, Math.Min(disposed.Length, _secureString.Length)); 
  '      '                    for (int i = 0; i < disposed.Length; ++i) 
  '      '                    { 
  '      '                        insecurePointer[i] = disposed[i]; 
  '      '                    } 
  '      '#End If
  '      '                    _gcHandle.Free(); 
  '      '               end if
  '      '      'Unsafe ende ???
  '    End Sub

  '    Public Function GetEnumerator() As IEnumerator(Of Char) Implements IEnumerable(Of Char).GetEnumerator
  '      ''Unsafe ???
  '      'If (_gcHandle.IsAllocated) Then
  '      '  Return Value.GetEnumerator()
  '      'Else
  '      '  Return EmptyEnumerable(Of Char).Instance.GetEnumerator()
  '      'End If
  '      ''Unsafe ende ???



  '      Throw New NotImplementedException
  '    End Function

  '    Public Function GetUntypedEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
  '      Return GetEnumerator()
  '    End Function

  '  End Class

End Namespace
