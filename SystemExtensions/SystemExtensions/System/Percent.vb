Imports System.Runtime.CompilerServices
Imports System.ComponentModel
Imports System.Text
Imports System
Imports System.Diagnostics

Namespace System

  <DebuggerDisplay("{ToString()}")>
  Public Class Percentage

    Private _CurrentValue As Double = 0
    Private _MinValue As Double = 0
    Private _MaxValue As Double = 100
    Private _RoundValueTo As Integer = -1
    Private _RoundPercentTo As Integer = -1

    Public Sub New(maxValue As Integer, Optional minValue As Integer = 0, Optional roundValueTo As Integer = -1, Optional roundPercentTo As Integer = -1)
      Me.New(Convert.ToDouble(maxValue), Convert.ToDouble(minValue), roundValueTo, roundPercentTo)
    End Sub

    Public Sub New(maxValue As Double, Optional minValue As Double = 0, Optional roundValueTo As Integer = -1, Optional roundPercentTo As Integer = -1)

      _MaxValue = maxValue
      _MinValue = minValue

      If (_MinValue >= _MaxValue) Then
        Throw New ArgumentException("MaxValue must be more than MinValue")
      End If

      _RoundValueTo = roundValueTo
      _RoundPercentTo = roundPercentTo
      _CurrentValue = _MinValue
    End Sub

    <EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Property MinValue As Double
      Get
        Return _MinValue
      End Get
      Set(value As Double)
        If (value >= _MaxValue) Then
          Throw New ArgumentException("MaxValue must be more than MinValue")
        End If
        _MinValue = value
      End Set
    End Property

    <EditorBrowsable(EditorBrowsableState.Advanced)>
        Public Property MaxValue As Double
      Get
        Return _MaxValue
      End Get
      Set(value As Double)
        If (_MinValue >= value) Then
          Throw New ArgumentException("MaxValue must be more than MinValue")
        End If
        _MaxValue = value

      End Set
    End Property

    Public Property CurrentValue As Double
      Get
        Return _CurrentValue
      End Get
      Set(value As Double)
        _CurrentValue = value
      End Set
    End Property

    <EditorBrowsable(EditorBrowsableState.Advanced)>
    Public ReadOnly Property ValueRange As Double
      Get
        Return (Me.MaxValue - Me.MinValue)
      End Get
    End Property

    <EditorBrowsable(EditorBrowsableState.Advanced)>
    Public ReadOnly Property ValuePerPercent As Double
      Get
        Return (Me.ValueRange / 100)
      End Get
    End Property

    Public Property CurrentPercent As Double
      Get
        Return Me.ValueToPercent(Me.CurrentValue)
      End Get
      Set(value As Double)
        Me.CurrentValue = Me.PercentToValue(value)
      End Set
    End Property

    Public Function ValueToPercent(value As Double) As Double
      If (_RoundPercentTo >= 0) Then
        Return Math.Round(((value - Me.MinValue) / Me.ValuePerPercent), _RoundPercentTo)
      Else
        Return ((value - Me.MinValue) / Me.ValuePerPercent)
      End If
    End Function

    Public Function PercentToValue(percent As Double) As Double
      If (_RoundValueTo >= 0) Then
        Return Math.Round(Me.MinValue + (percent * Me.ValuePerPercent), _RoundValueTo)
      Else
        Return Me.MinValue + (percent * Me.ValuePerPercent)
      End If
    End Function

    <EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Overrides Function ToString() As String
      Return String.Format("{0}% ({1})", Math.Round(Me.CurrentPercent, 2), Math.Round(Me.CurrentValue, 2))
    End Function

  End Class

  <DebuggerDisplay("{ToString()}")>
    Public Class RoundedPercentage
    Inherits Percentage

    Public Sub New(maxValue As Integer, Optional minValue As Integer = 0)
      MyBase.New(maxValue, minValue, 0, 0)
    End Sub

    Public Shadows Property CurrentPercent As Integer
      Get
        Return Convert.ToInt32(MyBase.CurrentPercent)
      End Get
      Set(value As Integer)
        MyBase.CurrentPercent = value
      End Set
    End Property

    Public Shadows Function ValueToPercent(value As Integer) As Integer
      Return Convert.ToInt32(MyBase.ValueToPercent(value))
    End Function

    Public Shadows Function PercentToValue(percent As Integer) As Integer
      Return Convert.ToInt32(MyBase.PercentToValue(percent))
    End Function

    Public Shadows Property CurrentValue As Integer
      Get
        Return Convert.ToInt32(MyBase.CurrentValue)
      End Get
      Set(value As Integer)
        MyBase.CurrentValue = value
      End Set
    End Property
    Public Shadows Property MinValue As Integer
      Get
        Return Convert.ToInt32(MyBase.MinValue)
      End Get
      Set(value As Integer)
        MyBase.MinValue = value
      End Set
    End Property

    Public Shadows Property MaxValue As Integer
      Get
        Return Convert.ToInt32(MyBase.MaxValue)
      End Get
      Set(value As Integer)
        MyBase.MaxValue = value
      End Set
    End Property

    <EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Overrides Function ToString() As String
      Return String.Format("{0}% ({1})", Math.Round(Me.CurrentPercent, 0), Math.Round(Me.CurrentValue, 0))
    End Function

  End Class

End Namespace
