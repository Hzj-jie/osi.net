
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.utt

Public NotInheritable Class console_key_info_mapping_test
    Inherits [case]

    Private Shared Function verify(ByVal x As ConsoleKey,
                                   ByVal caps_lock As Boolean,
                                   ByVal num_lock As Boolean,
                                   ByVal shift As Boolean) As Boolean
        Dim c As Char = Nothing
        Dim v As vector(Of keyinfo) = Nothing
        If Not x.char(c, caps_lock, num_lock, shift) Then
            Return True
        End If
        assertion.not_equal(c, character.null)
        assertion.is_true(c.keycode(v))
        If Not assertion.is_true(v IsNot Nothing AndAlso Not v.empty()) Then
            Return False
        End If
        For i As UInt32 = 0 To v.size() - uint32_1
            If Not assertion.is_true(v(i).valid()) Then
                Continue For
            End If
            assertion.equal(v(i).c, c)
            If v(i).console_key() = x AndAlso
               v(i).caps_lock = caps_lock AndAlso
               v(i).num_lock = num_lock AndAlso
               v(i).shift = shift Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Shared Function verify(ByVal x As ConsoleKey) As Boolean
        Return verify(x, False, False, False) AndAlso
               verify(x, True, False, False) AndAlso
               verify(x, False, True, False) AndAlso
               verify(x, True, True, False) AndAlso
               verify(x, False, False, True) AndAlso
               verify(x, True, False, True) AndAlso
               verify(x, False, True, True) AndAlso
               verify(x, True, True, True)
    End Function

    Public Overrides Function run() As Boolean
        enum_def(Of ConsoleKey).foreach(Sub(ByVal x As ConsoleKey)
                                            assertion.is_true(verify(x))
                                        End Sub)
        Return True
    End Function
End Class
