Imports System.Runtime.CompilerServices
Imports System.ComponentModel
Imports System.Text
Imports System

Namespace System

  Public Class HexString

#Region " Declarations "

    Private Shared _HexAlphabet As Char() = ("0123456789ABCDEF").ToCharArray()

    Private _Bytes As Byte()

#End Region

#Region " Factories & Constructors "

    Public Shared Function EncodeFromPlainString(plainString As String) As HexString
      Return EncodeFromPlainString(plainString, Encoding.Default)
    End Function

    Public Shared Function EncodeFromPlainString(plainString As String, encoder As Encoding) As HexString
      Return New HexString(encoder.GetBytes(plainString))
    End Function

    Public Sub New()
      Me.New(New Byte() {})
    End Sub

    Public Sub New(hexString As String)
      Me.New(ParseHexToBytes(hexString))
    End Sub

    Public Sub New(bytes As Byte())
      _Bytes = bytes
    End Sub

    Protected Sub New(bytesA As Byte(), bytesB As Byte())
      Dim combinedByteCount As Integer = bytesA.Length + bytesB.Length
      Dim bytes(((bytesA.Length + bytesB.Length) - 1)) As Byte

      For i As Integer = 0 To (bytesA.Length - 1)
        bytes(i) = bytesA(i)
      Next
      For i As Integer = 0 To (bytesB.Length - 1)
        bytes(i + bytesA.Length) = bytesB(i)
      Next

      _Bytes = bytes
    End Sub

#End Region

#Region " Properties "

    Public ReadOnly Property Bytes As Byte()
      Get
        Return _Bytes
      End Get
    End Property

#End Region

#Region " Internal Functions "

    Protected Shared Function GetHexLetter(value As Integer) As Char
      Return _HexAlphabet(value Mod 16)
    End Function

    Protected Shared Function GetHexValue(letter As Char) As Byte

      If Char.IsLower(letter) Then
        letter = Char.ToUpper(letter)
      End If

      For i As Integer = 0 To 15
        If (letter = _HexAlphabet(i)) Then
          Return Convert.ToByte(i)
        End If
      Next

      Return 0
    End Function

    Protected Shared Function ParseHexToBytes(hexString As String) As Byte()

      Dim hexChars As Char()
      Dim bytes As Byte()
      Dim hiValue As Integer
      Dim loValue As Integer

      hexChars = hexString.ToUpper().ToCharArray()

      If ((hexChars.Length Mod 2) > 0) Then
        Throw New ArgumentException("The hexString must not have an uneven number of chars.")
      End If

      ReDim bytes((hexChars.Length \ 2) - 1)

      For i As Integer = 0 To (hexChars.Length - 2) Step 2
        hiValue = GetHexValue(hexChars(i)) * 16
        loValue = GetHexValue(hexChars(i + 1))
        bytes(i \ 2) = Convert.ToByte(hiValue + loValue)
      Next

      Return bytes
    End Function

    Protected Sub SetBytes(bytes As Byte())
      _Bytes = bytes
    End Sub

#End Region

#Region " Operations "

    Public Function Append(hexString As String) As HexString
      Return Me.Append(New HexString(hexString))
    End Function

    Public Function Append(hexString As HexString) As HexString
      Return Me.Append(hexString.Bytes)
    End Function

    Public Function Append(bytes As Byte()) As HexString
      Return New HexString(Me.Bytes, bytes)
    End Function

    Public Function Prepend(hexString As String) As HexString
      Return Me.Prepend(New HexString(hexString))
    End Function

    Public Function Prepend(hexString As HexString) As HexString
      Return Me.Prepend(hexString.Bytes)
    End Function

    Public Function Prepend(bytes As Byte()) As HexString
      Return New HexString(bytes, Me.Bytes)
    End Function

    Public Function Left(byteCount As Integer) As HexString
      If (byteCount > Me.Bytes.Length) Then
        byteCount = Me.Bytes.Length
      End If
      Return Me.SubString(0, byteCount)
    End Function

    Public Function Right(byteCount As Integer) As HexString
      If (byteCount > Me.Bytes.Length) Then
        byteCount = Me.Bytes.Length
      End If
      Return Me.SubString(Me.Bytes.Length - byteCount, byteCount)
    End Function

    Public Function SubString(startIndex As Integer, byteCount As Integer) As HexString

      Dim bytesOver As Integer
      Dim newArray() As Byte

      If (startIndex < 0) Then
        Throw New ArgumentException("The startIndex must not be negative.")
      End If
      If (byteCount < 0) Then
        Throw New ArgumentException("The byteCount must not be negative.")
      End If

      bytesOver = Me.Bytes.Length - startIndex
      If (byteCount > bytesOver) Then
        byteCount = bytesOver
      End If

      ReDim newArray(byteCount - 1)

      For i As Integer = 0 To (byteCount - 1)
        newArray(i) = Me.Bytes(startIndex + i)
      Next

      Return New HexString(newArray)
    End Function

    Public Function DecodeToPlainString() As String
      Return Me.DecodeToPlainString(Encoding.Default)
    End Function

    Public Function DecodeToPlainString(decoder As Encoding) As String
      Return decoder.GetString(Me.Bytes)
    End Function

