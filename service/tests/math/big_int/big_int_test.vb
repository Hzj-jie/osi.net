
Imports osi.root.utt

Public Class big_int_test
    Inherits chained_case_wrapper

    Public Sub New()
        'the following tests depend on the previous ones
        MyBase.New(big_int_add_sub_multiply_case.create_case(),
                   big_int_shift_case.create_case(),
                   big_int_left_shift_multiply_case.create_case(),
                   big_int_right_shift_divide_case.create_case(),
                   big_int_divide_multiply_case.create_case(),
                   big_int_power_divide_case.create_case(),
                   big_int_power_extract_case.create_case(),
                   big_int_str_case.create_case(),
                   big_int_bytes_case.create_case())
    End Sub
End Class

Public Class big_int_add_sub_multiply_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_int_add_sub_multiply_case.create_case())
    End Sub
End Class

Public Class big_int_shift_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_int_shift_case.create_case())
    End Sub
End Class

Public Class big_int_left_shift_multiply_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_int_left_shift_multiply_case.create_case())
    End Sub
End Class

Public Class big_int_right_shift_divide_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_int_right_shift_divide_case.create_case())
    End Sub
End Class

Public Class big_int_divide_multiply_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_int_divide_multiply_case.create_case())
    End Sub
End Class

Public Class big_int_power_divide_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_int_power_divide_case.create_case())
    End Sub
End Class

Public Class big_int_power_extract_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_int_power_extract_case.create_case())
    End Sub
End Class

Public Class big_int_str_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_int_str_case.create_case())
    End Sub
End Class

Public Class big_int_bytes_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(big_int_bytes_case.create_case())
    End Sub
End Class