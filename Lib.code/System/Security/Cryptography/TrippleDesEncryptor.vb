Imports System.ComponentModel
Imports System.IO

Namespace System.Security.Cryptography

  Public Class TrippleDesEncryptor

    ''' <summary>
    ''' Generates the default initialisation Vector for 3DES
    ''' </summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Shared Sub Generate3DesInitVector(ByRef targetIV As Byte())
      If (targetIV Is Nothing) Then
        Dim newArray(7) As Byte
        targetIV = newArray
      End If
      For i As Integer = 0 To targetIV.Length - 1
        targetIV(i) = Convert.ToByte(i + 1)
      Next
    End Sub

    ''' <summary>
    ''' Copies bytewise from Source to Target until the exact lenght of the target key is filled.
    ''' If the source key is not long enough, it will be uses multiple times
    ''' </summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Shared Sub Normalize3DesKey(sourceKey As SecureString, ByRef targetKey As Byte())
      If (targetKey Is Nothing) Then
        Dim tk(23) As Byte
        targetKey = tk
      End If
      Dim keyBuffer(targetKey.Length - 1) As Byte

      sourceKey.WriteTo(Sub(buf, off, cnt)
                          Dim iSource As Integer = -1
                          Dim iTarget As Integer = -1
                          While (iTarget < (keyBuffer.Length - 1))
                            iTarget = iTarget + 1
                            iSource = iSource + 1
                            If iSource >= cnt Then
                              iSource = 0
                            End If
                            keyBuffer(iTarget) = buf(iSource)
                          End While
                        End Sub)

      For i As Integer = 0 To keyBuffer.Length - 1
        targetKey(i) = keyBuffer(i)
        keyBuffer(i) = 0
      Next
    End Sub

    Public Shared Function Encrypt(source As Byte(), encryptionKey As SecureString) As Byte()
      Dim key(23) As Byte
      Dim iv(7) As Byte
      Using provider3DES As New TripleDESCryptoServiceProvider
        Try
          Generate3DesInitVector(iv)
          encryptionKey.To3DesKey(key)
          Using buffer As New MemoryStream
            Using encryptTransform As ICryptoTransform = provider3DES.CreateEncryptor(key, iv)
              Using cryptStream As CryptoStream = New CryptoStream(buffer, encryptTransform, CryptoStreamMode.Write)
                cryptStream.Write(source, 0, source.Length)
                Try
                  cryptStream.FlushFinalBlock()
                Catch
                End Try
                Return buffer.ToArray()
              End Using
            End Using
          End Using
        Finally
          For i As Integer = 0 To key.Length - 1
            key(i) = 0
          Next
          key = Nothing
          For i As Integer = 0 To iv.Length - 1
            iv(i) = 0
          Next
          iv = Nothing
        End Try
      End Using
    End Function

    Public Shared Function Decrypt(source As Byte(), decryptionKey As SecureString) As Byte()
      Dim key(23) As Byte
      Dim iv(7) As Byte
      Using provider3DES As New TripleDESCryptoServiceProvider
        Try
          Generate3DesInitVector(iv)
          decryptionKey.To3DesKey(key)
          Using buffer As New MemoryStream
            Using decryptTransform As ICryptoTransform = provider3DES.CreateDecryptor(key, iv)
              Using cryptStream As CryptoStream = New CryptoStream(buffer, decryptTransform, CryptoStreamMode.Write)
                cryptStream.Write(source, 0, source.Length)
                Try
                  cryptStream.FlushFinalBlock()
                Catch
                End Try
                Return buffer.ToArray()
              End Using
            End Using
          End Using
        Finally
        For i As Integer = 0 To key.Length - 1
          key(i) = 0
        Next
        key = Nothing
        For i As Integer = 0 To iv.Length - 1
          iv(i) = 0
        Next
        iv = Nothing
      End Try

      End Using
    End Function

  End Class

End Namespace
