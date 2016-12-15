
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils

Partial Public Class rlexer
    Public Class string_matching_group
        Implements matching_group

        Public ReadOnly max_length As UInt32
        Private ReadOnly s() As String
        Private ReadOnly l() As UInt32

        Shared Sub New()
            assert(type_info(Of String()).is_cloneable)
        End Sub

        Public Sub New(ByVal ParamArray s() As String)
            assert(Not isemptyarray(s))
            ReDim Me.s(array_size(s) - uint32_1)
            ReDim Me.l(array_size(s) - uint32_1)
            For i As UInt32 = 0 To array_size(s) - uint32_1
                If Not s(i).c_unescape(Me.s(i)) Then
                    Me.s(i) = s(i)
                End If
                Me.l(i) = strlen(Me.s(i))
                If Me.l(i) > Me.max_length Then
                    Me.max_length = Me.l(i)
                End If
            Next
        End Sub

        Public Sub New(ByVal ParamArray c() As Char)
            assert(Not isemptyarray(c))
            ReDim Me.s(array_size(c) - uint32_1)
            ReDim Me.l(array_size(c) - uint32_1)
            For i As UInt32 = 0 To array_size(c) - uint32_1
                Me.s(i) = Convert.ToString(c(i))
                Me.l(i) = uint32_1
            Next
            Me.max_length = uint32_1
        End Sub

        Public Sub New(ByVal s As String)
            Me.New({s})
        End Sub

        Public Sub New(ByVal c As Char)
            Me.New(Convert.ToString(c))
        End Sub

        Public Shared Function create_as_multi_matches(ByVal i As String,
                                                       ByVal start As UInt32,
                                                       ByVal len As UInt32,
                                                       ByRef o As string_matching_group) As Boolean
            Dim v As vector(Of String) = Nothing
            If strsplit(strmid(i, start, len),
                        {Convert.ToString(characters.string_matches_separator)},
                        default_strings,
                        v,
                        False,
                        True) AndAlso
               Not v.null_or_empty() Then
                o = New string_matching_group(+v)
                Return True
            Else
                Return False
            End If
        End Function

        Public Function match(ByVal i As String,
                              ByVal pos As UInt32) As vector(Of UInt32) Implements matching_group.match
            If strlen(i) <= pos Then
                Return Nothing
            Else
                Dim r As vector(Of UInt32) = Nothing
                r = New vector(Of UInt32)()
                For j As Int32 = 0 To array_size(s) - 1
                    If strsame(i, pos, s(j), uint32_0, l(j)) Then
                        r.emplace_back(l(j) + pos)
                    End If
                Next
                Return r
            End If
        End Function

        Public Shared Operator +(ByVal this As string_matching_group) As String()
            Return If(this Is Nothing, Nothing, this.s)
        End Operator
    End Class
End Class
