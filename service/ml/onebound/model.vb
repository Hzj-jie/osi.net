
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class onebound(Of K)
    Public NotInheritable Class model
        Implements IEquatable(Of model)

        Private ReadOnly m As unordered_map(Of K, unordered_map(Of K, Double))

        <copy_constructor>
        Public Sub New(ByVal m As unordered_map(Of K, unordered_map(Of K, Double)))
            assert(Not m Is Nothing)
            Me.m = m
        End Sub

        Public Function dump(ByVal o As MemoryStream) As Boolean
            Return bytes_serializer.append_to(m, o)
        End Function

        Public Function dump(ByVal o As String) As Boolean
            Dim ms As MemoryStream = Nothing
            If Not memory_stream.write_file_wrapped(o, ms) Then
                Return False
            End If
            Using ms
                Return dump(ms)
            End Using
        End Function

        Public Shared Function load(ByVal i As MemoryStream, ByRef o As model) As Boolean
            Dim r As unordered_map(Of K, unordered_map(Of K, Double)) = Nothing
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

        Public Shared Function combo_load(ByRef o As model, ByVal ParamArray files() As String) As Boolean
            If files.isemptyarray() Then
                Return False
            End If

            If Not load(files(0), o) Then
                Return False
            End If

            For i As Int32 = 1 To files.array_size_i() - 1
                Dim m As model = Nothing
                If Not load(files(i), m) Then
                    Return False
                End If
                If Not o.add(m) Then
                    Return False
                End If
            Next
            Return True
        End Function

        Public Shared Function combo_load(ByVal ParamArray files() As String) As model
            Dim o As model = Nothing
            assert(combo_load(o, files))
            Return o
        End Function

        Public Function affinity(ByVal a As K, ByVal b As K) As Double
            If m.find(a) = m.end() OrElse m(a).find(b) = m(a).end() Then
                Return 0
            End If
            Return m(a)(b)
        End Function

        Public Function reverse() As model
            Dim r As New unordered_map(Of K, unordered_map(Of K, Double))()
            flat_map().foreach(Sub(ByVal v As first_const_pair(Of const_pair(Of K, K), Double))
                                   r(v.first.second)(v.first.first) = v.second
                               End Sub)
            Return New model(r)
        End Function

        Public Function multiply(ByVal other As model) As model
            assert(Not other Is Nothing)
            Dim r As New unordered_map(Of K, unordered_map(Of K, Double))()
            flat_map().foreach(Sub(ByVal v As first_const_pair(Of const_pair(Of K, K), Double))
                                   If v.second <= 0 Then
                                       Return
                                   End If
                                   Dim o As Double = other.affinity(v.first.first, v.first.second)
                                   If o <= 0 Then
                                       Return
                                   End If
                                   r(v.first.first)(v.first.second) = o * v.second
                               End Sub)
            Return New model(r)
        End Function

        Public Function add(ByVal other As model) As Boolean
            assert(Not other Is Nothing)
            Try
                other.m.
                      stream().
                      foreach(Sub(ByVal p As first_const_pair(Of K, unordered_map(Of K, Double)))
                                  If Not m.emplace(p).second Then
                                      break_lambda.at_here()
                                  End If
                              End Sub)
                Return True
            Catch ex As break_lambda
                Return False
            End Try
        End Function

        Public Overloads Function Equals(ByVal other As model) As Boolean Implements IEquatable(Of model).Equals
            Dim cmp As Int32 = object_compare(Me, other)
            If cmp <> object_compare_undetermined Then
                Return cmp = 0
            End If
            Return m.Equals(other.m)
        End Function

        Public Overrides Function Equals(ByVal other As Object) As Boolean
            Return Equals(cast(Of model)().from(other, False))
        End Function

        Public Overrides Function ToString() As String
            Return json_serializer.to_str(m)
        End Function

        Public Function filter(ByVal lower_bound As Double) As model
            If lower_bound <= 0 Then
                Return Me
            End If
            Return New model(m.stream().
                               map(m.second_mapper(Function(ByVal i As unordered_map(Of K, Double)) _
                                                           As unordered_map(Of K, Double)
                                                       Return i.stream().
                                                                filter(i.second_filter(
                                                                    Function(ByVal v As Double) As Boolean
                                                                        Return v >= lower_bound
                                                                    End Function)).
                                                                collect_to(Of unordered_map(Of K, Double))()
                                                   End Function)).
                               collect_to(Of unordered_map(Of K, unordered_map(Of K, Double)))())
        End Function

        Public Function flat_map() As stream(Of first_const_pair(Of const_pair(Of K, K), Double))
            Return m.stream().
                     flat_map(m.mapper(Function(ByVal k As K,
                                                ByVal v As unordered_map(Of K, Double)) _
                                               As stream(Of first_const_pair(Of const_pair(Of K, K), Double))
                                           Return v.stream().
                                                    map(v.mapper(Function(ByVal k2 As K,
                                                                          ByVal d As Double) _
                                                                         As first_const_pair(Of const_pair(Of K, K),
                                                                                                Double)
                                                                     Return first_const_pair.emplace_of(
                                                                                const_pair.emplace_of(k, k2), d)
                                                                 End Function))
                                       End Function))
        End Function

        Public Function flat_to_str() As String
            Return json_serializer.to_str(flat_map().collect_to(Of vector(Of const_pair(Of K, K)))())
        End Function

        Public Function map(Of R)(ByVal f As Func(Of K, R)) As onebound(Of R).model
            assert(Not f Is Nothing)
            Return New onebound(Of R).model(m.stream().
                                              map(m.mapper(Function(ByVal k As K,
                                                                    ByVal v As unordered_map(Of K, Double)) _
                                                                   As first_const_pair(Of R,
                                                                                          unordered_map(Of R, Double))
                                                               Return first_const_pair.emplace_of(
                                                                          f(k),
                                                                          v.stream().
                                                                            map(v.first_mapper(f)).
                                                                            collect_to(Of unordered_map(Of R, Double))())
                                                           End Function)).
                                              collect_to(Of unordered_map(Of R, unordered_map(Of R, Double)))())
        End Function

        Public Function map(ByVal f As Func(Of K, K, Double, Double)) As onebound(Of K).model
            assert(Not f Is Nothing)
            Return New onebound(Of K).model(
                       m.stream().
                         map(m.mapper(Function(ByVal k As K,
                                               ByVal v As unordered_map(Of K, Double)) _
                                              As first_const_pair(Of K,
                                                                     unordered_map(Of K, Double))
                                          Return first_const_pair.emplace_of(
                                                     k,
                                                     v.stream().
                                                       map(v.second_mapper(
                                                               Function(ByVal x As K, ByVal y As Double) As Double
                                                                   Return f(k, x, y)
                                                               End Function)).
                                                       collect_to(Of unordered_map(Of K, Double))())
                                      End Function)).
                                      collect_to(Of unordered_map(Of K, unordered_map(Of K, Double)))())
        End Function

        Public Function map_each(Of R)(ByVal f As Func(Of unordered_map(Of K, Double), R)) As unordered_map(Of K, R)
            assert(Not f Is Nothing)
            Return m.stream().
                     map(m.second_mapper(f)).
                     collect_to(Of unordered_map(Of K, R))()
        End Function

        Public Function to_map(Of K2)() As unordered_map(Of K2, Double)
            Return to_map(AddressOf binary_operator(Of K, K, K2).r.add)
        End Function

        Public Function to_map(Of K2)(ByVal f As Func(Of K, K, K2)) As unordered_map(Of K2, Double)
            assert(Not f Is Nothing)
            Return flat_map().map(Function(ByVal v As first_const_pair(Of const_pair(Of K, K), Double)) As first_const_pair(Of K2, Double)
                                      Return first_const_pair.emplace_of(f(v.first.first, v.first.second), v.second)
                                  End Function).
                              collect_to(Of unordered_map(Of K2, Double))()
        End Function

        Public Sub to_console()
            m.stream().
              sort(first_const_pair(Of K, unordered_map(Of K, Double)).first_comparer).
              foreach(m.on_pair(Sub(ByVal key As K, ByVal value As unordered_map(Of K, Double))
                                    value.stream().
                                          sort(Function(ByVal i As first_const_pair(Of K, Double),
                                                        ByVal j As first_const_pair(Of K, Double)) As Int32
                                                   If i.second <> j.second Then
                                                       Return i.second.CompareTo(j.second)
                                                   End If
                                                   Return compare(i.first, j.first)
                                               End Function).
                                          foreach(Sub(ByVal i As first_const_pair(Of K, Double))
                                                      Console.WriteLine(strcat(key, " ", i.first, ", ", i.second))
                                                  End Sub)
                                End Sub))
        End Sub

        Public Sub exponential_distributions_to_consle()
            selector.exponential_distributions(Me).
                     stream().
                     sort(first_const_pair(Of K, Double).second_first_comparer).
                     foreach(Sub(ByVal x As first_const_pair(Of K, Double))
                                 Console.WriteLine(strcat(x.first, ": ", x.second))
                             End Sub)
        End Sub
    End Class
End Class
