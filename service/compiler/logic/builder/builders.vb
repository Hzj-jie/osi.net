
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.formation

Namespace logic
    Partial Public NotInheritable Class builders
        Public Shared ReadOnly debug_dump As Boolean = env_bool(env_keys("compiler", "debug", "dump"))

        Public Class parameter_type
            Private Const type_ref_suffix As Char = character.and_mark
            Public ReadOnly type As String
            Public ReadOnly ref As Boolean

            Public Sub New(ByVal type As String)
                assert(Not type.null_or_whitespace())
                ' TODO: Try to avoid allowing only "&" as parameter type.
                ' assert(Not type.Equals(type_ref_suffix))
                Me.ref = type.EndsWith(type_ref_suffix) AndAlso Not type.Equals(type_ref_suffix)
                If Me.ref Then
                    Me.type = type.Remove(type.Length() - 1)
                Else
                    Me.type = type
                End If
            End Sub

            Public Function logic_type() As String
                If ref Then
                    Return type + type_ref_suffix
                End If
                Return type
            End Function

            Protected Shared Function of_ref(ByVal type As String) As parameter_type
                assert(Not type.null_or_whitespace())
                Return New parameter_type(type + type_ref_suffix)
            End Function

            Public Shared Function from_logic_callee_ref_input(ByVal parameters() As String) As parameter_type()
                If parameters Is Nothing Then
                    Return Nothing
                End If
                Return streams.of(parameters).
                               map(Function(ByVal p As String) As parameter_type
                                       assert(Not p Is Nothing)
                                       Return New parameter_type(p)
                                   End Function).
                               to_array()
            End Function

            Public Overrides Function ToString() As String
                Return logic_type()
            End Function
        End Class

        Public NotInheritable Class parameter
            Inherits parameter_type

            Public ReadOnly name As String

            Public Sub New(ByVal type As String, ByVal name As String)
                MyBase.New(type)
                assert(Not name.null_or_whitespace())
                Me.name = name
            End Sub

            Public Shared Shadows Function of_ref(ByVal type As String, ByVal name As String) As parameter
                assert(Not type.null_or_whitespace())
                Return New parameter(parameter_type.of_ref(type).logic_type(), name)
            End Function

            Public Shared Function to_ref(ByVal p As parameter) As parameter
                assert(Not p Is Nothing)
                Return p.to_ref()
            End Function

            Public Function to_ref() As parameter
                Return of_ref(type, name)
            End Function

            Public Overrides Function ToString() As String
                Return name + ": " + MyBase.ToString()
            End Function

            Public Shared Function from_logic_callee_input(ByVal parameters() As pair(Of String, String)) As parameter()
                If parameters Is Nothing Then
                    Return Nothing
                End If
                Return streams.of(parameters).
                               map(Function(ByVal p As pair(Of String, String)) As parameter
                                       assert(Not p Is Nothing)
                                       Return New parameter(p.second, p.first)
                                   End Function).
                               to_array()
            End Function
        End Class

        Public Shared Function of_callee(ByVal name As String,
                                         ByVal type As String,
                                         ByVal parameters As vector(Of parameter),
                                         ByVal paragraph As Func(Of writer, Boolean)) As callee_builder_16
            Return of_callee(name,
                             type,
                             parameters.stream().
                                        map(Function(ByVal i As parameter) As pair(Of String, String)
                                                assert(Not i Is Nothing)
                                                Return pair.emplace_of(i.name, i.logic_type())
                                            End Function).
                                        collect(Of vector(Of pair(Of String, String)))(),
                             paragraph)
        End Function

        Public NotInheritable Class start_scope_wrapper
            Private ReadOnly o As writer

            Public Sub New(ByVal o As writer)
                assert(Not o Is Nothing)
                Me.o = o
            End Sub

            Public Function [of](ByVal f As Func(Of writer, Boolean)) As Boolean
                Return of_start_scope(f).to(o)
            End Function
        End Class

        Public Shared Function start_scope(ByVal o As writer) As start_scope_wrapper
            Return New start_scope_wrapper(o)
        End Function

        Private Sub New()
        End Sub
    End Class
End Namespace
