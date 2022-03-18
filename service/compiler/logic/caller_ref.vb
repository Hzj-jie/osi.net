
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class logic
    Private NotInheritable Class _caller_ref
        Inherits anchor_caller
        Implements instruction_gen

        Private ReadOnly name As String

        ' @VisibleForTesting
        Public Sub New(ByVal name As String,
                       ByVal result As String,
                       ParamArray ByVal parameters() As String)
            MyBase.New(command.jmpr, result, parameters)
            assert(Not name.null_or_whitespace())
            Me.name = name
        End Sub

        Public Sub New(ByVal name As String,
                       ByVal result As String,
                       ByVal parameters As vector(Of String))
            Me.New(name, result, +parameters)
        End Sub

        Public Sub New(ByVal name As String, ByVal parameters As vector(Of String))
            Me.New(name, Nothing, +parameters)
        End Sub

        Protected Overrides Function retrieve_anchor(ByRef anchor As scope.anchor) As Boolean
            Dim f As Func(Of scope.exported_ref) =
                Function() As scope.exported_ref
                    Dim d As scope.exported_ref = Nothing
                    If Not scope.current().variables().export(name, d) Then
                        Return Nothing
                    End If
                    Return d
                End Function
            If f() Is Nothing Then
                errors.anchor_ref_undefined(name)
                Return False
            End If
            Dim r As scope.anchor_ref = Nothing
            If Not scope.current().anchor_refs().of(name, r) Then
                Return False
            End If
            anchor = r.with_begin(Function() As data_ref
                                      Return f().data_ref
                                  End Function)
            Return True
        End Function
    End Class
End Class
