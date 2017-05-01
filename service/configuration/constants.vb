
Imports osi.root.constants

Namespace constants
    Namespace multi_filter
        Friend Module _multi_filter
            Public ReadOnly separators() As Char = {character.comma, character.sheffer}
        End Module
    End Namespace

    Namespace compare_filter
        Friend Module _compare_filter
            Public Const larger As Char = character.right_angle_bracket
            Public Const less As Char = character.left_angle_bracket
            Public Const larger_or_equal As String = character.right_angle_bracket + character.equal_sign
            Public Const less_or_equal As String = character.left_angle_bracket + character.equal_sign
            Public Const equal As Char = character.equal_sign
        End Module
    End Namespace

    Namespace interval_filter
        Friend Module _interval_filter
            Public ReadOnly separators() As Char = {character.minus_sign, character.comma}
            Public Const include_min As Char = character.left_mid_bracket
            Public Const include_max As Char = character.right_mid_bracket
            Public Const exclude_min As Char = character.left_bracket
            Public Const exclude_max As Char = character.right_bracket
        End Module
    End Namespace

    Namespace static_filter
        Friend Module _static_filter
            Public Const version As String = "fv"
            Public Const short_version As String = "v"
            Public Const machine_name As String = "m"
            Public Const computer_name As String = "c"
            Public Const domain_name As String = "d"
            Public Const user_name As String = "u"
            Public Const working_directory As String = "wd"
            Public Const application_directory As String = "ad"
            Public Const service_name As String = "s"
            Public Const build As String = "b"
            Public Const running_mode As String = "r"

            Public Const os_full_name As String = "on"
            Public Const os_version As String = "ov"
            Public Const os_platform As String = "op"

            Public Const available_physical_memory As String = "apm"
            Public Const available_virtual_memory As String = "avm"
            Public Const total_physical_memory As String = "tpm"
            Public Const total_virtual_memory As String = "tvm"
            Public Const processor_count As String = "pc"
        End Module
    End Namespace

    Namespace filter_selector
        Friend Module _filter_selector
            Public Const [string] As String = "string"
            Public Const string_pattern As String = "str-pat"
            Public Const string_serial As String = "str-ser"
            Public Const string_patterns As String = "str-pats"
            Public Const string_compare As String = "str-com"
            Public Const string_interval As String = "str-int"
            Public Const string_pattern_cache As String = "str-pat-cache"
            Public Const string_patterns_cache As String = "str-pats-cache"
            Public Const string_prefix As String = "str-pre"
            Public Const string_prefixes As String = "str-pres"

            Public Const string_case_sensitive As String = "str-case"
            Public Const string_pattern_case_sensitive As String = "str-pat-case"
            Public Const string_serial_case_sensitive As String = "str-ser-case"
            Public Const string_patterns_case_sensitive As String = "str-pats-case"
            Public Const string_compare_case_sensitive As String = "str-com-case"
            Public Const string_interval_case_sensitive As String = "str-int-case"
            Public Const string_pattern_case_sensitive_cache As String = "str-pat-case-cache"
            Public Const string_patterns_case_sensitive_cache As String = "str-pats-case-cache"
            Public Const string_prefix_case_sensitive As String = "str-pre-case"
            Public Const string_prefixes_case_sensitive As String = "str-pres-case"

            Public Const int As String = "int"
            Public Const int_interval As String = "int-int"
            Public Const int_serial As String = "int-ser"
            Public Const int_compare As String = "int-com"

            Public Const [double] As String = "dbl"
            Public Const double_interval As String = "dbl-int"
            Public Const double_serial As String = "dbl-ser"
            Public Const double_compare As String = "dbl-com"
        End Module
    End Namespace
End Namespace
