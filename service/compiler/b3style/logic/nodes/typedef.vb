
'The following code is generated by /osi/root/codegen/sed/sed.exe
'by replacing
'  Class bstyle
'into
'  Class b3style
'from the input file ..\..\..\bstyle\logic\nodes\typedef.vb
'Do not edit it manually.

Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata

Partial Public NotInheritable Class b3style
    Private NotInheritable Class typedef
        Implements code_gen(Of logic_writer)

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 3)
            Return scope.current().type_alias().define(code_gen_of(n.child(2)).dump(), code_gen_of(n.child(1)).dump())
        End Function
    End Class
End Class
