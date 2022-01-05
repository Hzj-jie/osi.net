
Option Explicit On
Option Infer Off
Option Strict On

#Const provide_callee_pos_lookup = False

Imports osi.root.connector
Imports osi.root.formation

Namespace logic
    Public NotInheritable Class anchor
        Public ReadOnly name As String
        Public ReadOnly begin As UInt32
        Public ReadOnly return_type As String
        Public ReadOnly parameters As const_array(Of builders.parameter)

        Public Sub New(ByVal name As String,
                       ByVal begin As UInt32,
                       ByVal return_type As String,
                       ByVal parameters() As builders.parameter)
            assert(Not name.null_or_whitespace())
            assert(Not return_type.null_or_whitespace())
            Me.name = name
            Me.begin = begin
            Me.return_type = return_type
            Me.parameters = const_array.of(parameters)
        End Sub
    End Class

    Public NotInheritable Class anchors
        Public Shared ReadOnly empty As New anchors()

        Private ReadOnly m As New map(Of String, anchor)()
#If provide_callee_pos_lookup Then
        Private ReadOnly p As New map(Of UInt32, anchor)()
#End If

        Public Sub clear()
            m.clear()
#If provide_callee_pos_lookup Then
            p.clear()
#End If
        End Sub

        Public Function define(ByVal name As String,
                               ByVal o As vector(Of String),
                               ByVal return_type As String,
                               ByVal parameters() As builders.parameter) As Boolean
            assert(object_compare(Me, empty) <> 0)
            assert(Not name.null_or_whitespace())
            assert(Not o Is Nothing)
            assert(Not return_type.null_or_whitespace())
            Dim a As New anchor(name, o.size(), return_type, parameters)
#If provide_callee_pos_lookup Then
            If m.find(a.name) = m.end() Then
                assert(m.emplace(a.name, a).second() AndAlso p.emplace(a.begin, a).second())
                Return True
            End If
            errors.anchor_redefined(a.name, a.begin, m(a.name), p(a.begin))
#Else
            If m.emplace(a.name, a).second() Then
                Return True
            End If
            errors.anchor_redefined(a.name, a.begin, m(a.name), Nothing)
#End If
            Return False
        End Function

        Public Function [of](ByVal name As String, ByRef o As anchor) As Boolean
            Return m.find(name, o)
        End Function

#If provide_callee_pos_lookup Then
        Public Function [of](ByVal begin As UInt32, ByRef o As anchor) As Boolean
            Return p.find(begin, o)
        End Function
#End If
    End Class
End Namespace
