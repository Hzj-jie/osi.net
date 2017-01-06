
Imports osi.root.connector
Imports osi.root.formation

Namespace logic
    Public MustInherit Class compare
        Implements exportable

        Private ReadOnly types As types
        Private ReadOnly left As String
        Private ReadOnly right As String
        Private ReadOnly result As String

        Public Sub New(ByVal types As types, ByVal left As String, ByVal right As String, ByVal result As String)
            assert(Not types Is Nothing)
            assert(Not String.IsNullOrEmpty(left))
            assert(Not String.IsNullOrEmpty(right))
            assert(Not String.IsNullOrEmpty(result))
            Me.types = types
            Me.left = left
            Me.right = right
            Me.result = result
        End Sub

        Public Function export(ByVal scope As scope,
                               ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(Not scope Is Nothing)
            assert(Not o Is Nothing)
            Dim left_ref As String = Nothing
            If Not scope.export(left, left_ref) Then
                errors.variable_undefined(left)
                Return False
            End If

            Dim right_ref As String = Nothing
            If Not scope.export(right, right_ref) Then
                errors.variable_undefined(right)
                Return False
            End If

            Dim result_ref As String = Nothing
            Dim result_type As String = Nothing
            If Not scope.export(result, result_ref) OrElse
               Not assert(scope.type(result, result_type)) Then
                errors.variable_undefined(result)
                Return False
            End If

            If Not types.is_assignable(result_type, sizeof_bool) Then
                errors.unassignable_bool(result, result_type)
                Return False
            End If

            export(left_ref, right_ref, result_ref, o)
            Return True
        End Function

        Protected MustOverride Sub export(ByVal left_ref As String,
                                          ByVal right_ref As String,
                                          ByVal result_ref As String,
                                          ByVal o As vector(Of String))
    End Class
End Namespace
