Imports System
Imports System.ComponentModel
Imports System.Globalization
Imports System.Linq

Namespace System

  Public Module PublicHolidayCalculator

#Region " Non-Workday Evaluation "

    ''' <summary>
    ''' Evaluates only these days which are non-wokdays in all german regions
    ''' </summary>
    Public Function IsNonWorkday(targetDate As DateTime, Optional includeSaturdays As Boolean = True, Optional includeSundays As Boolean = True) As Boolean
      If (includeSaturdays AndAlso targetDate.DayOfWeek = DayOfWeek.Saturday) Then
        Return True
      End If
      If (includeSundays AndAlso targetDate.DayOfWeek = DayOfWeek.Sunday) Then
        Return True
      End If
      Dim hd As GermanPublicHoliday
      If (EvaluateHoliday(targetDate, hd)) Then
        Return IsNonWorkday(hd)
      Else
        Return False
      End If
    End Function

    Public Function IsNonWorkday(targetDate As DateTime, region As GermanRegion, Optional includeSaturdays As Boolean = True, Optional includeSundays As Boolean = True) As Boolean
      If (includeSaturdays AndAlso targetDate.DayOfWeek = DayOfWeek.Saturday) Then
        Return True
      End If
      If (includeSundays AndAlso targetDate.DayOfWeek = DayOfWeek.Sunday) Then
        Return True
      End If
      Dim hd As GermanPublicHoliday
      If (EvaluateHoliday(targetDate, hd)) Then
        Return IsNonWorkday(hd, region)
      Else
        Return False
      End If
    End Function

    ''' <summary>
    ''' Evaluates only these days which are non-wokdays in all german regions
    ''' </summary>
    Public Function IsNonWorkday(holiday As GermanPublicHoliday) As Boolean
      Select Case holiday
        Case GermanPublicHoliday.Neujahr
        Case GermanPublicHoliday.Karfreitag
        Case GermanPublicHoliday.Ostermontag
        Case GermanPublicHoliday.TagDerArbeit
        Case GermanPublicHoliday.ChristiHimmelfahrt
        Case GermanPublicHoliday.Pfingstmontag
        Case GermanPublicHoliday.TagDerDeutschenEinheit
        Case GermanPublicHoliday.ErsterWeihnachtsfeiertag
        Case GermanPublicHoliday.ZweiterWeihnachtsfeiertag
        Case Else : Return False
      End Select
      Return True
    End Function

    Public Function IsNonWorkday(holiday As GermanPublicHoliday, region As GermanRegion) As Boolean
      If (IsNonWorkday(holiday)) Then
        Return True
      End If
      Select Case holiday
        Case GermanPublicHoliday.HeiligeDreiKönige
          Return {GermanRegion.BadenWürttemberg, GermanRegion.Bayern, GermanRegion.SachsenAnhalt}.Contains(region)
        Case GermanPublicHoliday.Ostersonntag
          Return {GermanRegion.Brandenburg}.Contains(region)
        Case GermanPublicHoliday.Pfingstsonntag
          Return {GermanRegion.Brandenburg}.Contains(region)
        Case GermanPublicHoliday.Fronleichnam
          Return {GermanRegion.BadenWürttemberg, GermanRegion.Bayern, GermanRegion.Hessen, GermanRegion.NordrheinWestfalen, GermanRegion.RheinlandPfalz, GermanRegion.Saarland}.Contains(region)
        Case GermanPublicHoliday.MariäHimmelfahrt
          Return {GermanRegion.Bayern, GermanRegion.Saarland}.Contains(region)
        Case GermanPublicHoliday.Reformationstag
          Return {GermanRegion.Brandenburg, GermanRegion.MecklenburgVorpommern, GermanRegion.Sachsen, GermanRegion.SachsenAnhalt, GermanRegion.Thüringen}.Contains(region)
        Case GermanPublicHoliday.Allerheiligen
          Return {GermanRegion.BadenWürttemberg, GermanRegion.Bayern, GermanRegion.NordrheinWestfalen, GermanRegion.RheinlandPfalz, GermanRegion.Saarland}.Contains(region)
        Case GermanPublicHoliday.BussUndBettag
          Return {GermanRegion.Sachsen}.Contains(region)
      End Select
      Return False
    End Function

