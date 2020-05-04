
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.Text

Public Module _stringbuilder
    <Extension()> Public Sub clear(ByVal this As StringBuilder)
        If Not this Is Nothing Then
            this.Length() = 0
        End If
    End Sub

    <Extension()> Public Sub shrink_to_fit(ByVal this As StringBuilder)
        If Not this Is Nothing Then
            this.Capacity() = this.Length()
        End If
    End Sub

    <Extension()> Public Function empty(ByVal this As StringBuilder) As Boolean
        Return this Is Nothing OrElse this.Length() = 0
    End Function

    <Extension()> Public Function trim_end(ByVal this As StringBuilder,
                                           ByVal trim_it As Func(Of Char, Boolean)) As StringBuilder
        assert(Not this Is Nothing)
        assert(Not trim_it Is Nothing)
        For i As Int32 = this.last_index() To 0 Step -1
            If Not trim_it(this(i)) Then
                this.last_index(i)
                Return this
            End If
        Next
        this.last_index(-1)
        Return this
    End Function

    <Extension()> Public Function trim_end(ByVal this As StringBuilder,
                                           ParamArray ByVal chars() As Char) As StringBuilder
        Return trim_end(this, AddressOf chars.has)
    End Function

    <Extension()> Public Function trim_end(ByVal this As StringBuilder) As StringBuilder
        Return trim_end(this, AddressOf space)
    End Function
End Module
