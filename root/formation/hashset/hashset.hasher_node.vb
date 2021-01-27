
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template

Partial Public NotInheritable Class hashset(Of T, _HASHER As _to_uint32(Of T), _COMPARER As _comparer(Of T))
    Public NotInheritable Class hasher_node
        Inherits hasher_node(Of T, _HASHER, unimplemented_equaler(Of T), _COMPARER)
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub New(ByVal v As T)
            MyBase.New(v)
        End Sub
    End Class
End Class
