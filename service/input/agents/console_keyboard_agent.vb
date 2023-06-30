
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.constants
Imports osi.root.procedure
Imports osi.service.iosys
Imports rcec = osi.root.procedure.reference_count_event_comb_1

Public Class console_keyboard_agent
    Inherits agent(Of [case])

    Private Class console_keyboard
        Private Shared ReadOnly status As agent_status
        Private Shared ReadOnly rec As rcec

        Shared Sub New()
            assert(rcec.start_after_trigger())
            status = New agent_status(mode.keyboard)
            rec = rcec.ctor(Function() As Boolean
                                If Console.KeyAvailable() Then
                                    Dim ki As ConsoleKeyInfo = Nothing
                                    ki = Console.ReadKey(True)
                                    Dim ks As vector(Of Int32) = Nothing
                                    ks = New vector(Of Int32)()
                                    If Console.CapsLock() Then
                                        ks.emplace_back(constants.keyboard.caps_lock)
                                    End If
                                    If Console.NumberLock() Then
                                        ks.emplace_back(constants.keyboard.num_lock)
                                    End If
                                    If ki.ctrl() Then
                                        ks.emplace_back(constants.keyboard.ctrl)
                                    End If
                                    If ki.alt() Then
                                        ks.emplace_back(constants.keyboard.alt)
                                    End If
                                    If ki.shift() Then
                                        ks.emplace_back(constants.keyboard.shift)
                                    End If
                                    ks.emplace_back(ki.Key())
                                    status.new_status(+ks)
                                    Return True
                                Else
                                    Return False
                                End If
                            End Function,
                            constants.console_keyboard_agent.check_interval_ms)
        End Sub

        Public Shared Sub bind(ByVal h As agent_status.issueEventHandler)
            AddHandler status.issue, h
            rec.bind()
        End Sub

        Public Shared Sub release(ByVal h As agent_status.issueEventHandler)
            RemoveHandler status.issue, h
            rec.release()
        End Sub
    End Class

    Public Sub New()
        console_keyboard.bind(AddressOf received)
    End Sub

    Protected Overrides Sub Finalize()
        console_keyboard.release(AddressOf received)
        MyBase.Finalize()
    End Sub

    Protected Overrides Sub deliver_failed(ByVal c As [case])
        Console.Beep()
    End Sub
End Class
