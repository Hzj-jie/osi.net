
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Namespace logic
    Public NotInheritable Class anchor
        Private ReadOnly anchors As anchors
        Public ReadOnly name As String

        Public Sub New(ByVal anchors As anchors, ByVal name As String)
            assert(Not anchors Is Nothing)
            assert(Not name.null_or_whitespace())
            Me.anchors = anchors
            Me.name = name
        End Sub

        Public Function retrieve(ByRef pos As UInt32) As Boolean
            Return anchors.retrieve(name, pos)
        End Function

        Public Function return_type(ByRef o As String) As Boolean
            Return anchors.return_type(name, o)
        End Function

        Public Function parameters(ByRef o As const_array(Of builders.parameter)) As Boolean
            Return anchors.parameters(name, o)
        End Function

        Public Shared Operator +(ByVal this As anchor) As UInt32
            assert(Not this Is Nothing)
            Return this.anchors(this.name)
        End Operator
    End Class

    Public NotInheritable Class anchors
        Public Shared ReadOnly empty As anchors

        Private NotInheritable Class callee_ref
            Public ReadOnly begin As UInt32
            Public ReadOnly return_type As String
            Public ReadOnly parameters As const_array(Of builders.parameter)

            Public Sub New(ByVal begin As UInt32, ByVal return_type As String, ByVal parameters() As builders.parameter)
                assert(Not return_type.null_or_whitespace())
                Me.begin = begin
                Me.return_type = return_type
                Me.parameters = const_array.of(parameters)
            End Sub
        End Class

        Private ReadOnly m As map(Of String, callee_ref)

        Shared Sub New()
            empty = New anchors()
        End Sub

        Public Sub New()
            m = New map(Of String, callee_ref)()
        End Sub

        Public Sub clear()
            m.clear()
        End Sub

        Public Function define(ByVal name As String,
                               ByVal o As vector(Of String),
                               ByVal return_type As String,
                               ByVal parameters() As builders.parameter) As Boolean
            assert(object_compare(Me, empty) <> 0)
            assert(Not name.null_or_whitespace())
            assert(Not o Is Nothing)
            assert(Not return_type.null_or_whitespace())
            If m.find(name) = m.end() Then
                m.emplace(name, New callee_ref(o.size(), return_type, parameters))
                Return True
            End If
            errors.anchor_redefined(name, o.size(), Me(name))
            Return False
        End Function

        Private Function find(Of T)(ByVal name As String, ByVal f As Func(Of callee_ref, T), ByRef o As T) As Boolean
            assert(Not name.null_or_whitespace())
            assert(Not f Is Nothing)
            Dim it As map(Of String, callee_ref).iterator = Nothing
            it = m.find(name)
            If it = m.end() Then
                Return False
            End If
            o = f((+it).second)
            Return True
        End Function

        Public Function retrieve(ByVal name As String, ByRef pos As UInt32) As Boolean
            Return find(name,
                        Function(ByVal i As callee_ref) As UInt32
                            assert(Not i Is Nothing)
                            Return i.begin
                        End Function,
                        pos)
        End Function

        Public Function return_type(ByVal name As String, ByRef o As String) As Boolean
            Return find(name,
                        Function(ByVal i As callee_ref) As String
                            assert(Not i Is Nothing)
                            Return i.return_type
                        End Function,
                        o)
        End Function

        Public Function parameters(ByVal name As String, ByRef o As const_array(Of builders.parameter)) As Boolean
            Return find(name,
                        Function(ByVal i As callee_ref) As const_array(Of builders.parameter)
                            assert(Not i Is Nothing)
                            Return i.parameters
                        End Function,
                        o)
        End Function

        Default Public ReadOnly Property D(ByVal name As String) As UInt32
            Get
                Dim o As UInt32 = 0
                assert(retrieve(name, o))
                Return o
            End Get
        End Property

        Public Function [of](ByVal name As String) As anchor
            Return New anchor(Me, name)
        End Function
    End Class
End Namespace
