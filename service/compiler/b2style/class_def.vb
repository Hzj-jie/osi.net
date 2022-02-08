
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.compiler.logic

Partial Public NotInheritable Class b2style
    Public NotInheritable Class class_def
        Private ReadOnly name As name_with_namespace
        ' The type-name pair directly passes to bstyle/struct.
        Private ReadOnly _vars As New vector(Of builders.parameter)()

        Public Sub New(ByVal name As String)
            Me.name = name_with_namespace.of(name)
        End Sub

        Public Function append(ByVal other As class_def) As class_def
            assert(Not other Is Nothing)
            _vars.emplace_back(other._vars)
            Return Me
        End Function

        Public Function vars() As stream(Of builders.parameter)
            Return _vars.stream()
        End Function

        Public Function with_var(ByVal p As builders.parameter) As class_def
            assert(Not p Is Nothing)
            assert(Not p.ref)
            _vars.emplace_back(p)
            Return Me
        End Function

        Public Function check_duplications() As Boolean
            Dim c As unordered_map(Of String, UInt32) =
                _vars.stream().
                      map(Function(ByVal p As builders.parameter) As String
                              assert(Not p Is Nothing)
                              Return p.name
                          End Function).
                      count().
                      filter(Function(ByVal t As tuple(Of String, UInt32)) As Boolean
                                 assert(t.second() <= 2 AndAlso t.second() > 0)
                                 Return t.second() > 1
                             End Function).
                      collect(Of unordered_map(Of String, UInt32))()
            If Not c.empty() Then
                raise_error(error_type.user, "Duplicate variable in ", name, ": ", c)
                Return False
            End If
            Return True
        End Function
    End Class
End Class
