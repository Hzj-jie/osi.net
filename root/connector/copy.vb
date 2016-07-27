
Imports osi.root.constants

Public Module _copy
    Private Function clone_proxy(Of T)(ByRef r As T, ByVal s As T) As Boolean
        Try
            r = DirectCast(DirectCast(s, ICloneable).Clone(), T)
            Return True
        Catch ex As Exception
            raise_error(error_type.exclamation,
                          "failed to clone object, type ",
                          GetType(T).FullName,
                          ", ex ",
                          ex.Message(),
                          ", callstack ",
                          ex.StackTrace())
            Return False
        End Try
    End Function

    Public Function copy_clone(Of T)(ByRef dest As T, ByVal src As T) As Boolean
#If DEBUG Then
        assert(TypeOf src Is ICloneable)
#End If
        Return clone_proxy(dest, src)
    End Function

    Public Function copy(Of T)(ByRef dest As T, ByVal src As T) As Boolean
        If TypeOf src Is ICloneable Then
            Return clone_proxy(dest, src)
        Else
            dest = src
            Return True
        End If
    End Function

    Private Function copy(Of T)(ByVal i As T, ByVal err_msg As Boolean) As T
        If TypeOf i Is ICloneable Then
            Dim r As T = Nothing
            If clone_proxy(r, i) Then
                Return r
            Else
                Return Nothing
            End If
        ElseIf TypeOf i Is ValueType OrElse
               i Is Nothing Then
            Return i
        Else
            If err_msg AndAlso isdebugmode() Then
                raise_error(error_type.exclamation,
                              "input is not cloneable, type ",
                              GetType(T).FullName)
            End If
            Return i
        End If
    End Function

    Public Function copy(Of T)(ByVal i As T) As T
        Return copy(i, True)
    End Function

    Public Function copy_no_error(Of T)(ByVal i As T) As T
        Return copy(i, False)
    End Function
End Module
