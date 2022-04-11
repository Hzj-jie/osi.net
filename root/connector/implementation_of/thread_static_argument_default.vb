
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.delegates

Public NotInheritable Class thread_static_argument_default(Of T, PROTECTOR)
    Private ReadOnly d As T

    Public Sub New(ByVal d As T)
        Me.d = d
    End Sub

    Public Shared Operator Or(ByVal this As argument(Of T),
                              ByVal that As thread_static_argument_default(Of T, PROTECTOR)) As T
        assert(Not that Is Nothing)
        If -this Then
            Return +this
        End If

        Return thread_static_resolver(Of ref(Of T), PROTECTOR).resolve_or_default(ref.of(that.d)).get()
    End Operator

    Public Shared Function scoped_register(ByVal d As T) As IDisposable
        Return thread_static_resolver(Of ref(Of T), PROTECTOR).scoped_register(ref.of(d))
    End Function
End Class

Public NotInheritable Class thread_static_argument_default(Of PROTECTOR)
    Public Shared Function [of](Of T)(ByVal v As T) As thread_static_argument_default(Of T, PROTECTOR)
        Return New thread_static_argument_default(Of T, PROTECTOR)(v)
    End Function

    Private Sub New()
    End Sub
End Class
