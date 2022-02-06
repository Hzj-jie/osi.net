
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.compiler.logic

Partial Public NotInheritable Class b2style
    Public NotInheritable Class class_def
        Private readonly name As string
        ' The type-name pair directly passes to bstyle/struct.
        Private ReadOnly vars As New vector(Of builders.parameter)()

        Public Sub New(ByVal name As String)
            assert(Not name.null_or_whitespace())
        End Sub

        Public Function append(ByVal other As class_def) As Boolean
            assert(Not other Is Nothing)
            Dim c As unordered_map(Of String, UInt32) =
                vars.stream().
                     concat(other.vars.stream()).
                     map(Function(ByVal p As builders.parameter) As String
                             assert(Not p Is Nothing)
                             Return p.name
                         End Function).
                     count().
                     filter(Function(ByVal t As tuple(Of String, UInt32)) As Boolean
                                assert(t.second()<=2 AndAlso t.second()>0)
                                Return t.second() > 1
                            End Function).
                     collect(Of unordered_map(Of String, UInt32))()
            If Not c.empty() Then
                raise_error(error_type.user, "Duplicate variable in ", name, " and ", other.name, ": ", c)
                Return False
            End If
            vars.emplace_back(other.vars)
            Return True
        End Function

        Public Function with_var(ByVal type As String, ByVal name As String) As class_def
            vars.emplace_back(builders.parameter.no_ref(type, name))
            Return Me
        End Function
    End Class
End Class
