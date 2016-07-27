
Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _console_key_info_mapping
    Public ReadOnly console_key_min As ConsoleKey
    Public ReadOnly console_key_min_str As String
    Public ReadOnly console_key_max As ConsoleKey
    Public ReadOnly console_key_max_str As String
    Public ReadOnly console_key_max_char As Char
    Public ReadOnly console_key_max_char_int As Int32
    Public ReadOnly console_key_min_char As Char
    Public ReadOnly console_key_min_char_int As Int32
    Public ReadOnly console_key_info_mapping_height As Int32
    Public ReadOnly console_key_info_mapping_width As Int32

    Sub New()
        console_key_info_mapping_height = array_size(console_key_info_mapping)
        console_key_info_mapping_width = console_key_info_mapping.GetLength(1)

        Dim cmin As ConsoleKey = Nothing
        Dim cmins As String = Nothing
        Dim cmax As ConsoleKey = Nothing
        Dim cmaxs As String = Nothing
        cmin = ConsoleKey.A
        cmax = ConsoleKey.A
        assert(enum_traversal(Of ConsoleKey)(Sub(x As ConsoleKey, y As String)
                                                   If x.as_int32() < cmin.as_int32() Then
                                                       cmin = x
                                                       cmins = y
                                                   End If
                                                   If x.as_int32() > cmax.as_int32() Then
                                                       cmax = x
                                                       cmaxs = y
                                                   End If
                                               End Sub))
        assert(Not String.IsNullOrEmpty(cmins) AndAlso Not String.IsNullOrEmpty(cmaxs))
        console_key_min = cmin
        console_key_min_str = cmins
        console_key_max = cmax
        console_key_max_str = cmaxs
        assert(console_key_min >= 0 AndAlso console_key_max < console_key_info_mapping_height)

        console_key_min_char = character.a
        console_key_max_char = character.a
        For i As Int32 = 0 To console_key_info_mapping_height - 1
            For j As Int32 = 0 To console_key_info_mapping_width - 1
                Dim c As Int32 = 0
                c = Convert.ToInt32(console_key_info_mapping(i, j))
                If c < Convert.ToInt32(console_key_min_char) Then
                    console_key_min_char = console_key_info_mapping(i, j)
                End If
                If c > Convert.ToInt32(console_key_max_char) Then
                    console_key_max_char = console_key_info_mapping(i, j)
                End If
            Next
        Next
        console_key_min_char_int = Convert.ToInt32(console_key_min_char)
        console_key_max_char_int = Convert.ToInt32(console_key_max_char)
    End Sub

    Public Function console_key_info_mapping_column(ByVal caps_lock As Boolean,
                                                    ByVal num_lock As Boolean,
                                                    ByVal shift As Boolean) As Byte
        Dim r As Byte = 0
        r.setrbit(0, caps_lock)
        r.setrbit(1, num_lock)
        r.setrbit(2, shift)
        Return r
    End Function

    Public Sub console_key_info_mapping_column(ByVal i As Byte,
                                               ByRef caps_lock As Boolean,
                                               ByRef num_lock As Boolean,
                                               ByRef shift As Boolean)
        caps_lock = i.getrbit(0)
        num_lock = i.getrbit(1)
        shift = i.getrbit(2)
    End Sub

    <Extension()> Public Function as_int32(ByVal k As ConsoleKey) As Int32
        Return CInt(k)
    End Function

    <Extension()> Public Function keycode_char(ByVal k As Int32,
                                               ByRef o As Char,
                                               Optional ByVal caps_lock As Boolean = False,
                                               Optional ByVal num_lock As Boolean = False,
                                               Optional ByVal shift As Boolean = False) As Boolean
        If k < 0 OrElse k >= console_key_info_mapping_height Then
            Return False
        Else
            o = console_key_info_mapping(k, console_key_info_mapping_column(caps_lock, num_lock, shift))
            Return o <> character.null
        End If
    End Function

    <Extension()> Public Function [char](ByVal k As ConsoleKey,
                                         ByRef o As Char,
                                         Optional ByVal caps_lock As Boolean = False,
                                         Optional ByVal num_lock As Boolean = False,
                                         Optional ByVal shift As Boolean = False) As Boolean
        Return k.as_int32().keycode_char(o, caps_lock, num_lock, shift)
    End Function

    <Extension()> Public Function keycode_char(ByVal k As Int32,
                                               Optional ByVal caps_lock As Boolean = False,
                                               Optional ByVal num_lock As Boolean = False,
                                               Optional ByVal shift As Boolean = False) As Char
        Dim o As Char = Nothing
        If keycode_char(k, o, caps_lock, num_lock, shift) Then
            Return o
        Else
            Return character.null
        End If
    End Function

    <Extension()> Public Function [char](ByVal k As ConsoleKey,
                                         Optional ByVal caps_lock As Boolean = False,
                                         Optional ByVal num_lock As Boolean = False,
                                         Optional ByVal shift As Boolean = False) As Char
        Return k.as_int32().keycode_char(caps_lock, num_lock, shift)
    End Function
End Module
