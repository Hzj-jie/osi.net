
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class boolaffinity(Of K)
    Public NotInheritable Class trainer
        Private NotInheritable Class trend
            Public pos As UInt32
            Public neg As UInt32
        End Class

        Private ReadOnly m As unordered_map(Of K, trend)

        Public Sub New()
            m = New unordered_map(Of K, trend)()
        End Sub

        Public Function accumulate(ByVal result As Boolean,
                                   ByVal ParamArray v() As const_pair(Of K, Boolean)) As trainer
            Return accumulate(result, vector.of(v))
        End Function

        Public Function accumulate(ByVal result As Boolean,
                                   ByVal v As vector(Of const_pair(Of K, Boolean))) As trainer
            assert(Not v.null_or_empty())
            For i As UInt32 = 0 To v.size() - uint32_1
                If v(i).second = result Then
                    m(v(i).first).pos += uint32_1
                Else
                    m(v(i).first).neg += uint32_1
                End If
            Next
            Return Me
        End Function

        Public Function dump() As model
            Return New model(m.stream().
                               map(m.second_mapper(Function(ByVal t As trend) As model.affinity
                                                   End Function)).
                               collect(Of unordered_map(Of K, model.affinity))())
        End Function
    End Class
End Class
