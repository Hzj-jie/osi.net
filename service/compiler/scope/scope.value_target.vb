
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.service.compiler.logic

Partial Public Class scope(Of T As scope(Of T))
    Public NotInheritable Class value_target_t
        Private ReadOnly values As New read_scoped(Of target)()
        Private ReadOnly value_lists As New read_scoped(Of vector(Of String))()

        Public Sub with_value(ByVal type As String, ByVal vs As vector(Of String))
            values.push(New target(type, vs))
        End Sub

        Public Function value() As read_scoped(Of target).ref
            Return values.pop()
        End Function

        Public Function value(Of T)(ByVal f As _do_val_ref(Of target, T, Boolean)) _
                             As read_scoped(Of target).ref(Of T)
            Return values.pop(f)
        End Function

        Public Sub with_value_list(ByVal v As vector(Of String))
            assert(Not v Is Nothing)
            value_lists.push(v)
        End Sub

        Public Function value_list() As read_scoped(Of vector(Of String)).ref
            Return value_lists.pop()
        End Function

        Public NotInheritable Class target
            Implements IComparable(Of target)

            Public ReadOnly type As String
            Public ReadOnly names As vector(Of String)

            Public Sub New(ByVal type As String, ByVal names As vector(Of String))
                assert(Not type.null_or_whitespace())
                ' Allow empty struct, so the names can be empty.
                assert(Not names Is Nothing)
                type = scope(Of T).current().type_alias()(type)
                If Not scope(Of T).current().structs().types().defined(type) Then
                    assert(names.size() = 1)
                End If

                Me.type = type
                Me.names = names
            End Sub

            Public Function CompareTo(ByVal other As target) As Int32 Implements IComparable(Of target).CompareTo
                Dim c As Int32 = object_compare(Me, other)
                If c <> object_compare_undetermined Then
                    Return c
                End If
                c = type.CompareTo(other.type)
                If c <> 0 Then
                    Return c
                End If
                Return names.CompareTo(other.names)
            End Function
        End Class
    End Class
End Class