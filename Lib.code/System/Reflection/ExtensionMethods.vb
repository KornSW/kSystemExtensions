Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Diagnostics

Namespace System.Reflection

  Public Class ExtensionMethods
    Implements IEnumerable(Of MethodInfo)

#Region " Static Part "

    Private Shared _Instances As New Dictionary(Of Type, ExtensionMethods)
    Private Shared _AppDomainListenerEnabled As Boolean = False

    Public Shared Function OfType(Of TExtendee)() As ExtensionMethods
      Return OfType(GetType(TExtendee))
    End Function

    Public Shared Function OfType(tExtendee As Type) As ExtensionMethods

      If (_AppDomainListenerEnabled = False) Then
        _AppDomainListenerEnabled = True
        AddHandler AppDomain.CurrentDomain.AssemblyLoad, AddressOf AppDomain_AssemblyLoad
      End If

      If (_Instances.ContainsKey(tExtendee)) Then
        Return _Instances(tExtendee)
      Else
        Dim newInstance As New ExtensionMethods(tExtendee)
        _Instances.Add(tExtendee, newInstance)
        For Each ass In AppDomain.CurrentDomain.GetAssemblies().ToArray()
          If (tExtendee.Assembly.IsDirectReferencedBy(ass)) Then
            newInstance.AnalyzeAssembly(ass)
          End If
        Next
        Return newInstance
      End If
    End Function

    Private Shared Sub AppDomain_AssemblyLoad(sender As Object, args As AssemblyLoadEventArgs)
      Dim ass = args.LoadedAssembly
      For Each alreadyAnalyzedExtendee In _Instances.Keys.ToArray()
        If (alreadyAnalyzedExtendee.Assembly.IsDirectReferencedBy(ass)) Then
          _Instances(alreadyAnalyzedExtendee).AnalyzeAssembly(ass)
        End If
      Next
    End Sub

#End Region

#Region " Declarations & Constructor "

    Private _ExtendeeType As Type
    Private _ExtensionMethods As New List(Of MethodInfo)
    Private _InterfaceExtensionMethods As New Dictionary(Of Type, ExtensionMethods)
    Private _BaseTypeExtensionMethods As ExtensionMethods = Nothing

    Private Sub New(extendeeType As Type)
      _ExtendeeType = extendeeType

      If (extendeeType.BaseType IsNot Nothing AndAlso Not extendeeType.BaseType = GetType(Object)) Then
        _BaseTypeExtensionMethods = ExtensionMethods.OfType(extendeeType.BaseType)
      End If

      For Each implementedInterface In extendeeType.GetInterfaces
        _InterfaceExtensionMethods.Add(implementedInterface, ExtensionMethods.OfType(implementedInterface))
      Next

    End Sub

#End Region

#Region " Evaluation (Type Crawler) "

    Private Sub AnalyzeAssembly(a As Assembly)

      If (a.IsDynamic) Then
        Exit Sub
      End If

      Dim allTypes As Type()
      allTypes = a.GetTypesAccessable()

      Dim allExtensionMethodsOfAssembly As New List(Of MethodInfo)

      For Each type In allTypes
        If (type.IsSealed AndAlso Not type.IsGenericType AndAlso Not type.IsNested) Then
          For Each method In type.GetMethods(BindingFlags.[Static] Or BindingFlags.[Public] Or BindingFlags.NonPublic)
            Try
              If (method.IsDefined(GetType(ExtensionAttribute), False) AndAlso method.GetParameters.Any()) Then
                allExtensionMethodsOfAssembly.Add(method)
              End If
            Catch
              'this occours when the containing assembly of a parameter type is not available
            End Try
          Next
        End If
      Next

      Dim matchingExtensionMethods =
          allExtensionMethodsOfAssembly.Where(
            Function(method)
              Dim firstParam = method.GetParameters()(0)
              Select Case method.GetGenericArguments().Count
                Case 0
                  Return firstParam.ParameterType = _ExtendeeType
                Case 1
                  Dim genArgType = method.GetGenericArguments(0)

                  'lets find something like:
                  ' Function MyExtension(Of T As ExtendeeType)(arg1 as T)

                  If (Not genArgType = firstParam.ParameterType) Then
                    'the generic argument is not used for the first param...
                    Return False
                  End If

                  If (Not _ExtendeeType.IsClass AndAlso genArgType.HasGenericConstraintClass()) Then
                    'the extension is written only for classes, but our extendee is no class
                    Return False
                  End If

                  If (Not genArgType.GetNonGenericBaseType().IsAssignableFrom(_ExtendeeType)) Then
                    'there is a generic type constraint, which requires a basetype that is not assignable from our extendee 
                    Return False
                  End If

                  If (genArgType.HasGenericConstraintNew) Then
                    'the extension is written only for classes with a default constructor
                    If (Not _ExtendeeType.IsClass) Then
                      'our extendee is no class
                      Return False
                    End If
                    If (Not _ExtendeeType.HasDefaultConstructor) Then
                      'our extendee has no default constructor
                      Return False
                    End If
                  End If

                  For Each interfaceConstraint In genArgType.GetInterfaces()
                    If (Not interfaceConstraint.IsAssignableFrom(_ExtendeeType)) Then
                      'there is a generic type constraint, which requires a interface that is not assignable from our extendee 
                      Return False
                    End If
                  Next

                  Return True
                Case Else
                  Return False
              End Select
            End Function)

      For Each matchingExtensionMethod In matchingExtensionMethods
        If (matchingExtensionMethod.GetGenericArguments().Any()) Then
          'if we have an generic extension, the we need to generate a specific instance
          Try
            Dim genericMethod = matchingExtensionMethod.MakeGenericMethod(_ExtendeeType)
            _ExtensionMethods.Add(genericMethod)
          Catch
            'this occours due missmatches of special type constraints
            'extension methods doing such hardcore magic with generic constraints,
            'will not be included in our search result
          End Try
        Else
          _ExtensionMethods.Add(matchingExtensionMethod)
        End If
      Next

    End Sub

#End Region

#Region " Consume "

    Private Function GetEnumeratorUntyped() As IEnumerator Implements IEnumerable.GetEnumerator
      Return Me.GetEnumerator
    End Function

    Private Function GetEnumerator() As IEnumerator(Of MethodInfo) Implements IEnumerable(Of MethodInfo).GetEnumerator
      Dim result As IEnumerable(Of MethodInfo)
      result = _ExtensionMethods

      'add the extensionmethods from the baseclass
      If (_BaseTypeExtensionMethods IsNot Nothing) Then
        result = result.Union(_BaseTypeExtensionMethods)
      End If

      For Each interfaceExtensionMethods In _InterfaceExtensionMethods.Values
        result = result.Union(interfaceExtensionMethods)
      Next

      Return result.Distinct().GetEnumerator()
    End Function

    Default ReadOnly Property Item(methodName As String, ParamArray parameterTypeSignature() As Type) As MethodInfo
      Get
        Return Me.GetMethod(methodName, parameterTypeSignature)
      End Get
    End Property

#End Region

  End Class

End Namespace
