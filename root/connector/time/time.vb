
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _time
    'minutes to seconds, *60
    'microseconds to milliseconds, *1000
    'ticks to milliseconds, /10000
    'milliseconds to seconds, /1000

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function seconds(ByVal d As DateTime) As Int64
        Return ticks_to_seconds(d.Ticks())
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function milliseconds(ByVal d As DateTime) As Int64
        Return ticks_to_milliseconds(d.Ticks())
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function milliseconds_to_microseconds(ByVal ms As Int64) As Int64
        Const ratio As Int64 = milli_micro
        Const imax As Int64 = max_int64 \ ratio
        Const imin As Int64 = min_int64 \ ratio
        If ms > imax Then
            Return max_int64
        End If
        If ms < imin Then
            Return min_int64
        End If
        Try
            Return ms * ratio
        Catch
            assert(False)
            Return 0
        End Try
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function microseconds_to_milliseconds(ByVal ms As Int64) As Int64
        Return ms \ milli_micro
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function milliseconds_to_ticks(ByVal ms As Int64) As Int64
        Const ratio As Int64 = milli_tick
        Const imax As Int64 = max_int64 \ ratio
        Const imin As Int64 = min_int64 \ ratio
        If ms > imax Then
            Return max_int64
        End If
        If ms < imin Then
            Return min_int64
        End If
        Try
            Return ms * ratio
        Catch
            assert(False)
            Return 0
        End Try
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function milliseconds_to_ticks(ByVal ms As UInt64) As UInt64
        assert(ms <= max_int64)
        Dim r As Int64 = 0
        r = milliseconds_to_ticks(CLng(ms))
        assert(r >= 0)
        Return CULng(r)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function ticks_to_milliseconds(ByVal ticks As Int64) As Int64
        Return ticks \ milli_tick
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function ticks_to_milliseconds(ByVal ticks As UInt64) As UInt64
        Const uint_milli_tick As UInt32 = milli_tick
        Return ticks \ uint_milli_tick
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function milliseconds_to_seconds(ByVal ms As Int64) As Int64
        Return ms \ second_milli
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function milliseconds_to_seconds(ByVal ms As UInt64) As UInt64
        Const uint_second_milli As UInt32 = second_milli
        Return ms \ uint_second_milli
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function seconds_to_milliseconds(ByVal s As Int64) As Int64
        Const ratio As Int64 = second_milli
        Const imax As Int64 = max_int64 \ ratio
        Const imin As Int64 = min_int64 \ ratio
        If s > imax Then
            Return max_int64
        End If
        If s < imin Then
            Return min_int64
        End If
        Try
            Return s * ratio
        Catch
            assert(False)
            Return 0
        End Try
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function seconds_to_milliseconds(ByVal s As UInt64) As UInt64
        assert(s <= max_int64)
        Dim r As Int64 = 0
        r = seconds_to_milliseconds(CLng(s))
        assert(r >= 0)
        Return CULng(r)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function ticks_to_seconds(ByVal ticks As Int64) As Int64
        Return ticks \ second_milli \ milli_tick
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function seconds_to_ticks(ByVal s As Int64) As Int64
        Const ratio As Int64 = second_milli * milli_tick
        Const imax As Int64 = max_int64 \ ratio
        Const imin As Int64 = min_int64 \ ratio
        If s > imax Then
            Return max_int64
        End If
        If s < imin Then
            Return min_int64
        End If
        Try
            Return s * ratio
        Catch
            assert(False)
            Return 0
        End Try
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function minutes_to_seconds(ByVal m As Int64) As Int64
        Const ratio As Int64 = minute_second
        Const imax As Int64 = max_int64 \ ratio
        Const imin As Int64 = min_int64 \ ratio
        If m > imax Then
            Return max_int64
        End If
        If m < imin Then
            Return min_int64
        End If
        Try
            Return m * ratio
        Catch
            assert(False)
            Return 0
        End Try
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function minutes_to_milliseconds(ByVal m As Int64) As Int64
        Const ratio As Int64 = minute_second * second_milli
        Const imax As Int64 = max_int64 \ ratio
        Const imin As Int64 = min_int64 \ ratio
        If m > imax Then
            Return max_int64
        End If
        If m < imin Then
            Return min_int64
        End If
        Try
            Return m * ratio
        Catch
            assert(False)
            Return 0
        End Try
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function environment_milliseconds() As Int32
        Return Environment.TickCount()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function environment_milliseconds_uint32() As UInt32
        Return int32_uint32(environment_milliseconds())
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function long_time(ByVal d As Date) As String
        Return strcat(d.ToLongDateString(), character.comma, character.blank, d.ToLongTimeString())
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function long_time(Optional ByVal timezone As Int64 = max_int64) As String
        Return long_time(timezone_date(timezone))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function short_time(ByVal d As Date,
                                             Optional ByVal datetimeSeperator As String = character.blank,
                                             Optional ByVal dateSeperator As String = character.minus_sign,
                                             Optional ByVal timeSeperator As String = character.colon) As String
        Return strcat(strright(Convert.ToString(d.Year), 2),
                      dateSeperator,
                      strrfill(Convert.ToString(d.Month), character._0, 2),
                      dateSeperator,
                      strrfill(Convert.ToString(d.Day), character._0, 2),
                      datetimeSeperator,
                      strrfill(Convert.ToString(d.Hour), character._0, 2),
                      timeSeperator,
                      strrfill(Convert.ToString(d.Minute), character._0, 2),
                      timeSeperator,
                      strrfill(Convert.ToString(d.Second), character._0, 2))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function short_time(Optional ByVal datetime_seperator As String = character.blank,
                               Optional ByVal date_seperator As String = character.minus_sign,
                               Optional ByVal time_seperator As String = character.colon,
                               Optional ByVal timezone As Int64 = max_int64) As String
        Return short_time(timezone_date(timezone), datetime_seperator, date_seperator, time_seperator)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function timezone_date(Optional ByVal timezone As Int64 = max_int64) As Date
        Return timezone_date(DateTime.Now(), timezone)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function timezone_date(ByVal d As Date, Optional ByVal timezone As Int64 = max_int64) As Date
        If timezone = max_int64 Then
            'if no timezone defined, just use local time
            Return d
        End If
        Return d.ToUniversalTime().AddMinutes(timezone)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function to_gmt_time(ByVal d As Date) As String
        Return d.ToString("r")
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function thisyear(ByVal d As Date) As Date
        d = thismonth(d)
        'month is 1-based
        Return d.AddMonths(-d.Month() + 1)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function lastyear(ByVal d As Date) As Date
        d = thisyear(d)
        Return d.AddYears(-1)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function thisyear(ByVal tick As Int64) As Int64
        Return thisyear(New Date(tick)).Ticks()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function lastyear(ByVal tick As Int64) As Int64
        Return lastyear(New Date(tick)).Ticks()
    End Function

    'return 00:00 of the first day of this month
    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function thismonth(ByVal d As Date) As Date
        d = thisday(d)
        'day is 1-based
        Return d.AddDays(-d.Day() + 1)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function lastmonth(ByVal d As Date) As Date
        d = thismonth(d)
        Return d.AddMonths(-1)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function thismonth(ByVal tick As Int64) As Int64
        Return thismonth(New Date(tick)).Ticks()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function lastmonth(ByVal tick As Int64) As Int64
        Return lastmonth(New Date(tick)).Ticks()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function thisweek(ByVal d As Date) As Date
        d = thisday(d)
        Return d.AddDays(-d.DayOfWeek())
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function lastweek(ByVal d As Date) As Date
        d = thisweek(d)
        Return d.AddDays(-days_in_week)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function thisweek(ByVal tick As Int64) As Int64
        Return thisweek(New Date(tick)).Ticks()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function lastweek(ByVal tick As Int64) As Int64
        Return lastweek(New Date(tick)).Ticks()
    End Function

    'return 00:00 of this day
    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function thisday(ByVal d As Date) As Date
        d = thishour(d)
        Return d.AddHours(-d.Hour())
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function lastday(ByVal d As Date) As Date
        d = thisday(d)
        Return d.AddDays(-1)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function thisday(ByVal tick As Int64) As Int64
        Return thisday(New Date(tick)).Ticks()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function lastday(ByVal tick As Int64) As Int64
        Return lastday(New Date(tick)).Ticks()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function thishour(ByVal d As Date) As Date
        d = thisminute(d)
        Return d.AddMinutes(-d.Minute())
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function lasthour(ByVal d As Date) As Date
        d = thishour(d)
        Return d.AddHours(-1)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function thishour(ByVal tick As Int64) As Int64
        Return thishour(New Date(tick)).Ticks()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function lasthour(ByVal tick As Int64) As Int64
        Return lasthour(New Date(tick)).Ticks()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function thisminute(ByVal d As Date) As Date
        d = thissecond(d)
        Return d.AddSeconds(-d.Second())
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function lastminute(ByVal d As Date) As Date
        d = thisminute(d)
        Return d.AddMinutes(-1)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function thisminute(ByVal tick As Int64) As Int64
        Return thisminute(New Date(tick)).Ticks()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function lastminute(ByVal tick As Int64) As Int64
        Return lastminute(New Date(tick)).Ticks()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function thissecond(ByVal d As Date) As Date
        d = thismillisecond(d)
        Return d.AddMilliseconds(-d.Millisecond())
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function lastsecond(ByVal d As Date) As Date
        d = thissecond(d)
        Return d.AddSeconds(-1)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function thissecond(ByVal tick As Int64) As Int64
        Return thissecond(New Date(tick)).Ticks()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function lastsecond(ByVal tick As Int64) As Int64
        Return lastsecond(New Date(tick)).Ticks()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function thismillisecond(ByVal d As Date) As Date
        Static ticks_of_milliseconds As Int64 = milliseconds_to_ticks(1)
        Return d.AddTicks(-(d.Ticks() Mod ticks_of_milliseconds))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function lastmillisecond(ByVal d As Date) As Date
        d = thismillisecond(d)
        Return d.AddMilliseconds(-1)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function thismillisecond(ByVal tick As Int64) As Int64
        Return thismillisecond(New Date(tick)).Ticks()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function lastmillisecond(ByVal tick As Int64) As Int64
        Return lastmillisecond(New Date(tick)).Ticks()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function high_res_ticks() As Int64
        Return high_res_ticks_retriever.ticks()
    End Function

#If PocketPC OrElse Smartphone Then
    <Extension()> Public Function DateTryParse(ByVal input As String, ByRef o As Date) As Boolean
        Try
            o = Date.Parse(input)
        Catch ex As Exception
            Return False
        End Try

        Return True
    End Function
#End If

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function rate_to_ms(ByVal rate_sec As UInt32,
                               ByVal count As UInt64,
                               Optional ByVal min_value As Int64 = 256) As Int64
        If rate_sec = 0 Then
            Return npos
        End If
        Return max(CLng(seconds_to_milliseconds(If(count = 0, uint64_1, count)) \ rate_sec), min_value)
    End Function
End Module
