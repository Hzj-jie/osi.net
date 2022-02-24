
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.lock.slimlock

Public Module _locked
    <Extension()> Public Sub reader_locked(ByRef this As duallock, ByVal d As Action)
        assert(Not d Is Nothing)
        this.reader_wait()
        Try
            void_(d)
        Finally
            this.reader_release()
        End Try
    End Sub

    <Extension()> Public Function reader_locked(Of T)(ByRef this As duallock,
                                                      ByVal d As Func(Of T),
                                                      Optional ByVal false_value As T = Nothing) As T
        assert(Not d Is Nothing)
        this.reader_wait()
        Try
            Return do_(d, false_value)
        Finally
            this.reader_release()
        End Try
    End Function

    <Extension()> Public Sub writer_locked(ByRef this As duallock, ByVal d As Action)
        assert(Not d Is Nothing)
        this.writer_wait()
        Try
            void_(d)
        Finally
            this.writer_release()
        End Try
    End Sub

    <Extension()> Public Function writer_locked(Of T)(ByRef this As duallock,
                                                      ByVal d As Func(Of T),
                                                      Optional ByVal false_value As T = Nothing) As T
        assert(Not d Is Nothing)
        this.writer_wait()
        Try
            Return do_(d, false_value)
        Finally
            this.writer_release()
        End Try
    End Function

    <Extension()> Public Sub locked(Of T As islimlock)(ByRef this As T, ByVal d As Action)
        assert(Not this Is Nothing)
        assert(Not d Is Nothing)
        this.wait()
        Try
            void_(d)
        Finally
            this.release()
        End Try
    End Sub

    <Extension()> Public Function locked(Of T As islimlock, OT) _
                                        (ByRef this As T,
                                         ByVal d As Func(Of OT),
                                         Optional ByVal false_value As OT = Nothing) As OT
        assert(Not this Is Nothing)
        assert(Not d Is Nothing)
        this.wait()
        Try
            Return do_(d, false_value)
        Finally
            this.release()
        End Try
    End Function

    <Extension()> Public Sub reenterable_locked(Of T As ilock)(ByRef this As T, ByVal d As Action)
        assert(Not this Is Nothing)
        assert(Not d Is Nothing)
        If this.held_in_thread() Then
            void_(d)
        Else
            this.locked(d)
        End If
    End Sub

    <Extension()> Public Function reenterable_locked(Of T As ilock, OT) _
                                                    (ByRef this As T,
                                                     ByVal d As Func(Of OT),
                                                     Optional ByVal false_value As OT = Nothing) As OT
        assert(Not this Is Nothing)
        assert(Not d Is Nothing)
        If this.held_in_thread() Then
            Return do_(d, false_value)
        Else
            Return this.locked(d, false_value)
        End If
    End Function

    <Extension()> Public Sub reenterable_locked(ByRef this As slimlock.monitorlock, ByVal d As Action)
        locked(this, d)
    End Sub

    <Extension()> Public Function reenterable_locked(Of T)(ByRef this As slimlock.monitorlock,
                                                           ByVal d As Func(Of T),
                                                           Optional ByVal false_value As T = Nothing) As T
        Return locked(this, d, false_value)
    End Function

    <Extension()> Public Sub reenterable_locked(ByRef this As lock.monitorlock, ByVal d As Action)
        locked(this, d)
    End Sub

    <Extension()> Public Function reenterable_locked(Of T)(ByRef this As lock.monitorlock,
                                                           ByVal d As Func(Of T),
                                                           Optional ByVal false_value As T = Nothing) As T
        Return locked(this, d, false_value)
    End Function

    Public Sub assert_in_lock(Of T As ilock)(ByRef l As T)
        assert(l.held_in_thread())
    End Sub
End Module
