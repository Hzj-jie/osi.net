
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt

Public NotInheritable Class big_uint_test
    Inherits chained_case_wrapper

    Public Sub New()
        'the following tests depend on the previous ones
        MyBase.New(New big_uint_predefined_case(),
                   big_uint_add_sub_multiply_case.create_case(),
                   big_uint_shift_case.create_case(),
                   big_uint_left_shift_multiply_case.create_case(),
                   big_uint_right_shift_divide_case.create_case(),
                   big_uint_divide_multiply_case.create_case(),
                   big_uint_power_divide_case.create_case(),
                   big_uint_power_extract_case.create_case(),
                   big_uint_str_case.create_case(),
                   big_uint_bytes_case.create_case(),
                   big_uint_factorial_divide_case.create_case())
    End Sub
End Class

Public NotInheritable Class big_uint_add_sub_multiply_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_uint_add_sub_multiply_case.create_case())
    End Sub
End Class

Public NotInheritable Class big_uint_shift_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_uint_shift_case.create_case())
    End Sub
End Class

Public NotInheritable Class big_uint_left_shift_multiply_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_uint_left_shift_multiply_case.create_case())
    End Sub
End Class

Public NotInheritable Class big_uint_divide_multiply_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_uint_divide_multiply_case.create_case())
    End Sub
End Class

Public NotInheritable Class big_uint_right_shift_divide_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_uint_right_shift_divide_case.create_case())
    End Sub
End Class

Public NotInheritable Class big_uint_power_divide_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_uint_power_divide_case.create_case())
    End Sub
End Class

Public NotInheritable Class big_uint_power_extract_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_uint_power_extract_case.create_case())
    End Sub
End Class

Public NotInheritable Class big_uint_str_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_uint_str_case.create_case())
    End Sub
End Class

Public NotInheritable Class big_uint_bytes_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_uint_bytes_case.create_case())
    End Sub
End Class

Public NotInheritable Class big_uint_add_sub_multiply_specific_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_uint_add_sub_multiply_case.create_case(100))
    End Sub
End Class

Public NotInheritable Class big_uint_shift_specific_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_uint_shift_case.create_case(100))
    End Sub
End Class

Public NotInheritable Class big_uint_left_shift_multiply_specific_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_uint_left_shift_multiply_case.create_case(100))
    End Sub
End Class

Public NotInheritable Class big_uint_divide_multiply_specific_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_uint_divide_multiply_case.create_case(100))
    End Sub
End Class

Public NotInheritable Class big_uint_right_shift_divide_specific_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_uint_right_shift_divide_case.create_case(100))
    End Sub
End Class

Public NotInheritable Class big_uint_power_divide_specific_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_uint_power_divide_case.create_case(100))
    End Sub
End Class

Public NotInheritable Class big_uint_power_extract_specific_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_uint_power_extract_case.create_case(100))
    End Sub
End Class

Public NotInheritable Class big_uint_str_specific_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_uint_str_case.create_case(100))
    End Sub
End Class

Public NotInheritable Class big_uint_bytes_specific_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_uint_bytes_case.create_case(100))
    End Sub
End Class

Public NotInheritable Class big_uint_predefined_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(New big_uint_predefined_case())
    End Sub
End Class

Public NotInheritable Class big_uint_factorial_divide_specific_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_uint_factorial_divide_case.create_case(100))
    End Sub
End Class
