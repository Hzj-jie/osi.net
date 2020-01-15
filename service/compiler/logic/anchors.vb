
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Namespace logic
    Public NotInheritable Class anchors
        Public Shared ReadOnly empty As anchors

        Public NotInheritable Class callee_ref
            Public ReadOnly begin As UInt32
            Public ReadOnly return_type As String
            Public ReadOnly parameters As const_array(Of String)

            Public Sub New(ByVal begin As UInt32, ByVal return_type As String, ByVal parameters() As String)
                assert(Not return_type.null_or_whitespace())
                For i As Int32 = 0 To array_size_i(parameters) - 1
                    assert(Not parameters(i).null_or_whitespace())
                Next
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
                               ByVal parameters() As String) As Boolean
            assert(object_compare(Me, empty) <> 0)
            assert(Not name.null_or_whitespace())
            assert(Not o Is Nothing)
            assert(Not return_type.null_or_whitespace())
            If m.find(name) = m.end() Then
                m.emplace(name, New callee_ref(o.size(), return_type, parameters))
                Return True
            End If
            errors.anchor_redefined(name, Me(name))
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

        Public Function return_type_of(ByVal name As String, ByRef o As String) As Boolean
            Return find(name,
                        Function(ByVal i As callee_ref) As String
                            assert(Not i Is Nothing)
                            Return i.return_type
                        End Function,
                        o)
        End Function

        Public Function parameter_types_of(ByVal name As String, ByRef o As const_array(Of String)) As Boolean
            Return find(name,
                        Function(ByVal i As callee_ref) As const_array(Of String)
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
    End Class
End Namespace
