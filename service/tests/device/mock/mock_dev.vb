
Imports osi.root.template
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.device

Public Interface mock_dev_interface
End Interface

Public Class mock_dev(Of PROTECTOR)
    Inherits cd_object(Of PROTECTOR)
    Implements mock_dev_interface
    Private Shared ReadOnly closed_count As atomic_int
    Private Shared ReadOnly f_validator As Func(Of mock_dev(Of PROTECTOR), Boolean)
    Private Shared ReadOnly f_closer As Action(Of mock_dev(Of PROTECTOR))
    Private Shared ReadOnly f_identifier As Func(Of mock_dev(Of PROTECTOR), String)
    Private Shared ReadOnly f_check As Action(Of mock_dev(Of PROTECTOR))
    Private ReadOnly c As atomic_int

    Shared Sub New()
        closed_count = New atomic_int()
        f_validator = AddressOf validate
        f_closer = AddressOf close
        f_identifier = AddressOf identity
        f_check = AddressOf check
    End Sub

    Public Sub New()
        MyBase.New()
        c = New atomic_int()
    End Sub

    Public Shared Shadows Sub reset()
        closed_count.set(0)
        cd_object(Of PROTECTOR).reset()
    End Sub

    Public Shared Function closed_instance_count() As UInt32
        Return CUInt(+closed_count)
    End Function

    Private Class validator
        Inherits __do(Of mock_dev(Of PROTECTOR), Boolean)

        Public Overrides Function at(ByRef k As mock_dev(Of PROTECTOR)) As Boolean
            Return validate(k)
        End Function
    End Class

    Private Class closer
        Inherits __void(Of mock_dev(Of PROTECTOR))

        Public Overrides Sub at(ByRef i As mock_dev(Of PROTECTOR))
            close(i)
        End Sub
    End Class

    Private Class identifier
        Inherits __do(Of mock_dev(Of PROTECTOR), String)

        Public Overrides Function at(ByRef k As mock_dev(Of PROTECTOR)) As String
            Return identity(k)
        End Function
    End Class

    Private Class checker
        Inherits __void(Of mock_dev(Of PROTECTOR))

        Public Overrides Sub at(ByRef i As mock_dev(Of PROTECTOR))
            check(i)
        End Sub
    End Class

    Public Shared Function create(ByVal attach As Boolean,
                                  ByVal attach_by_function As Boolean) As idevice(Of mock_dev(Of PROTECTOR))
        If attach Then
            If attach_by_function Then
                If rnd_bool() Then
                    Return (New mock_dev(Of PROTECTOR)()).make_device(AddressOf validate,
                                                                      AddressOf close,
                                                                      AddressOf identity,
                                                                      AddressOf check)
                Else
                    Return (New mock_dev(Of PROTECTOR)()).make_device(f_validator,
                                                                      f_closer,
                                                                      f_identifier,
                                                                      f_check)
                End If
            Else
                Return (New mock_dev(Of PROTECTOR)()).make_device(Of validator, closer, identifier, checker)()
            End If
        Else
            Return (New mock_dev(Of PROTECTOR)()).make_device()
        End If
    End Function

    Public Shared Function create(Optional ByVal attach As Boolean = True) As idevice(Of mock_dev(Of PROTECTOR))
        Return create(attach, rnd_bool())
    End Function

    Public Shared Function validate(ByVal x As mock_dev(Of PROTECTOR)) As Boolean
        assert(Not x Is Nothing)
        Return Not x.closed()
    End Function

    Public Shared Sub close(ByVal x As mock_dev(Of PROTECTOR))
        assert(Not x Is Nothing)
        If x.c.increment() = 1 Then
            closed_count.increment()
        End If
    End Sub

    Public Shared Function identity(ByVal x As mock_dev(Of PROTECTOR)) As String
        assert(Not x Is Nothing)
        Return strcat("mock_dev_", x.id)
    End Function

    Public Shared Sub check(ByVal x As mock_dev(Of PROTECTOR))
    End Sub

    Public Function close_times() As UInt32
        Return CUInt(+c)
    End Function

    Public Function closed() As Boolean
        Return close_times() > 0
    End Function
End Class
