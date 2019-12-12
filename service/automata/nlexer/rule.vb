
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class nlexer
    Public NotInheritable Class rule
        Implements matcher

        Private ReadOnly ms As vector(Of matcher)

        Public Sub New(ByVal ms As vector(Of matcher))
            assert(Not ms Is Nothing)
            assert(Not ms.empty())
            Me.ms = ms
        End Sub

        Public Sub New(ByVal ParamArray gs() As matcher)
            Me.New(vector.of(gs))
        End Sub

        Public Function match(ByVal i As String, ByVal pos As UInt32) As [optional](Of UInt32) Implements matcher.match
            For j As UInt32 = 0 To ms.size() - uint32_1
                Dim r As [optional](Of UInt32) = Nothing
                r = ms(j).match(i, pos)
                assert(Not r Is Nothing)
                If Not r Then
                    Return [optional].of(Of UInt32)()
                End If
                pos = +r
            Next
            Return [optional].of(pos)
        End Function

        ' Process abc[d,e|f]+
        Public Shared Function [of](ByVal s As String, ByRef o As rule) As Boolean
            Dim ms As vector(Of matcher) = Nothing
            ms = New vector(Of matcher)()
            Dim i As UInt32 = 0
            While i < strlen(s)
                Dim j As UInt32 = 0
                j = i
                Dim m As matcher = Nothing
                If Not groups.of(s, i, m) Then
                    Return False
                End If
                assert(Not m Is Nothing)
                assert(i > j)
                ms.emplace_back(m)
            End While
            o = New rule(ms)
            Return True
        End Function
    End Class
End Class
