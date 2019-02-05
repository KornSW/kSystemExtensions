Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Text

Public Module ExtensionsForType

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function TryParse(targetType As Type, sourceText As String, ByRef target As Object) As Boolean
    Try

      If (targetType.IsEnum) Then
        If ([Enum].GetNames(targetType).Select(Function(n) n.ToLower).Contains(sourceText)) Then
          target = [Enum].Parse(targetType, sourceText, True)
          Return True
        End If
      End If

      'special fixes
      Select Case targetType
        Case GetType(String)
          target = sourceText
          Return True
        Case GetType(Boolean)
          If (sourceText = "1") Then
            sourceText = "true"
          End If
          If (sourceText = "0") Then
            sourceText = "false"
          End If
      End Select

      Dim flagMask =
        Reflection.BindingFlags.Static Or
        Reflection.BindingFlags.Public Or
        Reflection.BindingFlags.InvokeMethod

      Dim parseMethod = targetType.GetMethods(flagMask).Where(
        Function(m) m.Name = "TryParse" AndAlso
                    m.GetParameters.Count = 2 AndAlso
                    m.ReturnType = GetType(Boolean) AndAlso
                    m.GetParameters(0).ParameterType = GetType(String) AndAlso
                    m.GetParameters(1).ParameterType = targetType.MakeByRefType AndAlso
                    m.GetParameters(1).IsOut
        ).FirstOrDefault()

      If (parseMethod Is Nothing) Then
        Return False

      Else
        Dim callParams() As Object = {sourceText, Nothing}
        Dim success = DirectCast(parseMethod.Invoke(Nothing, callParams), Boolean)
        If (success) Then
          target = callParams(1)
        End If
        Return success
      End If

    Catch
      Return False
    End Try
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function FriendlyName(extendee As Type) As String
    Dim sb As New StringBuilder
    sb.Append(extendee.Namespace)
    sb.Append("."c)
    Dim genArgs = extendee.GetGenericArguments()
    If (genArgs IsNot Nothing AndAlso genArgs.Length > 0) Then
      sb.Append(extendee.Name.Substring(0, extendee.Name.IndexOf("`"c)))
      Dim first As Boolean = True
      sb.Append("("c)
      For Each genArg In genArgs
        If (first) Then
          first = False
          sb.Append("Of ")
        Else
          sb.Append(", ")
        End If
        sb.Append(genArg.FriendlyName)
      Next
      sb.Append(")"c)
    Else
      sb.Append(extendee.Name)
    End If
    Return sb.ToString()
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function WithIndexParams(extendee As IEnumerable(Of PropertyInfo), Optional count As Integer = -1) As IEnumerable(Of PropertyInfo)
    Return From pi In extendee
           Where pi.HasIndexParams(count)
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetEnumName(Of TEnum As Structure)(enumValue As TEnum) As String
    If (GetType(TEnum).IsEnum()) Then
      If ([Enum].IsDefined(GetType(TEnum), enumValue)) Then
        Return [Enum].GetName(GetType(TEnum), enumValue)
      Else
        Return Nothing
      End If
    Else
      Return Nothing
    End If
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function IsList(extendee As Type) As Boolean
    Dim type = extendee
    If (extendee.IsGenericType) Then
      type = type.GetGenericTypeDefinition
    End If
    Return ((GetType(IList).IsAssignableFrom(type)) OrElse (GetType(IList(Of )).IsAssignableFrom(type)))
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function HasIndexParams(extendee As PropertyInfo, Optional count As Integer = -1) As Boolean
    Dim p = extendee.GetIndexParameters
    If (count = -1) Then
      Return p.Any()
    End If
    Dim min As Integer = 0
    Dim max As Integer = 0
    For Each pi In p
      max += 1
      If (Not pi.IsOptional) Then
        min += 1
      End If
    Next
    Return min <= count AndAlso count <= max
  End Function

  ''' <summary>
  ''' Dynamically creates an List(Of ) type with the current type for the items.
  ''' The returned Type can be activated and casted to 'IList'
  ''' </summary>
  <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
  Function MakeListType(itemType As Type) As Type
    Return GetType(List(Of )).MakeGenericType(itemType)
  End Function


  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetDefaultProperties(extendee As Type) As IEnumerable(Of PropertyInfo)
    Return extendee.GetDefaultMembers().OfType(Of PropertyInfo)()
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function IsDefaultProperty(extendee As PropertyInfo) As Boolean
    Return extendee.DeclaringType.GetDefaultProperties().Contains(extendee)
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetValueFrom(extendee As Type, instance As Object, ParamArray propertyPath() As String) As Object
    Return extendee.GetValueFrom(instance, String.Join(".", propertyPath))
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetValueFrom(extendee As Type, instance As Object, memberPath As String) As Object
    If (memberPath.Length < 1) Then
      Return Nothing
    End If
    If (Not extendee.IsSoftAssignableFrom(instance.GetType())) Then
      Return Nothing
    End If



    'TODO: immer runtime types nehmen!!!! weil die nie weniger, sondern immer mehr können
    'TODO: instance=nothing geht in den "Static" Mode
    'TODO: Funktionen unterstützen



    'multiples are definitly defaultprop accessors
    memberPath = memberPath.Replace(")(", ").(")

    Dim currentPart As String
    If (memberPath.Contains(".")) Then
      currentPart = memberPath.Substring(0, memberPath.IndexOf("."))
    Else
      currentPart = memberPath
    End If

    Dim args() As String = {}
    If (currentPart.Contains("(")) Then
      Dim idx = currentPart.IndexOf("(")
      args = currentPart.Substring(idx + 1, currentPart.Length - idx - 2).Split(","c)
      currentPart = currentPart.Substring(0, idx)
    End If

    Dim foundProp As PropertyInfo = Nothing
    Dim typedArgs As New List(Of Object)
    For Each prop In extendee.GetProperties().WithIndexParams(args.Length)
      Dim isCompatible As Boolean
      If (String.IsNullOrWhiteSpace(currentPart)) Then
        isCompatible = prop.IsDefaultProperty
      Else
        isCompatible = prop.Name.ToLower() = currentPart.ToLower()
      End If
      If (isCompatible) Then
        typedArgs.Clear()
        For i As Integer = 0 To args.Length - 1
          Dim parsedArg As Object = Nothing
          If (prop.GetIndexParameters(i).ParameterType.TryParse(args(i), parsedArg)) Then
            typedArgs.Add(parsedArg)
          Else
            isCompatible = False
            Exit For
          End If
        Next
      End If
      If (isCompatible AndAlso prop.CanRead) Then
        foundProp = prop
        Exit For
      End If
    Next

    Dim subObj As Object
    Dim subType As Type

    'the default property was used, we need to retry this in two steps
    If (foundProp Is Nothing) Then

      If (Not String.IsNullOrEmpty(currentPart) AndAlso currentPart.Length < memberPath.Length AndAlso Not memberPath.StartsWith(currentPart & ".")) Then ' if not already using the default property
        Return extendee.GetValueFrom(instance, memberPath.Insert(currentPart.Length, "."))

      ElseIf (extendee.IsArray AndAlso extendee.GetArrayRank() = args.Length) Then
        Dim typedArrayIndices As New List(Of Integer)
        For i As Integer = 0 To args.Length - 1
          Dim parsedArg As Integer = 0
          If (Integer.TryParse(args(i), parsedArg)) Then
            typedArrayIndices.Add(parsedArg)
          Else
            Return False
          End If
        Next
        subObj = DirectCast(instance, Array).GetValue(typedArrayIndices.ToArray)
        subType = extendee.GetElementType

      ElseIf (GetType(IEnumerable).IsSoftAssignableFrom(extendee) AndAlso args.Length = 1) Then
        Dim index As Integer
        If (Integer.TryParse(args(0), index)) Then
          subObj = DirectCast(instance, IEnumerable)(index)


          If (extendee.GenericTypeArguments.Any) Then
            subType = extendee.GenericTypeArguments.First
          Else
            subType = subObj.GetType
          End If

        Else
          Return Nothing
        End If

      Else
        Return Nothing
      End If

    Else
      If (args.Length > 0) Then
        subObj = foundProp.GetValue(instance, typedArgs.ToArray())
      Else
        subObj = foundProp.GetValue(instance, Nothing)
      End If
      subType = foundProp.PropertyType
    End If

    If (memberPath.Contains(".")) Then
      Dim idx = memberPath.IndexOf(".") + 1
      Return subType.GetValueFrom(subObj, memberPath.Substring(idx, memberPath.Length - idx))
    Else
      Return subObj
    End If

  End Function

  ''' <summary>
  ''' Returns the name of a type, defined by the DisplayNameAttribute. If no DisplayNameAttribute is defined, it returns the name of the type.
  ''' </summary>
  ''' <param name="extendee">The existing type to be extended.</param>
  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function DisplayName(extendee As Type) As String
    If (extendee.IsDefined(GetType(DisplayNameAttribute), False)) Then
      Dim attribute As DisplayNameAttribute = CType(extendee.GetCustomAttributes(GetType(DisplayNameAttribute), False)(0), DisplayNameAttribute)

      Return attribute.DisplayName
    End If

    Return extendee.Name
  End Function

  ''' <summary>
  ''' Returns wether the given type is a collection or not.
  ''' </summary>
  ''' <param name="extendee">The existing type to be extended.</param>
  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function IsCollection(extendee As Type) As Boolean
    If ((extendee Is Nothing) OrElse (extendee Is GetType(String))) Then
      Return False
    End If

    Return GetType(IEnumerable).IsSoftAssignableFrom(extendee)
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function IsParameterlessInstantiable(extendee As Type) As Boolean

    If (extendee.IsPrimitive) Then
      Return True
    End If

    If (Not extendee.IsClass OrElse extendee.IsAbstract) Then
      Return False
    End If

    If (extendee.GetConstructor(BindingFlags.CreateInstance Or BindingFlags.Public Or BindingFlags.Instance, Nothing, New Type(0 - 1) {}, Nothing) Is Nothing) Then
      Return False 'no parameterless constructor
    End If

    Return True
  End Function

  ''' <returns>True, if the given type is a atomic type, otherwise false.</returns>
  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function IsAtomic(extendee As Type) As Boolean
    If (extendee.IsPrimitive) Then
      Return True
    End If
    If (extendee.IsEnum) Then
      Return True
    End If
    Select Case extendee
      Case GetType(Integer), GetType(System.Int16), GetType(System.Int32)
      Case GetType(UInteger), GetType(System.UInt16), GetType(System.UInt32)
      Case GetType(Long), GetType(System.Int64)
      Case GetType(ULong), GetType(System.UInt64)
      Case GetType(Decimal), GetType(System.Decimal)
      Case GetType(Single), GetType(System.Single)
      Case GetType(Double), GetType(System.Double)
      Case GetType(Boolean), GetType(System.Boolean)
      Case GetType(Byte), GetType(System.Byte)
      Case GetType(String), GetType(System.String)
      Case GetType(System.DateTime)
      Case GetType(System.Version)
      Case GetType(System.Guid)
      Case Else : Return False
    End Select
    Return True
  End Function

  ''' <summary>
  ''' Returns wether the given type is a nullable type or not.
  ''' </summary>
  ''' <param name="extendee">The existing type to be extended.</param>
  ''' <returns>True, if the given type is a nullable type, otherwise false.</returns>
  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function IsNullableType(extendee As Type) As Boolean
    If ((extendee.IsGenericType) AndAlso (extendee.GetGenericTypeDefinition Is GetType(Nullable(Of )))) Then
      Return True
    End If

    Return False
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function IsSoftAssignableFrom(extendee As Type, c As Type) As Boolean
    Return extendee.IsAssignableFrom(c)

    'TODO: hier eigene logik, die auch unterschidliche assmeblies erlaubt

  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function IsSoftAssignableTo(extendee As Type, c As Type) As Boolean
    Return c.IsSoftAssignableFrom(extendee)
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function IsAssignableTo(extendee As Type, c As Type) As Boolean
    Return c.IsAssignableFrom(extendee)
  End Function


  ''' <summary> Returns the first matching Method of the given attribute type. </summary>
  ''' <param name="extendee"> The type to be searched in. </param>
  ''' <typeparam name="TAttribute"> The desired attribute. </typeparam>
  ''' <returns> The methodInfo of the found method or nothing. </returns>
  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetMethod(Of TAttribute)(extendee As Type) As MethodInfo
    If (extendee Is Nothing) Then
      Return Nothing
    End If

    Return (From n In extendee.GetMethods() Where n.GetCustomAttributes(GetType(TAttribute), True).Length > 0).FirstOrDefault
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function Activate(typeToActivate As Type) As Object
    Return typeToActivate.Activate(Nothing)
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function Activate(typeToActivate As Type, ParamArray args() As Object) As Object
    If (typeToActivate.IsArray) Then
      Dim target As Array
      If (args IsNot Nothing AndAlso args.Length > 0) Then
        target = Array.CreateInstance(typeToActivate.GetItemType, args.Length)
        For i As Integer = 0 To target.Length - 1
          target.SetValue(args(i), i)
        Next
      Else
        target = Array.CreateInstance(typeToActivate.GetItemType, 0)
      End If
      Return target
    Else
      If (args IsNot Nothing AndAlso args.Length > 0) Then
        Return Activator.CreateInstance(typeToActivate, args)
      Else
        If (typeToActivate.HasParameterlessConstructor) Then
          Return Activator.CreateInstance(typeToActivate)
        ElseIf (typeToActivate = GetType(String)) Then
          Return String.Empty
        Else
          Dim newInst As Object = Nothing
          typeToActivate.TryParse(String.Empty, newInst)
          Return newInst
        End If
      End If
    End If
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function Activate(Of TCastTargetType)(typeToActivate As Type) As TCastTargetType
    Return DirectCast(typeToActivate.Activate(), TCastTargetType)
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function Activate(Of TCastTargetType)(typeToActivate As Type, ParamArray args() As Object) As TCastTargetType
    Return DirectCast(typeToActivate.Activate(args), TCastTargetType)
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function HasParameterlessConstructor(t As Type) As Boolean
    If (Not t.IsClass OrElse t.IsAbstract) Then
      Return False
    End If
    For Each c In t.GetConstructors()
      If (c.IsPublic AndAlso c.GetParameters().None) Then
        Return True
      End If
    Next
    Return False
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function HasCustomAttribute(Of T)(extendee As Type, checkCriteria As Predicate(Of T)) As Boolean

    Dim attribute As T = DirectCast(extendee.GetCustomAttributes(GetType(T), True).FirstOrDefault, T)
    If (attribute Is Nothing) Then
      Return False
    End If

    Return (checkCriteria.Invoke(attribute))
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function HasCustomAttribute(Of T)(extendee As Type) As Boolean
    Dim attribute As T = DirectCast(extendee.GetCustomAttributes(GetType(T), True).FirstOrDefault, T)
    Return attribute IsNot Nothing
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function CustomAttribute(Of T)(extendee As Type) As T
    Dim attribute As T = DirectCast(extendee.GetCustomAttributes(GetType(T), True).FirstOrDefault, T)
    Return attribute
  End Function

  '<Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  'Public Function SearchExtensionMethods(t As Type, includedAssemblyNamePattern As String) As MethodInfo()

  '  Dim types As New List(Of Type)()

  '  Dim assemblies = From anyLoadedAssembly In AppDomain.CurrentDomain.GetAssemblies()
  '                   Where anyLoadedAssembly.FullName.MatchesWildcardMask(includedAssemblyNamePattern)
  '                   Select anyLoadedAssembly

  '  For Each item As Assembly In assemblies
  '    types.AddRange(item.GetTypes())
  '  Next

  '  Dim query = From type In types
  '              Where type.IsSealed AndAlso Not type.IsGenericType AndAlso Not type.IsNested
  '              From method In type.GetMethods(BindingFlags.[Static] Or BindingFlags.[Public] Or BindingFlags.NonPublic)
  '              Where method.IsDefined(GetType(ExtensionAttribute), False)
  '              Where method.ReturnType IsNot GetType(Void)
  '              Where method.GetParameters.Any() AndAlso method.GetParameters().Count = 1 AndAlso method.GetParameters()(0).ParameterType = t
  '              Select method

  '  Return query.ToArray()
  'End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function HasDefaultConstructor(extendee As Type) As Boolean
    Return (
      (extendee.IsPrimitive) OrElse
      (Not extendee.GetConstructor(
         BindingFlags.CreateInstance Or
         BindingFlags.Public Or
         BindingFlags.Instance, Nothing, New Type(0 - 1) {}, Nothing) Is Nothing))
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
  Public Function HasGenericConstraint(extendee As Type, constraint As GenericParameterAttributes) As Boolean
    Return ((extendee.GenericParameterAttributes And constraint) = constraint)
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
  Public Function HasGenericConstraintNew(extendee As Type) As Boolean
    Return extendee.HasGenericConstraint(GenericParameterAttributes.DefaultConstructorConstraint)
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
  Public Function HasGenericConstraintClass(extendee As Type) As Boolean
    Return extendee.HasGenericConstraint(GenericParameterAttributes.ReferenceTypeConstraint)
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetExtensionMethods(extendee As Type) As IEnumerable(Of MethodInfo)
    'we need caching, so lets redirect...
    Return ExtensionMethods.OfType(extendee)
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetConverter(instance As Type) As TypeConverter
    Dim t As Type = instance.GetType()
    Try
      If (t.HasCustomAttribute(Of TypeConverterAttribute)()) Then
        Dim a = t.GetCustomAttribute(Of TypeConverterAttribute)(True)
        Dim converterType As Type = Type.GetType(a.ConverterTypeName, False)
        If (converterType IsNot Nothing) Then
          Return converterType.Activate(Of TypeConverter)({t})
        End If
      End If
    Catch
    End Try
    Return Nothing
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
  Public Function GetNonGenericBaseType(extendee As Type) As Type
    'actual the only indicator to identify generic parameter types
    If (extendee.FullName Is Nothing) Then
      Return extendee.BaseType.GetNonGenericBaseType()
    Else
      Return extendee
    End If
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
  Public Function GetNonDynamicBaseType(extendee As Type) As Type
    If (extendee.Assembly.IsDynamic) Then
      If (extendee.BaseType Is Nothing) Then
        Return Nothing
      Else
        Return extendee.BaseType.GetNonDynamicBaseType
      End If
    Else
      Return extendee
    End If
  End Function

  '<Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  'Public Function IsDictionary(t As Type) As Boolean
  '  Dim dictType As Type = GetType(Dictionary(Of ,))
  '  Return (t = dictType OrElse t.HasGenericBase(dictType))
  'End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function HasGenericBase(Of TBase)(t As Type) As Boolean
    Return t.HasGenericBase(GetType(TBase))
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function HasGenericBase(t As Type, base As Type) As Boolean
    If (t.IsGenericType) Then
      If (t.GetGenericTypeDefinition = base) Then
        Return True
      End If
    End If
    Return False
  End Function

  ''' <summary>
  ''' This is the same like 'GetElementType', but it also works on every IEnumerable
  ''' (not only on arrays)
  ''' </summary>
  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetItemType(enumerableType As Type) As Type

    If (enumerableType.IsArray) Then
      Return enumerableType.GetElementType
    End If

    If (GetType(IEnumerable).IsAssignableFrom(enumerableType)) Then
      Dim targetType As Type = GetType(IEnumerable(Of ))
      Dim ienumerableInterfaceType = (
        From t In enumerableType.GetInterfaces()
        Where t.HasGenericBase(targetType)
        ).FirstOrDefault()

      If (ienumerableInterfaceType IsNot Nothing) Then
        Return ienumerableInterfaceType.GetGenericArguments().First
      Else
        Return GetType(Object)
      End If
    End If

    Return Nothing
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function FindProperty(extendee As Type, name As String) As PropertyInfo
    If (Not name.Contains(".")) Then
      Return extendee.GetBindableProperty(name)
    End If

    Dim fields = Split(name, "[.]")

    For Each field In fields
      Dim propertyInfo = extendee.GetBindableProperty(field)

      If (propertyInfo IsNot Nothing) Then
        Return propertyInfo.PropertyType.FindProperty(name.Remove(0, field.Length + 1))
      End If
    Next

    Return Nothing
  End Function

  <Extension, EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetBindableProperties(extendee As Type) As PropertyInfo()
    Return extendee.GetProperties(BindingFlags.GetProperty Or BindingFlags.Instance Or BindingFlags.Public).Where(Function(p) p.GetIndexParameters.Count = 0).ToArray
  End Function

  <Extension, EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetBindableProperty(extendee As Type, name As String) As PropertyInfo
    Return extendee.GetBindableProperties.Where(Function(p) p.Name = name).SingleOrDefault
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetNameWithNamespace(extendee As Type, Optional lowerCase As Boolean = False) As String
    If (String.IsNullOrEmpty(extendee.Namespace)) Then
      If (lowerCase) Then
        Return extendee.Name.ToLower()
      Else
        Return extendee.Name
      End If
    Else
      If (lowerCase) Then
        Return (extendee.Namespace & "." & extendee.Name).ToLower()
      Else
        Return extendee.Namespace & "." & extendee.Name
      End If
    End If
  End Function

  <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetDisplayName(extendee As Type) As String
    Dim displayNameAttr = extendee.GetCustomAttributes().OfType(Of DisplayNameAttribute)().FirstOrDefault()
    If (displayNameAttr Is Nothing) Then
      Return extendee.Name
    Else
      Return displayNameAttr.DisplayName
    End If
  End Function

  ''' <summary>
  ''' Only the Properties which are: readable, writable, parameterless, public and atomic types | arrays of atomic types
  ''' </summary>
  <Extension, EditorBrowsable(EditorBrowsableState.Always)>
  Public Function GetPrimitiveProperties(extendee As Type) As IEnumerable(Of PropertyInfo)
    Return extendee.GetProperties().Where(
      Function(p)
        Return (
          p.CanWrite AndAlso
          p.CanRead AndAlso
          Not p.HasIndexParams() AndAlso
          p.PropertyType.IsAtomic()
        )
      End Function
    )
  End Function

  '<Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  'Public Function GetWritingMethods(extendee As Type) As IEnumerable(Of MethodInfo)
  '  Dim allMethods As IEnumerable(Of MethodInfo)

  '  allMethods = extendee.GetMethods(BindingFlags.SetProperty Or BindingFlags.InvokeMethod Or BindingFlags.Public).
  '    Where(Function(m) m.GetParameters().Count = 1)

  '  allMethods = allMethods.Union(extendee.GetExtensionMethods.
  '  Where(Function(m) m.GetParameters().Count = 2))

  '  Return allMethods
  'End Function

  '<Extension(), EditorBrowsable(EditorBrowsableState.Always)>
  'Public Function GetReadingMethods(extendee As Type) As IEnumerable(Of MethodInfo)
  '  Dim allMethods As IEnumerable(Of MethodInfo)

  '  allMethods = extendee.GetMethods(BindingFlags.GetProperty Or BindingFlags.InvokeMethod Or BindingFlags.Public).
  '    Where(Function(m) m.GetParameters().Count = 0 AndAlso m.ReturnType IsNot GetType(Void))

  '  allMethods = allMethods.Union(extendee.GetExtensionMethods.
  '  Where(Function(m) m.GetParameters().Count = 1 AndAlso m.ReturnType IsNot GetType(Void)))

  '  Return allMethods
  'End Function

End Module
