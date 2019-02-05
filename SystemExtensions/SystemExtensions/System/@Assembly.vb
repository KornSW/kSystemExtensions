Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.IO
Imports System.Linq
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Security
Imports System.Text

Namespace System.Reflection

  Public Module ExtensionsForAssembly

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsDirectReferencedBy(targetAssembly As Assembly, sourceAssembly As Assembly) As Boolean
      If (targetAssembly.FullName = sourceAssembly.FullName) Then
        Return True
      End If
      Return sourceAssembly.GetReferencedAssemblies.Where(Function(a) a.FullName = targetAssembly.FullName).Any()
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function KnowsAssembly(sourceAssembly As Assembly, targetAssembly As Assembly) As Boolean
      Return sourceAssembly.KnowsAssembly(targetAssembly.GetName())
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function KnowsAssembly(sourceAssembly As Assembly, targetAssemblyName As AssemblyName) As Boolean
      Dim sourceAssemblyName = sourceAssembly.GetName().FullName
      If (sourceAssemblyName = targetAssemblyName.FullName) Then
        Return True
      End If

      Dim referencedAssemblies = sourceAssembly.GetReferencedAssemblies()

      'soft search
      For Each assName In referencedAssemblies
        If (sourceAssemblyName = assName.FullName) Then
          Return True
        End If
      Next

      'deep search
      For Each assName In referencedAssemblies
        Dim ass = Assembly.Load(assName)
        If (ass.KnowsAssembly(targetAssemblyName)) Then
          Return True
        End If
      Next

      Return False
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsSystemAssembly(extendee As Assembly) As Boolean

      If (extendee.FullName.StartsWith("Microsoft")) Then
        Return True
      End If
      If (extendee.FullName.StartsWith("System.")) Then
        Return True
      End If

      If (extendee.IsDefined(GetType(AssemblyCompanyAttribute), True)) Then
        Dim companyAttribute As AssemblyCompanyAttribute = CType(extendee.GetCustomAttributes(GetType(AssemblyCompanyAttribute), True)(0), AssemblyCompanyAttribute)

        If (companyAttribute.Company.ToLower().Contains("microsoft")) Then
          Return True
        End If
      End If

      Return False
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsDotNetAssembly(assemblyFile As FileInfo) As Boolean
      Try
        Dim a = Assembly.LoadFrom(assemblyFile.FullName)
        Return (a IsNot Nothing)
      Catch ex As BadImageFormatException
        Return False
      End Try
    End Function

    ''' <summary>
    ''' Generates an unique Fingerprint for an Assembly using the PublicKeyToken
    ''' </summary>
    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetFingerPrint(assembly As Assembly) As SecureString
      Dim fingerPrint As New SecureString
      Try
        Dim pk() As Byte = assembly.GetName().GetPublicKeyToken()
        If (pk IsNot Nothing) Then
          fingerPrint.AppendBytes(pk)
          Return fingerPrint
        End If
      Catch
      End Try
      fingerPrint.AppendBytes(Encoding.ASCII.GetBytes(assembly.GetName.Name))
      Return fingerPrint
    End Function


    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetAssemblyInfo(Of T As Attribute)(sourceInstance As Assembly) As T
      Return sourceInstance.GetCustomAttributes(True).OfType(Of T).FirstOrDefault()
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetAssemblyCompany(sourceInstance As Assembly) As String
      Dim attrib = sourceInstance.GetAssemblyInfo(Of AssemblyCompanyAttribute)()
      If (attrib Is Nothing) Then
        Return String.Empty
      Else
        Return attrib.Company
      End If
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetAssemblyTitle(sourceInstance As Assembly) As String
      Dim attrib = sourceInstance.GetAssemblyInfo(Of AssemblyTitleAttribute)()
      If (attrib Is Nothing) Then
        Return String.Empty
      Else
        Return attrib.Title
      End If
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetAssemblyDescription(sourceInstance As Assembly) As String
      Dim attrib = sourceInstance.GetAssemblyInfo(Of AssemblyDescriptionAttribute)()
      If (attrib Is Nothing) Then
        Return String.Empty
      Else
        Return attrib.Description
      End If
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetAssemblyProduct(sourceInstance As Assembly) As String
      Dim attrib = sourceInstance.GetAssemblyInfo(Of AssemblyProductAttribute)()
      If (attrib Is Nothing) Then
        Return String.Empty
      Else
        Return attrib.Product
      End If
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetAssemblyCopyright(sourceInstance As Assembly) As String
      Dim attrib = sourceInstance.GetAssemblyInfo(Of AssemblyCopyrightAttribute)()
      If (attrib Is Nothing) Then
        Return String.Empty
      Else
        Return attrib.Copyright
      End If
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetAssemblyTrademark(sourceInstance As Assembly) As String
      Dim attrib = sourceInstance.GetAssemblyInfo(Of AssemblyTrademarkAttribute)()
      If (attrib Is Nothing) Then
        Return String.Empty
      Else
        Return attrib.Trademark
      End If
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetAssemblyComGuid(sourceInstance As Assembly) As Guid
      Dim attrib = sourceInstance.GetAssemblyInfo(Of GuidAttribute)()
      If (attrib Is Nothing) Then
        Return Guid.Empty
      Else
        Return Guid.Parse(attrib.Value)
      End If
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetAssemblyVersion(sourceInstance As System.Reflection.Assembly) As Version
      Dim attrib = sourceInstance.GetAssemblyInfo(Of AssemblyVersionAttribute)()
      If (attrib Is Nothing) Then
        Return New Version(0, 0, 0, 0)
      Else
        Return Version.Parse(attrib.Version)
      End If
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetAssemblyFileVersion(sourceInstance As System.Reflection.Assembly) As Version
      Dim attrib = sourceInstance.GetAssemblyInfo(Of AssemblyFileVersionAttribute)()
      If (attrib Is Nothing) Then
        Return New Version(0, 0, 0, 0)
      Else
        Return Version.Parse(attrib.Version)
      End If
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetAssemblyInformationalVersion(sourceInstance As Assembly) As Version
      Dim attrib = sourceInstance.GetAssemblyInfo(Of AssemblyInformationalVersionAttribute)()
      If (attrib Is Nothing) Then
        Return New Version(0, 0, 0, 0)
      Else
        Return Version.Parse(attrib.InformationalVersion)
      End If
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsInMemory(sourceInstance As Assembly) As Boolean
      Return sourceInstance.IsDynamic
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetResourceManager(sourceInstance As Assembly) As System.Resources.ResourceManager
      Return New System.Resources.ResourceManager("Resources", sourceInstance)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetDefaultNamespace(assembly As Assembly) As String
      Dim referenceType As Type =
        (From t As Type In assembly.GetTypesAccessable() Where t.FullName.Contains(".My.")).FirstOrDefault()
      If (referenceType Is Nothing) Then
        Return String.Empty
      Else
        Return referenceType.FullName.Substring(0, referenceType.FullName.IndexOf(".My."))
      End If
    End Function


    ''' <summary>
    ''' A really important secret is that the common method "GetTypes()" will fail
    ''' if the assembly contains one or more types with broken references (to non existing assemblies).
    ''' This method 'GetTypesAccessable()' will handle this problem and return at least those types,
    ''' which could be loaded!
    ''' </summary>
    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetTypesAccessable(assembly As Assembly) As Type()
      Return assembly.GetTypesAccessable(Sub(ex) Trace.TraceWarning(ex.Message))
    End Function

    ''' <summary>
    ''' A really important secret is that the common method "GetTypes()" will fail
    ''' if the assembly contains one or more types with broken references (to non existing assemblies).
    ''' This method 'GetTypesAccessable()' will handle this problem and return at least those types,
    ''' which could be loaded!
    ''' </summary>
    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetTypesAccessable(assembly As Assembly, loaderExceptionHandler As Action(Of Exception)) As Type()
      Try
        Return assembly.GetTypes()
      Catch ex As ReflectionTypeLoadException
        For Each le In ex.LoaderExceptions
          loaderExceptionHandler.Invoke(le)
        Next
        'This ugly workarround is the only way to get the types from a asembly
        'which contains one or more types with broken references f.e:
        '  [Type1] good!
        '  [Type2] good!
        '  [Type3] INHERITS [<non-exitings-assembly>.BaseType3] bad!
        Return ex.Types().Where(Function(t) t IsNot Nothing).ToArray()
      End Try
    End Function

  End Module

End Namespace
