
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class syntaxer
    Partial Public NotInheritable Class rule
        Public Function export() As exporter
            Return New exporter(Me)
        End Function

        Partial Public Class exporter
            Public ReadOnly syntaxer As syntaxer
            Public ReadOnly ignore_types As unordered_set(Of UInt32)
            Public ReadOnly root_types As vector(Of UInt32)
            Public ReadOnly collection As syntax_collection

            Public Sub New(ByVal i As rule)
                assert(Not i Is Nothing)
                Me.ignore_types = i.ignores
                Me.root_types = i.roots
                Me.collection = i.collection
                Me.syntaxer = New syntaxer(collection, root_types)
            End Sub
        End Class
    End Class
End Class
