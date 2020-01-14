
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Namespace logic
    Public NotInheritable Class anchors
        Public Shared ReadOnly empty As anchors
        Private ReadOnly begins As map(Of String, UInt32)
        Private ReadOnly types As map(Of String, String)

        Shared Sub New()
            empty = New anchors()
        End Sub

        Public Sub New()
            begins = New map(Of String, UInt32)()
            types = New map(Of String, String)()
        End Sub

        Public Sub clear()
            begins.clear()
            types.clear()
        End Sub

        Public Function define(ByVal name As String, ByVal o As vector(Of String), ByVal type As String) As Boolean
            assert(object_compare(Me, empty) <> 0)
            assert(Not name.null_or_whitespace())
            assert(Not o Is Nothing)
            assert(Not type.null_or_whitespace())
            If begins.find(name) = begins.end() Then
                assert(types.find(name) = types.end())
                begins(name) = o.size()
                types(name) = type
                Return True
            End If
            Return False
        End Function

        Public Function retrieve(ByVal name As String, ByRef pos As UInt32) As Boolean
            Return begins.find(name, pos)
        End Function

        Public Function type_of(ByVal name As String, ByRef o As String) As Boolean
            Return types.find(name, o)
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
