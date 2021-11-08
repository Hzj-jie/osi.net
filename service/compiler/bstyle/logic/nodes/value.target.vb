
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class value
        Inherits logic_gen_wrapper
        Implements logic_gen

        Private ReadOnly read_targets As New read_scoped(Of target)()

        Public NotInheritable Class target
            Implements IComparable(Of target)

            Public ReadOnly type As String
            Public ReadOnly names As vector(Of String)

            Public Sub New(ByVal type As String, ByVal names As vector(Of String))
                assert(Not type.null_or_whitespace())
                assert(Not names.null_or_empty())
                type = scope.current().type_alias()(type)
                If Not scope.current().structs().defined(type) Then
                    assert(names.size() = 1)
                End If

                Me.type = type
                Me.names = names
            End Sub

            Public Function CompareTo(ByVal other As target) As Int32 Implements IComparable(Of target).CompareTo
                If other Is Nothing Then
                    Return 1
                End If
                Dim c As Int32 = type.CompareTo(other.type)
                If c <> 0 Then
                    Return c
                End If
                Return names.CompareTo(other.names)
            End Function
        End Class

        Public NotInheritable Class single_data_slot_target
            Public ReadOnly type As String
            Public ReadOnly name As String

            Public Sub New(ByVal type As String, ByVal name As String)
                assert(Not type.null_or_whitespace())
                assert(Not name.null_or_whitespace())
                type = scope.current().type_alias()(type)
                assert(Not scope.current().structs().defined(type))

                Me.type = type
                Me.name = name
            End Sub

            Public Shared Function ignore_type(ByVal i As read_scoped(Of target).ref(Of single_data_slot_target),
                                               ByRef o As String) As Boolean
                assert(Not i Is Nothing)
                Dim t As single_data_slot_target = Nothing
                If Not i.retrieve(t) Then
                    Return False
                End If
                assert(Not t Is Nothing)
                o = t.name
                Return True
            End Function
        End Class

        Public Function read_target_single_data_slot() As read_scoped(Of target).ref(Of single_data_slot_target)
            Return read_targets.pop(Function(ByVal x As target, ByRef o As single_data_slot_target) As Boolean
                                        assert(Not x Is Nothing)
                                        If x.names.size() <> 1 Then
                                            Return False
                                        End If
                                        o = New single_data_slot_target(x.type, x.names(0))
                                        Return True
                                    End Function)
        End Function

        Public Function read_target() As read_scoped(Of target).ref
            Return read_targets.pop()
        End Function

        Private Sub define_single_data_slot_temp_target(ByVal type As String,
                                                        ByVal name As String,
                                                        ByVal o As writer)
            assert(Not o Is Nothing)
            type = scope.current().type_alias()(type)
            assert(Not scope.current().structs().defined(type))
            Dim existing_type As String = Nothing
            If scope.current().variables().try_resolve(name, existing_type) Then
                assert(type.Equals(existing_type))
            Else
                assert(scope.current().variables().define(type, name))
                assert(builders.of_define(name, type).to(o))
            End If
        End Sub

        Public Function with_single_data_slot_temp_target(ByVal type As String,
                                                          ByVal n As typed_node,
                                                          ByVal o As writer) As String
            Dim value_name As String = logic_name.temp_variable(n)
            define_single_data_slot_temp_target(type, value_name, o)
            with_single_data_slot_target(type, value_name)
            Return value_name
        End Function

        Public Function with_temp_target(ByVal type As String,
                                         ByVal n As typed_node,
                                         ByVal o As writer) As vector(Of String)
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Dim params As vector(Of builders.parameter) = Nothing
            assert(scope.current().structs().resolve(type, logic_name.temp_variable(n), params))
            assert(Not params Is Nothing)
            params.stream().foreach(Sub(ByVal p As builders.parameter)
                                        assert(Not p Is Nothing)
                                        define_single_data_slot_temp_target(p.type, p.name, o)
                                    End Sub)
            Return with_target(type, params)
        End Function

        Public Function with_target(ByVal type As String,
                                    ByVal ps As vector(Of builders.parameter)) As vector(Of String)
            assert(Not ps Is Nothing)
            Dim vs As vector(Of String) = ps.stream().
                                             map(Function(ByVal p As builders.parameter) As String
                                                     assert(Not p Is Nothing)
                                                     assert(Not p.name.null_or_whitespace())
                                                     Return p.name
                                                 End Function).
                                             collect(Of vector(Of String))()
            with_target(type, vs)
            Return vs
        End Function

        Public Function with_single_data_slot_target(ByVal type As String, ByVal v As String) As String
            assert(Not v.null_or_whitespace())
            with_target(type, vector.emplace_of(v))
            Return v
        End Function

        Private Sub with_target(ByVal type As String, ByVal vs As vector(Of String))
            read_targets.push(New target(type, vs))
        End Sub
    End Class
End Class
