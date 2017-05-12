
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation

Partial Public Class shared_component(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)
    Public Class creator
        Private p As PARAMETER_T
        Private component_ref As ref_instance(Of COMPONENT_T)
        Private local_port As PORT_T
        Private remote As const_pair(Of ADDRESS_T, PORT_T)
        Private sender As exclusive_sender
        Private accepter As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)).accepter
        Private dispenser As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T))
        Private data As DATA_T
        Private has_data As Boolean

        Public Shared Function [New]() As creator
            Return New creator()
        End Function

        Public Function with_parameter(ByVal p As PARAMETER_T) As creator
            Me.p = p
            Return Me
        End Function

        Public Function with_component_ref(ByVal component_ref As ref_instance(Of COMPONENT_T)) As creator
            Me.component_ref = component_ref
            Return Me
        End Function

        Public Function with_local_port(ByVal local_port As PORT_T) As creator
            Me.local_port = local_port
            Return Me
        End Function

        Public Function with_remote(ByVal remote As const_pair(Of ADDRESS_T, PORT_T)) As creator
            Me.remote = remote
            Return Me
        End Function

        Public Function with_sender(ByVal sender As exclusive_sender) As creator
            Me.sender = sender
            Return Me
        End Function

        Public Function with_accepter(
                ByVal accepter As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T)).accepter) As creator
            Me.accepter = accepter
            Return Me
        End Function

        Public Function with_dispenser(ByVal dispenser As dispenser(Of DATA_T, const_pair(Of ADDRESS_T, PORT_T))) _
                                      As creator
            Me.dispenser = dispenser
            Return Me
        End Function

        Public Function with_data(ByVal data As DATA_T) As creator
            Me.data = data
            Me.has_data = True
            Return Me
        End Function

        Public Function without_data() As creator
            Me.data = Nothing
            Me.has_data = False
            Return Me
        End Function

        Public Function with_collection(ByVal c As collection) As creator
            If Not c Is Nothing Then
                If Me.component_ref Is Nothing Then
                    If Not c.[New](Me.p, Me.local_port, Me.component_ref) Then
                        raise_error(error_type.warning, "Failed to create component on port(?) ", Me.local_port)
                        Return Me
                    End If
                End If

                If Me.dispenser Is Nothing Then
                    If Not c.[New](Me.p, Me.local_port, Me.component_ref, Me.dispenser) Then
                        raise_error(error_type.warning, "Failed to create dispenser on port ", Me.local_port)
                        Return Me
                    End If
                End If
            End If
            Return Me
        End Function

        Public Function with_functor(Of T As functor)() As creator
            Return with_functor(alloc(Of T)())
        End Function

        Public Function with_functor(ByVal f As functor) As creator
            If Not f Is Nothing Then
                If Me.accepter Is Nothing Then
                    Me.accepter = f.new_accepter(Me.p, Me.remote)
                End If
                If Me.sender Is Nothing Then
                    If Not f.create_sender(Me.p, Me.local_port, Me.component_ref, Me.remote, Me.sender) Then
                        raise_error(error_type.warning,
                                    "Failed to create sender from ",
                                    Me.local_port,
                                    " to ",
                                    Me.remote)
                    End If
                End If
            End If
            Return Me
        End Function

        Public Function valid() As Boolean
            Return Not component_ref Is Nothing AndAlso
                   Not sender Is Nothing AndAlso
                   Not accepter Is Nothing AndAlso
                   Not dispenser Is Nothing
        End Function

        Public Function create() As shared_component(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T)
            Return New shared_component(Of PORT_T, ADDRESS_T, COMPONENT_T, DATA_T, PARAMETER_T) _
                           (valid(), p, component_ref, local_port, remote, sender, accepter, dispenser, data, has_data)
        End Function
    End Class
End Class
