
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports builders = osi.service.compiler.logic.builders

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class logic_name(Of BUILDER As func_t(Of String, logic_writer, Boolean),
                                              CODE_GENS As func_t(Of code_gens(Of logic_writer)),
                                              T As scope(Of logic_writer, BUILDER, CODE_GENS, T))
        Public Shared Function of_function(Of PT As builders.parameter_type) _
                                          (ByVal raw_name As String,
                                           ByVal ParamArray params() As PT) As String
            assert(Not params Is Nothing)
            Return build(raw_name,
                         streams.of(params).
                                 map(Function(ByVal i As PT) As String
                                         assert(Not i Is Nothing)
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
            If Not scope(Of logic_writer, BUILDER, CODE_GENS, T).current().functions().define(return_type, name) Then
                Return False
            End If
            scope(Of logic_writer, BUILDER, CODE_GENS, T).
                current().
                current_function().
                define(name, return_type, parameters)
            If Not scope(Of logic_writer, BUILDER, CODE_GENS, T).current().variables().define(parameters) Then
                Return False
            End If
            Dim ta As scope(Of logic_writer, BUILDER, CODE_GENS, T).type_alias_proxy =
                    scope(Of logic_writer, BUILDER, CODE_GENS, T).current().type_alias()
            Return builders.of_callee(name,
                                      If(scope(Of logic_writer, BUILDER, CODE_GENS, T).
                                             current().
                                             structs().
                                             types().
                                             defined(return_type),
                                         compiler.logic.scope.type_t.variable_type,
                                         scope(Of logic_writer, BUILDER, CODE_GENS, T).
                                             current().
                                             type_alias()(return_type)),
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
                If Not scope(Of logic_writer, BUILDER, CODE_GENS, T).
                        current().
                        variables().
                        resolve(parameters(i), type) Then
                    Return False
                End If
                types.emplace_back(type)
                i += uint32_1
            End While
            o = build(raw_name, types)
            Return True
        End Function

        Private Shared Function build(ByVal raw_name As String, ByVal types As vector(Of String)) As String
            assert(Not types Is Nothing)
            Dim s As New StringBuilder(raw_name)
            Dim i As UInt32 = 0
            Dim ta As scope(Of logic_writer, BUILDER, CODE_GENS, T).type_alias_proxy =
                scope(Of logic_writer, BUILDER, CODE_GENS, T).current().type_alias()
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
