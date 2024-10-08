
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utt
Imports osi.service.math

Friend Class big_int_add_sub_multiply_case
    Inherits [case]

    Public Shared Function create_case(Optional ByVal round_scale As Int64 = 1) As [case]
        Return repeat(New big_int_add_sub_multiply_case(), round_scale * 1024 * If(isdebugbuild(), 1, 4))
    End Function

    Private Sub New()
        MyBase.New()
    End Sub

    Private Shared Function add_result(ByVal i As UInt64, ByVal j As UInt64) As big_int
        Dim r As big_int = Nothing
        r = New big_int()
        For k As UInt64 = 0 To j - 1
            r.add(i)
        Next
        Return r
    End Function

    Private Shared Function sub_result(ByVal i As UInt64, ByVal j As UInt64) As big_int
        Dim r As big_int = Nothing
        r = New big_int()
        For k As UInt64 = 0 To j - 1
            r.sub(i)
        Next
        Return r
    End Function

    Private Shared Function multiply_result(ByVal i As UInt64, ByVal j As Int64) As big_int
        Dim r As big_int = Nothing
        r = New big_int(i)
        r.multiply(j)
        Return r
    End Function

    Public Overrides Function run() As Boolean
        Dim i As UInt64 = 0
        Dim j As UInt64 = 0
        i = rnd_uint64(max_uint32, max_uint64)
        j = rnd_uint64(max_int8, max_uint16)
        Dim ar As big_int = Nothing
        ar = add_result(i, j)
        Dim mr As big_int = Nothing
        mr = multiply_result(i, j)
        assertion.is_true(ar.equal(mr))
        For k As UInt64 = 0 To j - 1
            ar.sub(i)
            assertion.is_true(ar.less(mr))
        Next
        assertion.is_true(ar.is_zero())

        ar = sub_result(i, j)
        mr = multiply_result(i, -j)
        assertion.is_true(ar.equal(mr))
        For k As UInt64 = 0 To j - 1
            ar.add(i)
            assertion.is_true(mr.less(ar))
        Next
        assertion.is_true(ar.is_zero())
        Return True
    End Function
End Class
