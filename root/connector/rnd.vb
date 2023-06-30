
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.Text
Imports osi.root.connector
Imports osi.root.constants

' Prefer to use thread_random.
<global_init(global_init_level.runtime_checkers)>
Public Module _rnd
    Private Sub init()
        assert(-max_double >= min_double)
    End Sub

#If VBRND Then
#If NEWRANDOM Then
    Public Function rnd() As Double
        Static seed As Double = CType(Now.Millisecond, Double) / Now.Ticks
        Static rndtimes As UInt64 = 1
        seed = (seed * rndtimes + Now.Millisecond()) / Now.Ticks
        rndtimes += 1

        Return seed
    End Function
#Else
    Public Function rnd() As Double
        Return Microsoft.VisualBasic.VBMath.Rnd()
    End Function

#If RETIRED Then
    Public Function rnd() As Double
        Dim rtn As Double
        rtn = Microsoft.VisualBasic.VBMath.Rnd()
        rtn *= ((Now.Ticks() Mod 100) \ 10) + 1
        rtn -= Int(rtn)
#If RETIRED Then
        Try
            rtn -= Convert.ToInt32(rtn)
        Catch ex As Exception
            raiseError("cannot convert rtn to int32, rtn " + Convert.ToString(rtn) + _
                       ", fallback to Microsoft.VisualBasic.VBMath.Rnd()", errorHandle.errorType.warning)
            rtn = Microsoft.VisualBasic.VBMath.Rnd()
        End Try
#End If

        Return rtn
    End Function
#End If
#End If

#If Not NEWRANDOM Then
#If PocketPC OrElse Smartphone Then
    Private Class rndtimes
        Public Shared d As UInt64
    End Class
#ElseIf TIMESBASED_RANDOMIZE Then
    Private Class rndtimes
        <ThreadStatic()> Public Shared d As UInt64
    End Class
#End If
#End If

#If Not TIMESBASED_RANDOMIZE AndAlso Not NEWRANDOM Then
    Sub New()
        Microsoft.VisualBasic.VBMath.Randomize(Now.hires_ticks())
    End Sub
#End If

    Public Function rnd(ByVal min As Double, ByVal max As Double, Optional ByVal int As Boolean = False, _
                        Optional ByVal forceRandomize As Boolean = False) As Double
        Dim result As Double
#If TIMESBASED_RANDOMIZE Then
        If rndtimes.d >= 1000 OrElse rndtimes.d = 0 OrElse forceRandomize Then
            rndtimes.d = 1
            Microsoft.VisualBasic.VBMath.Randomize()
        Else
            rndtimes.d += 1
        End If
#End If

        If min > max Then
            swap(min, max)
        End If
        result = rnd() * (max - min) + min

        If int Then
            Return Math.Truncate(result)
        Else
            Return result
        End If
    End Function
