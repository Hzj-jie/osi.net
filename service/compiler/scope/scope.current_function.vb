
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.compiler.logic

Partial Public Class scope(Of T As scope(Of T))
    Public NotInheritable Class current_function_t
        Inherits function_signature(Of builders.parameter)

        Private Sub New(ByVal type_alias As Func(Of String, String),
                        ByVal name As String,
                        ByVal return_type As String,
                        ByVal params As vector(Of builders.parameter))
            MyBase.New(name, assert_which.of(type_alias).is_not_null()(return_type), params)
        End Sub

        Public Shared Function ctor(ByVal type_alias As Func(Of String, String)) _
                                   As Func(Of String, String, vector(Of builders.parameter), current_function_t)
            Return Function(ByVal i As String,
                            ByVal j As String,
                            ByVal k As vector(Of builders.parameter)) As current_function_t
                       Return New current_function_t(type_alias, i, j, k)
                   End Function
        End Function

        Public Function allow_return_value() As Boolean
            Return Not return_type.Equals(compiler.logic.scope.type_t.zero_type)
        End Function
    End Class

    Public Structure current_function_proxy
        Private ReadOnly s As T
        Private ReadOnly ctor As Func(Of String, String, vector(Of builders.parameter), current_function_t)
        Private ReadOnly setter As Action(Of T, current_function_t)
        Private ReadOnly getter As Func(Of T, current_function_t)
        Private ReadOnly is_struct_type As Func(Of T, String, Boolean)

        Public Sub New(ByVal s As T,
                       ByVal ctor As Func(Of String, String, vector(Of builders.parameter), current_function_t),
                       ByVal setter As Action(Of T, current_function_t),
                       ByVal getter As Func(Of T, current_function_t),
                       ByVal is_struct_type As Func(Of T, String, Boolean))
            assert(Not s Is Nothing)
            assert(Not ctor Is Nothing)
            assert(Not setter Is Nothing)
            assert(Not getter Is Nothing)
            assert(Not is_struct_type Is Nothing)
            Me.s = s
            Me.ctor = ctor
            Me.setter = setter
            Me.getter = getter
            Me.is_struct_type = is_struct_type
        End Sub

        Public Sub define(ByVal name As String,
                          ByVal return_type As String,
                          ByVal params As vector(Of builders.parameter))
            assert(getter(s) Is Nothing)
            setter(s, ctor(name, return_type, params))
        End Sub

        Private Function current_function() As current_function_t
            Dim s As T = Me.s
            While getter(s) Is Nothing
                s = s.parent
                assert(Not s Is Nothing)
            End While
            Return getter(s)
        End Function

        Private Function current_function_opt() As [optional](Of current_function_t)
            Dim s As T = Me.s
            While getter(s) Is Nothing
                s = s.parent
                If s Is Nothing Then
                    Return [optional].empty(Of current_function_t)()
                End If
            End While
            Return [optional].of(getter(s))
        End Function

        Public Function allow_return_value() As Boolean
            Return current_function().allow_return_value()
        End Function

        Public Function name() As String
            Return current_function().name
        End Function

        Public Function name_opt() As [optional](Of String)
            Return current_function_opt().map(Function(ByVal x As current_function_t) As String
                                                  assert(Not x Is Nothing)
                                                  Return x.name
                                              End Function)
        End Function

        Public Function return_struct() As Boolean
            Return is_struct_type(s, return_type())
        End Function

        Public Function return_type() As String
            assert(allow_return_value())
            Return current_function().return_type
        End Function

        Public Function signature() As String
            Return current_function_opt().map(Function(ByVal x As current_function_t) As String
                                                  Return x.ToString()
                                              End Function).
                                              or_else("unknown_function")
        End Function
    End Structure
End Class
