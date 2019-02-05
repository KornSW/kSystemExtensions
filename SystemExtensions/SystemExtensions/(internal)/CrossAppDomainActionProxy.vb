Imports System

#Region " CrossAppdomainActionProxy '0 "

<Serializable>
Friend Class CrossAppDomainActionProxy

  Public Shared Sub Invoke(targetDomain As AppDomain, targetMethod As Action)

    Dim crossDomainCallId As String = Guid.NewGuid.ToString()

    Dim xadActionUntyped As Object =
      targetDomain.CreateInstanceAndUnwrap(
        GetType(CrossAppdomainActionProxy).Assembly.FullName,
        GetType(CrossAppdomainActionProxy).FullName, False,
        Reflection.BindingFlags.CreateInstance,
        Type.DefaultBinder,
        {targetMethod, crossDomainCallId},
        System.Globalization.CultureInfo.CurrentUICulture,
        {}
      )

    Dim xadAction As CrossAppdomainActionProxy = DirectCast(xadActionUntyped, CrossAppdomainActionProxy)
    targetDomain.DoCallBack(New CrossAppDomainDelegate(AddressOf xadAction.CallEntry))

    Dim sharedDataBuffer = targetDomain.GetData(crossDomainCallId)
    If (TypeOf (sharedDataBuffer) Is Exception) Then
      Dim ex = DirectCast(sharedDataBuffer, Exception)
      Throw New Reflection.TargetInvocationException(ex.Message, ex)
    End If

  End Sub

  Private _TargetMethod As Action
  Private _CrossDomainCallId As String

  Public Sub New(targetMethod As Action, crossDomainCallId As String)
    _TargetMethod = targetMethod
    _CrossDomainCallId = crossDomainCallId
  End Sub

  Public Sub CallEntry()
    Try
      _TargetMethod.Invoke()
    Catch ex As Exception
      AppDomain.CurrentDomain.SetData(_CrossDomainCallId, ex)
    End Try
  End Sub

End Class

#End Region

#Region " CrossAppdomainActionProxy '1 "

<Serializable>
Friend Class CrossAppdomainActionProxy(Of TArg1)

  <Serializable>
  Friend Class CrossAppdomainsharedData
    Public Property Arg1 As TArg1
  End Class

  Public Shared Sub Invoke(targetDomain As AppDomain, targetMethod As Action(Of TArg1), arg1 As TArg1)

    Dim crossDomainCallId As String = Guid.NewGuid.ToString()

    Dim sharedData As New CrossAppdomainsharedData
    With sharedData
      .Arg1 = arg1
    End With
    targetDomain.SetData(crossDomainCallId, sharedData)

    Dim xadActionUntyped As Object =
      targetDomain.CreateInstanceAndUnwrap(
        GetType(CrossAppdomainActionProxy(Of TArg1)).Assembly.FullName,
        GetType(CrossAppdomainActionProxy(Of TArg1)).FullName, False,
        Reflection.BindingFlags.CreateInstance,
        Type.DefaultBinder,
        {targetMethod, crossDomainCallId},
        System.Globalization.CultureInfo.CurrentUICulture,
        {}
      )

    Dim xadAction As CrossAppdomainActionProxy(Of TArg1) = DirectCast(xadActionUntyped, CrossAppdomainActionProxy(Of TArg1))
    targetDomain.DoCallBack(New CrossAppDomainDelegate(AddressOf xadAction.CallEntry))

    Dim sharedDataBuffer = targetDomain.GetData(crossDomainCallId)
    If (TypeOf (sharedDataBuffer) Is Exception) Then
      Dim ex = DirectCast(sharedDataBuffer, Exception)
      Throw New Reflection.TargetInvocationException(ex.Message, ex)
    End If

    targetDomain.SetData(crossDomainCallId, Nothing)
  End Sub

  Private _TargetMethod As Action(Of TArg1)
  Private _CrossDomainCallId As String

  Public Sub New(targetMethod As Action(Of TArg1), crossDomainCallId As String)
    _TargetMethod = targetMethod
    _CrossDomainCallId = crossDomainCallId
  End Sub

  Public Sub CallEntry()
    Try
      Dim sharedData As CrossAppdomainsharedData
      sharedData = DirectCast(AppDomain.CurrentDomain.GetData(_CrossDomainCallId), CrossAppdomainsharedData)
      With sharedData
        _TargetMethod.Invoke(.Arg1)
      End With
    Catch ex As Exception
      AppDomain.CurrentDomain.SetData(_CrossDomainCallId, ex)
    End Try
  End Sub

End Class

#End Region

#Region " CrossAppdomainActionProxy '2 "

<Serializable>
Friend Class CrossAppdomainActionProxy(Of TArg1, TArg2)

  <Serializable>
  Friend Class CrossAppdomainsharedData
    Public Property Arg1 As TArg1
    Public Property Arg2 As TArg2
  End Class

  Public Shared Sub Invoke(targetDomain As AppDomain, targetMethod As Action(Of TArg1, TArg2), arg1 As TArg1, arg2 As TArg2)

    Dim crossDomainCallId As String = Guid.NewGuid.ToString()

    Dim sharedData As New CrossAppdomainsharedData
    With sharedData
      .Arg1 = arg1
      .Arg2 = arg2
    End With
    targetDomain.SetData(crossDomainCallId, sharedData)

    Dim xadActionUntyped As Object =
      targetDomain.CreateInstanceAndUnwrap(
        GetType(CrossAppdomainActionProxy(Of TArg1, TArg2)).Assembly.FullName,
        GetType(CrossAppdomainActionProxy(Of TArg1, TArg2)).FullName, False,
        Reflection.BindingFlags.CreateInstance,
        Type.DefaultBinder,
        {targetMethod, crossDomainCallId},
        System.Globalization.CultureInfo.CurrentUICulture,
        {}
      )

    Dim xadAction As CrossAppdomainActionProxy(Of TArg1, TArg2) = DirectCast(xadActionUntyped, CrossAppdomainActionProxy(Of TArg1, TArg2))
    targetDomain.DoCallBack(New CrossAppDomainDelegate(AddressOf xadAction.CallEntry))

    Dim sharedDataBuffer = targetDomain.GetData(crossDomainCallId)
    If (TypeOf (sharedDataBuffer) Is Exception) Then
      Dim ex = DirectCast(sharedDataBuffer, Exception)
      Throw New Reflection.TargetInvocationException(ex.Message, ex)
    End If

    targetDomain.SetData(crossDomainCallId, Nothing)
  End Sub

  Private _TargetMethod As Action(Of TArg1, TArg2)
  Private _CrossDomainCallId As String

  Public Sub New(targetMethod As Action(Of TArg1, TArg2), crossDomainCallId As String)
    _TargetMethod = targetMethod
    _CrossDomainCallId = crossDomainCallId
  End Sub

  Public Sub CallEntry()
    Try
      Dim sharedData As CrossAppdomainsharedData
      sharedData = DirectCast(AppDomain.CurrentDomain.GetData(_CrossDomainCallId), CrossAppdomainsharedData)
      With sharedData
        _TargetMethod.Invoke(.Arg1, .Arg2)
      End With
    Catch ex As Exception
      AppDomain.CurrentDomain.SetData(_CrossDomainCallId, ex)
    End Try
  End Sub

End Class

#End Region
