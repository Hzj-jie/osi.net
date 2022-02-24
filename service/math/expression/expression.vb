
Imports osi.root.connector
Imports osi.root.template
Imports osi.root.utils
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.resource

Partial Public Class expression(Of T)
    Private Const default_base As Byte = 10
    Private ReadOnly l As lp(Of expression(Of T))
    Private ReadOnly c As calculator(Of T)
    Private ReadOnly n As iparser(Of T)
    Private ReadOnly o As ioutputter(Of T)

    Private base As Byte
    Private err As calculator_error

    Public Sub New(ByVal b As ibinary_calculator(Of T),
                   ByVal n As iparser(Of T),
                   ByVal o As ioutputter(Of T))
        Me.New(New calculator(Of T)(b), n, o)
    End Sub

    Public Sub New(ByVal c As calculator(Of T),
                   ByVal n As iparser(Of T),
                   ByVal o As ioutputter(Of T))
        assert(Not c Is Nothing)
        assert(Not n Is Nothing)
        assert(Not o Is Nothing)
        Me.c = c
        Me.n = n
        Me.o = o
        Me.base = default_base
        Me.l = lp(Of expression(Of T)).ctor(expression_syntax.as_lines())
        assert(Not Me.l Is Nothing)
    End Sub

    Public Function execute(ByVal input As String) As expression_result(Of T)
        base = default_base
        err = Nothing
        c.reset()
        Dim r As lp(Of expression(Of T)).result = Nothing
        r = l.execute(input, Me)
        'the calculator error during the parsing stage will stop the following parsing behavior
        'by return false and cause parse error
        If err.has_error() Then
            Return expression_result(Of T).calculator_error_result(err)
        ElseIf r.lex_error Then
            Return expression_result(Of T).lex_error_result
        ElseIf r.parse_error Then
            Return expression_result(Of T).parse_error_result
        Else
            Dim v As T = Nothing
            v = c.execute(err)
            If err.has_error() Then
                Return expression_result(Of T).calculator_error_result(err)
            Else
                Return expression_result(Of T).expression_result(v, o, base)
            End If
        End If
    End Function
End Class
