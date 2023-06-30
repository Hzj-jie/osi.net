
Imports osi.root.template
Imports osi.root.formation
Imports MAX_TYPE = osi.root.template._256

Public Class lp(Of RESULT_T)
    Inherits lp(Of MAX_TYPE, RESULT_T)

    Private Sub New()
        MyBase.New()
    End Sub

    Private Shared Shadows Function ctor() As Func(Of lp(Of RESULT_T))
        Return Function() New lp(Of RESULT_T)()
    End Function

    Public Shared Shadows Function ctor(ByVal inputs As vector(Of String),
                                        ByVal ParamArray [overrides]() As String) As lp(Of RESULT_T)
        Return lp(Of MAX_TYPE, RESULT_T).ctor(ctor(), inputs, [overrides])
    End Function

    Public Shared Shadows Function ctor(ByVal file_name As String,
                                        ByVal ParamArray [overrides]() As String) As lp(Of RESULT_T)
        Return lp(Of MAX_TYPE, RESULT_T).ctor(ctor(), file_name, [overrides])
    End Function

    Public Shared Shadows Function ctor(ByVal inputs As vector(Of String),
                                        ByVal t As Type) As lp(Of RESULT_T)
        Return lp(Of MAX_TYPE, RESULT_T).ctor(ctor(), inputs, t)
    End Function

    Public Shared Shadows Function ctor(ByVal file_name As String,
                                        ByVal t As Type) As lp(Of RESULT_T)
        Return lp(Of MAX_TYPE, RESULT_T).ctor(ctor(), file_name, t)
    End Function

    Public Shared Shadows Function ctor(Of T)(ByVal inputs As vector(Of String)) As lp(Of RESULT_T)
        Return lp(Of MAX_TYPE, RESULT_T).ctor(ctor(), inputs, GetType(T))
    End Function

    Public Shared Shadows Function ctor(Of T)(ByVal file_name As String) As lp(Of RESULT_T)
        Return lp(Of MAX_TYPE, RESULT_T).ctor(ctor(), file_name, GetType(T))
    End Function

    Public Shared Shadows Function ctor(ByVal inputs As vector(Of String)) As lp(Of RESULT_T)
        Return lp(Of MAX_TYPE, RESULT_T).ctor(ctor(), inputs, GetType(RESULT_T))
    End Function

    Public Shared Shadows Function ctor(ByVal file_name As String) As lp(Of RESULT_T)
        Return lp(Of MAX_TYPE, RESULT_T).ctor(ctor(), file_name, GetType(RESULT_T))
    End Function
End Class
