
Public Module _inherit_used_loops
    Public Class expected_max_loops
        'since the perf of these tests are diff in windows 2003 and windows 8, so just use the perf from windows 8
        'which is lower
        Public Class release_build
            Public Const base As Int64 = 11L * inherit_case.size
            Public Const base2 As Int64 = 9 * inherit_case.size
            Public Const impl As Int64 = 11L * inherit_case.size
            Public Const inherit As Int64 = 11L * inherit_case.size
            Public Const inter As Int64 = 11L * inherit_case.size
            Public Const override As Int64 = 8.5 * inherit_case.size
            Public Const [static] As Int64 = 11L * inherit_case.size
        End Class

        Public Class debug_build
            Public Const base As Int64 = 19.8 * inherit_case.size
            Public Const base2 As Int64 = 16.2 * inherit_case.size
            Public Const impl As Int64 = 19.8 * inherit_case.size
            Public Const inherit As Int64 = 19.8 * inherit_case.size
            Public Const inter As Int64 = 19.8 * inherit_case.size
            Public Const override As Int64 = 15.3 * inherit_case.size
            Public Const [static] As Int64 = 19.8 * inherit_case.size
        End Class
    End Class
End Module
