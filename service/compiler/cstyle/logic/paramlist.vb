
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata
Imports osi.service.constructor

Public NotInheritable Class paramlist
    Inherits builder_wrapper
    Implements builder

    <inject_constructor>
    Public Sub New(ByVal b As builders, ByVal lp As lang_parser)
        MyBase.New(b, lp)
    End Sub

    Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements builder.build
        assert(Not n Is Nothing)
        assert(Not o Is Nothing)
        Dim i As UInt32 = 0
        While i < n.child_count()
            If Not b.of(n.child(i)).build(o) Then
                o.err("@paramlist child ", i, " - ", n.child(i))
                Return False
            End If
            i += uint32_1
        End While
        Return True
    End Function
End Class
