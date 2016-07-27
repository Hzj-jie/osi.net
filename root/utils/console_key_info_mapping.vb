
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation

Public Module _console_key_info_mapping
    Public Structure keyinfo
        Public ReadOnly key As Int32
        Public ReadOnly c As Char
        Public ReadOnly caps_lock As Boolean
        Public ReadOnly num_lock As Boolean
        Public ReadOnly shift As Boolean

        Public Sub New(ByVal key As Int32,
                       ByVal c As Char,
                       ByVal caps_lock As Boolean,
                       ByVal num_lock As Boolean,
                       ByVal shift As Boolean)
            Me.key = key
            Me.c = c
            Me.caps_lock = caps_lock
            Me.num_lock = num_lock
            Me.shift = shift
            assert(valid())
        End Sub

        Public Function valid() As Boolean
            Return key >= console_key_min.as_int32() AndAlso
                   key <= console_key_max.as_int32()
        End Function

        Public Function console_key() As ConsoleKey
            assert(valid())
            Return key
        End Function
    End Structure

    Private ReadOnly m() As vector(Of keyinfo)

    Sub New()
        ReDim m(console_key_max_char_int)
        assert(console_key_min >= 0)
        assert(console_key_max < console_key_info_mapping_height)
        For i As Int32 = console_key_min To console_key_max
            For j As Int32 = 0 To console_key_info_mapping_width - 1
                Dim k As keyinfo = Nothing
                Dim c As Char = Nothing
                Dim ci As Int32 = 0
                Dim caps_lock As Boolean = False
                Dim num_lock As Boolean = False
                Dim shift As Boolean = False
                console_key_info_mapping_column(j, caps_lock, num_lock, shift)
                c = console_key_info_mapping(i, j)
                ci = Convert.ToInt32(c)
                k = New keyinfo(i, c, caps_lock, num_lock, shift)
                If m(ci) Is Nothing Then
                    m(ci) = New vector(Of keyinfo)()
                End If
                m(ci).emplace_back(k)
            Next
        Next
    End Sub

    <Extension()> Public Function keycode(ByVal c As Char,
                                          ByRef o As vector(Of keyinfo)) As Boolean
        Dim ci As Int32 = 0
        ci = Convert.ToInt32(c)
        If ci >= console_key_min_char_int AndAlso
           ci <= console_key_max_char_int Then
            o = m(ci)
            Return Not (o Is Nothing OrElse o.empty())
        Else
            Return False
        End If
    End Function

    <Extension()> Public Function keycode(ByVal c As Char, ByRef o As keyinfo) As Boolean
        Dim v As vector(Of keyinfo) = Nothing
        If keycode(c, v) Then
            assert(Not (v Is Nothing OrElse v.empty()))
            o = v(0)
            Return True
        Else
            Return False
        End If
    End Function
End Module
