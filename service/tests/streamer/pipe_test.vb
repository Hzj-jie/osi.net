
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.constants
Imports osi.service.streamer

Public Class pipe_test
    Inherits [case]

    Private Shared Function basic_case() As Boolean
        Const default_value As Int32 = 100
        Dim q As pipe(Of Int32) = Nothing
        q = New pipe(Of Int32)(1, 1000, False)
        assertion.is_true(async_sync(q.push(default_value)))
        assertion.equal(q.size(), uint32_1)
        assertion.is_false(async_sync(q.push(default_value)))
        assertion.equal(q.size(), uint32_1)
        assertion.is_false(async_sync(q.push(default_value)))
        assertion.equal(q.size(), uint32_1)
        Dim p As ref(Of Int32) = Nothing
        p = New ref(Of Int32)()
        assertion.is_true(async_sync(q.pop(p)))
        assertion.equal(+p, default_value)
        assertion.equal(q.size(), uint32_0)
        assertion.is_false(async_sync(q.pop(Nothing)))
        assertion.equal(q.size(), uint32_0)
        assertion.is_false(async_sync(q.pop(Nothing)))
        assertion.equal(q.size(), uint32_0)
        Return True
    End Function

    Private Shared Function basic_case2() As Boolean
        Dim v() As Int32 = Nothing
        v = rnd_ints(rnd_int(1024, 2048))
        Dim q As pipe(Of Int32) = Nothing
        q = New pipe(Of Int32)(array_size(v), 0, False)
        For i As Int32 = 0 To array_size(v) - 1
            assertion.is_true(q.sync_push(v(i)))
        Next
        For i As Int32 = 0 To array_size(v) - 1
            Dim o As Int32 = 0
            assertion.is_true(q.sync_pop(o))
            assertion.equal(o, v(i))
        Next
        Return True
    End Function

    Private Shared Function multi_procedure_case() As Boolean
        Const default_timeout_ms As Int64 = 1000
        Const default_value As Int32 = 100
        Dim q As pipe(Of Int32) = Nothing
        q = New pipe(Of Int32)(1, npos, True)
        assertion.is_true(async_sync(q.push(default_value)))
        assert_begin(New event_comb(Function() As Boolean
                                        Return waitfor(default_timeout_ms) AndAlso
                                               goto_next()
                                    End Function,
                                    Function() As Boolean
                                        Return assertion.is_true(begin(q.pop(Nothing))) AndAlso
                                               goto_end()
                                    End Function))
        Using assertion.timelimited_operation(default_timeout_ms, default_timeout_ms * 2)
            assertion.is_true(async_sync(q.push(default_value)))
        End Using
        assertion.equal(q.size(), uint32_1)
        Dim p As ref(Of Int32) = Nothing
        p = New ref(Of Int32)()
        assertion.is_true(async_sync(q.pop(p)))
        assertion.equal(+p, default_value)
        assert_begin(New event_comb(Function() As Boolean
                                        Return waitfor(default_timeout_ms) AndAlso
                                               goto_next()
                                    End Function,
                                    Function() As Boolean
                                        Return assertion.is_true(begin(q.push(default_value))) AndAlso
                                               goto_end()
                                    End Function))
        Using assertion.timelimited_operation(default_timeout_ms, default_timeout_ms * 2)
            assertion.is_true(async_sync(q.pop(Nothing)))
        End Using
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return basic_case() AndAlso
               basic_case2() AndAlso
               multi_procedure_case()
    End Function
End Class