#End Region

#Region " Public Static Helpers "

    <Obsolete("USE EncodeStringToHex")>
    Public Shared Function Hex(plainString As String) As String
      Return HexString.EncodeFromPlainString(plainString).ToString()
    End Function

    <Obsolete("USE EncodeStringToHex")>
    Public Shared Function Hex(plainString As String, encoder As Encoding) As String
      Return HexString.EncodeFromPlainString(plainString, encoder).ToString()
    End Function

    <Obsolete("USE DecodeStringFromHex")>
    Public Shared Function UnHex(hexString As String) As String
      Return New HexString(hexString).DecodeToPlainString()
    End Function

    <Obsolete("USE DecodeStringFromHex")>
    Public Shared Function UnHex(hexString As String, decoder As Encoding) As String
      Return New HexString(hexString).DecodeToPlainString(decoder)
    End Function

    Public Shared Function EncodeStringToHex(plainString As String) As String
      Return HexString.EncodeFromPlainString(plainString).ToString()
    End Function

    Public Shared Function EncodeStringToHex(plainString As String, encoder As Encoding) As String
      Return HexString.EncodeFromPlainString(plainString, encoder).ToString()
    End Function

    Public Shared Function DecodeStringFromHex(hexString As String) As String
      Return New HexString(hexString).DecodeToPlainString()
    End Function

    Public Shared Function DecodeStringFromHex(hexString As String, decoder As Encoding) As String
      Return New HexString(hexString).DecodeToPlainString(decoder)
    End Function

    Public Shared Function EncodeBytesToHex(bytes As Byte()) As String
      Return New HexString(bytes).ToString()
    End Function

    Public Shared Function DecodeBytesFromHex(hexString As String) As Byte()
      Dim inst As New HexString(hexString)
      Return inst.Bytes
    End Function
#End Region

#Region " System "

    <EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Overrides Function ToString() As String
      Dim hexString As New System.Text.StringBuilder

      For i As Integer = 0 To (Bytes.Length - 1)
        hexString.AppendFormat("{0:x2}", Bytes(i))
      Next

      Return hexString.ToString().ToUpper()
    End Function

    <EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Overrides Function GetHashCode() As Integer
      Return Me.ToString().GetHashCode()
    End Function

    <EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Overrides Function Equals(obj As Object) As Boolean

      If (TypeOf (obj) Is String) Then
        Return Me.ToString().Equals(DirectCast(obj, String).ToUpper())
      End If

      If (TypeOf (obj) Is HexString) Then
        Return Me.Equals(DirectCast(obj, HexString).Bytes)
      End If

      If (TypeOf (obj) Is Byte()) Then
        If (DirectCast(obj, Byte()).Length = Me.Bytes.Length) Then
          For i As Integer = 0 To (Bytes.Length - 1)
            If (Not DirectCast(obj, Byte())(i) = Me.Bytes(i)) Then
              Return False
            End If
          Next
          Return True
        Else
          Return False
        End If
      End If

      If (TypeOf (obj) Is String) Then
        Return Me.ToString().Equals(DirectCast(obj, String).ToUpper())
      End If

      Return MyBase.Equals(obj)
    End Function

    Public Shared Operator +(ByVal left As HexString, ByVal right As HexString) As HexString
      Return left.Append(right)
    End Operator

    Public Shared Operator &(ByVal left As HexString, ByVal right As HexString) As HexString
      Return left.Append(right)
    End Operator

    Public Shared Operator =(ByVal left As HexString, ByVal right As HexString) As Boolean
      Return (left.Equals(right) = True)
    End Operator

    Public Shared Operator <>(ByVal left As HexString, ByVal right As HexString) As Boolean
      Return (left.Equals(right) = False)
    End Operator

    Public Shared Operator +(ByVal left As String, ByVal right As HexString) As String
      Return left & right.ToString()
    End Operator

    Public Shared Operator &(ByVal left As String, ByVal right As HexString) As String
      Return left & right.ToString()
    End Operator

    Public Shared Operator =(ByVal left As String, ByVal right As HexString) As Boolean
      Return ((left = right.ToString()) = True)
    End Operator

    Public Shared Operator <>(ByVal left As String, ByVal right As HexString) As Boolean
      Return ((left = right.ToString()) = False)
    End Operator

    Public Shared Operator +(ByVal left As HexString, ByVal right As String) As String
      Return left.ToString() & right
    End Operator

    Public Shared Operator &(ByVal left As HexString, ByVal right As String) As String
      Return left.ToString() & right
    End Operator

    Public Shared Operator =(ByVal left As HexString, ByVal right As String) As Boolean
      Return ((left.ToString() = right) = True)
    End Operator

    Public Shared Operator <>(ByVal left As HexString, ByVal right As String) As Boolean
      Return ((left.ToString() = right) = False)
    End Operator

#End Region

  End Class

End Namespace
