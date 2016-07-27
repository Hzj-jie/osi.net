
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.utils

'public for test purpose only, looks like we do not need to export these functions.
Public Module _keyboard_case
    Private Function valid_keyboard_action(ByVal a As action) As Boolean
        Return a = action.down OrElse
               a = action.press OrElse
               a = action.up
    End Function

    Private Function valid_keyboard_meta(ByVal i As Int32) As Boolean
        Return i >= constants.keyboard.min_meta AndAlso i <= constants.keyboard.max_meta
    End Function

    Private Function valid_keyboard_meta(ByVal i() As Byte) As Boolean
        Dim o As Int32 = 0
        Return keyboard_code(i, o)
    End Function

    Private Function keyboard_meta(ByVal i As Int32) As Byte()
        assert(valid_keyboard_meta(i))
        Return uint16_bytes(i)
    End Function

    Private Function keyboard_code(ByVal i() As Byte, ByRef o As Int32) As Boolean
        Dim t As UInt32 = 0
        Return bytes_uint16(i, o, t) AndAlso
               valid_keyboard_meta(o) AndAlso
               t = array_size(i)
    End Function

    'test purpose only
    Private Sub status_key(ByRef cur As Boolean,
                           ByVal exp As Boolean,
                           ByVal o As vector(Of [case]),
                           ByVal k As Int32)
        If cur <> exp Then
            o.emplace_back(keyboard_case(If(cur, action.up, action.down), k))
            If exp Then
                o.emplace_back(keyboard_case(action.press, k))
            End If
            cur = exp
        End If
    End Sub

    Private Function keyboard_case(ByVal c As Char,
                                   ByRef caps_lock As Boolean,
                                   ByRef num_lock As Boolean,
                                   ByRef shift As Boolean,
                                   ByRef o As vector(Of [case])) As Boolean
        Dim v As vector(Of keyinfo) = Nothing
        If c.keycode(v) Then
            assert(Not (v Is Nothing OrElse v.empty()))
            Dim min_steps As Int32 = 0
            Dim min_case As Int32 = 0
            min_steps = max_int32
            For i As Int32 = 0 To v.size() - 1
                Dim cs As Int32 = 0
                cs += If(caps_lock <> v(i).caps_lock, 1, 0)
                cs += If(num_lock <> v(i).num_lock, 1, 0)
                cs += If(shift <> v(i).shift, 1, 0)
                If cs < min_steps Then
                    min_steps = cs
                    min_case = i
                    If min_steps = 0 Then
                        Exit For
                    End If
                End If
            Next

            status_key(caps_lock, v(min_case).caps_lock, o, constants.keyboard.caps_lock)
            status_key(num_lock, v(min_case).num_lock, o, constants.keyboard.num_lock)
            status_key(shift, v(min_case).shift, o, constants.keyboard.shift)
            o.emplace_back(keyboard_case(action.down, v(min_case).key))
            o.emplace_back(keyboard_case(action.press, v(min_case).key))
            o.emplace_back(keyboard_case(action.up, v(min_case).key))
            Return True
        Else
            Return False
        End If
    End Function

    <Extension()> Public Function keyboard_case(ByVal c As Char, ByRef o As vector(Of [case])) As Boolean
        o.renew()
        Return keyboard_case(c, False, False, False, o)
    End Function

    <Extension()> Public Function keyboard_case(ByVal c As Char) As vector(Of [case])
        Dim o As vector(Of [case]) = Nothing
        assert(keyboard_case(c, o))
        Return o
    End Function

    <Extension()> Public Function keyboard_case(ByVal s As String, ByRef o As vector(Of [case])) As Boolean
        o.renew()
        Dim caps_lock As Boolean = False
        Dim num_lock As Boolean = False
        Dim shift As Boolean = False
        For i As Int32 = 0 To strlen(s) - 1
            If Not keyboard_case(s(i), caps_lock, num_lock, shift, o) Then
                Return False
            End If
        Next
        If caps_lock Then
            o.emplace_back(keyboard_case(action.up, constants.keyboard.caps_lock))
        End If
        If num_lock Then
            o.emplace_back(keyboard_case(action.up, constants.keyboard.num_lock))
        End If
        If shift Then
            o.emplace_back(keyboard_case(action.up, constants.keyboard.shift))
        End If
        Return True
    End Function

    <Extension()> Public Function keyboard_case(ByVal s As String) As vector(Of [case])
        Dim o As vector(Of [case]) = Nothing
        assert(keyboard_case(s, o))
        Return o
    End Function

    <Extension()> Public Function keyboard_code(ByVal c As [case], ByRef o As Int32) As Boolean
        If c Is Nothing Then
            Return False
        Else
            Return keyboard_code(c.meta, o)
        End If
    End Function

    <Extension()> Public Function valid_keyboard_case(ByVal c As [case]) As Boolean
        Return Not c Is Nothing AndAlso
               c.mode = mode.keyboard AndAlso
               valid_keyboard_action(c.action) AndAlso
               valid_keyboard_meta(c.meta)
    End Function

    Public Function keyboard_case(ByVal action As action, ByVal meta() As Byte) As [case]
        Dim r As [case] = Nothing
        r = New [case](mode.keyboard, action, meta)
        assert(valid_keyboard_case(r))
        Return r
    End Function

    Public Function keyboard_case(ByVal action As action, ByVal meta As Int32) As [case]
        Return keyboard_case(action, keyboard_meta(meta))
    End Function

    Public Function keyboard_down_case(ByVal meta() As Byte) As [case]
        Return keyboard_case(action.down, meta)
    End Function

    Public Function keyboard_down_case(ByVal meta As Int32) As [case]
        Return keyboard_down_case(keyboard_meta(meta))
    End Function

    Public Function keyboard_up_case(ByVal meta() As Byte) As [case]
        Return keyboard_case(action.up, meta)
    End Function

    Public Function keyboard_up_case(ByVal meta As Int32) As [case]
        Return keyboard_up_case(keyboard_meta(meta))
    End Function

    Public Function keyboard_press_case(ByVal meta() As Byte) As [case]
        Return keyboard_case(action.press, meta)
    End Function

    Public Function keyboard_press_case(ByVal meta As Int32) As [case]
        Return keyboard_press_case(keyboard_meta(meta))
    End Function
End Module
