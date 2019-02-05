Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Diagnostics

Namespace System.Collections.Generic

  Public Class EnumerableProxy(Of TSource, TTarget)
    Implements IEnumerable(Of TTarget)

    <DebuggerBrowsable(DebuggerBrowsableState.Never)>
    Private _Source As IEnumerable(Of TSource)
    <DebuggerBrowsable(DebuggerBrowsableState.Never)>
    Private _ElementProxyMethod As Func(Of TSource, TTarget)

    Public Sub New(source As IEnumerable(Of TSource), action As Func(Of TSource, TTarget))
      _Source = source
      _ElementProxyMethod = action
    End Sub

    Public Function GetEnumerator() As IEnumerator(Of TTarget) Implements IEnumerable(Of TTarget).GetEnumerator
      Return New EnumeratorProxy(_Source.GetEnumerator(), _ElementProxyMethod)
    End Function

    Private Function GetEnumeratorUntyped() As IEnumerator Implements IEnumerable.GetEnumerator
      Return New EnumeratorProxy(_Source.GetEnumerator(), _ElementProxyMethod)
    End Function

    Private Class EnumeratorProxy
      Implements IEnumerator(Of TTarget)

      Private _Source As IEnumerator(Of TSource)
      Private _ElementProxyMethod As Func(Of TSource, TTarget)

      Public Sub New(source As IEnumerator(Of TSource), action As Func(Of TSource, TTarget))
        _Source = source
        _ElementProxyMethod = action
      End Sub

      Public ReadOnly Property Current As TTarget Implements IEnumerator(Of TTarget).Current
        Get
          Return _ElementProxyMethod.Invoke(_Source.Current())
        End Get
      End Property

      Private ReadOnly Property CurrentUntyped As Object Implements IEnumerator.Current
        Get
          Return _ElementProxyMethod.Invoke(_Source.Current())
        End Get
      End Property

      Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
        Return _Source.MoveNext()
      End Function

      Public Sub Reset() Implements IEnumerator.Reset
        _Source.Reset()
      End Sub

#Region " IDisposable "

      <DebuggerBrowsable(DebuggerBrowsableState.Never)>
      Private _AlreadyDisposed As Boolean = False

      ''' <summary>
      ''' Dispose the current object instance
      ''' </summary>
      Protected Overridable Sub Dispose(disposing As Boolean)
        If (Not _AlreadyDisposed) Then
          If (disposing) Then
            _Source.Dispose()
          End If
          _AlreadyDisposed = True
        End If
      End Sub

      ''' <summary>
      ''' Dispose the current object instance and suppress the finalizer
      ''' </summary>
      Public Sub Dispose() Implements IDisposable.Dispose
        Me.Dispose(True)
        GC.SuppressFinalize(Me)
      End Sub

#End Region

    End Class

  End Class

End Namespace
