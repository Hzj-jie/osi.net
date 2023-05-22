
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.formation

Partial Public NotInheritable Class logic
    Partial Public NotInheritable Class builders
        Public Shared ReadOnly debug_dump As Boolean = env_bool(env_keys("compiler", "debug", "dump"))

        ' Stores a type in logic with type(string) and ref(boolean). It's also widely used in bstyle and b2style since
        ' the same & mark is used for reference types.
        Public Class parameter_type
            Private Const type_ref_suffix As Char = character.and_mark
            Public ReadOnly type As String
            Public ReadOnly ref As Boolean

            Public Shared Function is_ref_type(ByVal type As String) As Boolean
                assert(Not type.null_or_whitespace())
                ' TODO: Try to avoid allowing only "&" as parameter type.
                ' assert(Not type.Equals(type_ref_suffix))
                Return type.EndsWith(type_ref_suffix) AndAlso Not type.Equals(type_ref_suffix)
            End Function

            <copy_constructor>
            Protected Sub New(ByVal type As String, ByVal ref As Boolean)
                assert(Not type.null_or_whitespace())
                assert(Not is_ref_type(type))
                Me.type = type
                Me.ref = ref
            End Sub

            Public Shared Function remove_ref(ByVal type As String) As String
                Return New parameter_type(type).type
            End Function

            Public Sub New(ByVal type As String)
                assert(Not type.null_or_whitespace())
                Me.ref = is_ref_type(type)
                If Me.ref Then
                    type = type.Remove(type.Length() - 1)
                Else
                    type = type
                End If
                Me.type = type.Trim()
            End Sub

            Public Function logic_type() As String
                If ref Then
                    Return type + type_ref_suffix
                End If
                Return type
            End Function

            Public Function map_type(ByVal f As Func(Of String, String)) As parameter_type
                assert(Not f Is Nothing)
                Return New parameter_type(f(type), ref)
            End Function

            Public Shared Function from(ByVal ParamArray parameters() As String) As parameter_type()
                assert(Not parameters Is Nothing)
                Return streams.of(parameters).
                               map(Function(ByVal p As String) As parameter_type
                                       assert(Not p Is Nothing)
                                       Return New parameter_type(p)
                                   End Function).
                               to_array()
            End Function

            Public Shared Function from(ByVal parameters As vector(Of String)) As parameter_type()
                assert(Not parameters Is Nothing)
                Return from(+parameters)
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

            <copy_constructor>
            Private Sub New(ByVal type As String, ByVal ref As Boolean, ByVal name As String)
                MyBase.New(type, ref)
                assert(Not name.null_or_whitespace())
                Me.name = name
            End Sub

            Public Shared Function no_ref(ByVal type As String, ByVal name As String) As parameter
                Dim r As New parameter(type, name)
                assert(Not r.ref)
                Return r
            End Function

            Public Shadows Function map_type(ByVal f As Func(Of String, String)) As parameter
                assert(Not f Is Nothing)
                Return New parameter(f(type), ref, name)
            End Function

            Public Function map_name(ByVal f As Func(Of String, String)) As parameter
                assert(Not f Is Nothing)
                Return New parameter(type, ref, f(name))
            End Function

            Public Shared Function to_ref(ByVal p As parameter) As parameter
                assert(Not p Is Nothing)
                assert(Not p.ref)
                Return New parameter(p.type, True, p.name)
            End Function

            Public Overrides Function ToString() As String
                Return name + ": " + MyBase.ToString()
            End Function

            Public Shared Shadows Function from(ByVal parameters() As pair(Of String, String)) As parameter()
                assert(Not parameters Is Nothing)
                Return streams.of(parameters).
                               map(Function(ByVal p As pair(Of String, String)) As parameter
                                       assert(Not p Is Nothing)
                                       Return New parameter(p.second, p.first)
                                   End Function).
                               to_array()
            End Function
        End Class

        Public Structure start_scope_wrapper
            Private ReadOnly o As logic_writer

            Public Sub New(ByVal o As logic_writer)
                assert(Not o Is Nothing)
                Me.o = o
            End Sub

            Public Function [of](ByVal f As Func(Of logic_writer, Boolean)) As Boolean
                Return of_start_scope(f).to(o)
            End Function

            Public Function [of](ByVal f As Func(Of Boolean)) As Boolean
                assert(Not f Is Nothing)
                Return of_start_scope(Function(ByVal x As logic_writer) As Boolean
                                          Return f()
                                      End Function).to(o)
            End Function
        End Structure

        Public Shared Function start_scope(ByVal o As logic_writer) As start_scope_wrapper
            Return New start_scope_wrapper(o)
        End Function

        Private Sub New()
        End Sub
    End Class
End Class
