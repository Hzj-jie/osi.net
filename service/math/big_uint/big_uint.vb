
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

'TODO: consider to add a temporary value for uint32 / uint64 to big_uint convert to save object create time
Partial Public NotInheritable Class big_uint
    Private ReadOnly v As adaptive_array_uint32

    Public Sub New()
        Me.v = New adaptive_array_uint32()
    End Sub

    Public Sub New(ByVal i As UInt32)
        Me.New()
        replace_by(i)
    End Sub

    Public Sub New(ByVal i As UInt64)
        Me.New()
        replace_by(i)
    End Sub

    Public Sub New(ByVal i As big_uint)
        Me.New()
        replace_by(i)
    End Sub

    Public Sub New(ByVal i() As Byte)
        Me.New()
        replace_by(i)
    End Sub

    Public Shared Function move(ByVal i As big_uint) As big_uint
        If i Is Nothing Then
            Return Nothing
        End If
        Return New big_uint(adaptive_array_uint32.move(i.v))
    End Function

    Public Shared Function swap(ByVal this As big_uint, ByVal that As big_uint) As Boolean
        If this Is Nothing OrElse that Is Nothing Then
            Return False
        End If
        Return assert(adaptive_array_uint32.swap(this.v, that.v))
    End Function

    Public Function replace_by(ByVal i As big_uint) As Boolean
        If i Is Nothing Then
            Return False
        End If
        If i.is_zero() Then
            set_zero()
        ElseIf i.is_one() Then
            set_one()
        Else
            set_zero()
            v.resize(i.v.size())
            memcpy(v.data(), i.v.data(), i.v.size())
        End If
        Return True
    End Function

    Public Sub replace_by(ByVal i As UInt32)
        set_zero()
        If i > 0 Then
            v.push_back(i)
        End If
    End Sub

    Public Sub replace_by(ByVal i As UInt64)
        set_zero()
        If i > 0 Then
            v.push_back(CUInt(i And max_uint32))
            i >>= bit_count_in_uint32
            assert(i <= max_uint32)
            If i > 0 Then
                v.push_back(CUInt(i))
            End If
        End If
    End Sub

    Public Sub replace_by(ByVal i As Boolean)
        set_zero()
        If i Then
            replace_by(max_uint32)
        Else
            replace_by(uint32_0)
        End If
    End Sub

    Public Function replace_by(ByVal a() As Byte) As Boolean
        set_zero()
        If isemptyarray(a) Then
            Return False
        End If
        v.reserve((array_size(a) + uint32_3) \ byte_count_in_uint32)
        Dim start As UInt32 = 0
        Dim [end] As UInt32 = 0
        Dim [step] As Int32 = 0
        If BitConverter.IsLittleEndian Then
            start = 0
            [end] = array_size(a) - uint32_1
            [step] = 1
        Else
            start = array_size(a) - uint32_1
            [end] = 0
            [step] = -1
        End If
        Dim x As UInt32 = 0
        Dim j As UInt32 = 0
        For i As Int32 = CInt(start) To CInt([end]) Step [step]
            x += (CUInt(a(i)) << assert_which.of(j << bit_shift_in_byte).can_cast_to_int32())
            j += uint32_1
            If j = byte_count_in_uint32 Then
                v.push_back(x)
                j = 0
                x = 0
            End If
        Next
        v.push_back(x)
        remove_extra_blank()
        Return True
    End Function

    Public Sub set_zero()
        v.clear()
    End Sub

    Public Function is_zero() As Boolean
        Return v.empty()
    End Function

    Public Sub set_one()
        v.resize(1)
        v(0) = 1
    End Sub

    Public Function is_one() As Boolean
        Return v.size() = 1 AndAlso v.back() = 1
    End Function

    Public Function is_zero_or_one() As Boolean
        Return is_zero() OrElse is_one()
    End Function

    Public Function [true]() As Boolean
        Return Not [false]()
    End Function

    Public Function [false]() As Boolean
        Return is_zero()
    End Function
End Class