#Else

    Private Function r() As Random
        Return thread_random.ref()
    End Function

    Private Function i(ByVal d As Int64) As Int32
        If d > max_int32 Then
            Return max_int32
        End If
        If d < min_int32 Then
            Return min_int32
        End If
        Return CInt(d)
    End Function

    Private Function i(ByVal d As Double) As Int32
        If d > max_int32 Then
            Return max_int32
        End If
        If d < min_int32 Then
            Return min_int32
        End If
        Return CInt(d)
    End Function

    Private Function ri(ByVal r As Random, ByVal min As Int32, ByVal max As Int32) As Int32
        assert(Not r Is Nothing)
        Return r.Next(min, max)
    End Function

    Private Function ri(ByVal min As Int32, ByVal max As Int32) As Int32
        Return ri(r(), min, max)
    End Function

    Public Function rnd_int(ByVal r As Random, ByVal min As Int32, ByVal max As Int32) As Int32
        Return ri(r, min, max)
    End Function

    Public Function rnd_int(ByVal min As Int32, ByVal max As Int32) As Int32
        Return ri(min, max)
    End Function

    Public Function rnd_int(ByVal r As Random) As Int32
        Return rnd_int(r, min_int32, max_int32)
    End Function

    Public Function rnd_int() As Int32
        Return rnd_int(min_int32, max_int32)
    End Function

    Public Function rnd_int16(ByVal r As Random) As Int16
        Return CShort(rnd_int(r, min_int16, max_int16))
    End Function

    Public Function rnd_int16() As Int16
        Return CShort(rnd_int(min_int16, max_int16))
    End Function

    Public Function rnd_uint16(ByVal r As Random) As UInt16
        Return CUShort(rnd_int(r, min_uint16, max_uint16))
    End Function

    Public Function rnd_uint16() As UInt16
        Return CUShort(rnd_int(min_uint16, max_uint16))
    End Function

    Public Function rnd_int8() As SByte
        Return CSByte(rnd_int(min_int8, max_int8))
    End Function

    Public Function rnd_uint8() As Byte
        Return CByte(rnd_int(min_uint8, max_uint8))
    End Function

    Public Function rnd_bool(ByVal r As Random) As Boolean
        assert(Not r Is Nothing)
        Return r.Next(2) = 0
    End Function

    Public Function rnd_bool() As Boolean
        Return rnd_bool(r())
    End Function

    Public Function rnd_bool_trues(ByVal r As Random, ByVal times As Int32) As Boolean
        assert(Not r Is Nothing)
        For i As Int32 = 0 To times - 1
            If Not rnd_bool() Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Function rnd_bool_trues(ByVal times As Int32) As Boolean
        Return rnd_bool_trues(r(), times)
    End Function

    Public Function rnd_bool(ByVal r As Random,
                             ByVal true_set As Int32,
                             ByVal sample As Int32) As Boolean
        If sample <= 0 OrElse true_set <= 0 Then
            'cannot select anything from an empty sample
            Return False
        ElseIf true_set >= sample Then
            Return True
        Else
            Return rnd_int(r, 0, sample) < true_set
        End If
    End Function

    Public Function rnd_bool(ByVal true_set As Int32,
                             ByVal sample As Int32) As Boolean
        Return rnd_bool(r(), true_set, sample)
    End Function

    Public Function rnd_bool(ByVal rate As Double) As Boolean
        If rate <= 0 Then
            Return False
        ElseIf rate >= 1 Then
            Return True
        Else
            Return rnd_double(0, 1) < rate
        End If
    End Function

    Public Function rnd_int64(ByVal min As Int64, ByVal max As Int64) As Int64
        Return CLng(Math.Truncate(next_double() * (CDbl(max) - min)) + min)
    End Function

    Public Function rnd_int64() As Int64
        Return rnd_int64(min_int64, max_int64)
    End Function

    Public Function rnd_uint(ByVal min As UInt32, ByVal max As UInt32) As UInt32
        Return CUInt(Math.Truncate(next_double() * (CDbl(max) - min)) + min)
    End Function

    Public Function rnd_uint() As UInt32
        Return rnd_uint(min_uint32, max_uint32)
    End Function

    Public Function rnd_uints(ByVal size As UInt32) As UInt32()
        Dim r() As UInt32 = Nothing
        If size = uint32_0 Then
            ReDim r(-1)
        Else
            ReDim r(CInt(size - uint32_1))
            For i As Int32 = 0 To CInt(size - uint32_1)
                r(i) = rnd_uint()
            Next
        End If
        Return r
    End Function

    Public Function rnd_uint64(ByVal min As UInt64, ByVal max As UInt64) As UInt64
        Return CULng(Math.Truncate(next_double() * (CDbl(max) - min)) + min)
    End Function

    Public Function rnd_uint64() As UInt64
        Return rnd_uint64(min_uint64, max_uint64)
    End Function

    Public Function next_double(ByVal r As Random) As Double
        assert(Not r Is Nothing)
        Return r.NextDouble()
    End Function

    Public Function next_double() As Double
        Return next_double(r())
    End Function

    Public Function next_bytes(ByVal r As Random, ByVal buff() As Byte) As Boolean
        assert(Not r Is Nothing)
        If buff Is Nothing Then
            Return False
        Else
            r.NextBytes(buff)
            Return True
        End If
    End Function

    Public Function next_bytes(ByVal r As Random, ByVal count As UInt32) As Byte()
        If count = 0 Then
            Return Nothing
        End If
        Dim b() As Byte = Nothing
        ReDim b(CInt(count) - 1)
        assert(next_bytes(r, b))
        Return b
    End Function

    Public Function next_bytes(ByVal buff() As Byte) As Boolean
        Return next_bytes(r(), buff)
    End Function

    Public Function next_bytes(ByVal count As UInt32) As Byte()
        Return next_bytes(r(), count)
    End Function

    Private Function rd(ByVal min As Double, ByVal max As Double) As Double
        Dim rtn As Double = 0
        rtn = next_double()
        Dim len As Double = 0
        len = max - min
        If len <> 1 Then
            rtn *= len
        End If
        rtn += min
        Return rtn
    End Function

    Public Function rnd_double(ByVal min As Double, ByVal max As Double) As Double
        Return rd(min, max)
    End Function

    Public Function rnd_double() As Double
        Dim d As Double = 0
        d = rnd_double(0, max_double)
        Return If(rnd_bool(), -d, d)
    End Function

    Public Function rnd(ByVal min As Int32, ByVal max As Int32, ByVal int As Boolean) As Double
        If int Then
            Return ri(min, max)
        Else
            Return rd(min, max)
        End If
    End Function

    Public Function rnd(ByVal min As Int64, ByVal max As Int64, ByVal int As Boolean) As Double
        If int Then
            Return ri(i(min), i(max))
        Else
            Return rd(min, max)
        End If
    End Function

    Public Function rnd(ByVal min As Int32, ByVal max As Int64, ByVal int As Boolean) As Double
        If int Then
            Return ri(min, i(max))
        Else
            Return rd(min, max)
        End If
    End Function

    Public Function rnd(ByVal min As Int64, ByVal max As Int32, ByVal int As Boolean) As Double
        If int Then
            Return ri(i(min), max)
        Else
            Return rd(min, max)
        End If
    End Function

    Public Function rnd(ByVal min As Double, ByVal max As Double) As Double
        Return rd(min, max)
    End Function

    Public Function rnd(ByVal min As Double, ByVal max As Double, ByVal int As Boolean) As Double
        If int Then
            Return ri(i(min), i(max))
        Else
            Return rd(min, max)
        End If
    End Function
