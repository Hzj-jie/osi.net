
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class onebound
    Partial Public NotInheritable Class typed(Of K)
        Public NotInheritable Class model
            Implements IEquatable(Of model)

            Private ReadOnly m As unordered_map(Of K, unordered_map(Of K, Double))

            Public Sub New()
                m = New unordered_map(Of K, unordered_map(Of K, Double))()
            End Sub

            Public Function dump(ByVal o As MemoryStream) As Boolean
                Return bytes_serializer.append_to(m, o)
            End Function

            Public Function dump(ByVal o As String) As Boolean
                Using ms As MemoryStream = New MemoryStream()
                    Return dump(ms) AndAlso ms.dump_to_file(o)
                End Using
            End Function

            Public Function load(ByVal i As MemoryStream) As Boolean
                Dim r As unordered_map(Of K, unordered_map(Of K, Double)) = Nothing
                Return bytes_serializer.consume_from(i, r) AndAlso
                       unordered_map(Of K, unordered_map(Of K, Double)).swap(r, m)
            End Function

            Public Function load(ByVal i As String) As Boolean
                Using ms As MemoryStream = New MemoryStream()
                    If Not ms.read_from_file(i) Then
                        Return False
                    End If
                    ms.Position() = 0
                    Return load(ms)
                End Using
            End Function

            Public Sub [set](ByVal a As K, ByVal b As K, ByVal v As Double)
                assert(m.find(a) = m.end() OrElse m(a).find(b) = m(a).end())
                assert(v > 0)
                assert(v <= 1)
                m(a)(b) = v
            End Sub

            Public Function possibility(ByVal a As K, ByVal b As K) As Double
                If m.find(a) = m.end() OrElse m(a).find(b) = m(a).end() Then
                    Return 0
                End If
                Return m(a)(b)
            End Function

            Public Overloads Function Equals(ByVal other As model) As Boolean Implements IEquatable(Of model).Equals
                Dim cmp As Int32 = 0
                cmp = object_compare(Me, other)
                If cmp <> object_compare_undetermined Then
                    Return cmp = 0
                End If
                Return m.Equals(other.m)
            End Function

            Public Overrides Function Equals(ByVal other As Object) As Boolean
                Return Equals(cast(Of model)(other, False))
            End Function
        End Class
    End Class
End Class
