
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utt
Imports osi.service.argument
Imports osi.service.device

Public Class constructor_test
    Inherits [case]

    Private Class protector1
        Private Sub New()
        End Sub
    End Class

    Private Class protector2
        Private Sub New()
        End Sub
    End Class

    Private Shared ReadOnly t1 As String
    Private Shared ReadOnly t2 As String

    Shared Sub New()
        t1 = GetType(protector1).AssemblyQualifiedName()
        t2 = GetType(protector2).AssemblyQualifiedName()
        assert(Not t1.null_or_empty())
        assert(Not t2.null_or_empty())
    End Sub

    Public Overrides Function prepare() As Boolean
        mock_dev(Of protector1).reset()
        mock_dev(Of protector2).reset()
        Return assertion.is_true(constructor(Of mock_dev_interface).register(
                               t1,
                               osi.service.selector.make_allocator(Function(s As var) As mock_dev_interface
                                                                       Return New mock_dev(Of protector1)()
                                                                   End Function))) AndAlso
               assertion.is_true(constructor(Of mock_dev_interface).register(
                               t2,
                               osi.service.selector.make_allocator(Function(s As var) As mock_dev_interface
                                                                       Return New mock_dev(Of protector2)()
                                                                   End Function))) AndAlso
               MyBase.prepare()
    End Function

    Public Overrides Function run() As Boolean
        Dim r As mock_dev_interface = Nothing
        assertion.is_true(constructor(Of mock_dev_interface).resolve(New var({strcat("--type=", t1)}), r))
        assertion.is_not_null(r)
        assertion.is_true(r.GetType().is(GetType(mock_dev(Of protector1))))
        assertion.equal(mock_dev(Of protector1).constructed(), uint32_1)
        assertion.equal(mock_dev(Of protector1).closed_instance_count(), uint32_0)
        assertion.equal(mock_dev(Of protector1).destructed(), uint32_0)

        Dim name As String = Nothing
        name = strcat("mock_dev_interface + ", guid_str())
        r = Nothing
        assertion.is_true(constructor(Of mock_dev_interface).resolve(
                        New var({strcat("--type=", t2), strcat("--name=", name)}),
                        r))
        assertion.is_not_null(r)
        assertion.is_true(r.GetType().is(GetType(mock_dev(Of protector2))))
        assertion.equal(mock_dev(Of protector2).constructed(), uint32_1)
        assertion.equal(mock_dev(Of protector2).closed_instance_count(), uint32_0)
        assertion.equal(mock_dev(Of protector2).destructed(), uint32_0)
        If assertion.is_true(device_pool_manager.register(name, singleton_device_pool.[New](r.make_device()))) Then
            Dim p As singleton_device_pool(Of mock_dev_interface) = Nothing
            assertion.is_true(device_pool_manager.get(Of mock_dev_interface, singleton_device_pool(Of mock_dev_interface)) _
                                               (name, p))
            If assertion.is_not_null(p) Then
                Dim i As idevice(Of mock_dev_interface) = Nothing
                assertion.is_true(p.get(i))
                assertion.reference_equal(i.get(), r)
            End If
            assertion.is_true(device_pool_manager.retire(Of mock_dev_interface)(name))
        End If
        Return True
    End Function

    Public Overrides Function finish() As Boolean
        Return assertion.is_true(constructor(Of mock_dev_interface).erase(t1)) And
               assertion.is_true(constructor(Of mock_dev_interface).erase(t2)) And
               MyBase.finish()
    End Function
End Class
