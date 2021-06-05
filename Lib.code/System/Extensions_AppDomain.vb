Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Collections.Generic

Namespace System

  Public Module ExtensionsForAppDomain

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function LocateType(appDomain As AppDomain, fullTypeName As String) As Type
      Return Type.GetType(
        fullTypeName,
        Function(assName As AssemblyName)
          Return (From a In appDomain.GetAssemblies() Where a.GetName.Name = assName.Name).FirstOrDefault()
        End Function,
        Function(ass, typeName, ignoreCase)
          Dim found As Type
          If (ass IsNot Nothing) Then
            found = ass.LocateType(typeName)
            If (found IsNot Nothing) Then
              Return found
            End If
          End If
          For Each a In appDomain.GetAssemblies
            found = a.LocateType(typeName)
            If (found IsNot Nothing) Then
              Return found
            End If
          Next
          Return Nothing
        End Function,
        False,
        False)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function LocateType(assembly As Assembly, fullTypeName As String) As Type
      Return assembly.GetType(fullTypeName, False, True)
    End Function

#If NET461 Then
    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ConfigurationFile(dom As AppDomain) As FileInfo
      Return New FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile)
    End Function
#End If

#Region " Type Resolving "

    ''' <summary>
    ''' This method adds the current assembly to a list of targets which
    ''' will be iterated tru when the current appdomain is trying to resolve
    ''' a typename into a System.Type.
    ''' </summary>
    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub AddToTypeResolver(extendee As Assembly)
      If (Not _HandlerRegistered) Then
        AddHandler AppDomain.CurrentDomain.TypeResolve, AddressOf ResolveType
        _HandlerRegistered = True
      End If
      If (Not _TypeResolvingTargets.Contains(extendee)) Then
        _TypeResolvingTargets.Add(extendee)
      End If
    End Sub

    ''' <summary>
    ''' This method removes the current assembly from the list of targets which
    ''' will be iterated tru when the current appdomain is trying to resolve
    ''' a typename into a System.Type.
    ''' </summary>
    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Sub RemoveFromTypeResolver(extendee As Assembly)
      If (_TypeResolvingTargets.Contains(extendee)) Then
        _TypeResolvingTargets.Remove(extendee)
      End If
    End Sub

    ''' <summary>
    ''' Enables a type-name crawler, which scans all loaded assemblies of the current
    ''' appdomain for the target type
    ''' </summary>
    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub EnableExtendedTypeResolving(extendee As AppDomain)
      If (Not _HandlerRegistered) Then
        AddHandler extendee.TypeResolve, AddressOf ResolveType
        _HandlerRegistered = True
      End If
      _GlobalTypeResolivingEnabled = True
    End Sub

    Private _HandlerRegistered As Boolean = False
    Private _GlobalTypeResolivingEnabled As Boolean = False
    Private _TypeResolvingTargets As New List(Of Assembly)
    Private _FailedTypeNames As New List(Of String)

    <DebuggerStepThrough>
    Private Function ResolveType(sender As Object, args As ResolveEventArgs) As Assembly
      If (_FailedTypeNames.Contains(args.Name)) Then
        Return Nothing
      End If


      Dim includedAssemblies As IEnumerable(Of Assembly)
      If (_GlobalTypeResolivingEnabled) Then
        includedAssemblies = AppDomain.CurrentDomain.GetAssemblies()
      Else
        includedAssemblies = _TypeResolvingTargets
      End If

      For Each includedAssembly In includedAssemblies
        Try
          Dim allTypes() As Type = includedAssembly.GetTypesAccessable()
          Dim exceptions As Integer = 0
          For Each foundType In allTypes
            Try
              Dim compareName As String = foundType.Namespace & "." & foundType.Name
              If (compareName.ToLower() = args.Name.ToLower()) Then

                Trace.TraceInformation(String.Format("Type '{0}' successfully resolved. Assembly: {1}", compareName, includedAssembly.FullName))
                Return includedAssembly

              End If
            Catch
              exceptions += 1
              If (exceptions >= 5) Then
                Exit For
              End If
            End Try
          Next
        Catch
        End Try
      Next

      Trace.TraceWarning(String.Format("Type '{0}' could not be resolved!", args.Name))
      _FailedTypeNames.Add(args.Name)
      Return Nothing
    End Function

