
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs

Namespace constants
    Public Module _constants
        Public Const pop_timeout_ms As Int64 = 1 * second_milli
        Public ReadOnly pop_interval_ms As Int64 = half_timeslice_length_ms
        Public Const console_keyboard_buff_size As Int32 = 1024 * 1024
    End Module

    Namespace console_keyboard_agent
        Public Module _console_keyboard_agent
            Public Const check_interval_ms As Int64 = 50
        End Module
    End Namespace

    Namespace keyboard
        Public Module _keyboard
            Public Const min_meta As Int32 = min_uint16
            Public Const max_meta As Int32 = max_uint16
            Public Const alt As Int32 = 1001
            Public Const shift As Int32 = 1002
            Public Const ctrl As Int32 = 1003
            Public Const caps_lock As Int32 = 1004
            Public Const num_lock As Int32 = 1005

            Sub New()
                assert(Not enum_def(Of ConsoleKey).has(alt) AndAlso
                       Not enum_def(Of ConsoleKey).has(shift) AndAlso
                       Not enum_def(Of ConsoleKey).has(ctrl) AndAlso
                       Not enum_def(Of ConsoleKey).has(caps_lock) AndAlso
                       Not enum_def(Of ConsoleKey).has(num_lock))
            End Sub
        End Module
    End Namespace
End Namespace