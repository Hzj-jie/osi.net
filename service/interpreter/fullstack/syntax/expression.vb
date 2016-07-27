
Imports osi.root.connector

Namespace fullstack.syntax
    'an expression should be (...(EXPRESSION_LEFT OPERATOR EXPRESSION_RIGHT)...) or
    '(...(FUNCTION_NAME(EXPRESSION, EXPRESSION, ..., EXPRESSION))...) or
    '(...(VARIABLE_NAME)...) or
    '(...(VARIABLE)...) or
    '(...(- EXPRESSION)...)
    Public Class expression
        Private ReadOnly inner As expression
        Private ReadOnly f As executor.function
        Private ReadOnly parameters() As expression
        Private ReadOnly var As variable

        Public Sub New(ByVal inner As expression)
            assert(Not inner Is Nothing)
            Me.inner = inner
        End Sub

        Public Sub New(ByVal f As executor.function,
                       ByVal parameters() As expression)
            assert(Not f Is Nothing)
            Me.f = f
            Me.parameters = parameters
        End Sub

        Public Sub New(ByVal var As variable)
            assert(Not var Is Nothing)
            Me.var = var
        End Sub
    End Class
End Namespace
