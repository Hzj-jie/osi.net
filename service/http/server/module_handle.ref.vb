
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports counter = osi.root.utils.counter

Partial Public Class module_handle
    Private NotInheritable Class ref
        Public ReadOnly [module] As [module]
        Public ReadOnly counter_index As Int64

        Public Sub New(ByVal name As String, ByVal m As [module])
            assert(m IsNot Nothing)
            [module] = m
            counter_index = counter.builder.
                                    [New]().
                                    with_name(name).
                                    write_count().
                                    write_rate().
                                    write_last_rate().
                                    register()
        End Sub
    End Class
End Class
