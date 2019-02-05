Imports System

#Region " CrossAppdomainFuncProxy '0 "

<Serializable>
Friend Class CrossAppdomainFuncProxy(Of TReturn)

  <Serializable>
  Friend Class CrossAppdomainsharedData
    Public Property Result As TReturn
  End Class

  Public Shared Function Invoke(targetDomain As AppDomain, targetMethod As Func(Of TReturn)) As TReturn

    Dim crossDomainCallId As String = Guid.NewGuid.ToString()

    Dim sharedData As New CrossAppdomainsharedData
    targetDomain.SetData(crossDomainCallId, sharedData)

    Dim xadActionUntyped As Object =
      targetDomain.CreateInstanceAndUnwrap(
        GetType(CrossAppdomainFuncProxy(Of TReturn)).Assembly.FullName,
        GetType(CrossAppdomainFuncProxy(Of TReturn)).FullName, False,
        Reflection.BindingFlags.CreateInstance,
        Type.DefaultBinder,
        {targetMethod, crossDomainCallId},
        System.Globalization.CultureInfo.CurrentUICulture,
        {}
      )

    Dim xadAction As CrossAppdomainFuncProxy(Of TReturn) = DirectCast(xadActionUntyped, CrossAppdomainFuncProxy(Of TReturn))
    targetDomain.DoCallBack(New CrossAppDomainDelegate(AddressOf xadAction.CallEntry))

    Dim sharedDataBuffer = targetDomain.GetData(crossDomainCallId)
    If (TypeOf (sharedDataBuffer) Is Exception) Then
      Dim ex = DirectCast(sharedDataBuffer, Exception)
      Throw New Reflection.TargetInvocationException(ex.Message, ex)
    End If
    sharedData = DirectCast(sharedDataBuffer, CrossAppdomainsharedData)

    targetDomain.SetData(crossDomainCallId, Nothing)
    Return sharedData.Result
  End Function

  Private _TargetMethod As Func(Of TReturn)
  Private _CrossDomainCallId As String

  Public Sub New(targetMethod As Func(Of TReturn), crossDomainCallId As String)
    _TargetMethod = targetMethod
    _CrossDomainCallId = crossDomainCallId
  End Sub

  Public Sub CallEntry()
    Try
      Dim sharedData As CrossAppdomainsharedData
      sharedData = DirectCast(AppDomain.CurrentDomain.GetData(_CrossDomainCallId), CrossAppdomainsharedData)
      With sharedData
        .Result = _TargetMethod.Invoke()
      End With
      AppDomain.CurrentDomain.SetData(_CrossDomainCallId, sharedData)
    Catch ex As Exception
      AppDomain.CurrentDomain.SetData(_CrossDomainCallId, ex)
    End Try
  End Sub

End Class

#End Region

#Region " CrossAppdomainFuncProxy '1 "

<Serializable>
Friend Class CrossAppdomainFuncProxy(Of TArg1, TReturn)

  <Serializable>
  Friend Class CrossAppdomainsharedData
    Public Property Arg1 As TArg1
    Public Property Result As TReturn
  End Class

  Public Shared Function Invoke(targetDomain As AppDomain, targetMethod As Func(Of TArg1, TReturn), arg1 As TArg1) As TReturn

    Dim crossDomainCallId As String = Guid.NewGuid.ToString()

    Dim sharedData As New CrossAppdomainsharedData
    With sharedData
      .Arg1 = arg1
    End With
    targetDomain.SetData(crossDomainCallId, sharedData)

    Dim xadActionUntyped As Object =
      targetDomain.CreateInstanceAndUnwrap(
        GetType(CrossAppdomainFuncProxy(Of TArg1, TReturn)).Assembly.FullName,
        GetType(CrossAppdomainFuncProxy(Of TArg1, TReturn)).FullName, False,
        Reflection.BindingFlags.CreateInstance,
        Type.DefaultBinder,
        {targetMethod, crossDomainCallId},
        System.Globalization.CultureInfo.CurrentUICulture,
        {}
      )

    Dim xadAction As CrossAppdomainFuncProxy(Of TArg1, TReturn) = DirectCast(xadActionUntyped, CrossAppdomainFuncProxy(Of TArg1, TReturn))
    targetDomain.DoCallBack(New CrossAppDomainDelegate(AddressOf xadAction.CallEntry))

    Dim sharedDataBuffer = targetDomain.GetData(crossDomainCallId)
    If (TypeOf (sharedDataBuffer) Is Exception) Then
      Dim ex = DirectCast(sharedDataBuffer, Exception)
      Throw New Reflection.TargetInvocationException(ex.Message, ex)
    End If
    sharedData = DirectCast(sharedDataBuffer, CrossAppdomainsharedData)

    targetDomain.SetData(crossDomainCallId, Nothing)
    Return sharedData.Result
  End Function

  Private _TargetMethod As Func(Of TArg1, TReturn)
  Private _CrossDomainCallId As String

  Public Sub New(targetMethod As Func(Of TArg1, TReturn), crossDomainCallId As String)
    _TargetMethod = targetMethod
    _CrossDomainCallId = crossDomainCallId
  End Sub

  Public Sub CallEntry()
    Try
      Dim sharedData As CrossAppdomainsharedData
      sharedData = DirectCast(AppDomain.CurrentDomain.GetData(_CrossDomainCallId), CrossAppdomainsharedData)
      With sharedData
        .Result = _TargetMethod.Invoke(.Arg1)
      End With
      AppDomain.CurrentDomain.SetData(_CrossDomainCallId, sharedData)
    Catch ex As Exception
      AppDomain.CurrentDomain.SetData(_CrossDomainCallId, ex)
    End Try
  End Sub

