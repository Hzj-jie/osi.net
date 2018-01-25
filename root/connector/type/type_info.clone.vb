
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.delegates

Partial Public NotInheritable Class type_info(Of T)
    Private NotInheritable Class clone_cache
        ' DirectCast(ICloneable.Clone(), T)
        Public Shared ReadOnly clone As _do_val_ref(Of T, T, Boolean)
        ' ICloneable(Of T).Clone()
        Public Shared ReadOnly clone_t As _do_val_ref(Of T, T, Boolean)
        ' DirectCast(i, ICloneable(Of T)).Clone() or DirectCast(i, ICloneable).Clone()
        Public Shared ReadOnly runtime_clone As _do_val_ref(Of T, T, Boolean)
        ' o = i
        Public Shared ReadOnly assignment_clone As _do_val_ref(Of T, T, Boolean)
        ' This function ensures a new object is always created.
        Public Shared ReadOnly new_object_clone As _do_val_ref(Of T, T, Boolean)
        ' The best clone function implemented by T.
        Public Shared ReadOnly dominated_clone As _do_val_ref(Of T, T, Boolean)

        Shared Sub New()
            If is_cloneable Then
                clone = Function(ByVal i As T, ByRef o As T) As Boolean
                            assert(Not i Is Nothing)
                            Try
                                o = direct_cast(Of T)(direct_cast(Of ICloneable)(i).Clone())
                                Return True
                            Catch ex As Exception
                                copy_error(ex)
                                Return False
                            End Try
                        End Function
            End If
            If is_cloneable_T Then
                clone_t = Function(ByVal i As T, ByRef o As T) As Boolean
                              assert(Not i Is Nothing)
                              Try
                                  o = (direct_cast(Of ICloneable(Of T))(i).Clone())
                                  Return True
                              Catch ex As Exception
                                  copy_error(ex)
                                  Return False
                              End Try
                          End Function
            End If
            If is_object Then
                raise_error(error_type.performance, "copy(Of Object) impacts performance seriously.")
                runtime_clone = Function(ByVal i As T, ByRef o As T) As Boolean
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
            End If
            assignment_clone = Function(ByVal i As T, ByRef o As T) As Boolean
                                   o = i
                                   Return True
                               End Function
            If Not clone_t Is Nothing Then
                new_object_clone = clone_t
            ElseIf Not clone Is Nothing Then
                new_object_clone = clone
            ElseIf Not runtime_clone Is Nothing Then
                new_object_clone = runtime_clone
            ElseIf is_valuetype Then
                new_object_clone = assignment_clone
            End If
            If Not new_object_clone Is Nothing Then
                dominated_clone = new_object_clone
            Else
                dominated_clone = assignment_clone
            End If
        End Sub

        Private Shared Sub copy_error(ByVal ex As Exception)
            raise_error(error_type.exclamation,
                        "Failed to clone object, type ",
                        GetType(T).full_name,
                        ", ex ",
                        ex.details())
        End Sub

        Private Sub New()
        End Sub
    End Class
End Class
