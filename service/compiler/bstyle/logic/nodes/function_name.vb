

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
        Public Shared Function of_function(ByVal ta As type_alias,
                                           ByVal raw_name As String,
                                           ByVal parameters As vector(Of pair(Of String, String))) As String
            assert(Not ta Is Nothing)
            Return build(raw_name,
                         parameters.map(Function(ByVal i As pair(Of String, String)) As String
                                            assert(Not i Is Nothing)
                                            Return ta(i.second)
                                        End Function))
        End Function

        Public Shared Function of_callee(ByVal ta As type_alias,
                                         ByVal raw_name As String,
                                         ByVal return_type As String,
                                         ByVal parameters As vector(Of pair(Of String, String)),
                                         ByVal paragraph As Func(Of Boolean)) As builders.callee_builder_12
            Return builders.of_callee(ta,
                                      of_function(ta, raw_name, parameters),
                                      return_type,
                                      parameters,
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
                                         ByVal parameters As vector(Of String)) As builders.caller_builder_14
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