End Class

#End Region

#Region " CrossAppdomainFuncProxy '2 "

<Serializable>
Friend Class CrossAppdomainFuncProxy(Of TArg1, TArg2, TReturn)

  <Serializable>
  Friend Class CrossAppdomainsharedData
    Public Property Arg1 As TArg1
    Public Property Arg2 As TArg2
    Public Property Result As TReturn
  End Class

  Public Shared Function Invoke(targetDomain As AppDomain, targetMethod As Func(Of TArg1, TArg2, TReturn), arg1 As TArg1, arg2 As TArg2) As TReturn

    Dim crossDomainCallId As String = Guid.NewGuid.ToString()

    Dim sharedData As New CrossAppdomainsharedData
    With sharedData
      .Arg1 = arg1
      .Arg2 = arg2
    End With
    targetDomain.SetData(crossDomainCallId, sharedData)

    Dim xadActionUntyped As Object =
      targetDomain.CreateInstanceAndUnwrap(
        GetType(CrossAppdomainFuncProxy(Of TArg1, TArg2, TReturn)).Assembly.FullName,
        GetType(CrossAppdomainFuncProxy(Of TArg1, TArg2, TReturn)).FullName, False,
        Reflection.BindingFlags.CreateInstance,
        Type.DefaultBinder,
        {targetMethod, crossDomainCallId},
        System.Globalization.CultureInfo.CurrentUICulture,
        {}
      )

    Dim xadAction As CrossAppdomainFuncProxy(Of TArg1, TArg2, TReturn) = DirectCast(xadActionUntyped, CrossAppdomainFuncProxy(Of TArg1, TArg2, TReturn))
    targetDomain.DoCallBack(New CrossAppDomainDelegate(AddressOf xadAction.CallEntry))

    Dim sharedDataBuffer = targetDomain.GetData(crossDomainCallId)
    If (TypeOf (sharedDataBuffer) Is Exception) Then
      Dim ex = DirectCast(sharedDataBuffer, Exception)
      Throw New Reflection.TargetInvocationException(ex.Message, ex)
    End If
    sharedData = DirectCast(sharedDataBuffer, CrossAppdomainsharedData)

    targetDomain.SetData(crossDomainCallId, Nothing)
    Return sharedData.Result
  End Function

  Private _TargetMethod As Func(Of TArg1, TArg2, TReturn)
  Private _CrossDomainCallId As String

  Public Sub New(targetMethod As Func(Of TArg1, TArg2, TReturn), crossDomainCallId As String)
    _TargetMethod = targetMethod
    _CrossDomainCallId = crossDomainCallId
  End Sub

  Public Sub CallEntry()
    Try
      Dim sharedData As CrossAppdomainsharedData
      sharedData = DirectCast(AppDomain.CurrentDomain.GetData(_CrossDomainCallId), CrossAppdomainsharedData)
      With sharedData
        .Result = _TargetMethod.Invoke(.Arg1, .Arg2)
      End With
      AppDomain.CurrentDomain.SetData(_CrossDomainCallId, sharedData)
    Catch ex As Exception
      AppDomain.CurrentDomain.SetData(_CrossDomainCallId, ex)
    End Try
  End Sub

End Class

#End Region
