
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Namespace logic
    Public MustInherit Class heap_operator
        Implements instruction_gen

        Private ReadOnly name As String

        Protected Sub New(ByVal name As String)
            assert(Not name.null_or_whitespace())
            Me.name = name
        End Sub

        Protected MustOverride Function process(ByVal ptr As variable, ByVal o As vector(Of String)) As Boolean

        Public Function build(ByVal o As vector(Of String)) As Boolean Implements instruction_gen.build
            assert(Not o Is Nothing)
            Dim ptr As variable = Nothing
            If variable.is_heap_name(name) Then
                errors.not_a_heap_ptr(name)
                Return False
            End If
            If Not variable.of(name, Nothing, ptr) Then
                Return False
            End If
            ' Note, the variable name is not necessary to be a ptr_type.
            Return process(ptr, o)
        End Function
    End Class
End Namespace
