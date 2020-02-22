Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Text

Namespace System

  Public Class Uid64

    Private Sub New()
    End Sub

    Private Shared _Random As Random = New Random()
    Private Shared _CurrentTimeFrame As Long
    Private Shared _RandomsOfCurrentTimeFrame As New HashSet(Of Integer)

    Public Shared Function NewUid() As Long
      SyncLock _Random

        Dim currentDate = DateTime.UtcNow

        If (Not currentDate.Ticks = _CurrentTimeFrame) Then
          _RandomsOfCurrentTimeFrame.Clear()
        End If

        Dim elapsedMilliseconds As Long = currentDate.Ticks \ 10000 - #01/01/1900#.Ticks \ 10000
        If (elapsedMilliseconds >= 17592186044416) Then
          Throw New Exception("Time stamp overflow") 'in year ~2457
        End If

        elapsedMilliseconds = elapsedMilliseconds << 19

        Dim rndInt As Integer
        Dim randomValueAlreadyUsedInCurrentTimeFrame As Boolean

        Do
          rndInt = _Random.Next(524287)
          randomValueAlreadyUsedInCurrentTimeFrame = Not _RandomsOfCurrentTimeFrame.Add(rndInt)
        Loop While randomValueAlreadyUsedInCurrentTimeFrame


        Dim rndLong As Long = rndInt
        Dim uid As Long = (elapsedMilliseconds Or rndLong)

        _CurrentTimeFrame = currentDate.Ticks

        Return uid
      End SyncLock
    End Function

    Public Shared Function NewUidInBase32() As String
      Return Base32Encoding.LongToBase32String(Uid64.NewUid)
    End Function

    Public Shared Function ExtractDateTime(b64Uid As Long) As DateTime

      If (b64Uid < 0) Then
        b64Uid = -b64Uid
      End If

      Dim decodedMilliseconds As Long = (b64Uid >> 19)
      Dim decodedTicks As Long = decodedMilliseconds * 10000
      Dim ts As New TimeSpan(decodedTicks)
      Dim decodedDate = New Date(#01/01/1900#.Ticks + decodedTicks)

      Return decodedDate
    End Function

  End Class

End Namespace
