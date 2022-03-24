
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public Class scope_b(Of T As scope_b(Of T))
    Public NotInheritable Class call_hierarchy_t
        Private ReadOnly main_name As String
        Private ReadOnly current_function_name As Func(Of [optional](Of String))
        ' From -> To
        Private ReadOnly m As New unordered_map(Of String, vector(Of String))()
        Private tm As unordered_set(Of String) = Nothing

        Public Sub New(ByVal main_name As String, ByVal current_function_name As Func(Of [optional](Of String)))
            assert(Not main_name.null_or_whitespace())
            assert(Not current_function_name Is Nothing)
            Me.main_name = main_name
            Me.current_function_name = current_function_name
        End Sub

        Public Sub [to](ByVal name As String)
            assert(Not name.null_or_whitespace())
            Dim from As String = current_function_name().or_else(main_name)
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

        Public NotInheritable Class calculator(Of WRITER)
            Implements statement(Of WRITER)

            Public Sub export(ByVal o As WRITER) Implements statement(Of WRITER).export
                scope(Of T).current().call_hierarchy().calculate()
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
