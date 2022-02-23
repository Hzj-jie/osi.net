
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public Class rlexer
    Public Class reverse_matching_group
        Inherits matching_group_wrapper

        Private ReadOnly consumes As UInt32

        Public Sub New(ByVal g As matching_group)
            MyBase.New(g)
            Dim sg As string_matching_group = Nothing
            If cast(g, sg) AndAlso assert(sg IsNot Nothing) Then
                Me.consumes = sg.max_length
            ElseIf cast(g, [default](Of any_character_matching_group).null) Then
                Me.consumes = uint32_1
            Else
                Me.consumes = uint32_0
            End If
        End Sub

        Public Overrides Function match(ByVal i As String, ByVal pos As UInt32) As vector(Of UInt32)
            Dim r As vector(Of UInt32) = Nothing
            r = MyBase.match(i, pos)
            If Not r.null_or_empty() Then
                Return Nothing
            End If
            If pos + consumes > strlen(i) Then
                Return Nothing
            End If
            r.renew()
            r.emplace_back(pos + consumes)
            Return r
        End Function
    End Class
End Class
