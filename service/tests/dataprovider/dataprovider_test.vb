
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.dataprovider

Public Class dataprovider_test
    Inherits [case]

    Private Shared ReadOnly filename As String = Path.Combine(temp_folder, guid_str())
    Private Shared ReadOnly ds() As String = {"first text",
                                              "second text",
                                              "third text",
                                              "forth text"}

    Private Overloads Shared Function run(ByVal ctor As Func(Of idataprovider)) As Boolean
        assert(ctor IsNot Nothing)
        File.Delete(filename)
        Dim sz As Int64 = 0
        sz = collection.dataprovider_count()
        Dim dp As dataprovider(Of String) = cast(Of dataprovider(Of String))(ctor())
        assertion.equal(collection.dataprovider_count(), sz + 1)
        assertion.is_false(dp.valid())
        For i As Int32 = 0 To array_size_i(ds) - 1
            File.WriteAllText(filename, ds(i))
            sleep()
            If assertion.is_true(dp.valid()) Then
                assertion.equal(dp.get(), ds(i))
            End If
        Next
        File.Delete(filename)
        If assertion.is_true(dp.valid()) Then
            assertion.equal(dp.get(), ds(array_size_i(ds) - 1))
        End If
        dp.expire()
        Return True
    End Function

    Public Overrides Function reserved_processors() As Int16
        Return CShort(Environment.ProcessorCount())
    End Function

    Public Overrides Function run() As Boolean
        collection.stop_auto_cleanup()
        collection.waitfor_auto_cleanup_stop()
        Return run(Function() file_content_dataprovider.generate(filename, 500)) AndAlso
               run(Function() localfile_content_dataprovider.generate(filename))
    End Function
End Class
