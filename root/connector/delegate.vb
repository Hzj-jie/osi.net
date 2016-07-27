
'this file is generated by osi/root/codegen/delegate/delegate.exe
'so edit osi/root/codegen/delegate/delegate.cpp instead of this file

Imports System.Threading
Imports osi.root.delegates
Imports osi.root.constants
Imports osi.root.connector

Public Module _delegate
    Public Function do_(Of RT) _
                    (ByVal d As Func(Of RT),
                     ByVal false_value As RT) As RT
            If d Is Nothing Then
                Return false_value
            End If
            Try
                Return d()
            Catch ex As ThreadAbortException
                raise_error(error_type.warning, "thread abort")
                Return false_value
            Catch ex As Exception
                log_unhandled_exception(ex)
                Return false_value
            End Try
        End Function

    Public Sub void_ _
                    (ByVal d As Action)
            If d Is Nothing Then
                Return
            End If
            Try
                d()
            Catch ex As ThreadAbortException
                raise_error(error_type.warning, "thread abort")
                Return
            Catch ex As Exception
                log_unhandled_exception(ex)
                Return
            End Try
        End Sub

    Public Function do_(Of T0, RT) _
                    (ByVal d As _do(Of T0, RT),
                     ByRef i0 As T0,
                     ByVal false_value As RT) As RT
            If d Is Nothing Then
                Return false_value
            End If
            Try
                Return d(i0)
            Catch ex As ThreadAbortException
                raise_error(error_type.warning, "thread abort")
                Return false_value
            Catch ex As Exception
                log_unhandled_exception(ex)
                Return false_value
            End Try
        End Function

    Public Sub void_(Of T0) _
                    (ByVal d As void(Of T0),
                     ByRef i0 As T0)
            If d Is Nothing Then
                Return
            End If
            Try
                d(i0)
            Catch ex As ThreadAbortException
                raise_error(error_type.warning, "thread abort")
                Return
            Catch ex As Exception
                log_unhandled_exception(ex)
                Return
            End Try
        End Sub

    Public Function do_(Of T0, RT) _
                    (ByVal d As Func(Of T0, RT),
                     ByVal i0 As T0,
                     ByVal false_value As RT) As RT
            If d Is Nothing Then
                Return false_value
            End If
            Try
                Return d(i0)
            Catch ex As ThreadAbortException
                raise_error(error_type.warning, "thread abort")
                Return false_value
            Catch ex As Exception
                log_unhandled_exception(ex)
                Return false_value
            End Try
        End Function

    Public Sub void_(Of T0) _
                    (ByVal d As Action(Of T0),
                     ByVal i0 As T0)
            If d Is Nothing Then
                Return
            End If
            Try
                d(i0)
            Catch ex As ThreadAbortException
                raise_error(error_type.warning, "thread abort")
                Return
            Catch ex As Exception
                log_unhandled_exception(ex)
                Return
            End Try
        End Sub

    Public Function do_(Of T0, T1, RT) _
                    (ByVal d As _do(Of T0, T1, RT),
                     ByRef i0 As T0,
                     ByRef i1 As T1,
                     ByVal false_value As RT) As RT
            If d Is Nothing Then
                Return false_value
            End If
            Try
                Return d(i0, i1)
            Catch ex As ThreadAbortException
                raise_error(error_type.warning, "thread abort")
                Return false_value
            Catch ex As Exception
                log_unhandled_exception(ex)
                Return false_value
            End Try
        End Function

    Public Sub void_(Of T0, T1) _
                    (ByVal d As void(Of T0, T1),
                     ByRef i0 As T0,
                     ByRef i1 As T1)
            If d Is Nothing Then
                Return
            End If
            Try
                d(i0, i1)
            Catch ex As ThreadAbortException
                raise_error(error_type.warning, "thread abort")
                Return
            Catch ex As Exception
                log_unhandled_exception(ex)
                Return
            End Try
        End Sub

    Public Function do_(Of T0, T1, RT) _
                    (ByVal d As Func(Of T0, T1, RT),
                     ByVal i0 As T0,
                     ByVal i1 As T1,
                     ByVal false_value As RT) As RT
            If d Is Nothing Then
                Return false_value
            End If
            Try
                Return d(i0, i1)
            Catch ex As ThreadAbortException
                raise_error(error_type.warning, "thread abort")
                Return false_value
            Catch ex As Exception
                log_unhandled_exception(ex)
                Return false_value
            End Try
        End Function

    Public Sub void_(Of T0, T1) _
                    (ByVal d As Action(Of T0, T1),
                     ByVal i0 As T0,
                     ByVal i1 As T1)
            If d Is Nothing Then
                Return
            End If
            Try
                d(i0, i1)
            Catch ex As ThreadAbortException
                raise_error(error_type.warning, "thread abort")
                Return
            Catch ex As Exception
                log_unhandled_exception(ex)
                Return
            End Try
        End Sub

    Public Function do_(Of T0, T1, T2, RT) _
                    (ByVal d As _do(Of T0, T1, T2, RT),
                     ByRef i0 As T0,
                     ByRef i1 As T1,
                     ByRef i2 As T2,
                     ByVal false_value As RT) As RT
            If d Is Nothing Then
                Return false_value
            End If
            Try
                Return d(i0, i1, i2)
            Catch ex As ThreadAbortException
                raise_error(error_type.warning, "thread abort")
                Return false_value
            Catch ex As Exception
                log_unhandled_exception(ex)
                Return false_value
            End Try
        End Function

    Public Sub void_(Of T0, T1, T2) _
                    (ByVal d As void(Of T0, T1, T2),
                     ByRef i0 As T0,
                     ByRef i1 As T1,
                     ByRef i2 As T2)
            If d Is Nothing Then
                Return
            End If
            Try
                d(i0, i1, i2)
            Catch ex As ThreadAbortException
                raise_error(error_type.warning, "thread abort")
                Return
            Catch ex As Exception
                log_unhandled_exception(ex)
                Return
            End Try
        End Sub

    Public Function do_(Of T0, T1, T2, RT) _
                    (ByVal d As Func(Of T0, T1, T2, RT),
                     ByVal i0 As T0,
                     ByVal i1 As T1,
                     ByVal i2 As T2,
                     ByVal false_value As RT) As RT
            If d Is Nothing Then
                Return false_value
            End If
            Try
                Return d(i0, i1, i2)
            Catch ex As ThreadAbortException
                raise_error(error_type.warning, "thread abort")
                Return false_value
            Catch ex As Exception
                log_unhandled_exception(ex)
                Return false_value
            End Try
        End Function

    Public Sub void_(Of T0, T1, T2) _
                    (ByVal d As Action(Of T0, T1, T2),
                     ByVal i0 As T0,
                     ByVal i1 As T1,
                     ByVal i2 As T2)
            If d Is Nothing Then
                Return
            End If
            Try
                d(i0, i1, i2)
            Catch ex As ThreadAbortException
                raise_error(error_type.warning, "thread abort")
                Return
            Catch ex As Exception
                log_unhandled_exception(ex)
                Return
            End Try
        End Sub

    Public Function do_(Of T0, T1, T2, T3, RT) _
                    (ByVal d As _do(Of T0, T1, T2, T3, RT),
                     ByRef i0 As T0,
                     ByRef i1 As T1,
                     ByRef i2 As T2,
                     ByRef i3 As T3,
                     ByVal false_value As RT) As RT
            If d Is Nothing Then
                Return false_value
            End If
            Try
                Return d(i0, i1, i2, i3)
            Catch ex As ThreadAbortException
                raise_error(error_type.warning, "thread abort")
                Return false_value
            Catch ex As Exception
                log_unhandled_exception(ex)
                Return false_value
            End Try
        End Function

    Public Sub void_(Of T0, T1, T2, T3) _
                    (ByVal d As void(Of T0, T1, T2, T3),
                     ByRef i0 As T0,
                     ByRef i1 As T1,
                     ByRef i2 As T2,
                     ByRef i3 As T3)
            If d Is Nothing Then
                Return
            End If
            Try
                d(i0, i1, i2, i3)
            Catch ex As ThreadAbortException
                raise_error(error_type.warning, "thread abort")
                Return
            Catch ex As Exception
                log_unhandled_exception(ex)
                Return
            End Try
        End Sub

    Public Function do_(Of T0, T1, T2, T3, RT) _
                    (ByVal d As Func(Of T0, T1, T2, T3, RT),
                     ByVal i0 As T0,
                     ByVal i1 As T1,
                     ByVal i2 As T2,
                     ByVal i3 As T3,
                     ByVal false_value As RT) As RT
            If d Is Nothing Then
                Return false_value
            End If
            Try
                Return d(i0, i1, i2, i3)
            Catch ex As ThreadAbortException
                raise_error(error_type.warning, "thread abort")
                Return false_value
            Catch ex As Exception
                log_unhandled_exception(ex)
                Return false_value
            End Try
        End Function

    Public Sub void_(Of T0, T1, T2, T3) _
                    (ByVal d As Action(Of T0, T1, T2, T3),
                     ByVal i0 As T0,
                     ByVal i1 As T1,
                     ByVal i2 As T2,
                     ByVal i3 As T3)
            If d Is Nothing Then
                Return
            End If
            Try
                d(i0, i1, i2, i3)
            Catch ex As ThreadAbortException
                raise_error(error_type.warning, "thread abort")
                Return
            Catch ex As Exception
                log_unhandled_exception(ex)
                Return
            End Try
        End Sub

    Public Function do_(Of T0, T1, T2, T3, T4, RT) _
                    (ByVal d As _do(Of T0, T1, T2, T3, T4, RT),
                     ByRef i0 As T0,
                     ByRef i1 As T1,
                     ByRef i2 As T2,
                     ByRef i3 As T3,
                     ByRef i4 As T4,
                     ByVal false_value As RT) As RT
            If d Is Nothing Then
                Return false_value
            End If
            Try
                Return d(i0, i1, i2, i3, i4)
            Catch ex As ThreadAbortException
                raise_error(error_type.warning, "thread abort")
                Return false_value
            Catch ex As Exception
                log_unhandled_exception(ex)
                Return false_value
            End Try
        End Function

    Public Sub void_(Of T0, T1, T2, T3, T4) _
                    (ByVal d As void(Of T0, T1, T2, T3, T4),
                     ByRef i0 As T0,
                     ByRef i1 As T1,
                     ByRef i2 As T2,
                     ByRef i3 As T3,
                     ByRef i4 As T4)
            If d Is Nothing Then
                Return
            End If
            Try
                d(i0, i1, i2, i3, i4)
            Catch ex As ThreadAbortException
                raise_error(error_type.warning, "thread abort")
                Return
            Catch ex As Exception
                log_unhandled_exception(ex)
                Return
            End Try
        End Sub

End Module
