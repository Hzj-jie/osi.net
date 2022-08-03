
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public Class scope(Of T As scope(Of T))
    ' Allows b2style.scope.call_hierarchy_t extending the functionality.
    Public Class call_hierarchy_t
        Private Const main_name As String = "main"
        ' From -> To
        Private ReadOnly m As New unordered_map(Of String, vector(Of String))()
        Private tm As unordered_set(Of String) = Nothing
        Private ReadOnly s As scope(Of T)

        Public Sub New(ByVal s As scope(Of T))
            assert(Not s Is Nothing)
            Me.s = s
        End Sub

        Public Sub [to](ByVal name As String)
            assert(tm Is Nothing)
            assert(Not name.null_or_whitespace())
            Dim from As String = s.accessor().current_function_name().or_else(main_name)
            assert(Not from.null_or_whitespace())
            If Not name.Equals(from) Then
                m(from).emplace_back(name)
            End If
        End Sub

        Private Sub calculate()
            assert(tm Is Nothing)
            tm = New unordered_set(Of String)()
            Dim q As New queue(Of String)()
            q.emplace(main_name)
            assert(tm.emplace(main_name).second())
            Dim f As String = Nothing
            While q.pop(f)
                Dim fs As vector(Of String) = Nothing
                If Not m.find(f, fs) Then
                    Continue While
                End If
                assert(Not fs Is Nothing)
                fs.stream().
                   filter(Function(ByVal other As String) As Boolean
                              ' filter requires stateless operations.
                              Return tm.find(other) = tm.end()
                          End Function).
                   foreach(Sub(ByVal other As String)
                               q.emplace(other)
                               assert(tm.emplace(other).second())
                           End Sub)
            End While
        End Sub

        Default Public ReadOnly Property can_reach_root(ByVal f As String) As Boolean
            Get
                assert(Not tm Is Nothing)
                Return tm.find(f) <> tm.end()
            End Get
        End Property

        Public Function filter(ByVal f As String, ByVal o As Func(Of String)) As Func(Of String)
            Return AddressOf New filtered_writer(Me, f, o).str
        End Function

        Public MustInherit Class calculator(Of WRITER, IMPL_T As {calculator(Of WRITER, IMPL_T), New})
            Implements statement(Of WRITER)

            Private Shared ReadOnly instance As New IMPL_T()

            Public Shared Sub register(ByVal p As statements(Of WRITER))
                assert(Not p Is Nothing)
                p.register(instance)
            End Sub

            Protected MustOverride Function current() As call_hierarchy_t

            Public Sub export(ByVal o As WRITER) Implements statement(Of WRITER).export
                current().calculate()
            End Sub
        End Class

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
                If (arguments.remove_unused_functions Or True) AndAlso Not ch(f) Then
                    Return ""
                End If
                Return o()
            End Function
        End Class
    End Class
End Class
