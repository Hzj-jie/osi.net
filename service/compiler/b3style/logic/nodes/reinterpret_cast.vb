
'The following code is generated by /osi/root/codegen/sed/sed.exe
'by replacing
'  Class bstyle
'into
'  Class b3style
'from the input file ..\..\..\bstyle\logic\nodes\reinterpret_cast.vb
'Do not edit it manually.

Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports builders = osi.service.compiler.logic.builders

Partial Public NotInheritable Class b3style
    Private NotInheritable Class reinterpret_cast
        Implements code_gen(Of logic_writer)

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 6)
            Dim name As String = n.child(2).input_without_ignored()
            Dim type As String = scope.normalized_type.logic_type_of(n.child(4).input_without_ignored())
            Return struct.redefine(name, type, o) OrElse
                   (builders.of_redefine(name, type).to(o) AndAlso scope.current().variables().redefine(type, name))
        End Function
    End Class
End Class
