Imports System

Namespace System.Collections.Generic

  Public Class ThreadSafeQueue(Of T)

    Private _Queue As New Queue(Of T)

    Public Function Dequeue() As T
      SyncLock _Queue
        Return _Queue.Dequeue()
      End SyncLock
    End Function

    Public Sub Enqueue(item As T)
      SyncLock _Queue
        _Queue.Enqueue(item)
      End SyncLock
    End Sub

    Public ReadOnly Property Count As Integer
      Get
        SyncLock _Queue
          Return _Queue.Count
        End SyncLock
      End Get
    End Property

    Public Sub Clear()
      SyncLock _Queue
        _Queue.Clear()
      End SyncLock
    End Sub

  End Class

End Namespace
