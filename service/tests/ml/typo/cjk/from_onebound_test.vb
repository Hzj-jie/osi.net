
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.template
Imports osi.root.utt.attributes
Imports osi.service.resource
Imports from_onebound = osi.service.ml.typo.cjk.from_onebound

Namespace typo.cjk
    <test>
    Public NotInheritable Class from_onebound_test
        Private Shared model As argument(Of String)
        Private Shared shards As argument(Of UInt32)
        Private Shared use_raw As argument(Of Boolean)
        Private Shared input As argument(Of String)
        Private Shared concurrency As argument(Of UInt32)

        Private NotInheritable Class concurrency_t
            Inherits _int64

            Protected Overrides Function at() As Int64
                Return concurrency Or uint32_2
            End Function
        End Class

        Private Shared ReadOnly runner As New concurrency_runner(Of concurrency_t)()

        <test>
        <command_line_specified>
        Private Shared Sub run()
            Dim num_of_shards As UInt32 = (shards Or 3)
            concurrency_runner.execute(
                num_of_shards,
                Sub(ByVal i As UInt32)
                    Dim model_name As String = String.Concat(model Or "cjk.nplus1.2",
                                                             ".",
                                                             i,
                                                             If(use_raw Or True, ".raw", ""),
                                                             ".bin")
                    from_onebound.from_dump(i, num_of_shards, model_name) _
                                           (New tar.reader(New tar.selector() With
                                               {.pattern = input Or "tar_manual_test.zip_*"})).
                                  stream().
                                  foreach(Sub(ByVal s As String)
                                              Console.WriteLine(s)
                                          End Sub)
                End Sub)
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
