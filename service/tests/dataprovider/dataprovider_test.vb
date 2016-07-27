
Imports System.IO
Imports osi.root.utt
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utils
Imports osi.root.delegates
Imports osi.service.dataprovider

Public Class dataprovider_test
    Inherits [case]

    Private Shared ReadOnly filename As String = Path.Combine(temp_folder, guid_str())
    Private Shared ReadOnly ds() As String = {"first text",
                                              "second text",
                                              "third text",
                                              "forth text"}

    Private Overloads Shared Function run(ByVal ctor As Func(Of idataprovider)) As Boolean
        assert(Not ctor Is Nothing)
        File.Delete(filename)
        Dim sz As Int64 = 0
        sz = dataprovider_count()
        Dim dp As dataprovider(Of String) = cast(Of dataprovider(Of String))(ctor())
        assert_equal(dataprovider_count(), sz + 1)
        assert_false(dp.valid())
        For i As Int32 = 0 To array_size(ds) - 1
            File.WriteAllText(filename, ds(i))
            sleep()
            If assert_true(dp.valid()) Then
                assert_equal(dp.get(), ds(i))
            End If
        Next
        File.Delete(filename)
        If assert_true(dp.valid()) Then
            assert_equal(dp.get(), ds(array_size(ds) - 1))
        End If
        dp.expire()
        Return True
    End Function

    Public Overrides Function preserved_processors() As Int16
        Return Environment.ProcessorCount()
    End Function

    Public Overrides Function run() As Boolean
        stop_auto_cleanup()
        waitfor_auto_cleanup_stop()
        Return run(Function() file_content_dataprovider.generate(filename, 500)) AndAlso
               run(Function() localfile_content_dataprovider.generate(filename))
    End Function
End Class
