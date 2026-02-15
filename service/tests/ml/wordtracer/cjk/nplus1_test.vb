
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.delegates
Imports osi.root.utils
Imports osi.root.utt.attributes
Imports osi.service.resource
Imports nplus1 = osi.service.ml.wordtracer.cjk.nplus1

' C:\Users\Hzj_j\git\osi.net\root\utt\bin\Release\training-data\zh>v:\Users\hzj_jie\git\osi.net\root\utt\bin\Release\osi.root.utt nplus1_test.from_tar_raw_2 --percent=0

Namespace wordtracer.cjk
    <test>
    Public NotInheritable Class nplus1_test
        Private Shared input As argument(Of String)
        Private Shared output As argument(Of String)
        Private Shared shards As argument(Of UInt32)
        Private Shared percent As argument(Of Double)
        Private Shared percentage As argument(Of Double)

        <test>
        <command_line_specified>
        Private Shared Sub from_tar()
            Dim n As New nplus1(1)
            n.train(tar.reader.unzip(New tar.selector() With {.pattern = input Or "tar_manual_test.zip_*"}))
            n.dump(percentage Or 0.9).
              dump(output Or "cjk.nplus1.1.bin")
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub from_tar_raw()
            Dim n As New nplus1(1)
            n.train(tar.reader.unzip(New tar.selector() With {.pattern = input Or "tar_manual_test.zip_*"}))
            n.dump_raw(percent Or 0.2).
              dump(output Or "cjk.nplus1.1.raw.bin")
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub from_tar_2()
            Dim num_of_shards As UInt32 = (shards Or 3)
            concurrency_runner.execute(
                num_of_shards,
                Sub(ByVal i As UInt32)
                    Dim n As New nplus1(New shard(Of String)(i, num_of_shards), 2)
                    n.train(tar.reader.unzip(New tar.selector() With {.pattern = input Or "tar_manual_test.zip_*"}))
                    n.dump(percentage Or 0.9).
                      dump(String.Concat(output Or "cjk.nplus1.2", ".", i, ".bin"))
                End Sub)
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub from_tar_raw_2()
            Dim num_of_shards As UInt32 = (shards Or 3)
            concurrency_runner.execute(
                num_of_shards,
                Sub(ByVal i As UInt32)
                    Dim n As New nplus1(New shard(Of String)(i, num_of_shards), 2)
                    n.train(tar.reader.unzip(New tar.selector() With {.pattern = input Or "tar_manual_test.zip_*"}))
                    n.dump_raw(percent Or 0.2).
                      dump(String.Concat(output Or "cjk.nplus1.2", ".", i, ".raw.bin"))
                End Sub)
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub from_tar_3()
            Dim num_of_shards As UInt32 = (shards Or 7)
            concurrency_runner.execute(
                num_of_shards,
                Sub(ByVal i As UInt32)
                    Dim n As New nplus1(New shard(Of String)(i, num_of_shards), 3)
                    n.train(tar.reader.unzip(New tar.selector() With {.pattern = input Or "tar_manual_test.zip_*"}))
                    n.dump(percentage Or 0.9).
                      dump(String.Concat(output Or "cjk.nplus1.3", ".", i, ".bin"))
                End Sub)
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub from_tar_raw_3()
            Dim num_of_shards As UInt32 = (shards Or 7)
            concurrency_runner.execute(
                num_of_shards,
                Sub(ByVal i As UInt32)
                    Dim n As New nplus1(New shard(Of String)(i, num_of_shards), 3)
                    n.train(tar.reader.unzip(New tar.selector() With {.pattern = input Or "tar_manual_test.zip_*"}))
                    n.dump_raw(percent Or 0.2).
                      dump(String.Concat(output Or "cjk.nplus1.3", ".", i, ".raw.bin"))
                End Sub)
        End Sub

        <test>
        <command_line_specified>
        Private Shared Sub from_tar_4()
            Dim num_of_shards As UInt32 = (shards Or 15)
            concurrency_runner.execute(
                num_of_shards,
                Sub(ByVal i As UInt32)
                    Dim n As New nplus1(New shard(Of String)(i, num_of_shards), 4)
                    n.train(tar.reader.unzip(New tar.selector() With {.pattern = input Or "tar_manual_test.zip_*"}))
                    n.dump(percentage Or 0.9).
                      dump(String.Concat(output Or "cjk.nplus1.4", ".", i, ".bin"))
                End Sub)
        End Sub
    End Class
End Namespace
