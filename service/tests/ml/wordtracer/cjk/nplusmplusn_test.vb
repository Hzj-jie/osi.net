
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.utils
Imports osi.root.utt.attributes
Imports osi.service.resource
Imports nplusmplusn = osi.service.ml.wordtracer.cjk.nplusmplusn

Namespace wordtracer.cjk
    <test>
    Public NotInheritable Class nplusmplusn_test
        Private Shared n As argument(Of UInt32)
        Private Shared m As argument(Of UInt32)
        Private Shared num_of_shards As argument(Of UInt32)
        Private Shared input As argument(Of String)
        Private Shared output As argument(Of String)
        Private Shared percent As argument(Of Double)
        Private Shared percentage As argument(Of Double)
        Private Shared lambda As argument(Of Double)

        Private Shared Sub run(ByVal n As UInt32, ByVal m As UInt32, ByVal dump_raw As Boolean)
            Dim num_of_shards As UInt32 = nplusmplusn_test.num_of_shards Or uint32_1
            assert(num_of_shards > 0)
            For i As UInt32 = 0 To num_of_shards - uint32_1
                Dim t As New nplusmplusn(New shard(Of String)(i, num_of_shards), n, m)
                t.train(tar.reader.unzip(New tar.selector() With {.pattern = input Or "tar_manual_test.zip_*"}))
                Dim base_name As String = String.Concat("cjk.nplusmplusn.", n, ".", m, ".", i, "-", num_of_shards)
                If dump_raw Then
                    t.dump(percent Or 0.2).
                      dump(output Or String.Concat(base_name, ".raw.bin"))
                Else
                    t.dump(percentage Or 0.7, lambda Or 0.1).
                      dump(output Or String.Concat(base_name, ".bin"))
                End If
            Next
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub run()
            run(n Or 1, m Or 1, False)
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub all()
            Dim n As UInt32 = nplusmplusn_test.n Or 2
            For i As UInt32 = 1 To n - uint32_1
                run(i, n - i, False)
            Next
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub run_raw()
            run(n Or 1, m Or 1, True)
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub all_raw()
            Dim n As UInt32 = nplusmplusn_test.n Or 2
            For i As UInt32 = 1 To n - uint32_1
                run(i, n - i, True)
            Next
        End Sub
    End Class
End Namespace
