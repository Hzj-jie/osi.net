
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils

Partial Public Class big_int
    Public Function less(ByVal that As big_uint) As Boolean
        Return compare(that) < 0
    End Function

    Public Function less(ByVal that As big_int) As Boolean
        Return compare(that) < 0
    End Function

    Public Function equal(ByVal that As big_uint) As Boolean
        Return compare(that) = 0
    End Function

    Public Function equal(ByVal that As big_int) As Boolean
        Return compare(that) = 0
    End Function

    Public Function less_or_equal(ByVal that As big_uint) As Boolean
        Return compare(that) <= 0
    End Function

    Public Function less_or_equal(ByVal that As big_int) As Boolean
        Return compare(that) <= 0
    End Function

    Public Function compare(ByVal that As big_uint) As Int32
        Return compare(Me, that)
    End Function

    Public Function compare(ByVal that As big_int) As Int32
        Return compare(Me, that)
    End Function

    Public Shared Function compare(ByVal this As big_int, ByVal that As big_int) As Int32
        Dim c As Int32 = 0
        c = object_compare(this, that)
        If c = object_compare_undetermined Then
            assert(Not this Is Nothing)
            assert(Not that Is Nothing)
            If this.positive() <> that.positive() Then
                Return If(this.positive(), 1, -1)
            Else
                Dim r As Int32 = 0
                r = this.d.compare(that.d)
                assert(r <> min_int32)
                Return If(this.positive(), r, -r)
            End If
        Else
            Return c
        End If
    End Function

    Public Shared Function compare(ByVal this As big_uint, ByVal that As big_int) As Int32
        Return compare(share(this), that)
    End Function

    Public Shared Function compare(ByVal this As big_int, ByVal that As big_uint) As Int32
        Return compare(this, share(that))
    End Function
End Class
