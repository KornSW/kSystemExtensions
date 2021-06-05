Imports System.Runtime.CompilerServices
Imports System.ComponentModel
Imports System.Linq
Imports System.Text
Imports System
Imports System.Reflection
Imports System.Diagnostics
Imports System.Collections.Generic
Imports System.Globalization

Namespace System

  Public Module DateTimeExtensions

#Region " ItemsPerSecond "

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function ItemsPerSecond(processingStartTime As DateTime, alreadyProcessedItemCount As Double) As Integer
      Return DateTime.Now.Subtract(processingStartTime).ItemsPerSecond(alreadyProcessedItemCount)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function ItemsPerSecond(processingStartTime As DateTime, alreadyProcessedItemCount As Double, roundDigits As Integer) As Double
      Return DateTime.Now.Subtract(processingStartTime).ItemsPerSecond(alreadyProcessedItemCount, roundDigits)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ItemsPerSecond(processingDuration As TimeSpan, totalProcessedItemCount As Double) As Integer
      Return CInt(processingDuration.ItemsPerSecond(totalProcessedItemCount, 0))
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ItemsPerSecond(processingDuration As TimeSpan, totalProcessedItemCount As Double, roundDigits As Integer) As Double
      Dim totalSeconds As Double

      If (totalProcessedItemCount <= 0) Then
        Return 0
      End If

      totalSeconds = (processingDuration.TotalMilliseconds / 1000)

      If (totalSeconds <= 0) Then
        Return totalProcessedItemCount
      End If

      Return Math.Round(totalProcessedItemCount / totalSeconds, roundDigits)
    End Function

#End Region

#Region " PublicHolidays & Workdays (for Germany) "

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function IsGermanPublicHoliday(ByVal extendee As DateTime) As Boolean
      Return PublicHolidayCalculator.EvaluateHoliday(extendee)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function IsGermanPublicHoliday(ByVal extendee As DateTime, ByRef resolvedTo As GermanPublicHoliday) As Boolean
      Return PublicHolidayCalculator.EvaluateHoliday(extendee, resolvedTo)
    End Function

    ''' <summary>
    ''' Evaluates only these days which are non-wokdays in all german regions
    ''' </summary>
    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function IsWorkdayInGermany(ByVal extendee As DateTime) As Boolean
      Return Not PublicHolidayCalculator.IsNonWorkday(extendee, True, True)
    End Function

    ''' <summary>
    ''' Evaluates only these days which are non-wokdays in all german regions
    ''' </summary>
    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function IsWorkdayInGermany(ByVal extendee As DateTime, Optional excludeSaturdays As Boolean = True) As Boolean
      Return Not PublicHolidayCalculator.IsNonWorkday(extendee, excludeSaturdays, True)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function IsWorkdayInGermany(ByVal extendee As DateTime, region As GermanRegion) As Boolean
      Return Not PublicHolidayCalculator.IsNonWorkday(extendee, region, True, True)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function IsWorkdayInGermany(ByVal extendee As DateTime, region As GermanRegion, Optional excludeSaturdays As Boolean = True) As Boolean
      Return Not PublicHolidayCalculator.IsNonWorkday(extendee, region, excludeSaturdays, True)
    End Function

#End Region

