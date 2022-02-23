
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class stopwatch
    Public NotInheritable Class [event]
        Private ReadOnly waitms As UInt32
        Private ReadOnly d As Action
        Private _canceled As Boolean
        Private ms As Int64

        Public Sub New(ByVal waitms As UInt32, ByVal d As Action)
            assert(d IsNot Nothing)
            Me.d = d
            Me.waitms = waitms
            Me.restart()
        End Sub

        Public Shared Property current() As [event]
            Get
                Return instance_stack(Of [event]).current()
            End Get
            Private Set(ByVal value As [event])
                instance_stack(Of [event]).current() = value
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
            End If

            Dim diff As Int64 = 0
            diff = nowadays.milliseconds() - ms
            If diff <= 0 Then
                Return True
            End If

            current() = Me
            Try
                void_(d)
            Finally
                current() = Nothing
            End Try
            Return False
        End Function
    End Class
End Class
