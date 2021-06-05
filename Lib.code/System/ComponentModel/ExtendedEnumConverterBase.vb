Imports System
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Linq
Imports System.Resources

Namespace System.ComponentModel

  Public MustInherit Class ExtendedEnumConverterBase
    Inherits EnumConverter

#Region " Declarations "
    Private Class LookupTable
      Inherits Dictionary(Of String, Object)
    End Class

    Private _FlagValues As Array
    Private _IsFlagEnum As Boolean = False
    Private _LookupTables As New Dictionary(Of CultureInfo, LookupTable)()

#End Region

#Region " Constructors "

    ''' <summary>
    ''' Create a new instance of the converter using translations from the default resource manager
    ''' </summary>
    Protected Sub New(ByVal type As Type)
      MyBase.New(type)

      Dim flagAttributes As Object() = type.GetCustomAttributes(GetType(System.FlagsAttribute), True)

      _IsFlagEnum = flagAttributes.Length > 0

      If (_IsFlagEnum) Then
        _FlagValues = System.Enum.GetValues(type)
      End If

    End Sub

#End Region

#Region " Methods "

    ''' <summary>
    ''' Convert string values to enum values
    ''' </summary>
    Public Overloads Overrides Function ConvertFrom(ByVal context As ITypeDescriptorContext, ByVal culture As CultureInfo, ByVal value As Object) As Object
      If (TypeOf value Is String) Then
        Dim result As Object

        If (_IsFlagEnum) Then
          result = GetFlagValue(culture, DirectCast(value, String))
        Else
          result = GetValue(culture, DirectCast(value, String))
        End If

        If (result Is Nothing) Then
          If (Not value Is Nothing) Then
            result = MyBase.ConvertFrom(context, culture, value)
          End If
        End If

        Return result
      End If

      Return MyBase.ConvertFrom(context, culture, value)
    End Function

    ''' <summary>
    ''' Convert the enum value to a string
    ''' </summary>
    Public Overloads Overrides Function ConvertTo(ByVal context As ITypeDescriptorContext, ByVal culture As CultureInfo, ByVal value As Object, ByVal destinationType As Type) As Object
      If ((Not value Is Nothing) AndAlso (destinationType.Equals(GetType(System.String)))) Then
        Dim result As Object

        If (_IsFlagEnum) Then
          result = GetFlagValueText(culture, value)
        Else
          result = GetValueText(culture, value)
        End If

        Return result
      End If

      Return MyBase.ConvertTo(context, culture, value, destinationType)
    End Function

    ''' <summary>
    ''' Convert the given enum value to string using the registered type converter
    ''' </summary>
    ''' <param name="value">The enum value to convert to string</param>
    ''' <returns>The localized string value for the enum</returns>
    Public Shared Shadows Function ConvertToString(ByVal value As System.Enum) As String
      If (Not value Is Nothing) Then
        Dim converter As TypeConverter = TypeDescriptor.GetConverter(value.GetType())

        Return converter.ConvertToString(value)
      End If

      Return String.Empty
    End Function

    ''' <summary>
    ''' Return the Enum value for a flagged enum
    ''' </summary>
    ''' <param name="culture">The culture to convert using</param>
    ''' <param name="text">The text to convert</param>
    ''' <returns>The enum value</returns>
    Private Function GetFlagValue(ByVal culture As CultureInfo, ByVal text As String) As Object
      Dim lookupTable As LookupTable = GetLookupTable(culture)
      Dim textValues As String() = text.Split(","c)
      Dim result As ULong = 0

      For Each textValue As String In textValues
        Dim value As Object = Nothing
        Dim trimmedTextValue As String = textValue.Trim()

        If (Not lookupTable.TryGetValue(trimmedTextValue, value)) Then
          Return Nothing
        End If

        result = result Or Convert.ToUInt32(value)
      Next

      Return System.Enum.ToObject(EnumType, result)
    End Function

    ''' <summary>
    ''' Return the text to display for a flag value in the given culture
    ''' </summary>
    ''' <param name="culture">The culture to get the text for</param>
    ''' <param name="value">The flag enum value to get the text for</param>
    ''' <returns>The localized text</returns>
    Private Function GetFlagValueText(ByVal culture As CultureInfo, ByVal value As Object) As String
      If (System.Enum.IsDefined(value.GetType(), value)) Then
        Return GetValueText(culture, value)
      End If

      Dim lValue As Long = System.Convert.ToInt32(value)
      Dim result As String = Nothing

      For Each flagValue As Object In _FlagValues
        Dim lFlagValue As Long = System.Convert.ToInt32(flagValue)

        If IsSingleBitValue(lFlagValue) Then
          If (lFlagValue And lValue) = lFlagValue Then
            Dim valueText As String = GetValueText(culture, flagValue)

            If result Is Nothing Then
              result = valueText
            Else
              result = String.Format("{0}+{1}", result, valueText)
            End If
          End If
        End If
      Next

      Return result
    End Function

    ''' <summary>
    ''' Get the lookup table for the given culture (creating if necessary)
    ''' </summary>
    Private Function GetLookupTable(ByVal culture As CultureInfo) As LookupTable
      Dim result As LookupTable = Nothing

      If (culture Is Nothing) Then
        culture = CultureInfo.CurrentCulture
      End If

      If (Not _LookupTables.TryGetValue(culture, result)) Then
        result = New LookupTable()

        For Each value As Object In GetStandardValues()
          Dim text As String = GetValueText(culture, value)

          If (Not text Is Nothing) Then
            result.Add(text, value)
          End If
        Next

        _LookupTables.Add(culture, result)
      End If

      Return result
    End Function

    ''' <summary>
    ''' Return the Enum value for a simple (non-flagged enum)
    ''' </summary>
    ''' <param name="culture">The culture to convert using</param>
    ''' <param name="text">The text to convert</param>
    ''' <returns>The enum value</returns>
    Private Function GetValue(ByVal culture As CultureInfo, ByVal text As String) As Object
      Dim lookupTable As LookupTable = GetLookupTable(culture)
      Dim result As Object = Nothing

      lookupTable.TryGetValue(text, result)

      Return result
    End Function

    ''' <summary>
    ''' Return the text to display for a simple value in the given culture
    ''' </summary>
    ''' <param name="culture">The culture to get the text for</param>
    ''' <param name="value">The enum value to get the text for</param>
    ''' <returns>The localized text</returns>
    Protected MustOverride Function GetValueText(ByVal culture As CultureInfo, ByVal value As Object) As String


    ''' <summary>
    ''' Return a list of the enum values and their associated display text for the given enum type in the current UI Culture
    ''' </summary>
    ''' <param name="enumType">The enum type to get the values for</param>
    ''' <returns>
    ''' A list of KeyValuePairs where the key is the enum value and the value is the text to display
    ''' </returns>
    ''' <remarks>
    ''' This method can be used to provide localized binding to enums in ASP.NET applications.   Unlike 
    ''' windows forms the standard ASP.NET controls do not use TypeConverters to convert from enum values
    ''' to the displayed text.   You can bind an ASP.NET control to the list returned by this method by setting
    ''' the DataValueField to "Key" and theDataTextField to "Value". 
    ''' </remarks>
    Public Shared Function GetValues(ByVal enumType As Type) As List(Of KeyValuePair(Of System.Enum, String))
      Return GetValues(enumType, CultureInfo.CurrentUICulture)
    End Function

    ''' <summary>
    ''' Return a list of the enum values and their associated display text for the given enum type
    ''' </summary>
    ''' <param name="enumType">The enum type to get the values for</param>
    ''' <param name="culture">The culture to get the text for</param>
    ''' <returns>
    ''' A list of KeyValuePairs where the key is the enum value and the value is the text to display
    ''' </returns>
    ''' <remarks>
    ''' This method can be used to provide localized binding to enums in ASP.NET applications.   Unlike 
    ''' windows forms the standard ASP.NET controls do not use TypeConverters to convert from enum values
    ''' to the displayed text.   You can bind an ASP.NET control to the list returned by this method by setting
    ''' the DataValueField to "Key" and theDataTextField to "Value". 
    ''' </remarks>
    Public Shared Function GetValues(ByVal enumType As System.Type, ByVal culture As CultureInfo) As List(Of KeyValuePair(Of System.Enum, String))
      Dim result As New List(Of KeyValuePair(Of System.Enum, String))()
      Dim converter As TypeConverter = TypeDescriptor.GetConverter(enumType)

      For Each value As System.Enum In System.Enum.GetValues(enumType)
        Dim pair As New KeyValuePair(Of System.Enum, String)(value, converter.ConvertToString(Nothing, culture, value))

        result.Add(pair)
      Next

      Return result
    End Function

    ''' <summary>
    ''' Return true if the given value is can be represented using a single bit
    ''' </summary>
    Private Function IsSingleBitValue(ByVal value As Long) As Boolean
      Select Case value
        Case 0
          Return False
        Case 1
          Return True
      End Select

      Return ((value And (value - 1)) = 0)
    End Function

#End Region

  End Class

End Namespace
