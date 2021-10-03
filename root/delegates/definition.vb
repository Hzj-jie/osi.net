
Option Explicit On
Option Infer Off
Option Strict On

Public Module _definition
    Public Delegate Sub void(Of T)(ByRef i As T)
    Public Delegate Sub void(Of T1, T2)(ByRef i1 As T1, ByRef i2 As T2)
    Public Delegate Sub void(Of T1, T2, T3)(ByRef i1 As T1, ByRef i2 As T2, ByRef i3 As T3)
    Public Delegate Sub void(Of T1, T2, T3, T4)(ByRef i1 As T1, ByRef i2 As T2, ByRef i3 As T3, ByRef i4 As T4)
    Public Delegate Sub void(Of T1, T2, T3, T4, T5)(ByRef i1 As T1,
                                                    ByRef i2 As T2,
                                                    ByRef i3 As T3,
                                                    ByRef i4 As T4,
                                                    ByRef i5 As T5)

    Public Delegate Function _do(Of T, RT)(ByRef i As T) As RT
    Public Delegate Function _do(Of T, T2, RT)(ByRef i As T, ByRef i2 As T2) As RT
    Public Delegate Function _do(Of T, T2, T3, RT)(ByRef i As T, ByRef i2 As T2, ByRef i3 As T3) As RT
    Public Delegate Function _do(Of T, T2, T3, T4, RT)(ByRef i As T,
                                                       ByRef i2 As T2,
                                                       ByRef i3 As T3,
                                                       ByRef i4 As T4) As RT
    Public Delegate Function _do(Of T, T2, T3, T4, T5, RT)(ByRef i As T,
                                                           ByRef i2 As T2,
                                                           ByRef i3 As T3,
                                                           ByRef i4 As T4,
                                                           ByRef i5 As T5) As RT

    Public Delegate Function _do_val_ref(Of T, T2, RT)(ByVal i As T, ByRef o As T2) As RT
    Public Delegate Function _do_ref_val(Of T, T2, RT)(ByRef o As T, ByVal i As T2) As RT
    Public Delegate Function _do_val_val_ref(Of T, T2, T3, RT)(ByVal i1 As T, ByVal i2 As T2, ByRef o As T3) As RT
    Public Delegate Function _do_val_ref_val(Of T, T2, T3, RT)(ByVal i1 As T, ByRef i2 As T2, ByVal i3 As T3) As RT
    Public Delegate Function _do_val_ref_ref(Of T, T2, T3, RT)(ByVal i As T, ByRef o1 As T2, ByRef o2 As T3) As RT
    Public Delegate Function _do_ref_val_val(Of T, T2, T3, RT)(ByRef o As T, ByVal i1 As T2, ByVal i2 As T3) As RT
    Public Delegate Function _do_val_val_val_ref(Of T, T2, T3, T4, RT) _
                                                (ByVal i1 As T, ByVal i2 As T2, ByVal i3 As T3, ByRef o As T4) As RT
    Public Delegate Function _do_val_ref_val_ref(Of T, T2, T3, T4, RT) _
                                                (ByVal i1 As T, ByRef i2 As T2, ByVal i3 As T3, ByRef o As T4) As RT
    Public Delegate Function _do_val_val_val_val_ref(Of T, T2, T3, T4, T5, RT) _
                                                    (ByVal i1 As T,
                                                     ByVal i2 As T2,
                                                     ByVal i3 As T3,
                                                     ByVal i4 As T4,
                                                     ByRef o As T5) As RT
    Public Delegate Function _do_val_val_val_val_val_ref(Of T, T2, T3, T4, T5, T6, RT) _
                                                        (ByVal i1 As T,
                                                         ByVal i2 As T2,
                                                         ByVal i3 As T3,
                                                         ByVal i4 As T4,
                                                         ByVal i5 As T5,
                                                         ByRef o As T6) As RT
    Public Delegate Sub void_val_ref_val(Of T1, T2, T3)(ByVal i1 As T1, ByRef i2 As T2, ByVal i3 As T3)

    Public Delegate Function out_bool(Of R)(ByRef o As R) As Boolean
    Public Delegate Function out_bool(Of T, R)(ByVal v As T, ByRef o As R) As Boolean
    Public Delegate Function out_bool(Of T, T2, R)(ByVal v As T, ByVal v2 As T2, ByRef o As R) As Boolean
    Public Delegate Function out_bool(Of T, T2, T3, R)(ByVal v As T,
                                                       ByVal v2 As T2,
                                                       ByVal v3 As T3,
                                                       ByRef o As R) As Boolean
    Public Delegate Function out_bool(Of T, T2, T3, T4, R)(ByVal v As T,
                                                           ByVal v2 As T2,
                                                           ByVal v3 As T3,
                                                           ByVal v4 As T4,
                                                           ByRef o As R) As Boolean

    '.net 3.5 does not have delegates with more than 4 parameters
    Public Delegate Function _func(Of T, T2, T3, T4, T5, RT)(ByVal i As T,
                                                             ByVal j As T2,
                                                             ByVal k As T3,
                                                             ByVal l As T4,
                                                             ByVal m As T5) As RT

    Public Delegate Function _func(Of T, T2, T3, T4, T5, T6, RT)(ByVal i As T,
                                                                 ByVal j As T2,
                                                                 ByVal k As T3,
                                                                 ByVal l As T4,
                                                                 ByVal m As T5,
                                                                 ByVal n As T6) As RT

    Public Delegate Function _func(Of T, T2, T3, T4, T5, T6, T7, RT)(ByVal i As T,
                                                                     ByVal j As T2,
                                                                     ByVal k As T3,
                                                                     ByVal l As T4,
                                                                     ByVal m As T5,
                                                                     ByVal n As T6,
                                                                     ByVal o As T7) As RT

    Public Delegate Function _func(Of T, T2, T3, T4, T5, T6, T7, T8, RT)(ByVal i As T,
                                                                         ByVal j As T2,
                                                                         ByVal k As T3,
                                                                         ByVal l As T4,
                                                                         ByVal m As T5,
                                                                         ByVal n As T6,
                                                                         ByVal o As T7,
                                                                         ByVal p As T8) As RT

    Public Delegate Function _func(Of T, T2, T3, T4, T5, T6, T7, T8, T9, RT)(ByVal i As T,
                                                                             ByVal j As T2,
                                                                             ByVal k As T3,
                                                                             ByVal l As T4,
                                                                             ByVal m As T5,
                                                                             ByVal n As T6,
                                                                             ByVal o As T7,
                                                                             ByVal p As T8,
                                                                             ByVal q As T9) As RT

    Public Delegate Function _func(Of T, T2, T3, T4, T5, T6, T7, T8, T9, T10, RT)(ByVal i As T,
                                                                                  ByVal j As T2,
                                                                                  ByVal k As T3,
                                                                                  ByVal l As T4,
                                                                                  ByVal m As T5,
                                                                                  ByVal n As T6,
                                                                                  ByVal o As T7,
                                                                                  ByVal p As T8,
                                                                                  ByVal q As T9,
                                                                                  ByVal r As T10) As RT
End Module
