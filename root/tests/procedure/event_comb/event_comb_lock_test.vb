
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.utt

Public NotInheritable Class event_comb_lock_test
    Inherits multi_procedure_case_wrapper

    Private Shared ReadOnly pc As Int32
    Private Shared ReadOnly rc As Int64

    Shared Sub New()
        pc = 8 * If(isreleasebuild(), 2, 1)
        rc = 1024 * If(isreleasebuild(), 2, 1)
    End Sub

    Public Sub New()
        MyBase.New(repeat(New event_comb_lock_case(), rc), pc)
    End Sub

    Private NotInheritable Class event_comb_lock_case
        Inherits utt.event_comb_case

        Private ReadOnly l As ref(Of event_comb_lock)
        Private started As Boolean
        Private i As Int64

        Public Sub New()
            l = New ref(Of event_comb_lock)()
            i = 0
            started = False
        End Sub

        Public Overrides Function create() As event_comb
            started = True
            Return New event_comb(Function() As Boolean
                                      Return waitfor(l) AndAlso
                                             goto_next()
                                  End Function,
                                  Function() As Boolean
                                      i += 1
                                      l.release()
                                      Return goto_end()
                                  End Function)
        End Function

        Public Overrides Function finish() As Boolean
            If started Then
                assertion.equal(i, pc * rc)
            End If
            Return MyBase.finish()
        End Function
    End Class
End Class
