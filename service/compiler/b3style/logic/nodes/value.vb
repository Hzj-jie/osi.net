
'The following code is generated by /osi/root/codegen/sed/sed.exe
'by replacing
'  Class bstyle
'into
'  Class b3style
'from the input file ..\..\..\bstyle\logic\nodes\value.vb
'Do not edit it manually.

Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata

Partial Public NotInheritable Class b3style
    Private NotInheritable Class value
        Implements code_gen(Of logic_writer)

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(strsame(n.type_name, "value") OrElse
                   (strsame(n.type_name, "ignore-result-function-call") AndAlso
                    strsame(n.child().type_name, "function-call")))
            Return code_gen_of(n.child()).build(o)
        End Function
    End Class
End Class