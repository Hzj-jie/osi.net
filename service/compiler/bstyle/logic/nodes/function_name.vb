

Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class function_name
        Public Shared Function of_function(ByVal raw_name As String,
                                           ByVal parameters As vector(Of builders.parameter)) As String
            Return build(raw_name,
                         parameters.map(Function(ByVal i As builders.parameter) As String
                                            assert(Not i Is Nothing)
                                            Return scope.current().type_alias()(i.type)
                                        End Function))
        End Function

        Public Shared Function of_callee(ByVal raw_name As String,
                                         ByVal return_type As String,
                                         ByVal parameters As vector(Of builders.parameter),
                                         ByVal paragraph As Func(Of Boolean)) As builders.callee_builder_14
            Return builders.of_callee(of_function(raw_name, parameters),
                                      scope.current().type_alias()(return_type),
                                      parameters.map(AddressOf scope.current().type_alias().canonical_of),
                                      paragraph)
        End Function

        Public Shared Function of_function_call(ByVal raw_name As String,
                                                ByVal parameters As vector(Of String)) As String
            Return build(raw_name,
                         parameters.map(Function(ByVal i As String) As String
                                            Return macros.type_of(i)
                                        End Function))
        End Function

        Public Shared Function of_caller(ByVal raw_name As String,
                                         ByVal parameters As vector(Of String)) As builders.caller_builder_16
            Return builders.of_caller(of_function_call(raw_name, parameters), parameters)
        End Function

        Private Shared Function build(ByVal raw_name As String, ByVal parameters As vector(Of String)) As String
            assert(Not parameters Is Nothing)
            Dim s As StringBuilder = Nothing
            s = New StringBuilder()
            s.Append(raw_name)
            Dim i As UInt32 = 0
            While i < parameters.size()
                s.Append("&").Append(macros.size_of_type_of(parameters(i)))
                i += uint32_1
            End While
            Return Convert.ToString(s)
        End Function

        Private Sub New()
        End Sub
    End Class
End Class
