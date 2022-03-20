
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public Class scope(Of T As scope(Of T))
    Public MustInherit Class call_hierarchy
        Private ReadOnly main_name As String
        ' To -> From
        Private ReadOnly m As New unordered_map(Of String, vector(Of String))()
        Private ReadOnly tm As New unordered_map(Of String, Boolean)()

        Protected Sub New(ByVal main_name As String)
            assert(Not main_name.null_or_whitespace())
            Me.main_name = main_name
        End Sub

        Protected Sub New()
            Me.New("main")
        End Sub

        Protected MustOverride Function current_function_name() As [optional](Of String)

        Public Sub [to](ByVal name As String)
            assert(Not name.null_or_whitespace())
            Dim from As String = current_function_name().or_else(main_name)
            assert(Not from.null_or_whitespace())
            If Not name.Equals(from) Then
                m(name).emplace_back(from)
            End If
        End Sub

        Private Function _can_reach_root(ByVal f As String) As Boolean
            Dim q As New queue(Of String)()
            Dim v As New unordered_set(Of String)()
            q.emplace(f)
            assert(v.emplace(f).second())
            While q.pop(f)
                Dim it As unordered_map(Of String, vector(Of String)).iterator = m.find(f)
                If it = m.end() Then
                    Continue While
                End If
                Dim fs As vector(Of String) = (+it).second
                assert(Not fs.null_or_empty())
                For i As UInt32 = 0 To fs.size() - uint32_1
                    Dim r As Boolean = True
                    If fs(i).Equals(main_name) OrElse
                       (tm.find(fs(i), r) AndAlso r) Then
                        assert(tm.emplace(f, True).second())
                        Return True
                    End If
                    If Not r Then
                        Continue For
                    End If
                    q.emplace(fs(i))
                Next
            End While
            Return False
        End Function

        Default Public ReadOnly Property can_reach_root(ByVal f As String) As Boolean
            Get
                assert(Not f.null_or_whitespace())
                If f.Equals(main_name) Then
                    Return True
                End If
                Dim it As unordered_map(Of String, Boolean).iterator = tm.find(f)
                If it <> tm.end() Then
                    Return (+it).second
                End If
                Return _can_reach_root(f)
            End Get
        End Property

        Public Function filter(ByVal f As String, ByVal o As Func(Of String)) As Func(Of String)
            Return AddressOf New filtered_writer(Me, f, o).str
        End Function

        Private NotInheritable Class filtered_writer
            Private ReadOnly ch As call_hierarchy
            Private ReadOnly f As String
            Private ReadOnly o As Func(Of String)

            Public Sub New(ByVal ch As call_hierarchy, ByVal f As String, ByVal o As Func(Of String))
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
