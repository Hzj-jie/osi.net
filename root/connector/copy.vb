
Imports osi.root.constants
Imports osi.root.delegates

Public Module _copy
    Private NotInheritable Class clone_cache(Of T)
        Public Shared ReadOnly cloneable As Boolean
        Private Shared ReadOnly non_nothing_copy As _do_val_ref(Of T, T, Boolean)

        Shared Sub New()
            If type_info(Of T).is_cloneable_T Then
                non_nothing_copy = Function(ByVal i As T, ByRef o As T) As Boolean
                                       assert(Not i Is Nothing)
                                       Try
                                           o = DirectCast(i, ICloneable(Of T)).Clone()
                                           Return True
                                       Catch ex As Exception
                                           copy_error(ex)
                                           Return False
                                       End Try
                                   End Function
                cloneable = True
            ElseIf type_info(Of T).is_cloneable Then
                non_nothing_copy = Function(ByVal i As T, ByRef o As T) As Boolean
                                       assert(Not i Is Nothing)
                                       Try
                                           o = DirectCast(DirectCast(i, ICloneable).Clone(), T)
                                           Return True
                                       Catch ex As Exception
                                           copy_error(ex)
                                           o = Nothing
                                           Return False
                                       End Try
                                   End Function
                cloneable = True
            ElseIf type_info(Of T).is_valuetype Then
                non_nothing_copy = Function(ByVal i As T, ByRef o As T) As Boolean
                                       o = i
                                       Return True
                                   End Function
                cloneable = False
            ElseIf type_info(Of T).is_object Then
                raise_error(error_type.performance, "copy(Of Object) is definitely a performance destroyer.")
                non_nothing_copy = Function(ByVal i As T, ByRef o As T) As Boolean
                                       assert(Not i Is Nothing)
                                       Using code_block
                                           Dim c As ICloneable(Of T) = Nothing
                                           c = TryCast(i, ICloneable(Of T))
                                           If Not c Is Nothing Then
                                               Try
                                                   o = c.Clone()
                                                   Return True
                                               Catch ex As Exception
                                                   copy_error(ex)
                                                   Return False
                                               End Try
                                           End If
                                       End Using
                                       Using code_block
                                           Dim c As ICloneable = Nothing
                                           c = TryCast(i, ICloneable)
                                           If Not c Is Nothing Then
                                               Try
                                                   o = DirectCast(c.Clone(), T)
                                                   Return True
                                               Catch ex As Exception
                                                   copy_error(ex)
                                                   Return False
                                               End Try
                                           End If
                                       End Using
                                       Return False
                                   End Function
                cloneable = True
            Else
                non_nothing_copy = Function(ByVal i As T, ByRef o As T) As Boolean
                                       o = i
                                       Return False
                                   End Function
                cloneable = False
            End If
        End Sub

        Private Shared Sub copy_error(ByVal ex As Exception)
            raise_error(error_type.exclamation,
                        "failed to clone object, type ",
                        GetType(T).FullName,
                        ", ex ",
                        ex.Message(),
                        ", callstack ",
                        ex.StackTrace())
        End Sub

        Public Shared Function copy(ByVal i As T, ByRef o As T) As Boolean
            If i Is Nothing Then
                o = Nothing
                Return False
            Else
                Return non_nothing_copy(i, o)
            End If
        End Function
    End Class

    Public Function copy_clone(Of T)(ByRef dest As T, ByVal src As T) As Boolean
#If DEBUG Then
        assert(clone_cache(Of T).cloneable AndAlso Not src Is Nothing)
#End If
        Return clone_cache(Of T).copy(src, dest)
    End Function

    Public Function copy(Of T)(ByRef dest As T, ByVal src As T) As Boolean
        Dim r As Boolean = False
        r = clone_cache(Of T).copy(src, dest)
        Return Not clone_cache(Of T).cloneable OrElse r
    End Function

    Private Function copy(Of T)(ByVal i As T, ByVal err_msg As Boolean) As T
        If Not clone_cache(Of T).cloneable AndAlso err_msg AndAlso isdebugmode() Then
            raise_error(error_type.exclamation,
                        "input is not cloneable, type ",
                        GetType(T).FullName)
        End If
        Dim r As T = Nothing
        clone_cache(Of T).copy(i, r)
        Return r
    End Function

    Public Function copy(Of T)(ByVal i As T) As T
        Return copy(i, True)
    End Function

    Public Function copy_no_error(Of T)(ByVal i As T) As T
        Return copy(i, False)
    End Function
End Module