#Region " Navigation & Evaluation "

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function SubtractDays(extendee As DateTime, value As Integer) As Date
      Return extendee.Subtract(New TimeSpan(value, 0, 0, 0))
    End Function
    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function SubtractHours(extendee As DateTime, value As Integer) As Date
      Return extendee.Subtract(New TimeSpan(value, 0, 0))
    End Function
    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function SubtractMinutes(extendee As DateTime, value As Integer) As Date
      Return extendee.Subtract(New TimeSpan(0, value, 0))
    End Function
    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function SubtractSeconds(extendee As DateTime, value As Integer) As Date
      Return extendee.Subtract(New TimeSpan(0, 0, value))
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetWeekdayName(extendee As DateTime) As String
      Return extendee.ToString("dddd")
      'Return extendee.DayOfWeek.GetWeekdayName()
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function GetMonthName(extendee As DateTime) As String
      Return extendee.ToString("MMMM")
      'Dim dayNames As String()
      'Select Case System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLower()
      '  Case "de"
      '    dayNames = {"Januar", "Februar", "M", "Mittwoch", "Donnerstag", "Freitag", "Samstag"}
      '  Case Else
      '    dayNames = {"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"}
      'End Select
      'Return dayNames(extendee.Month - 1)
    End Function

    ''' <summary>
    ''' Returns the age to the present day.
    ''' </summary>
    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function Age(ByVal extendee As DateTime) As Integer
      Return extendee.AgeAt(DateTime.Today)
    End Function

    ''' <summary>
    ''' Returns the age to the specified date.
    ''' </summary>
    <Extension(), EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Function AgeAt(ByVal extendee As DateTime, ByVal referenceDate As DateTime) As Integer
      If (referenceDate < extendee) Then
        Throw New ArgumentOutOfRangeException("The reference date value shall not be less than the current date value.")
      End If

      Return extendee.TotalYears(referenceDate)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsToday(ByVal extendee As DateTime) As Boolean
      Return (extendee.Date = DateTime.Today)
    End Function

    ''' <summary>
    ''' Returns the difference between two date values ​​in total years.
    ''' </summary>
    ''' <param name="extendee">The existing data type to be extended.</param>
    ''' <returns>The number of years between two date values.</returns>
    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function TotalYears(ByVal extendee As DateTime) As Integer
      Return TotalYears(extendee, DateTime.Now)
    End Function

    ''' <summary>
    ''' Returns the difference between two date values ​​in total years.
    ''' </summary>
    ''' <param name="extendee">The existing data type to be extended.</param>
    ''' <param name="referenceDate">The reference date value.</param>
    ''' <returns>The number of years between two date values.</returns>
    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function TotalYears(ByVal extendee As DateTime, ByVal referenceDate As DateTime) As Integer
      Dim years As Integer = 0

      If (extendee < referenceDate) Then
        years = referenceDate.Year - extendee.Year

        If ((extendee.Month > referenceDate.Month) OrElse ((extendee.Month = referenceDate.Month) AndAlso (extendee.Day > referenceDate.Day))) Then
          years -= 1
        End If
      ElseIf (extendee > referenceDate) Then
        years = extendee.Year - referenceDate.Year

        If ((extendee.Month < referenceDate.Month) OrElse ((extendee.Month = referenceDate.Month) AndAlso (extendee.Day < referenceDate.Day))) Then
          years -= 1
        End If

        years = -years
      End If

      Return years
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsAfter(ByVal extendee As DateTime, ByVal after As DateTime, Optional ByVal inclusive As Boolean = True) As Boolean
      Dim result As Integer = DateTime.Compare(extendee, after)

      Return If(inclusive, result >= 0, result > 0)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsBefore(ByVal extendee As DateTime, ByVal before As DateTime, Optional ByVal inclusive As Boolean = True) As Boolean
      Dim result As Integer = DateTime.Compare(extendee, before)

      Return If(inclusive, result <= 0, result < 0)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsA(ByVal extendee As DateTime, ByVal dayOfWeek As DayOfWeek) As Boolean
      Return extendee.DayOfWeek.Equals(dayOfWeek)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsNotA(ByVal extendee As DateTime, ByVal dayOfWeek As DayOfWeek) As Boolean
      Return Not extendee.IsA(dayOfWeek)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function FirstDayOfMonth(ByVal extendee As DateTime) As DateTime
      Return New DateTime(extendee.Year, extendee.Month, 1)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function FirstDayOfYear(ByVal extendee As DateTime) As DateTime
      Return New DateTime(extendee.Year, 1, 1)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function FirstDayOfMonth(ByVal extendee As DateTime, ByVal dayOfWeek As DayOfWeek) As DateTime
      extendee = extendee.FirstDayOfMonth

      While extendee.IsNotA(dayOfWeek)
        extendee = extendee.NextDay
      End While

      Return extendee
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function LastDayOfMonth(ByVal extendee As DateTime) As DateTime
      Return New DateTime(extendee.Year, extendee.Month, 1).AddMonths(1).AddDays(-1)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function LastDayOfYear(ByVal extendee As DateTime) As DateTime
      Return New DateTime(extendee.Year, 12, 31)
    End Function


    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function LastDayOfMonth(ByVal extendee As DateTime, ByVal dayOfWeek As DayOfWeek) As DateTime
      extendee = extendee.LastDayOfMonth

      While extendee.IsNotA(dayOfWeek)
        extendee = extendee.PreviousDay
      End While

      Return extendee
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function NextDay(ByVal extendee As DateTime) As DateTime
      Return extendee.AddDays(1)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function NextDay(ByVal extendee As DateTime, ByVal dayOfWeek As DayOfWeek) As DateTime
      If (extendee.IsA(dayOfWeek)) Then
        extendee = extendee.NextDay
      End If

      Do While extendee.IsNotA(dayOfWeek)
        extendee = extendee.NextDay
      Loop

      Return extendee
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function NextMonth(ByVal extendee As DateTime) As DateTime
      Return extendee.AddMonths(1)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function NextYear(ByVal extendee As DateTime) As DateTime
      Return extendee.AddYears(1)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function PreviousDay(ByVal extendee As DateTime) As DateTime
      Return extendee.AddDays(-1)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function PreviousDay(ByVal extendee As DateTime, ByVal dayOfWeek As DayOfWeek) As DateTime
      If (extendee.IsA(dayOfWeek)) Then
        extendee = extendee.PreviousDay
      End If

      Do While extendee.IsNotA(dayOfWeek)
        extendee = extendee.PreviousDay
      Loop

      Return extendee
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function PreviousMonth(ByVal extendee As DateTime) As DateTime
      Return extendee.AddMonths(-1)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function PreviousYear(ByVal extendee As DateTime) As DateTime
      Return extendee.AddYears(-1)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsFirstDayOfMonth(extendee As DateTime) As Boolean
      Return extendee.Equals(extendee.FirstDayOfMonth)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsFirstDayOfYear(extendee As DateTime) As Boolean
      Return extendee.Equals(extendee.FirstDayOfYear)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsBetween(ByVal extendee As DateTime, ByVal after As DateTime, ByVal before As DateTime, Optional ByVal inclusive As Boolean = True) As Boolean
      Return ((extendee.IsAfter(after, inclusive)) AndAlso (extendee.IsBefore(before, inclusive)))
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsNotBetween(ByVal extendee As DateTime, ByVal after As DateTime, ByVal before As DateTime, Optional ByVal inclusive As Boolean = True) As Boolean
      Return Not extendee.IsBetween(after, before, inclusive)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub GoToFirstDayOfNextMonth(ByRef extendee As DateTime)
      extendee = extendee.NextMonth.FirstDayOfMonth
    End Sub

    ''' <summary>
    ''' Returns the first day of the year for the current date.
    ''' </summary>
    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub GoToFirstDayOfNextYear(ByRef extendee As DateTime)
      extendee = extendee.NextYear.FirstDayOfYear
    End Sub

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Sub GoToNextDay(ByRef extendee As DateTime)
      extendee = extendee.NextDay
    End Sub

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsInSameMonth(extendee As DateTime, value As DateTime) As Boolean
      Return extendee.IsInSameYear(value) AndAlso extendee.Month.Equals(value.Month)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsInSameYear(extendee As DateTime, value As DateTime) As Boolean
      Return extendee.Year.Equals(value.Year)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Iterator Function ForEachMonth(start As DateTime, [end] As DateTime) As IEnumerable(Of DateTime)
      Yield start

      start.AddMonths(1)

      Dim current As DateTime = New DateTime(start.Year, start.Month, 1)

      While (current < [end])
        Yield current

        current = current.AddMonths(1)
      End While

      Yield [end]
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function DaysInMonth(extendee As DateTime) As Integer
      Return Date.DaysInMonth(extendee.Year, extendee.Month)
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function IsInMonth(extendee As DateTime, value As DateTime) As Boolean
      Return extendee.Year = value.Year AndAlso extendee.Month = value.Month
    End Function

#End Region

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function ToParsableISO8601(extendee As DateTime) As String
      Return extendee.ToString("yyyy-MM-dd HH':'mm':'ss")
    End Function

  End Module

End Namespace
