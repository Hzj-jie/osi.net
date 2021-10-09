
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants

Partial Public Class stream(Of T)
    Public NotInheritable Class collectors
        Public Shared Function to_str(Of ST)(ByVal sep As ST) As Action(Of StringBuilder, T)
            Return Sub(ByVal s As StringBuilder, ByVal v As T)
                       If Not s.empty() Then
                           s.Append(sep)
                       End If
                       s.Append(v)
                   End Sub
        End Function

        Public Shared Function to_str() As Action(Of StringBuilder, T)
            Return to_str(", ")
        End Function

        Public Shared Function count() As Action(Of ref(Of UInt32), T)
            Return Sub(ByVal r As ref(Of UInt32), ByVal v As T)
                       assert(Not r Is Nothing)
                       r.p += uint32_1
                   End Sub
        End Function

        Public Shared Function count_unique() As Action(Of ref(Of UInt32), T)
            Dim s As New unordered_set(Of T)()
            Return Sub(ByVal r As ref(Of UInt32), ByVal v As T)
                       assert(Not r Is Nothing)
                       If s.emplace(v).second() Then
                           r.p += uint32_1
                       End If
                   End Sub
        End Function

        Public Shared Function frequency() As Action(Of unordered_map(Of T, UInt32), T)
            Return Sub(ByVal r As unordered_map(Of T, UInt32), ByVal v As T)
                       assert(Not r Is Nothing)
                       r(v) += uint32_1
                   End Sub
        End Function

        Public Shared Function sorted_frequency() As Action(Of map(Of T, UInt32), T)
            Return Sub(ByVal r As map(Of T, UInt32), ByVal v As T)
                       assert(Not r Is Nothing)
                       r(v) += uint32_1
                   End Sub
        End Function

        Public Shared Function unique() As Action(Of unordered_set(Of T), T)
            Return Sub(ByVal r As unordered_set(Of T), ByVal v As T)
                       assert(Not r Is Nothing)
                       r.emplace(v)
                   End Sub
        End Function

        Public Shared Function sorted_unique() As Action(Of [set](Of T), T)
            Return Sub(ByVal r As [set](Of T), ByVal v As T)
                       assert(Not r Is Nothing)
                       r.emplace(v)
                   End Sub
        End Function

        Public Shared Function with_index() As Action(Of vector(Of tuple(Of UInt32, T)), T)
            Return Sub(ByVal r As vector(Of tuple(Of UInt32, T)), ByVal v As T)
                       assert(Not r Is Nothing)
                       r.emplace_back(tuple.emplace_of(r.size() - uint32_1, v))
                   End Sub
        End Function

        Public Shared Function values(Of V)(ByVal merger As Func(Of V, V, V)) _
                                   As Action(Of unordered_map(Of T, V), first_const_pair(Of T, V))
            Return Sub(ByVal r As unordered_map(Of T, V), ByVal c As first_const_pair(Of T, V))
                       assert(Not r Is Nothing)
                       r.emplace_merge(c, merger)
                   End Sub
        End Function

        Public Shared Function values(Of V)() _
                                   As Action(Of unordered_map(Of T, vector(Of V)), first_const_pair(Of T, V))
            Return Sub(ByVal r As unordered_map(Of T, vector(Of V)), ByVal c As first_const_pair(Of T, V))
                       assert(Not r Is Nothing)
                       assert(Not c Is Nothing)
                       r(c.first).emplace_back(c.second)
                   End Sub
        End Function

        Public Shared Function ordered_values(Of V)(ByVal merger As Func(Of V, V, V)) _
                                   As Action(Of map(Of T, V), first_const_pair(Of T, V))
            Return Sub(ByVal r As map(Of T, V), ByVal c As first_const_pair(Of T, V))
                       assert(Not r Is Nothing)
                       r.emplace_merge(c, merger)
                   End Sub
        End Function

        Public Shared Function ordered_values(Of V)() _
                                   As Action(Of map(Of T, vector(Of V)), first_const_pair(Of T, V))
            Return Sub(ByVal r As map(Of T, vector(Of V)), ByVal c As first_const_pair(Of T, V))
                       assert(Not r Is Nothing)
                       assert(Not c Is Nothing)
                       r(c.first).emplace_back(c.second)
                   End Sub
        End Function

        Public Shared Function samples(ByVal sample_count As UInt32) As Action(Of vector(Of T), T)
            Dim count As UInt32 = sample_count
            Return Sub(ByVal r As vector(Of T), ByVal v As T)
                       assert(Not r Is Nothing)
                       If r.size() < sample_count Then
                           r.emplace_back(v)
                           Return
                       End If

                       count += uint32_1
                       Dim rand As UInt32 = rnd_uint(0, count)
                       If rand >= sample_count Then
                           Return
                       End If

                       r.replace(rnd_uint(0, sample_count), v)
                   End Sub
        End Function

        Private Sub New()
        End Sub
    End Class
End Class
