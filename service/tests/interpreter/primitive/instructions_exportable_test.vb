
' This file is generated by commands-parser, with commands.txt file.
' So change commands-parser or commands.txt instead of this file.

Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.interpreter.primitive
Imports osi.service.interpreter.primitive.instructions

Namespace primitive
    Namespace instructions
        Public Class push_exportable_test
            Inherits exportable_test(Of [push])

            Protected Overrides Function create() As [push]
                Return New [push]()
            End Function
        End Class

        Public Class pop_exportable_test
            Inherits exportable_test(Of [pop])

            Protected Overrides Function create() As [pop]
                Return New [pop]()
            End Function
        End Class

        Public Class jump_exportable_test
            Inherits exportable_test(Of [jump])

            Protected Overrides Function create() As [jump]
                Return New [jump]( _
                        data_ref.random())
            End Function
        End Class

        Public Class cpc_exportable_test
            Inherits exportable_test(Of [cpc])

            Protected Overrides Function create() As [cpc]
                Return New [cpc]( _
                        data_ref.random(),
                        data_block.random())
            End Function
        End Class

        Public Class mov_exportable_test
            Inherits exportable_test(Of [mov])

            Protected Overrides Function create() As [mov]
                Return New [mov]( _
                        data_ref.random(),
                        data_ref.random())
            End Function
        End Class

        Public Class cp_exportable_test
            Inherits exportable_test(Of [cp])

            Protected Overrides Function create() As [cp]
                Return New [cp]( _
                        data_ref.random(),
                        data_ref.random())
            End Function
        End Class

        Public Class add_exportable_test
            Inherits exportable_test(Of [add])

            Protected Overrides Function create() As [add]
                Return New [add]( _
                        data_ref.random(),
                        data_ref.random(),
                        data_ref.random())
            End Function
        End Class

        Public Class sub_exportable_test
            Inherits exportable_test(Of [sub])

            Protected Overrides Function create() As [sub]
                Return New [sub]( _
                        data_ref.random(),
                        data_ref.random(),
                        data_ref.random())
            End Function
        End Class

        Public Class mul_exportable_test
            Inherits exportable_test(Of [mul])

            Protected Overrides Function create() As [mul]
                Return New [mul]( _
                        data_ref.random(),
                        data_ref.random(),
                        data_ref.random())
            End Function
        End Class

        Public Class div_exportable_test
            Inherits exportable_test(Of [div])

            Protected Overrides Function create() As [div]
                Return New [div]( _
                        data_ref.random(),
                        data_ref.random(),
                        data_ref.random(),
                        data_ref.random())
            End Function
        End Class

        Public Class ext_exportable_test
            Inherits exportable_test(Of [ext])

            Protected Overrides Function create() As [ext]
                Return New [ext]( _
                        data_ref.random(),
                        data_ref.random(),
                        data_ref.random(),
                        data_ref.random())
            End Function
        End Class

        Public Class pow_exportable_test
            Inherits exportable_test(Of [pow])

            Protected Overrides Function create() As [pow]
                Return New [pow]( _
                        data_ref.random(),
                        data_ref.random(),
                        data_ref.random())
            End Function
        End Class

        Public Class lfs_exportable_test
            Inherits exportable_test(Of [lfs])

            Protected Overrides Function create() As [lfs]
                Return New [lfs]( _
                        data_ref.random(),
                        data_ref.random(),
                        data_ref.random())
            End Function
        End Class

        Public Class rfs_exportable_test
            Inherits exportable_test(Of [rfs])

            Protected Overrides Function create() As [rfs]
                Return New [rfs]( _
                        data_ref.random(),
                        data_ref.random(),
                        data_ref.random())
            End Function
        End Class

        Public Class jumpif_exportable_test
            Inherits exportable_test(Of [jumpif])

            Protected Overrides Function create() As [jumpif]
                Return New [jumpif]( _
                        data_ref.random(),
                        data_ref.random())
            End Function
        End Class

        Public Class cpco_exportable_test
            Inherits exportable_test(Of [cpco])

            Protected Overrides Function create() As [cpco]
                Return New [cpco]( _
                        data_ref.random())
            End Function
        End Class

        Public Class cpdbz_exportable_test
            Inherits exportable_test(Of [cpdbz])

            Protected Overrides Function create() As [cpdbz]
                Return New [cpdbz]( _
                        data_ref.random())
            End Function
        End Class

        Public Class cpin_exportable_test
            Inherits exportable_test(Of [cpin])

            Protected Overrides Function create() As [cpin]
                Return New [cpin]( _
                        data_ref.random())
            End Function
        End Class

        Public Class stop_exportable_test
            Inherits exportable_test(Of [stop])

            Protected Overrides Function create() As [stop]
                Return New [stop]()
            End Function
        End Class

        Public Class equal_exportable_test
            Inherits exportable_test(Of [equal])

            Protected Overrides Function create() As [equal]
                Return New [equal]( _
                        data_ref.random(),
                        data_ref.random(),
                        data_ref.random())
            End Function
        End Class

        Public Class less_exportable_test
            Inherits exportable_test(Of [less])

            Protected Overrides Function create() As [less]
                Return New [less]( _
                        data_ref.random(),
                        data_ref.random(),
                        data_ref.random())
            End Function
        End Class

        Public Class app_exportable_test
            Inherits exportable_test(Of [app])

            Protected Overrides Function create() As [app]
                Return New [app]( _
                        data_ref.random(),
                        data_ref.random())
            End Function
        End Class

        Public Class sapp_exportable_test
            Inherits exportable_test(Of [sapp])

            Protected Overrides Function create() As [sapp]
                Return New [sapp]( _
                        data_ref.random(),
                        data_ref.random())
            End Function
        End Class

        Public Class cut_exportable_test
            Inherits exportable_test(Of [cut])

            Protected Overrides Function create() As [cut]
                Return New [cut]( _
                        data_ref.random(),
                        data_ref.random(),
                        data_ref.random())
            End Function
        End Class

        Public Class cutl_exportable_test
            Inherits exportable_test(Of [cutl])

            Protected Overrides Function create() As [cutl]
                Return New [cutl]( _
                        data_ref.random(),
                        data_ref.random(),
                        data_ref.random(),
                        data_ref.random())
            End Function
        End Class

        Public Class int_exportable_test
            Inherits exportable_test(Of [int])

            Protected Overrides Function create() As [int]
                Return New [int]( _
                        data_ref.random(),
                        data_ref.random(),
                        data_ref.random())
            End Function
        End Class

        Public Class clr_exportable_test
            Inherits exportable_test(Of [clr])

            Protected Overrides Function create() As [clr]
                Return New [clr]( _
                        data_ref.random())
            End Function
        End Class

        Public Class scut_exportable_test
            Inherits exportable_test(Of [scut])

            Protected Overrides Function create() As [scut]
                Return New [scut]( _
                        data_ref.random(),
                        data_ref.random(),
                        data_ref.random())
            End Function
        End Class

        Public Class sizeof_exportable_test
            Inherits exportable_test(Of [sizeof])

            Protected Overrides Function create() As [sizeof]
                Return New [sizeof]( _
                        data_ref.random(),
                        data_ref.random())
            End Function
        End Class

        Public Class empty_exportable_test
            Inherits exportable_test(Of [empty])

            Protected Overrides Function create() As [empty]
                Return New [empty]( _
                        data_ref.random(),
                        data_ref.random())
            End Function
        End Class

        Public Class and_exportable_test
            Inherits exportable_test(Of [and])

            Protected Overrides Function create() As [and]
                Return New [and]( _
                        data_ref.random(),
                        data_ref.random(),
                        data_ref.random())
            End Function
        End Class

        Public Class or_exportable_test
            Inherits exportable_test(Of [or])

            Protected Overrides Function create() As [or]
                Return New [or]( _
                        data_ref.random(),
                        data_ref.random(),
                        data_ref.random())
            End Function
        End Class

        Public Class not_exportable_test
            Inherits exportable_test(Of [not])

            Protected Overrides Function create() As [not]
                Return New [not]( _
                        data_ref.random(),
                        data_ref.random())
            End Function
        End Class

        Public Class stst_exportable_test
            Inherits exportable_test(Of [stst])

            Protected Overrides Function create() As [stst]
                Return New [stst]()
            End Function
        End Class

        Public Class rest_exportable_test
            Inherits exportable_test(Of [rest])

            Protected Overrides Function create() As [rest]
                Return New [rest]()
            End Function
        End Class

        Public Class fadd_exportable_test
            Inherits exportable_test(Of [fadd])

            Protected Overrides Function create() As [fadd]
                Return New [fadd]( _
                        data_ref.random(),
                        data_ref.random(),
                        data_ref.random())
            End Function
        End Class

        Public Class fsub_exportable_test
            Inherits exportable_test(Of [fsub])

            Protected Overrides Function create() As [fsub]
                Return New [fsub]( _
                        data_ref.random(),
                        data_ref.random(),
                        data_ref.random())
            End Function
        End Class

        Public Class fmul_exportable_test
            Inherits exportable_test(Of [fmul])

            Protected Overrides Function create() As [fmul]
                Return New [fmul]( _
                        data_ref.random(),
                        data_ref.random(),
                        data_ref.random())
            End Function
        End Class

        Public Class fdiv_exportable_test
            Inherits exportable_test(Of [fdiv])

            Protected Overrides Function create() As [fdiv]
                Return New [fdiv]( _
                        data_ref.random(),
                        data_ref.random(),
                        data_ref.random())
            End Function
        End Class

        Public Class fext_exportable_test
            Inherits exportable_test(Of [fext])

            Protected Overrides Function create() As [fext]
                Return New [fext]( _
                        data_ref.random(),
                        data_ref.random(),
                        data_ref.random())
            End Function
        End Class

        Public Class fpow_exportable_test
            Inherits exportable_test(Of [fpow])

            Protected Overrides Function create() As [fpow]
                Return New [fpow]( _
                        data_ref.random(),
                        data_ref.random(),
                        data_ref.random())
            End Function
        End Class

        Public Class fequal_exportable_test
            Inherits exportable_test(Of [fequal])

            Protected Overrides Function create() As [fequal]
                Return New [fequal]( _
                        data_ref.random(),
                        data_ref.random(),
                        data_ref.random())
            End Function
        End Class

        Public Class fless_exportable_test
            Inherits exportable_test(Of [fless])

            Protected Overrides Function create() As [fless]
                Return New [fless]( _
                        data_ref.random(),
                        data_ref.random(),
                        data_ref.random())
            End Function
        End Class

    End Namespace
End Namespace
