Imports System.Diagnostics

Namespace System

  Public Interface IFactory
    Inherits IDisposable
    Function CreateNew() As Object
    Function CreateNew(ParamArray args() As Object) As Object
  End Interface

  Public Class Factory(Of TInstance)
    Implements IFactory

    Private _FactoryMethod As Func(Of Object(), TInstance) = Nothing

    Public Sub New(factoryMethod As Func(Of TInstance))
      _FactoryMethod = Function(args As Object()) factoryMethod.Invoke()
    End Sub

    Public Sub New(factoryMethod As Func(Of Object(), TInstance))
      _FactoryMethod = factoryMethod
    End Sub

    Public Function CreateNew() As TInstance
      Return _FactoryMethod.Invoke({})
    End Function
    Public Function CreateNew(ParamArray args() As Object) As TInstance
      Return _FactoryMethod.Invoke(args)
    End Function

    Private Function CreateNewUntyped() As Object Implements IFactory.CreateNew
      Return _FactoryMethod.Invoke({})
    End Function
    Private Function CreateNewUntyped(ParamArray args() As Object) As Object Implements IFactory.CreateNew
      Return _FactoryMethod.Invoke(args)
    End Function

#Region " IDisposable "

    <DebuggerBrowsable(DebuggerBrowsableState.Never)>
    Private _AlreadyDisposed As Boolean = False

    ''' <summary>
    ''' Dispose the current object instance
    ''' </summary>
    Protected Overridable Sub Dispose(disposing As Boolean)
      If (Not _AlreadyDisposed) Then
        If (disposing) Then
        End If
        _AlreadyDisposed = True
      End If
    End Sub

    ''' <summary>
    ''' Dispose the current object instance and suppress the finalizer
    ''' </summary>
    Private Sub Dispose() Implements IDisposable.Dispose
      Me.Dispose(True)
      GC.SuppressFinalize(Me)
    End Sub

#End Region


  End Class

  Public Class DefaultConstructorFactory(Of TInstance As New)
    Inherits Factory(Of TInstance)

    Public Sub New()
      MyBase.New(Function() New TInstance)
    End Sub

  End Class

  Public Class Factory(Of TInstance, TArgument)
    Inherits Factory(Of TInstance)

    Public Sub New(factoryMethod As Func(Of TArgument, TInstance))
      MyBase.New(Function(args As Object()) factoryMethod.Invoke(DirectCast(args(0), TArgument)))
    End Sub

    Public Shadows Function CreateNew(argument As TArgument) As TInstance
      Return MyBase.CreateNew(argument)
    End Function

  End Class

End Namespace
