
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class boolaffinity(Of K)
    Public NotInheritable Class model
        Implements IEquatable(Of model)

        Public Structure affinity
            Implements IEquatable(Of affinity)

            Public ReadOnly true_affinity As Double
            Public ReadOnly false_affinity As Double

            Shared Sub New()
                struct(Of affinity).register()
            End Sub

            Public Sub New(ByVal true_affinity As Double, ByVal false_affinity As Double)
                assert(true_affinity >= 0 AndAlso true_affinity <= 1)
                assert(false_affinity >= 0 AndAlso false_affinity <= 1)
                assert(true_affinity > 0 OrElse false_affinity > 0)
                Me.true_affinity = true_affinity
                Me.false_affinity = false_affinity
            End Sub

            Public Overloads Function Equals(ByVal other As affinity) As Boolean _
                    Implements IEquatable(Of affinity).Equals
                Return true_affinity = other.true_affinity AndAlso false_affinity = other.false_affinity
            End Function

            Public Overrides Function Equals(ByVal obj As Object) As Boolean
                Return Equals(cast(Of affinity)(obj, False))
            End Function
        End Structure

        Private ReadOnly m As unordered_map(Of K, affinity)

        Public Sub New()
            Me.New(New unordered_map(Of K, affinity)())
        End Sub

        Public Sub New(ByVal m As unordered_map(Of K, affinity))
            assert(m IsNot Nothing)
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
            Dim r As unordered_map(Of K, affinity) = Nothing
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

        Public Sub [set](ByVal k As K, ByVal true_affinity As Double, ByVal false_affinity As Double)
            assert(m.find(k) = m.end())
            m(k) = New affinity(true_affinity, false_affinity)
        End Sub

        Public Function has(ByVal k As K) As Boolean
            Return m.find(k) <> m.end()
        End Function

        Default Public ReadOnly Property [get](ByVal a As K) As [optional](Of affinity)
            Get
                If m.find(a) = m.end() Then
                    Return [optional].empty(Of affinity)()
                End If
                Return [optional].of(m(a))
            End Get
        End Property

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
