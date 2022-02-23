
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt
Imports osi.root.formation

Public Class generic_perf
    Inherits [case]

    Private ReadOnly test_size_scale As Int64

    Public Sub New()
        Me.New(1)
    End Sub

    Public Sub New(ByVal test_size_scale As Int64)
        assert(test_size_scale >= 1)
        Me.test_size_scale = test_size_scale
    End Sub

    Private Class generic_static_class(Of T)
        Public Shared v As T
    End Class

    Private Class generic_class(Of T)
        Public v As T
    End Class

    Private Class generic_static_struct(Of T)
        Public Shared v As T
    End Class

    Private Class generic_struct(Of T)
        Public v As T
    End Class

    Private Structure int_struct
        Public v As Int32
    End Structure

    Private Structure int_static_struct
        Public Shared v As Int32
    End Structure

    Private Class int_class
        Public v As Int32
    End Class

    Private Class int_static_class
        Public Shared v As Int32
    End Class

    Public Overrides Function reserved_processors() As Int16
        Return Environment.ProcessorCount()
    End Function

    Private Sub report_performance(ByVal p As ref(Of Int64), ByVal c As String)
        assert(p IsNot Nothing)
        raise_error(error_type.performance,
                    "generic_perf * ",
                    test_size_scale,
                    " test uses ",
                    +p,
                    " processor loops to ",
                    c)
    End Sub

    Public Overrides Function run() As Boolean
        Dim alloc_count As Int64 = 0
        alloc_count = test_size_scale * 1024 * 1024 * 128
        Dim read_count As Int64 = 0
        read_count = test_size_scale * 1024 * 1024 * 1024
        Dim write_count As Int64 = 0
        write_count = test_size_scale * 1024 * 1024 * 1024
        Dim p As ref(Of Int64) = Nothing
        p = New ref(Of Int64)()

        Using New boost()
            Using New processor_loops_timing_counter(p)
                Dim t As generic_class(Of Int32) = Nothing
                For i As Int64 = 0 To alloc_count - 1
                    t = New generic_class(Of Int32)()
                Next
            End Using
            report_performance(p, strcat("allocate generic_class(Of Int32) * ", alloc_count))

            Using New processor_loops_timing_counter(p)
                Dim t As generic_struct(Of Int32) = Nothing
                For i As Int64 = 0 To alloc_count - 1
                    t = New generic_struct(Of Int32)()
                Next
            End Using
            report_performance(p, strcat("allocate generic_struct(Of Int32) * ", alloc_count))

            Using New processor_loops_timing_counter(p)
                Dim t As int_class = Nothing
                For i As Int64 = 0 To alloc_count - 1
                    t = New int_class()
                Next
            End Using
            report_performance(p, strcat("allocate int_class * ", alloc_count))

            Using New processor_loops_timing_counter(p)
                Dim t As int_struct = Nothing
                For i As Int64 = 0 To alloc_count - 1
                    t = New int_struct()
                Next
            End Using
            report_performance(p, strcat("allocate int_struct * ", alloc_count))

            Using New processor_loops_timing_counter(p)
                Dim t As generic_class(Of Int32) = Nothing
                t = New generic_class(Of Int32)()
                For i As Int64 = 0 To read_count - 1
                    Dim v As Int32 = 0
                    v = t.v
                Next
            End Using
            report_performance(p, strcat("read value from generic_class(Of Int32) * ", read_count))

            Using New processor_loops_timing_counter(p)
                Dim t As generic_struct(Of Int32) = Nothing
                t = New generic_struct(Of Int32)()
                For i As Int64 = 0 To read_count - 1
                    Dim v As Int32 = 0
                    v = t.v
                Next
            End Using
            report_performance(p, strcat("read value from generic_struct(Of Int32) * ", read_count))

            Using New processor_loops_timing_counter(p)
                For i As Int64 = 0 To read_count - 1
                    Dim v As Int32 = 0
                    v = generic_static_class(Of Int32).v
                Next
            End Using
            report_performance(p, strcat("read value from generic_static_class(Of Int32) * ", read_count))

            Using New processor_loops_timing_counter(p)
                For i As Int64 = 0 To read_count - 1
                    Dim v As Int32 = 0
                    v = generic_static_struct(Of Int32).v
                Next
            End Using
            report_performance(p, strcat("read value from generic_static_struct(Of Int32) * ", read_count))

            Using New processor_loops_timing_counter(p)
                Dim t As int_class = Nothing
                t = New int_class()
                For i As Int64 = 0 To read_count - 1
                    Dim v As Int32 = 0
                    v = t.v
                Next
            End Using
            report_performance(p, strcat("read value from int_class * ", read_count))

            Using New processor_loops_timing_counter(p)
                For i As Int64 = 0 To read_count - 1
                    Dim v As Int32 = 0
                    v = int_static_class.v
                Next
            End Using
            report_performance(p, strcat("read value from int_static_class * ", read_count))

            Using New processor_loops_timing_counter(p)
                Dim t As int_struct = Nothing
                t = New int_struct()
                For i As Int64 = 0 To read_count - 1
                    Dim v As Int32 = 0
                    v = t.v
                Next
            End Using
            report_performance(p, strcat("read value from int_struct * ", read_count))

            Using New processor_loops_timing_counter(p)
                For i As Int64 = 0 To read_count - 1
                    Dim v As Int32 = 0
                    v = int_static_struct.v
                Next
            End Using
            report_performance(p, strcat("read value from int_static_struct * ", read_count))

            Using New processor_loops_timing_counter(p)
                Dim t As generic_class(Of Int32) = Nothing
                t = New generic_class(Of Int32)()
                For i As Int64 = 0 To write_count - 1
                    t.v = max_int32
                Next
            End Using
            report_performance(p, strcat("write value to generic_class(Of Int32) * ", write_count))

            Using New processor_loops_timing_counter(p)
                Dim t As generic_struct(Of Int32) = Nothing
                t = New generic_struct(Of Int32)()
                For i As Int64 = 0 To write_count - 1
                    t.v = max_int32
                Next
            End Using
            report_performance(p, strcat("write value to generic_struct(Of Int32) * ", write_count))

            Using New processor_loops_timing_counter(p)
                For i As Int64 = 0 To write_count - 1
                    generic_static_class(Of Int32).v = max_int32
                Next
            End Using
            report_performance(p, strcat("write value to generic_static_class(Of Int32) * ", write_count))

            Using New processor_loops_timing_counter(p)
                For i As Int64 = 0 To write_count - 1
                    generic_static_struct(Of Int32).v = max_int32
                Next
            End Using
            report_performance(p, strcat("write value to generic_static_struct(Of Int32) * ", write_count))

            Using New processor_loops_timing_counter(p)
                Dim t As int_class = Nothing
                t = New int_class()
                For i As Int64 = 0 To write_count - 1
                    t.v = max_int32
                Next
            End Using
            report_performance(p, strcat("write value to int_class * ", write_count))

            Using New processor_loops_timing_counter(p)
                For i As Int64 = 0 To write_count - 1
                    int_static_class.v = max_int32
                Next
            End Using
            report_performance(p, strcat("write value to int_static_class * ", write_count))

            Using New processor_loops_timing_counter(p)
                Dim t As int_struct = Nothing
                t = New int_struct()
                For i As Int64 = 0 To write_count - 1
                    t.v = max_int32
                Next
            End Using
            report_performance(p, strcat("write value to int_struct * ", write_count))

            Using New processor_loops_timing_counter(p)
                For i As Int64 = 0 To write_count - 1
                    int_static_struct.v = max_int32
                Next
            End Using
            report_performance(p, strcat("write value to int_static_struct * ", write_count))
        End Using
        Return True
    End Function
End Class
