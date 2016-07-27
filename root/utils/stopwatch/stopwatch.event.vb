﻿
Imports osi.root.connector
Imports osi.root.lock

Partial Public Class stopwatch
    Public Class [event]
        Private ReadOnly waitms As uint32
        Private ReadOnly d As Action
        Private _canceled As Boolean
        Private ms As Int64

        Public Sub New(ByVal waitms As UInt32, ByVal d As Action)
            assert(Not d Is Nothing)
            Me.d = d
            Me.waitms = waitms
            Me.restart()
        End Sub

        Public Shared Property current() As [event]
            Get
                Return call_stack(Of [event]).current()
            End Get
            Private Set(ByVal value As [event])
                call_stack(Of [event]).current() = value
            End Set
        End Property

        Public Sub restart()
            Me.ms = nowadays.milliseconds() + waitms
        End Sub

        Public Sub cancel()
            _canceled = True
        End Sub

        Public Sub [resume]()
            _canceled = False
        End Sub

        Public Function canceled() As Boolean
            Return _canceled
        End Function

        Friend Function [do]() As Boolean
            If canceled() Then
                Return False
            Else
                Dim diff As Int64 = 0
                diff = nowadays.milliseconds() - ms
                If diff > 0 Then
                    current() = Me
                    Try
                        void_(d)
                    Finally
                        current() = Nothing
                    End Try
                    Return False
                Else
                    Return True
                End If
            End If
        End Function
    End Class
End Class
