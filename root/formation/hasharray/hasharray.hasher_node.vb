
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template

Partial Public Class hasharray(Of T,
                                  _UNIQUE As _boolean,
                                  _HASHER As _to_uint32(Of T),
                                  _EQUALER As _equaler(Of T))
    Public NotInheritable Class hasher_node
        Inherits hasher_node(Of T, _HASHER, _EQUALER, unimplemented_comparer(Of T))
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub New(ByVal v As T)
            MyBase.New(v)
        End Sub
    End Class
End Class
