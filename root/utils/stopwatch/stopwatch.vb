
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class stopwatch
    Private Shared Function push(ByVal p As Func(Of Func(Of Boolean), Boolean), ByVal e As [event]) As Boolean
        assert(Not p Is Nothing)
        If e Is Nothing OrElse e.canceled() Then
            Return False
        End If
        Return p(AddressOf e.do)
    End Function

    Public Shared Function push(ByVal e As [event]) As Boolean
        Return push(AddressOf queue_runner.push, e)
    End Function

    Public Shared Function push_only(ByVal e As [event]) As Boolean
        Return push(AddressOf queue_runner.push_only, e)
    End Function

    Private Shared Function push(ByVal p As Func(Of [event], Boolean),
                                 ByVal waitms As UInt32,
                                 ByVal d As Action) As [event]
        assert(Not p Is Nothing)
        If d Is Nothing Then
            Return Nothing
        End If
        Dim rtn As [event] = Nothing
        rtn = New [event](waitms, d)
        assert(p(rtn))
        Return rtn
    End Function

    Public Shared Function push(ByVal waitms As UInt32, ByVal d As Action) As [event]
        Return push(Function(ByVal e As [event]) As Boolean
                        Return push(e)
                    End Function, waitms, d)
    End Function

    Public Shared Function push_only(ByVal waitms As UInt32, ByVal d As Action) As [event]
        Return push(Function(ByVal e As [event]) As Boolean
                        Return push_only(e)
                    End Function, waitms, d)
    End Function

    'the waitms is not consistant with other timeout settings,
    'since it's not reasonable to have a stopwatch event to be run in max_uint32 milliseconds later
    Private Shared Function _uint32(ByVal i As Int32) As UInt32
        Return If(i < 0, uint32_0, CUInt(i))
    End Function

    Private Shared Function _uint32(ByVal i As Int64) As UInt32
        Return If(i < 0, uint32_0, If(i > max_uint32, max_uint32, CUInt(i)))
    End Function

    Public Shared Function push(ByVal waitms As Int64, ByVal d As Action) As [event]
        Return push(_uint32(waitms), d)
    End Function

    Public Shared Function push(ByVal waitms As Int32, ByVal d As Action) As [event]
        Return push(_uint32(waitms), d)
    End Function

    Private Sub New()
    End Sub
End Class
