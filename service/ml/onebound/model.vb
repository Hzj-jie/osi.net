
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class onebound(Of K)
    Public NotInheritable Class model
        Implements IEquatable(Of model)

        Public NotInheritable Class bind
            Public ReadOnly independence As Double
            Public ReadOnly followers As unordered_map(Of K, Double)

            Shared Sub New()
                struct(Of bind).register()
            End Sub

            ' For struct.byte_serializer only.
            Private Sub New()
            End Sub

            Public Sub New(ByVal independence As Double, ByVal followers As unordered_map(Of K, Double))
                assert(independence >= 0)
                followers.stream().foreach(Sub(ByVal i As first_const_pair(Of K, Double))
                                               assert(i.second >= 0)
                                           End Sub)
                assert(Not followers Is Nothing)
                Me.independence = independence
                Me.followers = followers
            End Sub

            Public Overrides Function ToString() As String
                Return json_serializer.to_str(Me)
            End Function

            Public Function filter(ByVal lower_bound As Double) As bind
                Return New bind(independence,
                                followers.stream().
                                          filter(followers.second_filter(Function(ByVal v As Double) As Boolean
                                                                             Return v >= lower_bound
                                                                         End Function)).
                                          collect(Of unordered_map(Of K, Double))())
            End Function
        End Class

        Private ReadOnly m As unordered_map(Of K, bind)

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

        Public Shared Function load(ByVal i As MemoryStream, ByRef o As model) As Boolean
            Dim r As unordered_map(Of K, bind) = Nothing
            If bytes_serializer.consume_from(i, r) Then
                o = New model(r)
                Return True
            End If
            Return False
        End Function

        Public Shared Function load(ByVal i As MemoryStream) As model
            Dim o As model = Nothing
            assert(load(i, o))
            Return o
        End Function

        Public Shared Function load(ByVal i As String, ByRef o As model) As Boolean
            Using ms As MemoryStream = New MemoryStream()
                If Not ms.read_from_file(i) Then
                    Return False
                End If
                ms.Position() = 0
                Return load(ms, o)
            End Using
        End Function

        Public Shared Function load(ByVal i As String) As model
            Dim o As model = Nothing
            assert(load(i, o))
            Return o
        End Function

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

        Public Overrides Function ToString() As String
            Return json_serializer.to_str(m)
        End Function

        Public Overloads Function ToString(ByVal lower_bound As Double) As String
            Return json_serializer.to_str(m.stream().
                                            map(m.second_mapper(Function(ByVal i As bind) As bind
                                                                    Return i.filter(lower_bound)
                                                                End Function)).
                                            collect(Of unordered_map(Of K, bind))())
        End Function
    End Class
End Class
