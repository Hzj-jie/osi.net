
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _file_system
    Public Enum path_compare_result
        equal = 0
        left_small = -1
        right_small = 1
        left_large = right_small
        right_large = left_small
        left_invalid = -2
        right_invalid = 2
        both_invalid = min_int32
    End Enum

    Public ReadOnly file_system_case_sensitive As Boolean = Function() As Boolean
                                                                Dim tf As String = guid_str()
                                                                Try
                                                                    File.WriteAllText(strtoupper(tf), character.null)
                                                                    Return Not File.Exists(strtolower(tf))
                                                                Finally
                                                                    File.Delete(strtoupper(tf))
                                                                End Try
                                                            End Function()

    <Extension()> Public Function full_path(ByVal this As String, ByRef output As String) As Boolean
        If this Is Nothing Then
            'avoid to have an exception
            Return False
        End If
        Try
            output = Path.GetFullPath(this)
            Return True
        Catch
            Return False
        End Try
    End Function

    <Extension()> Public Function full_path(ByVal this As String) As String
        Dim o As String = Nothing
        If full_path(this, o) Then
            Return o
        End If
        Return Nothing
    End Function

    <Extension()> Public Sub path_compare(ByVal this As String,
                                          ByRef that As String,
                                          ByRef o As path_compare_result)
        Dim ls As Boolean = False
        Dim rs As Boolean = False
        ls = this.full_path(this)
        rs = that.full_path(that)
        If ls AndAlso rs Then
            Dim c As Int32 = 0
            c = strcmp(this, that, file_system_case_sensitive)
            o = If(c < 0,
                   path_compare_result.left_small,
                   If(c > 0,
                      path_compare_result.right_small,
                      path_compare_result.equal))
        ElseIf ls Then
            o = path_compare_result.right_invalid
        ElseIf rs Then
            o = path_compare_result.left_invalid
        Else
            o = path_compare_result.both_invalid
        End If
    End Sub

    <Extension()> Public Function path_compare(ByVal this As String, ByVal that As String) As Int32
        Dim o As path_compare_result = Nothing
        path_compare(this, that, o)
        Select Case o
            Case path_compare_result.both_invalid, path_compare_result.equal
                Return 0
            Case path_compare_result.left_invalid, path_compare_result.left_small
                Return -1
            Case path_compare_result.right_invalid, path_compare_result.right_small
                Return 1
            Case Else
                assert(False)
                Return max_int32
        End Select
    End Function

    <Extension()> Public Function path_same(ByVal this As String, ByVal that As String) As Boolean
        Return path_compare(this, that) = 0
    End Function
End Module
