﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    Partial Public NotInheritable Class scope
        ' Inject any code before the current root-type.
        Public NotInheritable Class root_type_injector_t
            Private i As typed_node_writer

            Public Sub _new(ByVal o As typed_node_writer)
                assert(Not o Is Nothing)
                i = New typed_node_writer()
                o.append(i)
            End Sub

            Public Function current() As typed_node_writer
                assert(Not i Is Nothing)
                Return i
            End Function
        End Class

        Public Function root_type_injector() As root_type_injector_t
            Return from_root(Function(ByVal i As scope) As root_type_injector_t
                                 assert(Not i Is Nothing)
                                 Return i.i
                             End Function)
        End Function
    End Class
End Class