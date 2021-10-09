
Option Explicit On
Option Infer Off
Option Strict On

<AttributeUsage(AttributeTargets.Constructor, AllowMultiple:=False, Inherited:=False)>
Public NotInheritable Class copy_constructor
    Inherits Attribute
End Class

' Retrieve the copy or move constructor of type T.
Public NotInheritable Class copy_constructor(Of T)
    Private Shared ReadOnly v As Func(Of Object(), T) =
        assert_which.of(type_info(Of T).annotated_constructor(Of copy_constructor)()).is_not_null()

    Public Shared Function invoke(ByVal ParamArray args() As Object) As T
        Try
            Return v(args)
        Catch ex As Exception
            log_unhandled_exception(ex)
            assert(False)
            Return Nothing
        End Try
    End Function

    Public Shared Function copy_from(Of T1)(ByVal i As T1) As T
        Return invoke(copy_no_error(i))
    End Function

    Public Shared Function copy_from(Of T1, T2)(ByVal i1 As T1, ByVal i2 As T2) As T
        Return invoke(copy_no_error(i1), copy_no_error(i2))
    End Function

    Public Shared Function copy_from(Of T1, T2, T3)(ByVal i1 As T1, ByVal i2 As T2, ByVal i3 As T3) As T
        Return invoke(copy_no_error(i1), copy_no_error(i2), copy_no_error(i3))
    End Function

    Public Shared Function copy_from(Of T1, T2, T3, T4)(ByVal i1 As T1,
                                                        ByVal i2 As T2,
                                                        ByVal i3 As T3,
                                                        ByVal i4 As T4) As T
        Return invoke(copy_no_error(i1), copy_no_error(i2), copy_no_error(i3), copy_no_error(i4))
    End Function

    Public Shared Function copy_from(Of T1, T2, T3, T4, T5)(ByVal i1 As T1,
                                                            ByVal i2 As T2,
                                                            ByVal i3 As T3,
                                                            ByVal i4 As T4,
                                                            ByVal i5 As T5) As T
        Return invoke(copy_no_error(i1), copy_no_error(i2), copy_no_error(i3), copy_no_error(i4), copy_no_error(i5))
    End Function

    Private Sub New()
    End Sub
End Class