#End Region

#Region " Holiday Evaluation "

    Public Function EvaluateHoliday(targetDate As DateTime) As Boolean
      Return EvaluateHoliday(targetDate, Nothing)
    End Function

    Public Function EvaluateHoliday(targetDate As DateTime, ByRef resolvedTo As GermanPublicHoliday) As Boolean
      Dim phd As DateTime

      phd = GetHoliday(targetDate.Year, GermanPublicHoliday.Ostermontag)
      If (phd.Day = targetDate.Day AndAlso phd.Month = targetDate.Month) Then
        resolvedTo = GermanPublicHoliday.Ostermontag
        Return True
      End If

      phd = GetHoliday(targetDate.Year, GermanPublicHoliday.Ostersonntag)
      If (phd.Day = targetDate.Day AndAlso phd.Month = targetDate.Month) Then
        resolvedTo = GermanPublicHoliday.Ostersonntag
        Return True
      End If

      phd = GetHoliday(targetDate.Year, GermanPublicHoliday.Rosenmontag)
      If (phd.Day = targetDate.Day AndAlso phd.Month = targetDate.Month) Then
        resolvedTo = GermanPublicHoliday.Rosenmontag
        Return True
      End If

      'TODO: Feiertage fertig ergänzen

      Return False
    End Function

    Public Function GetHoliday(year As Integer, holiday As GermanPublicHoliday) As DateTime
      Select Case holiday

        Case GermanPublicHoliday.Ostermontag
          Return GetEastern(year).AddDays(1)

        Case GermanPublicHoliday.Ostersonntag
          Return GetEastern(year)

        Case GermanPublicHoliday.Rosenmontag
          Return GetEastern(year).AddDays(-48)

        Case GermanPublicHoliday.Karfreitag
          Return GetEastern(year).AddDays(-2)

        Case GermanPublicHoliday.ChristiHimmelfahrt
          Return GetEastern(year).AddDays(39)

        Case GermanPublicHoliday.Pfingstsonntag
          Return GetEastern(year).AddDays(49)

        Case GermanPublicHoliday.Pfingstmontag
          Return GetEastern(year).AddDays(50)

        Case GermanPublicHoliday.Fronleichnam
          Return GetEastern(year).AddDays(60)

        Case Else
          'TODO: Feiertage fertig ergänzen
          Throw New NotImplementedException
      End Select

    End Function

#End Region

#Region " Helpers "

    ''' <summary>
    ''' Gets the Easter-Sunday (via Gauss Formula)
    ''' </summary>
    Private Function GetEastern(year As Integer) As Date

      Dim paramA As Long
      Dim paramK As Long
      Dim paramM As Long
      Dim paramD As Long
      Dim paramS As Long
      Dim paramR As Long
      Dim paramOG As Long
      Dim paramSZ As Long
      Dim paramOE As Long
      Dim paramOS As Long

      ' 1. die Säkularzahl
      paramK = year \ 100
      ' 2. die säkulare Mondschaltung
      paramM = 15 + (3 * paramK + 3) \ 4 - (8 * paramK + 13) \ 25
      ' 3. die säkulare Sonnenschaltung
      paramS = 2 - (3 * paramK + 3) \ 4
      ' 4. den Mondparameter
      paramA = year Mod 19
      ' 5. den Keim für den ersten Vollmond im Frühling
      paramD = (19 * paramA + paramM) Mod 30
      ' 6. die kalendarische Korrekturgröße
      paramR = (paramD + paramA \ 11) \ 29
      ' 7. die Ostergrenze
      paramOG = 21 + paramD - paramR
      ' 8. den ersten Sonntag im März
      paramSZ = 7 - (year + year \ 4 + paramS) Mod 7
      ' 9. die Entfernung des Ostersonntags von der Ostergrenze (Osterentfernung in Tagen)
      paramOE = 7 - (paramOG - paramSZ) Mod 7
      '10. das Datum des Ostersonntags als Märzdatum (32. März = 1. April usw.)
      paramOS = paramOG + paramOE

      If (paramOS > 31) Then
        Return New DateTime(year, 4, CInt(paramOS - 31))
      Else
        Return New DateTime(year, 3, CInt(paramOS))
      End If

    End Function

#End Region

  End Module

End Namespace