#End If

    Private ReadOnly char_min_value As Int32 = Convert.ToInt32(Char.MinValue)
    Private ReadOnly char_max_value As Int32 = Convert.ToInt32(Char.MaxValue)
    Private ReadOnly char_max_value_p_1 As Int32 = char_max_value + 1

    Public Function rnd_char(ByVal r As Random) As Char
        Return Convert.ToChar(ri(r, char_min_value, char_max_value_p_1))
    End Function

    Public Function rnd_char() As Char
        Return rnd_char(r())
    End Function

    Public Function rnd_chars(ByVal r As Random, ByVal len As Int32) As String
        Dim s As StringBuilder = Nothing
        s = New StringBuilder(If(len < 0, 0, len))
        For i As Int32 = 0 To len - 1
            s.Append(rnd_char(r))
        Next
        Return Convert.ToString(s)
    End Function

    Public Function rnd_chars(ByVal len As Int32) As String
        Return rnd_chars(r(), len)
    End Function

    Public Function rnd_utf8_char(ByVal r As Random) As Char
        Dim c As Char = Nothing
        Do
            c = rnd_char(r)
        Loop Until c.utf8_supported() AndAlso c.visible()
        Return c
    End Function

    Public Function rnd_utf8_char() As Char
        Return rnd_utf8_char(r())
    End Function

    Public Function rnd_utf8_chars(ByVal r As Random, ByVal len As Int32) As String
        Dim s As StringBuilder = Nothing
        s = New StringBuilder(If(len < 0, 0, len))
        For ri As Int32 = 0 To len - 1
            s.Append(rnd_utf8_char(r))
        Next
        Return Convert.ToString(s)
    End Function

    Public Function rnd_utf8_chars(ByVal len As Int32) As String
        Return rnd_utf8_chars(r(), len)
    End Function

    Public Function rnd_en_chars(ByVal r As Random,
                                 ByVal length As Int32,
                                 Optional ByVal case_sensitive As Boolean = True) As String
        Dim rtn As StringBuilder = Nothing
        rtn = New StringBuilder(If(length < 0, 0, length))
        For i As Int32 = 0 To length - 1
            rtn.Append(rnd_en_char(r, case_sensitive))
        Next

        Return Convert.ToString(rtn)
    End Function

    Public Function rnd_en_chars(ByVal length As Int32, Optional ByVal case_sensitive As Boolean = True) As String
        Return rnd_en_chars(r(), length, case_sensitive)
    End Function

    Private ReadOnly a As Int32 = Convert.ToInt32(character.a)
    Private ReadOnly z As Int32 = Convert.ToInt32(character.z)
    Private ReadOnly z_p_1 As Int32 = z + 1
    Private ReadOnly _A As Int32 = Convert.ToInt32(character._A)
    Private ReadOnly a_A_diff As Int32 = _A - a

    Public Function rnd_en_char(ByVal r As Random, Optional ByVal case_sensitive As Boolean = True) As Char
        Return Convert.ToChar(ri(r, a, z_p_1) + If(case_sensitive AndAlso ri(r, 0, 2) = 0, a_A_diff, 0))
    End Function

    Public Function rnd_en_char(Optional ByVal case_sensitive As Boolean = True) As Char
        Return rnd_en_char(r(), case_sensitive)
    End Function

    Public Function rnd_byte() As Byte
        Return CByte(rnd(0, max_int8 + 1, True))
    End Function

    Public Function rnd_bytes(Optional ByVal size As UInt32 = 1024) As Byte()
        Dim rtn() As Byte = Nothing
        ReDim rtn(CInt(size) - 1)
        assert(next_bytes(rtn))
        Return rtn
    End Function

    Public Function rnd_alpha() As Byte
        Return CByte(rnd(192, max_uint8 + 1, True))
    End Function

    Public Function guid_str() As String
        Return Guid.NewGuid().ToString("N")
    End Function

    Public Function guid_strs(ByVal count As UInt32) As String()
        If count = 0 Then
            Return Nothing
        End If
        Dim r() As String = Nothing
        ReDim r(CInt(count) - 1)
        For i As Int32 = 0 To CInt(count) - 1
            r(i) = guid_str()
            If i > 0 Then
                Dim j As Int32 = 0
                For j = 0 To i - 1
                    If r(i) = r(j) Then
                        Exit For
                    End If
                Next
                If j < i Then
                    i -= 1
                End If
            End If
        Next
        Return r
    End Function

    Public Function rnd_ascii_display_char(ByVal r As Random,
                                           ByVal ParamArray excepts() As Char) As Char
        Dim c As Char = Nothing
        Do
            c = Convert.ToChar(ri(r, character.ascii_lower_bound, character.ascii_upper_bound + 1))
        Loop While Char.IsControl(c) OrElse excepts.has(c)
        Return c
    End Function

    Public Function rnd_ascii_display_char(ByVal ParamArray excepts() As Char) As Char
        Return rnd_ascii_display_char(r(), excepts)
    End Function

    Public Function rnd_ascii_display_chars(ByVal r As Random,
                                            ByVal length As Int32,
                                            ByVal ParamArray excepts() As Char) As String
        Dim rtn As StringBuilder = Nothing
        rtn = New StringBuilder(If(length < 0, 0, length))
        For i As Int32 = 0 To length - 1
            rtn.Append(rnd_ascii_display_char(r, excepts))
        Next

        Return Convert.ToString(rtn)
    End Function

    Public Function rnd_ascii_display_chars(ByVal len As Int32,
                                            ByVal ParamArray excepts() As Char) As String
        Return rnd_ascii_display_chars(r(), len, excepts)
    End Function

    Public Function rnd_ints(ByVal r As Random, ByVal v() As Int32) As Boolean
        assert(Not r Is Nothing)
        If v Is Nothing Then
            Return False
        End If
        For i As Int32 = 0 To array_size_i(v) - 1
            v(i) = rnd_int(r)
        Next
        Return True
    End Function

    Public Function rnd_ints(ByVal v() As Int32) As Boolean
        Return rnd_ints(r(), v)
    End Function

    Public Function rnd_ints(ByVal r As Random, ByVal count As Int32) As Int32()
        If count <= 0 Then
            Return Nothing
        End If
        Dim b() As Int32 = Nothing
        ReDim b(count - 1)
        assert(rnd_ints(r, b))
        Return b
    End Function

    Public Function rnd_ints(ByVal count As Int32) As Int32()
        Return rnd_ints(r(), count)
    End Function

    Public Function rnd(Of T)() As T
        Return rnd_register(Of T).rnd()
    End Function

    <Extension()> Public Function inplace_shuffle(Of T)(ByVal v() As T, ByVal r As Random) As T()
        If isemptyarray(v) Then
            Return v
        End If
        For i As Int32 = 0 To array_size_i(v) - 1
            Dim j As Int32 = 0
            j = rnd_int(r, 0, array_size_i(v))
            If j <> i Then
                swap(v(i), v(j))
            End If
        Next
        Return v
    End Function

    <Extension()> Public Function inplace_shuffle(Of T)(ByVal v() As T) As T()
        Return inplace_shuffle(v, r())
    End Function

    <Extension()> Public Function shuffle(Of T)(ByVal v() As T, ByVal r As Random) As T()
        Return inplace_shuffle(copy(v), r)
    End Function

    <Extension()> Public Function shuffle(Of T)(ByVal v() As T) As T()
        Return shuffle(v, r())
    End Function
End Module
