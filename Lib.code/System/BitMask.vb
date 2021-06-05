Imports System.Runtime.CompilerServices
Imports System.Collections.Generic
Imports System.Collections
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Text
Imports System

Namespace System

  ''' <summary>
  ''' Represents an array of binary encoded boolean flags,
  ''' which are accessable using the bit index.
  ''' </summary>
  <DebuggerDisplay("BitMask ({ToString()})")>
  Public Class BitMask

#Region " Declarations "

    ' NOTE: We can use this class in 'Bound' oder 'Unbound' mode.
    '  'Bound':   the DecimalValue will be stored external,
    '             and any access to Bitmask which have effect to
    '             DecimalValue will be routed in realtime using
    '             the getter and setter delegates.
    '  'Unbound': the DecimalValue will be stored in the local
    '             '_UnboundDecimalValue'.

    ''' <summary>Used to get the DecimalValue (especially in "Bound' mode, where it is stored external)</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> Public Delegate Function GetterMethod() As Long
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> Private _Getter As GetterMethod

    ''' <summary>Used to set the DecimalValue (especially in "Bound' mode, where it is stored external)</summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)> Public Delegate Sub SetterMethod(newValue As Long)
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> Private _Setter As SetterMethod

    <DebuggerBrowsable(DebuggerBrowsableState.Never)>
    Private _BitCount As Integer

    ''' <summary>Stores the DecimalValue if we are running in "Unbound' mode</summary>
    <DebuggerBrowsable(DebuggerBrowsableState.Never)>
    Private _UnboundDecimalValue As Long = 0

#End Region

#Region " Constructors "

    Public Sub New(bitCount As Integer)
      Me.New(bitCount, Nothing, Nothing)
    End Sub

    Public Sub New(bitCount As Integer, getterForDirectBinding As GetterMethod, setterForDirectBinding As SetterMethod)

      If (bitCount < 1 Or 63 < bitCount) Then
        Throw New ArgumentException("The bitCount must be in the range from 1 to 63 (singned Long).", "bitCount")
      Else
        _BitCount = bitCount
      End If

      If (getterForDirectBinding IsNot Nothing AndAlso setterForDirectBinding IsNot Nothing) Then
        _Getter = getterForDirectBinding
        _Setter = setterForDirectBinding
      Else
        _Getter = Function() _UnboundDecimalValue
        _Setter = Sub(v) _UnboundDecimalValue = v
      End If

    End Sub

#End Region

#Region " Properties "

    ''' <param name="indexFromRight">Zero based Index from the Right side of the Binary Value</param>
    <DebuggerBrowsable(DebuggerBrowsableState.Never)>
    Default Public Property BitField(indexFromRight As Integer) As Boolean
      Get
        Dim bits() As Boolean
        Dim indexFromLeft As Integer

        bits = DecimalNumberToBitArray(Me.DecimalValue, _BitCount)
        indexFromLeft = (bits.Length - 1 - indexFromRight)

        Return bits(indexFromLeft)
      End Get
      Set(value As Boolean)
        Dim bits() As Boolean
        Dim indexFromLeft As Integer

        bits = DecimalNumberToBitArray(Me.DecimalValue, _BitCount)
        indexFromLeft = (bits.Length - 1 - indexFromRight)

        If (Not bits(indexFromLeft) = value) Then
          bits(indexFromLeft) = value
          Me.DecimalValue = BitArrayToDecimalNumber(bits)
        End If
      End Set
    End Property

    Public Property DecimalValue As Long
      Get
        Return _Getter.Invoke
      End Get
      Set(value As Long)
        _Setter.Invoke(value)
      End Set
    End Property

    <EditorBrowsable(EditorBrowsableState.Advanced)>
    Public ReadOnly Property BinaryValue As Long
      Get
        Dim value As Long = 0

        For i As Integer = (_BitCount - 1) To 1 Step -1
          If (i > 15) Then
            Return -1
          End If
          If (Me.BitField(i)) Then
            value = value + 1
          End If
          value = value * 10
        Next

        If (Me.BitField(0)) Then
          value = value + 1
        End If

        Return value
      End Get
    End Property

    <EditorBrowsable(EditorBrowsableState.Advanced), DebuggerBrowsable(DebuggerBrowsableState.Never)>
    Public ReadOnly Property BitCount As Integer
      Get
        Return _BitCount
      End Get
    End Property

#End Region

#Region " Helpers "

    Protected Shared Function DecimalNumberToBitArray(decimalNumber As Long, Optional minimumArrayLength As Integer = 1) As Boolean()
      Dim arrayUBound As Integer = -1
      Dim maxValue As Long = 0
      Dim bits() As Boolean
      Dim currentBitValue As Long

      Do 'calculate the array size
        arrayUBound = arrayUBound + 1
        maxValue = maxValue + Convert.ToInt64(2 ^ arrayUBound)
      Loop Until (maxValue >= decimalNumber AndAlso arrayUBound >= (minimumArrayLength - 1))

      ReDim bits(arrayUBound)

      For indexFromLeft As Integer = 0 To arrayUBound
        If (decimalNumber > 0) Then

          currentBitValue = Convert.ToInt64(2 ^ (arrayUBound - indexFromLeft))

          bits(indexFromLeft) = (decimalNumber \ currentBitValue) > 0
          decimalNumber = decimalNumber Mod currentBitValue
        Else
          bits(indexFromLeft) = False
        End If
      Next

      Return bits
    End Function

    Protected Shared Function BitArrayToDecimalNumber(bits As Boolean()) As Long
      Dim value As Long = 0
      Dim bitCount As Integer = bits.Length
      Dim indexFromLeft As Integer

      For indexFromRight As Integer = 0 To (bitCount - 1)
        indexFromLeft = bitCount - 1 - indexFromRight
        If (bits(indexFromLeft)) Then
          value = value + Convert.ToInt64(2 ^ indexFromRight)
        End If
      Next

      Return value
    End Function

    ''' <summary>
    ''' Searches the zero based bit index for a decimal number.
    ''' </summary>
    ''' <param name="number">Decimal number for wich the bit index should be searched</param>
    ''' <returns>Sample: 1>0, 2>1, 4>2, 8>3, 16>4. For all invalid numbers (which have more than one bit) the method will return -1.</returns>
    Protected Shared Function GetBitIndex(number As Integer) As Integer
      Dim currentIndex As Integer = 0

      Do Until ((2 ^ currentIndex) = number)
        If ((2 ^ currentIndex) > number) Then
          Return -1
        Else
          currentIndex = currentIndex + 1
        End If
      Loop

      Return currentIndex
    End Function

#End Region

#Region " System "

    <EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Overrides Function ToString() As String
      Dim binStr As New StringBuilder

      For i As Integer = (_BitCount - 1) To 0 Step -1
        If (Me.BitField(i)) Then
          binStr.Append("x"c)
        Else
          binStr.Append("-"c)
        End If
      Next

      Return binStr.ToString()
    End Function

#End Region

  End Class

  ''' <summary>
  ''' Represents an array of binary encoded boolean flags,
  ''' which are accessable using the fieldnames of an enumeration type (generic param).
  ''' </summary>
  <DebuggerDisplay("BitMask ({ToString()})")>
  Public Class BitMask(Of EnumType)
    Inherits BitMask

#Region " Factory & Constructors "

    Public Shared Function FromDecimalValue(ByRef decimalValue As Long) As BitMask(Of EnumType)
      Dim newInstance As BitMask(Of EnumType)
      newInstance = New BitMask(Of EnumType)
      newInstance.DecimalValue = decimalValue
      Return newInstance
    End Function

    Public Sub New(getterForDirectBinding As GetterMethod, setterForDirectBinding As SetterMethod)
      MyBase.new(EvaluateBitCount(GetType(EnumType)), getterForDirectBinding, setterForDirectBinding)
    End Sub

    Public Sub New()
      MyBase.new(EvaluateBitCount(GetType(EnumType)))
    End Sub

    Public Sub New(getterForDirectBinding As GetterMethod, setterForDirectBinding As SetterMethod, ParamArray enableBits() As EnumType)
      Me.New(getterForDirectBinding, setterForDirectBinding)
      For Each bitToEnable As EnumType In enableBits
        Me.BitField(bitToEnable) = True
      Next
    End Sub

    Public Sub New(ParamArray enableBits() As EnumType)
      Me.New()
      For Each bitToEnable As EnumType In enableBits
        Me.BitField(bitToEnable) = True
      Next
    End Sub

#End Region

#Region " Enum Evaluation (Compatibility check) "

    <DebuggerBrowsable(DebuggerBrowsableState.Never)>
    Protected Shared BitCountOfEnumType As Integer = -1

    Protected Shared Function EvaluateBitCount(enumType As Type) As Integer

      If (BitCountOfEnumType > 0) Then
        Return BitCountOfEnumType
      End If

      Dim bitCount As Integer = 0

      If (Not enumType.IsEnum) Then
        Throw New ArgumentException(String.Format("The type '{0}', specified for the generic type parameter 'enumType' is not an Enum! ", enumType.Name), "enumType")
      End If

      For Each enumValue As Object In System.Enum.GetValues(GetType(EnumType))

        Dim enumValueInt As Integer = DirectCast(enumValue, Integer)
        Dim bitIndex As Integer = GetBitIndex(enumValueInt)

        If (bitIndex < 0) Then
          Throw New ArgumentException(String.Format("The enum type '{0}' cannot be used for a bit mask, because the value {1} (field '{2}') is not binary value! All fields of the enum type have to be represented by numbers based on 2^x.", enumType.Name, enumValueInt, [Enum].GetName(GetType(EnumType), enumValue)), "enumType")
        ElseIf (bitCount < bitIndex + 1) Then
          bitCount = bitIndex + 1
        End If

      Next

      BitCountOfEnumType = bitCount
      Return bitCount
    End Function

#End Region

#Region " Properties (wrappers for the EnumType) "

    <DebuggerBrowsable(DebuggerBrowsableState.Never)>
    Default Public Overloads Property BitField(field As EnumType) As Boolean
      Get
        Return MyBase.BitField(GetBitIndex(DirectCast(DirectCast(field, Object), Integer)))
      End Get
      Set(value As Boolean)
        MyBase.BitField(GetBitIndex(DirectCast(DirectCast(field, Object), Integer))) = value
      End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.RootHidden)>
    Public ReadOnly Property EnabledBits() As IEnumerable(Of EnumType)
      Get
        Return Me.FilterByState(True)
      End Get
    End Property

    Public ReadOnly Property DisabledBits() As IEnumerable(Of EnumType)
      Get
        Return Me.FilterByState(False)
      End Get
    End Property

#End Region

#Region " Helpers "

    Private Function FilterByState(active As Boolean) As IEnumerable(Of EnumType)
      Dim result As New List(Of EnumType)
      For Each enumValue As Object In System.Enum.GetValues(GetType(EnumType))
        If (Me.BitField(DirectCast(enumValue, EnumType)) = active) Then
          result.Add(DirectCast(enumValue, EnumType))
        End If
      Next
      Return result
    End Function

#End Region

#Region " System "

    <EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Overrides Function ToString() As String
      Dim binStr As New StringBuilder
      Dim enumValues() As EnumType = DirectCast(System.Enum.GetValues(GetType(EnumType)), EnumType())
      Dim enumValue As EnumType
      For i As Integer = (enumValues.Length - 1) To 0 Step -1
        enumValue = DirectCast(enumValues(i), EnumType)

        If (Me.BitField(enumValue)) Then
          If (binStr.Length > 0) Then
            binStr.Append(", ")
          End If
          binStr.Append(System.Enum.GetName(GetType(EnumType), enumValue))
        End If
      Next

      Return binStr.ToString()
    End Function

#End Region

  End Class

End Namespace

#Region " Sample "

'Private Property MyMaskValue As Long = 7632
'
'Private _MyMask As BitMask(Of MyEnum) = Nothing
'Private ReadOnly Property MyMask As BitMask(Of MyEnum)
'  Get
'    If (_MyMask Is Nothing) Then
'      _MyMask = New BitMask(Of MyEnum)(Function() MyMaskValue, Sub(v) MyMaskValue = v)
'    End If
'    Return _MyMask
'  End Get
'End Property

#End Region
