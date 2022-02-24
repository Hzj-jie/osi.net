
Imports osi.root.formation
Imports osi.root.connector

Public Class expression_result(Of T)
    Public Shared ReadOnly lex_error_result As expression_result(Of T)
    Public Shared ReadOnly parse_error_result As expression_result(Of T)

    Shared Sub New()
        lex_error_result = New expression_result(Of T)(lex_error:=True)
        parse_error_result = New expression_result(Of T)(parse_error:=True)
    End Sub

    Public Shared Function calculator_error_result(ByVal err As calculator_error) As expression_result(Of T)
        Return New expression_result(Of T)(err:=err)
    End Function

    Public Shared Function expression_result(ByVal r As T,
                                             ByVal o As ioutputter(Of T),
                                             ByVal base As Byte) As expression_result(Of T)
        Return New expression_result(Of T)(r:=r, o:=o, base:=base)
    End Function

    Public ReadOnly r As T
    Public ReadOnly err As calculator_error
    Public ReadOnly lex_error As Boolean
    Public ReadOnly parse_error As Boolean
    Private ReadOnly rs As lazier(Of String)

    Private Sub New(Optional ByVal r As T = Nothing,
                    Optional ByVal err As calculator_error = Nothing,
                    Optional ByVal lex_error As Boolean = False,
                    Optional ByVal parse_error As Boolean = False,
                    Optional ByVal o As ioutputter(Of T) = Nothing,
                    Optional ByVal base As Byte = 0)
        Me.r = r
        Me.err = err
        Me.lex_error = lex_error
        Me.parse_error = parse_error
        Me.rs = New lazier(Of String)(Function() As String
                                          If Not err Is Nothing Then
                                              Return err.str()
                                          ElseIf lex_error Then
                                              Return "expression lexing error"
                                          ElseIf parse_error Then
                                              Return "expression parsing error"
                                          Else
                                              assert(Not o Is Nothing)
                                              Return o.output(r, base)
                                          End If
                                      End Function)
    End Sub

    Public Function has_error() As Boolean
        Return lex_error OrElse
               parse_error OrElse
               err.has_error()
    End Function

    Public Function has_result() As Boolean
        Return Not has_error() AndAlso
               assert(Not r Is Nothing)
    End Function

    Public Function str() As String
        Return +rs
    End Function
End Class
