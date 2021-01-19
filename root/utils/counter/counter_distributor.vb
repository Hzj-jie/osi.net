
Option Explicit On
Option Infer Off
Option Strict On

Imports System.DateTime
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.envs

Namespace counter
    Friend Module counter_distributor
        Private ReadOnly writers As object_unique_set(Of icounter_writer) = Nothing

        Sub New()
            writers = New object_unique_set(Of icounter_writer)()
            assert(insert(file_counter_writer.instance))
            distribute(If(envs.counter_selfhealth, Now().Ticks(), 0),
                       strcat("#start counter_distributor for process ",
                              application_name,
                              ", ver ",
                              application_version,
                              ", process id ",
                              this_process.ref.Id(),
                              ", source control current changeset id ",
                              source_control.current.id,
                              ", built at ",
                              buildtime,
                              newline.incode()))
        End Sub

        Public Function insert(ByVal w As icounter_writer, Optional ByVal need_lock As Boolean = True) As Boolean
            Return writers.insert(w)
        End Function

        Public Sub clear()
            writers.clear()
        End Sub

        Public Function [erase](ByVal w As icounter_writer) As Boolean
            Return writers.erase(w)
        End Function

        Friend Sub distribute(ByVal startticks As Int64, ByVal msg As String)
            writers.foreach(Sub(ByVal writer As icounter_writer)
                                assert(Not writer Is Nothing)
                                writer.write(msg)
                            End Sub)
            If envs.counter_selfhealth Then
                selfhealth.record_write_latency(startticks)
            End If
        End Sub
    End Module
End Namespace