#End Region

#Region " Assembly Resolving "

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub AddAssemblyResolver(dom As AppDomain, resolvingMethod As Func(Of AssemblyName, Assembly))
      AssemblyResolver.GetInstance(dom).ResolvingMethods.Add(resolvingMethod)
    End Sub

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub AddAssemblyResolver(dom As AppDomain, directory As DirectoryInfo, Optional recursive As Boolean = False)
      AssemblyResolver.GetInstance(dom).ResolvingMethods.Add(Function(fqan) ResolveFromDirectory(fqan, directory, recursive))
    End Sub

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub AddAssemblyResolver(dom As AppDomain, directoryName As String, Optional recursive As Boolean = False)
      dom.AddAssemblyResolver(New DirectoryInfo(directoryName), recursive)
    End Sub

    Private Function ResolveFromDirectory(fqan As AssemblyName, directory As DirectoryInfo, recursive As Boolean) As Assembly
      Dim ass As Assembly = Nothing
      Try
        Dim file As FileInfo = directory.GetFiles(fqan.Name & ".dll").SingleOrDefault()
        If (file IsNot Nothing) Then
          ass = Assembly.LoadFrom(file.FullName)
        ElseIf (recursive) Then
          For Each subDir In directory.GetDirectories
            Try
              ass = ResolveFromDirectory(fqan, subDir, recursive)
            Catch
            End Try
            If (ass IsNot Nothing) Then
              Exit For
            End If
          Next
        End If
      Catch
      End Try
      Return ass
    End Function

    Private Class AssemblyResolver

      Private Shared _Instances As New Dictionary(Of AppDomain, AssemblyResolver)

      Public Shared Function GetInstance(dom As AppDomain) As AssemblyResolver
        Dim instance As AssemblyResolver
        If (_Instances.ContainsKey(dom)) Then
          instance = _Instances(dom)
        Else
          instance = New AssemblyResolver
          _Instances.Add(dom, instance)
          AddHandler dom.AssemblyResolve, AddressOf instance.Resolve
        End If
        Return instance
      End Function

      Public Property ResolvingMethods As New List(Of Func(Of AssemblyName, Assembly))
      Public Function Resolve(sender As Object, args As ResolveEventArgs) As Assembly
        Dim result As Assembly = Nothing
        Dim name As New AssemblyName(args.Name)
        For Each resolvingMethod In ResolvingMethods
          result = resolvingMethod.Invoke(name)
          If (result IsNot Nothing) Then
            Exit For
          End If
        Next
        Return result
      End Function
    End Class

#End Region

#If NET461 Then

#Region " CrossAppDomain Calls "

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub Invoke(appDomain As AppDomain, method As Action)
      CrossAppDomainActionProxy.Invoke(appDomain, method)
    End Sub

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub Invoke(Of TArg1)(appDomain As AppDomain, method As Action(Of TArg1), arg1 As TArg1)
      CrossAppdomainActionProxy(Of TArg1).Invoke(appDomain, method, arg1)
    End Sub

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub Invoke(Of TArg1, TArg2)(appDomain As AppDomain, method As Action(Of TArg1, TArg2), arg1 As TArg1, arg2 As TArg2)
      CrossAppdomainActionProxy(Of TArg1, TArg2).Invoke(appDomain, method, arg1, arg2)
    End Sub

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function Invoke(Of TReturn)(appDomain As AppDomain, method As Func(Of TReturn)) As TReturn
      Return CrossAppdomainFuncProxy(Of TReturn).Invoke(appDomain, method)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function Invoke(Of TArg1, TReturn)(appDomain As AppDomain, method As Func(Of TArg1, TReturn), arg1 As TArg1) As TReturn
      Return CrossAppdomainFuncProxy(Of TArg1, TReturn).Invoke(appDomain, method, arg1)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function Invoke(Of TArg1, TArg2, TReturn)(appDomain As AppDomain, method As Func(Of TArg1, TArg2, TReturn), arg1 As TArg1, arg2 As TArg2) As TReturn
      Return CrossAppdomainFuncProxy(Of TArg1, TArg2, TReturn).Invoke(appDomain, method, arg1, arg2)
    End Function

#End Region

#End If

  End Module

End Namespace
