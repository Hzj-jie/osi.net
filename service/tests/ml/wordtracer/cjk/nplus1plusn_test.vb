
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.utils
Imports osi.root.utt.attributes
Imports osi.service.resource
Imports nplus1plusn = osi.service.ml.wordtracer.cjk.nplus1plusn

Namespace wordtracer.cjk
    <test>
    Public NotInheritable Class nplus1plusn_test
        Private Shared input As argument(Of String)
        Private Shared output As argument(Of String)
        Private Shared percent As argument(Of Double)
        Private Shared percentage As argument(Of Double)

        <test>
        <command_line_specified>
        Private Shared Sub from_tar()
            Dim n As New nplus1plusn(1)
            n.train(tar.reader.unzip(New tar.selector() With {.pattern = input Or "tar_manual_test.zip_*"}))
            n.dump(percentage Or 0.9).
              dump(output Or "cjk.nplus1plusn.1.bin")
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub from_tar_raw()
            Dim n As New nplus1plusn(1)
            n.train(tar.reader.unzip(New tar.selector() With {.pattern = input Or "tar_manual_test.zip_*"}))
            n.dump_raw(percent Or 0.2).
              dump(output Or "cjk.nplus1plusn.1.raw.bin")
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub from_tar_2()
            Const num_of_shards As UInt32 = 2
            For i As UInt32 = 0 To num_of_shards - uint32_1
                Dim n As New nplus1plusn(New shard(Of String)(i, num_of_shards), 2)
                n.train(tar.reader.unzip(New tar.selector() With {.pattern = input Or "tar_manual_test.zip_*"}))
                n.dump(percentage Or 0.9).
                  dump(String.Concat(output Or "cjk.nplus1plusn.2", ".", i, ".bin"))
            Next
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub from_tar_raw_2()
            Const num_of_shards As UInt32 = 257
            For i As UInt32 = 0 To num_of_shards - uint32_1
                Dim n As New nplus1plusn(New shard(Of String)(i, num_of_shards), 2)
                n.train(tar.reader.unzip(New tar.selector() With {.pattern = input Or "tar_manual_test.zip_*"}))
                n.dump_raw(percent Or 0.2).
                  dump(String.Concat(output Or "cjk.nplus1plusn.2", ".", i, ".raw.bin"))
            Next
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub from_tar_3()
            Const num_of_shards As UInt32 = 4
            For i As UInt32 = 0 To num_of_shards - uint32_1
                Dim n As New nplus1plusn(New shard(Of String)(i, num_of_shards), 3)
                n.train(tar.reader.unzip(New tar.selector() With {.pattern = input Or "tar_manual_test.zip_*"}))
                n.dump(percentage Or 0.9).
                  dump(String.Concat(output Or "cjk.nplus1plusn.3", ".", i, ".bin"))
            Next
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub from_tar_raw_3()
            Const num_of_shards As UInt32 = 1031
            For i As UInt32 = 0 To num_of_shards - uint32_1
                Dim n As New nplus1plusn(New shard(Of String)(i, num_of_shards), 3)
                n.train(tar.reader.unzip(New tar.selector() With {.pattern = input Or "tar_manual_test.zip_*"}))
                n.dump_raw(percent Or 0.2).
                  dump(String.Concat(output Or "cjk.nplus1plusn.3", ".", i, ".raw.bin"))
            Next
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub from_tar_4()
            Const num_of_shards As UInt32 = 8
            For i As UInt32 = 0 To num_of_shards - uint32_1
                Dim n As New nplus1plusn(New shard(Of String)(i, num_of_shards), 4)
                n.train(tar.reader.unzip(New tar.selector() With {.pattern = input Or "tar_manual_test.zip_*"}))
                n.dump(percentage Or 0.9).
                  dump(String.Concat(output Or "cjk.nplus1plusn.4", ".", i, ".bin"))
            Next
        End Sub
    End Class
End Namespace
