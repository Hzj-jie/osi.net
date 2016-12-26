
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.utils

Public MustInherit Class measurement_case_wrapper
    Inherits case_wrapper

    Private ReadOnly interval_ms As Int64 = 0
    Private _max As Int64 = min_int64
    Private _min As Int64 = max_int64
    Private _count As Int64 = 0
    Private _average As Int64 = 0
    Private _last_sample As Int64 = 0
    Private _fs As Int64 = 0
    Private sampling As singleentry

    Protected Sub New(ByVal c As [case], Optional ByVal interval_ms As Int64 = second_milli)
        MyBase.New(c)
        Me.interval_ms = interval_ms
    End Sub

    Protected MustOverride Function sample() As Int64

    Public Function max_sample() As Int64
        Return _max
    End Function

    Public Function min_sample() As Int64
        Return _min
    End Function

    Public Function sample_count() As Int64
        Return _count
    End Function

    Public Function average_sample() As Int64
        Return _average
    End Function

    Public Function first_sample() As Int64
        Return _fs
    End Function

    Protected Overridable Function max_lower_bound() As Int64
        Return min_int64
    End Function

    Protected Overridable Function max_upper_bound() As Int64
        Return max_int64
    End Function

    Protected Overridable Function min_lower_bound() As Int64
        Return min_int64
    End Function

    Protected Overridable Function min_upper_bound() As Int64
        Return max_int64
    End Function

    Protected Overridable Function count_lower_bound() As Int64
        Return min_int64
    End Function

    Protected Overridable Function count_upper_bound() As Int64
        Return max_int64
    End Function

    Protected Overridable Function average_lower_bound() As Int64
        Return min_int64
    End Function

    Protected Overridable Function average_upper_bound() As Int64
        Return max_int64
    End Function

    Protected Overridable Function last_sample_lower_bound() As Int64
        Return min_int64
    End Function

    Protected Overridable Function last_sample_upper_bound() As Int64
        Return max_int64
    End Function

    Private Function assert_less(Of T)(ByVal i As T,
                                       ByVal j As T,
                                       ByVal name As String)
        Return utt.assert_less(i, j,
                               "in case ", MyBase.name, ", value ", name, " is larger than ", name, "_upper_bound")
    End Function

    Private Function assert_more(Of T)(ByVal i As T,
                                       ByVal j As T,
                                       ByVal name As String)
        Return utt.assert_more(i, j,
                               "in case ", MyBase.name, ", value ", name, " is smaller than ", name, "_lower_bound")
    End Function

    Public NotOverridable Overrides Function preserved_processors() As Int16
        Return Environment.ProcessorCount()
    End Function

    Public NotOverridable Overrides Function run() As Boolean
        Dim finished As atomic_bool = Nothing
        finished = New atomic_bool()
        _fs = sample()
        stopwatch.repeat(interval_ms,
                         Function() As Boolean
                             assert(sampling.mark_in_use())
                             _last_sample = sample()
                             If _last_sample > _max Then
                                 _max = _last_sample
                             End If
                             If _last_sample < _min Then
                                 _min = _last_sample
                             End If
                             _average = _average * _count + _last_sample
                             _count += 1
                             _average /= _count
                             sampling.release()
                             Return Not +finished
                         End Function)

        Dim rtn As Boolean = False
        rtn = MyBase.run()
        finished.set(True)
        sleep(interval_ms)
        lazy_wait_when(Function() sampling.in_use())
        raise_error("finished measurement case ",
                    name,
                    ", max = ",
                    _max,
                    ", min = ",
                    _min,
                    ", count = ",
                    _count,
                    ", average = ",
                    _average)
        assert_less(_max, max_upper_bound(), "max")
        assert_more(_max, max_lower_bound(), "max")
        assert_less(_min, min_upper_bound(), "min")
        assert_more(_min, min_lower_bound(), "min")
        assert_less(_count, count_upper_bound(), "count")
        assert_more(_count, count_lower_bound(), "count")
        assert_less(_average, average_upper_bound(), "average")
        assert_more(_average, average_lower_bound(), "average")
        assert_less(_last_sample, last_sample_upper_bound(), "last_sample")
        assert_more(_last_sample, last_sample_lower_bound(), "last_sample")
        Return rtn
    End Function
End Class
