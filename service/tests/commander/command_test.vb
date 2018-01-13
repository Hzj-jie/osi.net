
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utt
Imports osi.service.commander
Imports osi.service.convertor

Public Class command_test
    Inherits [case]

    Private Class constants
        Public ReadOnly action As String
        Public ReadOnly para1 As String
        Public ReadOnly value1 As Int64
        Public ReadOnly para2 As String
        Public ReadOnly value2 As Boolean
        Public ReadOnly para3 As String
        Public ReadOnly value3 As String
        Public ReadOnly para4 As String
        Public ReadOnly value4 As Int32
        Public ReadOnly para5 As Int32
        Public ReadOnly value5 As Int32

        Public Sub New()
            action = guid_str()
            para1 = guid_str()
            para2 = guid_str()
            para3 = guid_str()
            para4 = Nothing
            para5 = rnd_int(min_int32, max_int32)
            value1 = rnd_int(min_int32, max_int32)
            value2 = (rnd_int(0, 2) = 0)
            value3 = guid_str()
            value4 = rnd_int(min_int32, max_int32)
            value5 = rnd_int(min_int32, max_int32)
        End Sub
    End Class

    Private Shared Function command_validation(ByVal c As command, ByVal constants As constants) As Boolean
        assert(Not constants Is Nothing)
        If Not assert_not_nothing(c) Then
            Return False
        End If
        If assert_not_nothing(c.action()) Then
            assert_equal(c.action().to_string(), constants.action)
        End If
        If assert_not_nothing(c.parameter(constants.para1)) Then
            assert_equal(c.parameter(constants.para1).to_int64(), constants.value1)
        End If
        assert_equal(c.parameter(Of String, Int64)(constants.para1), constants.value1)
        If assert_not_nothing(c.parameter(constants.para2)) Then
            assert_equal(c.parameter(constants.para2).to_bool(), constants.value2)
        End If
        assert_equal(c.parameter(Of String, Boolean)(constants.para2), constants.value2)
        If assert_not_nothing(c.parameter(constants.para3)) Then
            assert_equal(c.parameter(constants.para3).to_string(), constants.value3)
        End If
        assert_equal(c.parameter(Of String, String)(constants.para3), constants.value3)
        assert_nothing(c.parameter(constants.para4))
        If assert_not_nothing(c.parameter(constants.para5)) Then
            assert_equal(c.parameter(constants.para5).to_int32(), constants.value5)
        End If
        assert_equal(c.parameter(Of Int32, Int32)(constants.para5), constants.value5)
        Return True
    End Function

    Private Shared Function run_case() As Boolean
        Dim c As command = Nothing
        c = New command()
        Dim constants As constants = Nothing
        constants = New constants()
        c.attach(constants.action) _
         .attach(constants.para1, constants.value1) _
         .attach(constants.para2, constants.value2) _
         .attach(constants.para3, constants.value3) _
         .attach(constants.para4, constants.value4) _
         .attach(constants.para5, constants.value5)
        If Not command_validation(c, constants) Then
            Return False
        End If
        Dim b() As Byte = Nothing
        assert_true(bytes_serializer.to_bytes(c, b))
        assert_more(array_size(b), uint32_0)
        Dim r As command = Nothing
        assert_true(bytes_serializer.from_bytes(b, r))
        If Not command_validation(r, constants) Then
            Return False
        End If

        Dim s As String = Nothing
        assert_true(uri_serializer.to_str(c, s))
        assert_false(String.IsNullOrEmpty(s))
        assert_true(uri_serializer.from_str(s, r))
        If Not command_validation(r, constants) Then
            Return False
        End If

        assert_true(string_serializer.to_str(c, s))
        assert_false(String.IsNullOrEmpty(s))
        assert_true(string_serializer.from_str(s, r))
        If Not command_validation(r, constants) Then
            Return False
        End If

        Return True
    End Function

    Public Overrides Function run() As Boolean
        For i As Int32 = 0 To 128 - 1
            If Not run_case() Then
                Return False
            End If
        Next
        Return True
    End Function
End Class
