
Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _lcs
    Private Function lcs(Of T)(ByVal this() As T,
                               ByVal that() As T,
                               ByRef start As UInt32,
                               ByRef len As UInt32,
                               ByVal compare As Func(Of T, T, Int32)) As Boolean
        assert(compare IsNot Nothing)
        If isemptyarray(this) OrElse isemptyarray(that) Then
            Return False
        Else
            Dim l(,) As UInt32 = Nothing
            ReDim l(1, array_size(that) - 1)
            Dim cl As Int32 = 0
            len = 0
            For i As UInt32 = 0 To array_size(this) - 1
                cl = (i And uint32_1)
                For j As UInt32 = 0 To array_size(that) - 1
                    If compare(this(i), that(j)) = 0 Then
                        l(cl, j) = 1
                        If i > 0 AndAlso j > 0 Then
                            l(cl, j) += l(1 - cl, j - 1)
                        End If

                        If l(cl, j) > len Then
                            len = l(cl, j)
                            start = i + uint32_1 - len
                        End If
                    Else
                        l(cl, j) = 0
                    End If
                Next
            Next
            Return len > 0
        End If
    End Function

    <Extension()> Public Function lcs(Of T)(ByVal this() As T,
                                            ByVal that() As T,
                                            ByRef start As UInt32,
                                            ByRef len As UInt32) As Boolean
        Return lcs(this, that, start, len, AddressOf compare)
    End Function

    <Extension()> Public Function have_common_subsequence(Of T)(ByVal this() As T,
                                                                ByVal that() As T) As Boolean
        Return lcs(this, that, 0, 0)
    End Function

    <Extension()> Public Function lcs(ByVal this As String,
                                      ByVal that As String,
                                      ByRef start As UInt32,
                                      ByRef len As UInt32) As Boolean
        Return lcs(c_str(this), c_str(that), start, len, Function(a, b) Convert.ToInt32(a) - Convert.ToInt32(b))
    End Function

    <Extension()> Public Function have_common_subsequence(ByVal this As String,
                                                          ByVal that As String) As Boolean
        Return lcs(this, that, 0, 0)
    End Function

    <Extension()> Public Function lcs(ByVal this As String,
                                      ByVal that As String,
                                      ByRef s As String) As Boolean
        Dim start As UInt32 = 0
        Dim len As UInt32 = 0
        If lcs(this, that, start, len) Then
            s = this.strmid(start, len)
            Return True
        Else
            Return False
        End If
    End Function

    <Extension()> Public Function lcs(ByVal this As String, ByVal that As String) As String
        Dim s As String = Nothing
        If lcs(this, that, s) Then
            Return s
        Else
            Return Nothing
        End If
    End Function
End Module
