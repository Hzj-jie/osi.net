
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

            Public NotInheritable Class bind
                Public ReadOnly independence As Double
                Public ReadOnly followers As unordered_map(Of K, Double)

                Shared Sub New()
                    struct(Of bind).register()
                End Sub

                Public Sub New()
                    Me.New(0, New unordered_map(Of K, Double)())
                End Sub

                Public Sub New(ByVal independence As Double, ByVal followers As unordered_map(Of K, Double))
                    assert(independence >= 0 AndAlso independence <= 1)
                    assert(Not followers Is Nothing)
                    Me.independence = independence
                    Me.followers = followers
                End Sub

                Public Function replace_by(ByVal independence As Double) As bind
                    Return New bind(independence, followers)
                End Function
            End Class

            Private ReadOnly m As unordered_map(Of K, bind)

            Public Sub New()
                m = New unordered_map(Of K, bind)()
            End Sub

            <copy_constructor>
            Public Sub New(ByVal m As unordered_map(Of K, bind))
                assert(Not m Is Nothing)
                Me.m = m
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
                Dim r As unordered_map(Of K, bind) = Nothing
                Return bytes_serializer.consume_from(i, r) AndAlso unordered_map.swap(r, m)
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

            Public Sub [set](ByVal a As K, ByVal v As Double)
                assert(m.find(a) = m.end() OrElse m(a).independence = 0)
                assert(v > 0 AndAlso v <= 1)
                m(a) = m(a).replace_by(v)
            End Sub

            Public Sub [set](ByVal a As K, ByVal b As K, ByVal v As Double)
                assert(m.find(a) = m.end() OrElse m(a).followers.find(b) = m(a).followers.end())
                assert(v > 0 AndAlso v <= 1)
                m(a).followers(b) = v
            End Sub

            Public Function independence(ByVal a As K) As Double
                If m.find(a) = m.end() Then
                    Return 0
                End If
                Return m(a).independence
            End Function

            Public Function affinity(ByVal a As K, ByVal b As K) As Double
                If m.find(a) = m.end() OrElse m(a).followers.find(b) = m(a).followers.end() Then
                    Return 0
                End If
                Return m(a).followers(b)
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
