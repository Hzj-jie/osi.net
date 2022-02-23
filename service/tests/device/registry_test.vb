
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.argument
Imports osi.service.device
Imports osi.service.selector
Imports constructor = osi.service.device.constructor
Imports wrapper = osi.service.device.wrapper

Public Class registry_test
    Inherits [case]

    Private Interface mock_dev
    End Interface

    Private Class mock_dev_impl
        Implements mock_dev

        Public ReadOnly v As var

        Public Sub New(ByVal v As var)
            assertion.is_not_null(v)
            Me.v = v
        End Sub

        Public Shared Function create_device(ByVal v As var) As idevice(Of mock_dev)
            Return DirectCast(New mock_dev_impl(v), mock_dev).make_device()
        End Function
    End Class

    Private Class mock_dev_wrapper
        Implements mock_dev

        Public ReadOnly r As mock_dev
        Public ReadOnly v As var

        Public Sub New(ByVal r As mock_dev, ByVal v As var)
            assertion.is_not_null(r)
            assertion.is_not_null(v)
            Me.r = r
            Me.v = v
        End Sub
    End Class

    Private Class mock_dev_creator
        Implements idevice_creator(Of mock_dev)

        Public ReadOnly v As var

        Public Sub New(ByVal v As var)
            assertion.is_not_null(v)
            Me.v = v
        End Sub

        Public Function create(ByRef o As idevice(Of mock_dev)) As Boolean _
                              Implements idevice_creator(Of mock_dev).create
            o = mock_dev_impl.create_device(v)
            Return True
        End Function
    End Class

    Private Class mock_dev_manual_device_exporter
        Inherits device_exporter(Of mock_dev)
        Implements imanual_device_exporter(Of mock_dev)

        Public ReadOnly v As var

        Public Sub New(ByVal v As var)
            assertion.is_not_null(v)
            Me.v = v
        End Sub

        Public Function inject(ByVal d As idevice(Of async_getter(Of mock_dev))) As event_comb _
                              Implements imanual_device_exporter(Of mock_dev).inject
            assert(False)
            Return Nothing
        End Function

        Public Function inject(ByVal d As idevice(Of mock_dev)) As Boolean _
                              Implements imanual_device_exporter(Of mock_dev).inject
            assert(False)
            Return Nothing
        End Function
    End Class

    Public Overrides Function prepare() As Boolean
        If MyBase.prepare() Then
            assert(constructor.register(Function(v As var, ByRef o As mock_dev) As Boolean
                                            o = New mock_dev_impl(v)
                                            Return True
                                        End Function))
            assert(constructor.register(Function(v As var, ByRef o As idevice(Of mock_dev)) As Boolean
                                            o = mock_dev_impl.create_device(v)
                                            Return True
                                        End Function))
            assert(constructor.register(Function(v As var, ByRef o As idevice_creator(Of mock_dev)) As Boolean
                                            o = New mock_dev_creator(v)
                                            Return True
                                        End Function))
            assert(constructor.register(Function(v As var, ByRef o As imanual_device_exporter(Of mock_dev)) As Boolean
                                            o = New mock_dev_manual_device_exporter(v)
                                            Return True
                                        End Function))
            assert(wrapper.register("mock_dev_wrapper",
                                    Function(v As var, i As mock_dev, ByRef o As mock_dev) As Boolean
                                        o = New mock_dev_wrapper(i, v)
                                        Return True
                                    End Function))
            Return True
        Else
            Return False
        End If
    End Function

    Private Shared Function assert_mock_dev_impl(ByVal i As mock_dev, ByVal v As var) As Boolean
        If assertion.is_not_null(i) Then
            Dim j As mock_dev_impl = Nothing
            If assertion.is_true(cast(i, j)) AndAlso assert(j IsNot Nothing) Then
                assertion.reference_equal(j.v, v)
            End If
        End If
        Return True
    End Function

    Private Shared Function assert_mock_dev_wrapper(ByVal i As mock_dev, ByVal v As var) As Boolean
        If assertion.is_not_null(i) Then
            Dim j As mock_dev_wrapper = Nothing
            If assertion.is_true(cast(i, j)) AndAlso assert(j IsNot Nothing) Then
                assertion.reference_equal(j.v, v)
                If Not assert_mock_dev_impl(j.r, v) Then
                    Return False
                End If
            End If
        End If
        Return True
    End Function

    Private Shared Function assert_mock_dev_impl(ByVal i As idevice(Of mock_dev), ByVal v As var) As Boolean
        If assertion.is_not_null(i) Then
            If Not assert_mock_dev_impl(i.get(), v) Then
                Return False
            End If
        End If
        Return True
    End Function

    Private Shared Function assert_mock_dev_wrapper(ByVal i As idevice(Of mock_dev), ByVal v As var) As Boolean
        If assertion.is_not_null(i) Then
            If Not assert_mock_dev_wrapper(i.get(), v) Then
                Return False
            End If
        End If
        Return True
    End Function

    Private Shared Function assert_mock_dev_impl(ByVal i As idevice_creator(Of mock_dev), ByVal v As var) As Boolean
        If assertion.is_not_null(i) Then
            Dim j As mock_dev_creator = Nothing
            If assertion.is_true(cast(i, j)) AndAlso assert(j IsNot Nothing) Then
                assertion.reference_equal(j.v, v)
            End If
        End If
        Return True
    End Function

    Private Shared Function assert_mock_dev_impl(ByVal i As imanual_device_exporter(Of mock_dev),
                                                 ByVal v As var) As Boolean
        If assertion.is_not_null(i) Then
            Dim j As mock_dev_manual_device_exporter = Nothing
            If assertion.is_true(cast(i, j)) AndAlso assert(j IsNot Nothing) Then
                assertion.reference_equal(j.v, v)
            End If
        End If
        Return True
    End Function

    Private Shared Function assert_mock_dev_impl(ByVal i As singleton_device_pool(Of mock_dev),
                                                 ByVal v As var) As Boolean
        If assertion.is_not_null(i) Then
            Dim j As idevice(Of mock_dev) = Nothing
            assert(valuer.try_get(i, binding_flags.instance_private, "d", j))
            If Not assert_mock_dev_impl(j, v) Then
                Return False
            End If
        End If
        Return True
    End Function

    Private Shared Function assert_mock_dev_impl(ByVal i As manual_pre_generated_device_pool(Of mock_dev),
                                                 ByVal v As var) As Boolean
        If assertion.is_not_null(i) Then
            Dim j As imanual_device_exporter(Of mock_dev) = Nothing
            assert(valuer.try_get(i, binding_flags.instance_private, "i", j))
            If Not assert_mock_dev_impl(j, v) Then
                Return False
            End If
        End If
        Return True
    End Function

    Private Shared Function assert_mock_dev_wrapper(ByVal i As idevice_creator(Of mock_dev), ByVal v As var) As Boolean
        If assertion.is_not_null(i) Then
            Dim j As wrappered_device_creator(Of mock_dev) = Nothing
            If assertion.is_true(cast(i, j)) AndAlso assert(j IsNot Nothing) Then
                Dim k As idevice_creator(Of mock_dev) = Nothing
                assert(valuer.try_get(j, binding_flags.instance_private, "c", k))
                If Not assert_mock_dev_impl(k, v) Then
                    Return False
                End If
                assertion.reference_equal(valuer.get(Of var)(j, binding_flags.instance_private, "v"), v)
            End If
        End If
        Return True
    End Function

    Private Shared Function assert_mock_dev_wrapper(ByVal i As iauto_device_exporter(Of mock_dev),
                                                    ByVal v As var) As Boolean
        If assertion.is_not_null(i) Then
            Dim j As auto_device_exporter(Of mock_dev) = Nothing
            If assertion.is_true(cast(i, j)) AndAlso assert(j IsNot Nothing) Then
                Dim k As idevice_creator(Of mock_dev) = Nothing
                k = (New valuer(Of idevice_creator(Of mock_dev))(j, binding_flags.instance_private, "c")).get()
                If Not assert_mock_dev_wrapper(k, v) Then
                    Return False
                End If
            End If
        End If
        Return True
    End Function

    Private Shared Function assert_mock_dev_wrapper(ByVal i As imanual_device_exporter(Of mock_dev),
                                                    ByVal v As var) As Boolean
        Return assert_mock_dev_impl(i, v)
    End Function

    Private Shared Function assert_mock_dev_wrapper(ByVal i As auto_pre_generated_device_pool(Of mock_dev),
                                                    ByVal v As var) As Boolean
        If assertion.is_not_null(i) Then
            Dim j As iauto_device_exporter(Of mock_dev) = Nothing
            assert(valuer.try_get(i, binding_flags.instance_private, "e", j))
            If Not assert_mock_dev_wrapper(j, v) Then
                Return False
            End If
        End If
        Return True
    End Function

    Private Shared Function assert_mock_dev_wrapper(ByVal i As idevice_pool(Of mock_dev),
                                                    ByVal v As var) As Boolean
        If assertion.is_not_null(i) Then
            Dim j As idevice_creator(Of mock_dev) = Nothing
            assert(valuer.try_get_recursively(DirectCast(i, Object), binding_flags.instance_private, "c", j))
            If Not assert_mock_dev_wrapper(j, v) Then
                Return False
            End If
        End If
        Return True
    End Function

    Private Shared Function assert_mock_dev_wrapper(ByVal i As delay_generate_device_pool(Of mock_dev),
                                                    ByVal v As var) As Boolean
        Return assert_mock_dev_wrapper(DirectCast(i, idevice_pool(Of mock_dev)), v)
    End Function

    Private Shared Function assert_mock_dev_wrapper(ByVal i As one_off_device_pool(Of mock_dev),
                                                    ByVal v As var) As Boolean
        Return assert_mock_dev_wrapper(DirectCast(i, idevice_pool(Of mock_dev)), v)
    End Function

    Private Shared Function assert_mock_dev_wrapper(ByVal i As singleton_device_pool(Of mock_dev),
                                                    ByVal v As var) As Boolean
        If assertion.is_not_null(i) Then
            Dim j As idevice(Of mock_dev) = Nothing
            assert(valuer.try_get(i, binding_flags.instance_private, "d", j))
            If Not assert_mock_dev_wrapper(j, v) Then
                Return False
            End If
        End If
        Return True
    End Function

    Private Shared Function assert_mock_dev_wrapper(ByVal i As manual_pre_generated_device_pool(Of mock_dev),
                                                    ByVal v As var) As Boolean
        If assertion.is_not_null(i) Then
            Dim j As imanual_device_exporter(Of mock_dev) = Nothing
            assert(valuer.try_get(i, binding_flags.instance_private, "i", j))
            If Not assert_mock_dev_wrapper(j, v) Then
                Return False
            End If
        End If
        Return True
    End Function

    Private Shared Function mock_dev_case() As Boolean
        Dim v As var = Nothing
        v = New var()
        Dim i As mock_dev = Nothing
        assertion.is_true(constructor.resolve(v, i))
        If Not assert_mock_dev_impl(i, v) Then
            Return False
        End If

        v = New var("--wrapper=mock_dev_wrapper")
        assertion.is_true(constructor.resolve(v, i))
        Return assert_mock_dev_wrapper(i, v)
    End Function

    Private Shared Function idevice_case() As Boolean
        Dim v As var = Nothing
        v = New var()
        Dim i As idevice(Of mock_dev) = Nothing
        assertion.is_true(constructor.resolve(v, i))
        If Not assert_mock_dev_impl(i, v) Then
            Return False
        End If

        v = New var("--wrapper=mock_dev_wrapper")
        assertion.is_true(constructor.resolve(v, i))
        Return assert_mock_dev_wrapper(i, v)
    End Function

    Private Shared Function idevice_creator_case() As Boolean
        Dim v As var = Nothing
        v = New var()
        Dim i As idevice_creator(Of mock_dev) = Nothing
        assertion.is_true(constructor.resolve(v, i))
        ' Always wrappered by wrappered_device_creator
        If Not assert_mock_dev_wrapper(i, v) Then
            Return False
        End If

        v = New var("--wrapper=mock_dev_wrapper")
        assertion.is_true(constructor.resolve(v, i))
        Return assert_mock_dev_wrapper(i, v)
    End Function

    Private Shared Function idevice_exportor_case() As Boolean
        Dim v As var = Nothing
        v = New var()
        Dim i As iauto_device_exporter(Of mock_dev) = Nothing
        assertion.is_true(constructor.resolve(v, i))
        ' Always wrappered by wrappered_device_creator
        If Not assert_mock_dev_wrapper(i, v) Then
            Return False
        End If

        v = New var("--wrapper=mock_dev_wrapper")
        assertion.is_true(constructor.resolve(v, i))
        Return assert_mock_dev_wrapper(i, v)
    End Function

    Private Shared Function auto_pre_generated_device_pool_case() As Boolean
        Dim v As var = Nothing
        v = New var("--max-count=10")
        Dim i As auto_pre_generated_device_pool(Of mock_dev) = Nothing
        assertion.is_true(constructor.resolve(v, i))
        Try
            If Not assert_mock_dev_wrapper(i, v) Then
                Return False
            End If
        Finally
            If i IsNot Nothing Then
                i.close()
            End If
        End Try

        v = New var("--warpper=mock_dev_wrapper --max-count=10")
        assertion.is_true(constructor.resolve(v, i))
        Try
            Return assert_mock_dev_wrapper(i, v)
        Finally
            If i IsNot Nothing Then
                i.close()
            End If
        End Try
    End Function

    Private Shared Function delay_generate_device_pool_case() As Boolean
        Dim v As var = Nothing
        v = New var()
        Dim i As delay_generate_device_pool(Of mock_dev) = Nothing
        assertion.is_true(constructor.resolve(v, i))
        If Not assert_mock_dev_wrapper(i, v) Then
            Return False
        End If

        v = New var("--wrapper=mock_dev_wrapper")
        assertion.is_true(constructor.resolve(v, i))
        Return assert_mock_dev_wrapper(i, v)
    End Function

    Private Shared Function manual_pre_generated_device_pool_case() As Boolean
        Dim v As var = Nothing
        v = New var()
        Dim i As manual_pre_generated_device_pool(Of mock_dev) = Nothing
        assertion.is_true(constructor.resolve(v, i))
        Try
            If Not assert_mock_dev_impl(i, v) Then
                Return False
            End If
        Finally
            If i IsNot Nothing Then
                i.close()
            End If
        End Try

        v = New var("--wrapper=mock_dev_wrapper")
        assertion.is_true(constructor.resolve(v, i))
        Try
            Return assert_mock_dev_wrapper(i, v)
        Finally
            If i IsNot Nothing Then
                i.close()
            End If
        End Try
    End Function

    Private Shared Function one_off_device_pool_case() As Boolean
        Dim v As var = Nothing
        v = New var()
        Dim i As one_off_device_pool(Of mock_dev) = Nothing
        assertion.is_true(constructor.resolve(v, i))
        If Not assert_mock_dev_wrapper(i, v) Then
            Return False
        End If

        v = New var("--wrapper=mock_dev_wrapper")
        assertion.is_true(constructor.resolve(v, i))
        Return assert_mock_dev_wrapper(i, v)
    End Function

    Private Shared Function singleton_device_pool_case() As Boolean
        Dim v As var = Nothing
        v = New var()
        Dim i As singleton_device_pool(Of mock_dev) = Nothing
        assertion.is_true(constructor.resolve(v, i))
        If Not assert_mock_dev_impl(i, v) Then
            Return False
        End If

        v = New var("--wrapper=mock_dev_wrapper")
        assertion.is_true(constructor.resolve(v, i))
        Return assert_mock_dev_wrapper(i, v)
    End Function

    Private Shared Function idevice_pool_case() As Boolean
        For i As Int32 = 0 To 1
            Dim extra As String = Nothing
            If i = 1 Then
                extra = " --wrapper=mock_dev_wrapper"
            End If

            Dim v As var = Nothing
            Dim j As idevice_pool(Of mock_dev) = Nothing
            Using code_block
                v = New var("--pool-type=auto-pre-generated" + extra + " --max-count=10")
                assertion.is_true(constructor.resolve(v, j))
                Dim k As auto_pre_generated_device_pool(Of mock_dev) = Nothing
                If assertion.is_true(cast(j, k)) Then
                    Try
                        If Not assert_mock_dev_wrapper(k, v) Then
                            Return False
                        End If
                    Finally
                        k.close()
                    End Try
                End If
            End Using

            Using code_block
                v = New var("--pool-type=delay-generate" + extra)
                assertion.is_true(constructor.resolve(v, j))
                Dim k As delay_generate_device_pool(Of mock_dev) = Nothing
                If assertion.is_true(cast(j, k)) Then
                    If Not assert_mock_dev_wrapper(k, v) Then
                        Return False
                    End If
                End If
            End Using

            Using code_block
                v = New var("--pool-type=manual-pre-generated" + extra)
                assertion.is_true(constructor.resolve(v, j))
                Dim k As manual_pre_generated_device_pool(Of mock_dev) = Nothing
                Try
                    If assertion.is_true(cast(j, k)) Then
                        If Not assert_mock_dev_wrapper(k, v) Then
                            Return False
                        End If
                    End If
                Finally
                    k.close()
                End Try
            End Using

            Using code_block
                v = New var("--pool-type=one-off" + extra)
                assertion.is_true(constructor.resolve(v, j))
                Dim k As one_off_device_pool(Of mock_dev) = Nothing
                If assertion.is_true(cast(j, k)) Then
                    If Not assert_mock_dev_wrapper(k, v) Then
                        Return False
                    End If
                End If
            End Using

            Using code_block
                v = New var("--pool-type=singleton" + extra)
                assertion.is_true(constructor.resolve(v, j))
                Dim k As singleton_device_pool(Of mock_dev) = Nothing
                If assertion.is_true(cast(j, k)) Then
                    If Not If(i = 0, assert_mock_dev_impl(k, v), assert_mock_dev_wrapper(k, v)) Then
                        Return False
                    End If
                End If
            End Using
        Next

        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return mock_dev_case() AndAlso
               idevice_case() AndAlso
               idevice_creator_case() AndAlso
               idevice_exportor_case() AndAlso
               auto_pre_generated_device_pool_case() AndAlso
               delay_generate_device_pool_case() AndAlso
               manual_pre_generated_device_pool_case() AndAlso
               one_off_device_pool_case() AndAlso
               singleton_device_pool_case() AndAlso
               idevice_pool_case()
    End Function

    Public Overrides Function finish() As Boolean
        assert(constructor.erase(Of mock_dev)())
        assert(constructor.erase(Of idevice_creator(Of mock_dev))())
        assert(wrapper.erase(Of mock_dev)("mock_dev_wrapper"))
        Return MyBase.finish()
    End Function
End Class
