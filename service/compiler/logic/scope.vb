
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.constructor
Imports osi.service.interpreter.primitive

Namespace logic
    Partial Public NotInheritable Class scope
        Inherits scope(Of scope)

        Private ReadOnly v As New variable_t()
        Private ReadOnly t As type_t
        Private ReadOnly a As anchor_t
        Private ReadOnly ar As New anchor_ref_t()
        Private ReadOnly f As interrupts

        <inject_constructor>
        Public Sub New(ByVal parent As scope)
            MyBase.New(parent)
        End Sub

        Public Sub New(ByVal functions As interrupts)
            Me.New([default](Of scope).null)
            ' TODO: Types should be scoped.
            Me.t = New type_t()
            Me.a = New anchor_t()
            assert(Not functions Is Nothing)
            Me.f = functions
        End Sub

        ' @VisibleForTesting
        Public Sub New()
            Me.New(interrupts.default)
        End Sub

        Public Function functions() As interrupts
            If is_root() Then
                assert(Not f Is Nothing)
                Return f
            End If
            assert(f Is Nothing)
            Return (+root).functions()
        End Function
    End Class
End Namespace
