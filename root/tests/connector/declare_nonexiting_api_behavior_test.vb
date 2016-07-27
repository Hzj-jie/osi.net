
Imports System.Runtime.InteropServices
Imports osi.root.utt

Public Class declare_nonexiting_api_behavior_test
    Inherits [case]

    Private Class container
        Public Declare Function THIS_SHOULD_NOT_EXIST _
                                Lib "this_should_not_exist.not_exist" _
                                (ByVal i As Int32) As Boolean
    End Class

    Private Class container2
        <DllImport("this_should_not_exist.not_exist")> _
        Public Shared Function THIS_SHOULD_NOT_EXIST(ByVal i As Int32) As Boolean
        End Function
    End Class

    Private Class container3
        Private Declare Function THIS_SHOULD_NOT_EXIST _
                                 Lib "this_should_not_exist.not_exist" _
                                 (ByVal i As Int32) As Boolean

        Public Shared Function proxy(ByVal i As Int32) As Boolean
            Try
                Return THIS_SHOULD_NOT_EXIST(i)
            Catch ex As DllNotFoundException
                Return False
            End Try
        End Function
    End Class

    Private Class container4
        <DllImport("this_should_not_exist.not_exist")> _
        Private Shared Function THIS_SHOULD_NOT_EXIST(ByVal i As Int32) As Boolean
        End Function

        Public Shared Function proxy(ByVal i As Int32) As Boolean
            Try
                Return THIS_SHOULD_NOT_EXIST(i)
            Catch ex As DllNotFoundException
                Return False
            End Try
        End Function
    End Class

    Private Shared Function declare_case() As Boolean
        Dim r As Boolean = False
        Dim has_ex As Boolean = False
        Try
            r = container.THIS_SHOULD_NOT_EXIST(0)
        Catch ex As DllNotFoundException
            has_ex = True
        End Try
        assert_false(r)
        assert_true(has_ex)
        Return True
    End Function

    Private Shared Function dllimport_case() As Boolean
        Dim r As Boolean = False
        Dim has_ex As Boolean = False
        Try
            r = container2.THIS_SHOULD_NOT_EXIST(0)
        Catch ex As DllNotFoundException
            has_ex = True
        End Try
        assert_false(r)
        assert_true(has_ex)
        Return True
    End Function

    Private Shared Function proxy_case() As Boolean
        assert_false(container3.proxy(0))
        assert_false(container4.proxy(0))
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return declare_case() AndAlso
               dllimport_case() AndAlso
               proxy_case()
    End Function
End Class
