
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with make_tuple.vbp ----------
'so change make_tuple.vbp instead of this file


Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Public NotInheritable Class tuple


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with tuple_make.vbp ----------
'so change tuple_make.vbp instead of this file



    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function [of] _
                              (Of T1, T2, T3) _
                              (ByVal _1 As T1,
                               ByVal _2 As T2,
                               ByVal _3 As T3) As tuple(Of T1, T2, T3)
        Return New tuple _
                        (Of T1, T2, T3) _
                        (copy_no_error(_1),
                         copy_no_error(_2),
                         copy_no_error(_3))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function [of] _
                              (Of T1, T2, T3, T4) _
                              (ByVal _1 As T1,
                               ByVal _2 As T2,
                               ByVal _3 As T3,
                               ByVal _4 As T4) As tuple(Of T1, T2, T3, T4)
        Return New tuple _
                        (Of T1, T2, T3, T4) _
                        (copy_no_error(_1),
                         copy_no_error(_2),
                         copy_no_error(_3),
                         copy_no_error(_4))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function [of] _
                              (Of T1, T2, T3, T4, T5) _
                              (ByVal _1 As T1,
                               ByVal _2 As T2,
                               ByVal _3 As T3,
                               ByVal _4 As T4,
                               ByVal _5 As T5) As tuple(Of T1, T2, T3, T4, T5)
        Return New tuple _
                        (Of T1, T2, T3, T4, T5) _
                        (copy_no_error(_1),
                         copy_no_error(_2),
                         copy_no_error(_3),
                         copy_no_error(_4),
                         copy_no_error(_5))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function [of] _
                              (Of T1, T2, T3, T4, T5, T6) _
                              (ByVal _1 As T1,
                               ByVal _2 As T2,
                               ByVal _3 As T3,
                               ByVal _4 As T4,
                               ByVal _5 As T5,
                               ByVal _6 As T6) As tuple(Of T1, T2, T3, T4, T5, T6)
        Return New tuple _
                        (Of T1, T2, T3, T4, T5, T6) _
                        (copy_no_error(_1),
                         copy_no_error(_2),
                         copy_no_error(_3),
                         copy_no_error(_4),
                         copy_no_error(_5),
                         copy_no_error(_6))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function [of] _
                              (Of T1, T2, T3, T4, T5, T6, T7) _
                              (ByVal _1 As T1,
                               ByVal _2 As T2,
                               ByVal _3 As T3,
                               ByVal _4 As T4,
                               ByVal _5 As T5,
                               ByVal _6 As T6,
                               ByVal _7 As T7) As tuple(Of T1, T2, T3, T4, T5, T6, T7)
        Return New tuple _
                        (Of T1, T2, T3, T4, T5, T6, T7) _
                        (copy_no_error(_1),
                         copy_no_error(_2),
                         copy_no_error(_3),
                         copy_no_error(_4),
                         copy_no_error(_5),
                         copy_no_error(_6),
                         copy_no_error(_7))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function [of] _
                              (Of T1, T2, T3, T4, T5, T6, T7, T8) _
                              (ByVal _1 As T1,
                               ByVal _2 As T2,
                               ByVal _3 As T3,
                               ByVal _4 As T4,
                               ByVal _5 As T5,
                               ByVal _6 As T6,
                               ByVal _7 As T7,
                               ByVal _8 As T8) As tuple(Of T1, T2, T3, T4, T5, T6, T7, T8)
        Return New tuple _
                        (Of T1, T2, T3, T4, T5, T6, T7, T8) _
                        (copy_no_error(_1),
                         copy_no_error(_2),
                         copy_no_error(_3),
                         copy_no_error(_4),
                         copy_no_error(_5),
                         copy_no_error(_6),
                         copy_no_error(_7),
                         copy_no_error(_8))
    End Function

'finish tuple_make.vbp --------


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with tuple_make.vbp ----------
'so change tuple_make.vbp instead of this file



    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function [emplace_of] _
                              (Of T1, T2, T3) _
                              (ByVal _1 As T1,
                               ByVal _2 As T2,
                               ByVal _3 As T3) As tuple(Of T1, T2, T3)
        Return New tuple _
                        (Of T1, T2, T3) _
                        ((_1),
                         (_2),
                         (_3))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function [emplace_of] _
                              (Of T1, T2, T3, T4) _
                              (ByVal _1 As T1,
                               ByVal _2 As T2,
                               ByVal _3 As T3,
                               ByVal _4 As T4) As tuple(Of T1, T2, T3, T4)
        Return New tuple _
                        (Of T1, T2, T3, T4) _
                        ((_1),
                         (_2),
                         (_3),
                         (_4))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function [emplace_of] _
                              (Of T1, T2, T3, T4, T5) _
                              (ByVal _1 As T1,
                               ByVal _2 As T2,
                               ByVal _3 As T3,
                               ByVal _4 As T4,
                               ByVal _5 As T5) As tuple(Of T1, T2, T3, T4, T5)
        Return New tuple _
                        (Of T1, T2, T3, T4, T5) _
                        ((_1),
                         (_2),
                         (_3),
                         (_4),
                         (_5))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function [emplace_of] _
                              (Of T1, T2, T3, T4, T5, T6) _
                              (ByVal _1 As T1,
                               ByVal _2 As T2,
                               ByVal _3 As T3,
                               ByVal _4 As T4,
                               ByVal _5 As T5,
                               ByVal _6 As T6) As tuple(Of T1, T2, T3, T4, T5, T6)
        Return New tuple _
                        (Of T1, T2, T3, T4, T5, T6) _
                        ((_1),
                         (_2),
                         (_3),
                         (_4),
                         (_5),
                         (_6))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function [emplace_of] _
                              (Of T1, T2, T3, T4, T5, T6, T7) _
                              (ByVal _1 As T1,
                               ByVal _2 As T2,
                               ByVal _3 As T3,
                               ByVal _4 As T4,
                               ByVal _5 As T5,
                               ByVal _6 As T6,
                               ByVal _7 As T7) As tuple(Of T1, T2, T3, T4, T5, T6, T7)
        Return New tuple _
                        (Of T1, T2, T3, T4, T5, T6, T7) _
                        ((_1),
                         (_2),
                         (_3),
                         (_4),
                         (_5),
                         (_6),
                         (_7))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function [emplace_of] _
                              (Of T1, T2, T3, T4, T5, T6, T7, T8) _
                              (ByVal _1 As T1,
                               ByVal _2 As T2,
                               ByVal _3 As T3,
                               ByVal _4 As T4,
                               ByVal _5 As T5,
                               ByVal _6 As T6,
                               ByVal _7 As T7,
                               ByVal _8 As T8) As tuple(Of T1, T2, T3, T4, T5, T6, T7, T8)
        Return New tuple _
                        (Of T1, T2, T3, T4, T5, T6, T7, T8) _
                        ((_1),
                         (_2),
                         (_3),
                         (_4),
                         (_5),
                         (_6),
                         (_7),
                         (_8))
    End Function

'finish tuple_make.vbp --------

    Private Sub New()
    End Sub

End Class
'finish make_tuple.vbp --------
