
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class nlexer
    Public NotInheritable Class result
        Public ReadOnly start As UInt32
        Public ReadOnly [end] As UInt32
        Public ReadOnly name As String
        Public ReadOnly rule_index As UInt32

        Shared Sub New()
            struct(Of result).register()
        End Sub

        Public Sub New(ByVal start As UInt32,
                       ByVal [end] As UInt32,
                       ByVal name As String,
                       ByVal rule_index As UInt32)
            assert([end] > start)
            assert(Not name.null_or_whitespace())
            Me.start = start
            Me.end = [end]
            Me.name = name
            Me.rule_index = rule_index
        End Sub

        Public Overrides Function ToString() As String
            Dim o As String = Nothing
            assert(struct.to_str(Me, o))
            Return o
        End Function
    End Class

    Public NotInheritable Class str_result
        Public ReadOnly str As String
        Public ReadOnly name As String

        Shared Sub New()
            struct(Of str_result).register()
        End Sub

        Public Sub New(ByVal str As String, ByVal name As String)
            assert(Not str.null_or_empty())
            assert(Not name.null_or_whitespace())
            Me.str = str
            Me.name = name
        End Sub

        Public Overrides Function ToString() As String
            Dim o As String = Nothing
            assert(struct.to_str(Me, o))
            Return o
        End Function

        Public Shared Function [of](ByVal raw As String, ByVal v As vector(Of result)) As vector(Of str_result)
            assert(Not raw.null_or_empty())
            assert(Not v Is Nothing)
            Dim r As vector(Of str_result) = Nothing
            r = New vector(Of str_result)(v.size())
            Dim i As UInt32 = 0
            While i < v.size()
                assert(v(i).end <= strlen(raw))
                assert(v(i).end > v(i).start)
                r.emplace_back(New str_result(strmid(raw, v(i).start, v(i).end - v(i).start), v(i).name))
                i += uint32_1
            End While
            Return r
        End Function

        Public Shared Function map_from_str(ByVal raw As String) As Func(Of vector(Of result), vector(Of str_result))
            Return Function(ByVal i As vector(Of result)) As vector(Of str_result)
                       Return [of](raw, i)
                   End Function
        End Function

        Public Shared Function [of](ByVal raw() As String, ByVal v As vector(Of result)) As vector(Of str_result)
            Return [of](raw.strjoin(character.newline), v)
        End Function

        Public Shared Function map_from_str(ByVal raw() As String) As Func(Of vector(Of result), vector(Of str_result))
            Return Function(ByVal i As vector(Of result)) As vector(Of str_result)
                       Return [of](raw, i)
                   End Function
        End Function
    End Class
End Class
