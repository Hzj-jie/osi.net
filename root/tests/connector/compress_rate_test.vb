
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt

Public Class compress_less_1024_rate_test
    Inherits compress_rate_test

    Protected Overrides Sub compare(ByVal gzip_better As Int32, ByVal deflate_better As Int32)
        assertion.equal(gzip_better, 0)
        assertion.equal(deflate_better, iteration_count)
    End Sub

    Public Sub New()
        MyBase.New(1, 1024)
    End Sub
End Class

Public Class compress_1024_4096_rate_test
    Inherits compress_rate_test

    Protected Overrides Sub compare(ByVal gzip_better As Int32, ByVal deflate_better As Int32)
        assertion.equal(gzip_better, 0)
        assertion.equal(deflate_better, iteration_count)
    End Sub

    Public Sub New()
        MyBase.New(1024, 4096)
    End Sub
End Class

Public Class compress_4096_65536_rate_test
    Inherits compress_rate_test

    Protected Overrides Sub compare(ByVal gzip_better As Int32, ByVal deflate_better As Int32)
        assertion.equal(gzip_better, 0)
        assertion.equal(deflate_better, iteration_count)
    End Sub

    Public Sub New()
        MyBase.New(4096, 65536)
    End Sub
End Class

'MustInherit for utt
Public MustInherit Class compress_rate_test
    Inherits [case]

    Protected Const iteration_count As Int32 = 1024
    Private ReadOnly lower As Int32
    Private ReadOnly upper As Int32

    Protected Sub New(ByVal lower As Int32, ByVal upper As Int32)
        Me.lower = lower
        Me.upper = upper
    End Sub

    Protected Overridable Sub compare(ByVal gzip_better As Int32, ByVal deflate_better As Int32)
    End Sub

    Public Overrides Function run() As Boolean
        Dim gzip_better As Int32 = 0
        Dim deflate_better As Int32 = 0
        For i As Int32 = 0 To iteration_count - 1
            Dim raw() As Byte = Nothing
            Dim gzipped() As Byte = Nothing
            Dim deflated() As Byte = Nothing
            raw = rnd_bytes(rnd_int(lower, upper + 1))
            If assertion.is_true(raw.gzip(gzipped)) AndAlso
               assertion.is_true(raw.deflate(deflated)) Then
                If array_size(gzipped) < array_size(deflated) Then
                    gzip_better += 1
                ElseIf array_size(gzipped) > array_size(deflated) Then
                    deflate_better += 1
                End If
            End If
        Next
        compare(gzip_better, deflate_better)
        Return True
    End Function
End Class
