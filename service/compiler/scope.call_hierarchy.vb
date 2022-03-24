
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public MustInherit Class scope_b(Of CH As {call_hierarchy_t, New}, T As scope_b(Of CH, T))
    Public MustInherit Class call_hierarchy_t
        ' From -> To
        Private ReadOnly m As New unordered_map(Of String, vector(Of String))()
        Private tm As unordered_set(Of String) = Nothing

        ' TODO: Is it possible to use scope.current().current_function().name() directly?
        Protected MustOverride Function current_function_name() As [optional](Of String)

        Public Sub [to](ByVal name As String)
            assert(Not name.null_or_whitespace())
            Dim from As String = current_function_name().or_else("main")
            assert(Not from.null_or_whitespace())
            If Not name.Equals(from) Then
                m(from).emplace_back(name)
            End If
        End Sub

        Private Sub calculate()
            assert(tm Is Nothing)
            tm = New unordered_set(Of String)()
            ' TODO: Implement
        End Sub

        Default Public ReadOnly Property can_reach_root(ByVal f As String) As Boolean
            Get
                assert(Not tm Is Nothing)
                ' TODO: Implement
                Return True
                Return tm.find(f) <> tm.end()
            End Get
        End Property

        Protected NotInheritable Class calculator(Of WRITER)
            Implements statement(Of WRITER)

            Private Shared ReadOnly instance As New calculator(Of WRITER)()

            Public Shared Sub register(ByVal p As statements(Of WRITER))
                assert(Not p Is Nothing)
                p.register(instance)
            End Sub

            Public Sub export(ByVal o As WRITER) Implements statement(Of WRITER).export
                scope(Of T).current().call_hierarchy().calculate()
            End Sub

            Private Sub New()
            End Sub
        End Class

        Public Function filter(ByVal f As String, ByVal o As Func(Of String)) As Func(Of String)
            Return AddressOf New filtered_writer(Me, f, o).str
        End Function

        Private NotInheritable Class filtered_writer
            Private ReadOnly ch As call_hierarchy_t
            Private ReadOnly f As String
            Private ReadOnly o As Func(Of String)

            Public Sub New(ByVal ch As call_hierarchy_t, ByVal f As String, ByVal o As Func(Of String))
                assert(Not ch Is Nothing)
                assert(Not f.null_or_whitespace())
                assert(Not o Is Nothing)
                Me.ch = ch
                Me.f = f
                Me.o = o
            End Sub

            Public Function str() As String
                If (scope_arguments.remove_unused_functions Or True) AndAlso Not ch(f) Then
                    Return ""
                End If
                Return o()
            End Function
        End Class
    End Class
End Class
