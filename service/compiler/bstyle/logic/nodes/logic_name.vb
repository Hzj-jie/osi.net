
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class logic_name
        Public Shared Function temp_variable(ByVal n As typed_node) As String
            assert(n IsNot Nothing)
            Return strcat("temp_value_@",
                          code_builder.current().nested_build_level(),
                          "@",
                          n.char_start(),
                          "-",
                          n.char_end())
        End Function

        Public Shared Function of_function(Of T As builders.parameter_type) _
                                          (ByVal raw_name As String,
                                           ByVal ParamArray params() As T) As String
            assert(params IsNot Nothing)
            Return build(raw_name,
                         streams.of(params).
                                 map(Function(ByVal i As T) As String
                                         assert(i IsNot Nothing)
                                         Return i.type
                                     End Function).
                                 collect_to(Of vector(Of String))())
        End Function

        Public Shared Function of_callee(ByVal raw_name As String,
                                         ByVal return_type As String,
                                         ByVal parameters As vector(Of builders.parameter),
                                         ByVal paragraph As Func(Of logic_writer, Boolean),
                                         ByVal o As logic_writer) As Boolean
            Dim name As String = of_function(raw_name, +parameters)
            If Not scope.current().functions().define(return_type, name) Then
                Return False
            End If
            scope.current().current_function().define(name, return_type, parameters)
            If Not scope.current().variables().define(parameters) Then
                Return False
            End If
            Dim ta As scope.type_alias_proxy = scope.current().type_alias()
            Return builders.of_callee(name,
                                      If(scope.current().structs().defined(return_type),
                                         compiler.logic.scope.type_t.variable_type,
                                         scope.current().type_alias()(return_type)),
                                      parameters.stream().
                                                 map(AddressOf ta.canonical_of).
                                                 collect_to(Of vector(Of builders.parameter))(),
                                      paragraph).to(o)
        End Function

        Public Shared Function of_function_call(ByVal raw_name As String,
                                                ByVal parameters As vector(Of String),
                                                ByRef o As String) As Boolean
            Dim types As New vector(Of String)()
            Dim i As UInt32 = 0
            While i < parameters.size()
                Dim type As String = Nothing
                If Not scope.current().variables().resolve(parameters(i), type) Then
                    Return False
                End If
                types.emplace_back(type)
                i += uint32_1
            End While
            o = build(raw_name, types)
            Return True
        End Function

        Private Shared Function build(ByVal raw_name As String, ByVal types As vector(Of String)) As String
            assert(types IsNot Nothing)
            Dim s As New StringBuilder(raw_name)
            Dim i As UInt32 = 0
            Dim ta As scope.type_alias_proxy = scope.current().type_alias()
            While i < types.size()
                assert(Not types(i).contains_any(space_chars))
                assert(Not builders.parameter_type.is_ref_type(types(i)))
                s.Append(":").Append(ta(types(i)))
                i += uint32_1
            End While
            Return Convert.ToString(s)
        End Function

        Private Sub New()
        End Sub
    End Class
End Class
