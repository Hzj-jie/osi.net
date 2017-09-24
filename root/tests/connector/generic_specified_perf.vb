
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt
Imports osi.root.formation

Public Class generic_specified_perf
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(New generic_perf(1024))
    End Sub

#If RESULT Then
        Const alloc_count As Int64 = CLng(1024) * 1024 * 1024
        Const read_count As Int64 = CLng(1024) * 1024 * 1024 * 1024
        Const write_count As Int64 = CLng(1024) * 1024 * 1024 * 1024

p, 13-12-27 15:25:02, generic_perf test uses 33750467175 processor loops to allocate generic_class(Of Int32) * 1073741824, osi.tests.root.connector.dll.osi.tests.root.connector.generic_specified_perf.report_performance
p, 13-12-27 15:25:14, generic_perf test uses 32802464850 processor loops to allocate generic_struct(Of Int32) * 1073741824, osi.tests.root.connector.dll.osi.tests.root.connector.generic_specified_perf.report_performance
p, 13-12-27 15:25:25, generic_perf test uses 27602975775 processor loops to allocate int_class * 1073741824, osi.tests.root.connector.dll.osi.tests.root.connector.generic_specified_perf.report_performance
p, 13-12-27 15:25:26, generic_perf test uses 2598424200 processor loops to allocate int_struct * 1073741824, osi.tests.root.connector.dll.osi.tests.root.connector.generic_specified_perf.report_performance
p, 13-12-27 15:37:31, generic_perf test uses 1911024809400 processor loops to read value from generic_class(Of Int32) * 1099511627776, osi.tests.root.connector.dll.osi.tests.root.connector.generic_specified_perf.report_performance
p, 13-12-27 15:48:52, generic_perf test uses 1794956580450 processor loops to read value from generic_struct(Of Int32) * 1099511627776, osi.tests.root.connector.dll.osi.tests.root.connector.generic_specified_perf.report_performance
p, 13-12-27 15:59:52, generic_perf test uses 1738760375775 processor loops to read value from generic_static_class(Of Int32) * 1099511627776, osi.tests.root.connector.dll.osi.tests.root.connector.generic_specified_perf.report_performance
p, 13-12-27 16:12:01, generic_perf test uses 1918864973475 processor loops to read value from generic_static_struct(Of Int32) * 1099511627776, osi.tests.root.connector.dll.osi.tests.root.connector.generic_specified_perf.report_performance
p, 13-12-27 16:24:35, generic_perf test uses 1979803830450 processor loops to read value from int_class * 1099511627776, osi.tests.root.connector.dll.osi.tests.root.connector.generic_specified_perf.report_performance
p, 13-12-27 16:36:29, generic_perf test uses 1882964996850 processor loops to read value from int_static_class * 1099511627776, osi.tests.root.connector.dll.osi.tests.root.connector.generic_specified_perf.report_performance
p, 13-12-27 16:48:45, generic_perf test uses 1939287953925 processor loops to read value from int_struct * 1099511627776, osi.tests.root.connector.dll.osi.tests.root.connector.generic_specified_perf.report_performance
p, 13-12-27 17:01:10, generic_perf test uses 1963260001575 processor loops to read value from int_static_struct * 1099511627776, osi.tests.root.connector.dll.osi.tests.root.connector.generic_specified_perf.report_performance
p, 13-12-27 17:12:48, generic_perf test uses 1838733690600 processor loops to write value to generic_class(Of Int32) * 1099511627776, osi.tests.root.connector.dll.osi.tests.root.connector.generic_specified_perf.report_performance
p, 13-12-27 17:25:59, generic_perf test uses 2084979275025 processor loops to write value to generic_struct(Of Int32) * 1099511627776, osi.tests.root.connector.dll.osi.tests.root.connector.generic_specified_perf.report_performance
p, 13-12-27 17:40:34, generic_perf test uses 2306547751575 processor loops to write value to generic_static_class(Of Int32) * 1099511627776, osi.tests.root.connector.dll.osi.tests.root.connector.generic_specified_perf.report_performance
p, 13-12-27 17:54:45, generic_perf test uses 2240694598425 processor loops to write value to generic_static_struct(Of Int32) * 1099511627776, osi.tests.root.connector.dll.osi.tests.root.connector.generic_specified_perf.report_performance
p, 13-12-27 18:08:13, generic_perf test uses 2127929853900 processor loops to write value to int_class * 1099511627776, osi.tests.root.connector.dll.osi.tests.root.connector.generic_specified_perf.report_performance
p, 13-12-27 18:20:47, generic_perf test uses 1987232049225 processor loops to write value to int_static_class * 1099511627776, osi.tests.root.connector.dll.osi.tests.root.connector.generic_specified_perf.report_performance
p, 13-12-27 18:31:15, generic_perf test uses 1657271785950 processor loops to write value to int_struct * 1099511627776, osi.tests.root.connector.dll.osi.tests.root.connector.generic_specified_perf.report_performance
p, 13-12-27 18:41:36, generic_perf test uses 1640228869500 processor loops to write value to int_static_struct * 1099511627776, osi.tests.root.connector.dll.osi.tests.root.connector.generic_specified_perf.report_performance
#End If
End Class
