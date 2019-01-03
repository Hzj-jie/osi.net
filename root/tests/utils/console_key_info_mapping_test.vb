
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.connector
Imports osi.root.utils

Public Class console_key_info_mapping_test
    Inherits [case]

    Private Shared Function verify(ByVal x As ConsoleKey,
                                   ByVal caps_lock As Boolean,
                                   ByVal num_lock As Boolean,
                                   ByVal shift As Boolean) As Boolean
        Dim c As Char = Nothing
        Dim v As vector(Of keyinfo) = Nothing
        If x.char(c, caps_lock, num_lock, shift) Then
            assertion.not_equal(c, character.null)
            assertion.is_true(c.keycode(v))
            If assertion.is_true(Not v Is Nothing AndAlso Not v.empty()) Then
                For i As Int32 = 0 To v.size() - 1
                    If assertion.is_true(v(i).valid()) Then
                        assertion.equal(v(i).c, c)
                        If v(i).console_key() = x AndAlso
                           v(i).caps_lock = caps_lock AndAlso
                           v(i).num_lock = num_lock AndAlso
                           v(i).shift = shift Then
                            Return True
                        End If
                    End If
                Next
                Return False
            Else
                Return False
            End If
        Else
            Return True
        End If
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
        Return assert(enum_traversal(Of ConsoleKey)(Sub(x As ConsoleKey)
                                                        assertion.is_true(verify(x))
                                                    End Sub))
    End Function
End Class
