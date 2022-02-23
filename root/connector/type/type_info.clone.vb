﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.delegates

Partial Public NotInheritable Class type_info(Of T)
    Private NotInheritable Class clone_cache
        ' DirectCast(ICloneable.Clone(), T)
        Public Shared ReadOnly clone As _do_val_ref(Of T, T, Boolean) =
            If(is_cloneable,
               Function(ByVal i As T, ByRef o As T) As Boolean
                   assert(i IsNot Nothing)
                   Try
                       o = direct_cast(Of T)(direct_cast(Of ICloneable)(i).Clone())
                       Return True
                   Catch ex As Exception
                       copy_error(ex)
                       Return False
                   End Try
               End Function,
               Nothing)
        ' ICloneable(Of T).Clone()
        Public Shared ReadOnly clone_t As _do_val_ref(Of T, T, Boolean) =
            If(is_cloneable_T,
               Function(ByVal i As T, ByRef o As T) As Boolean
                   assert(i IsNot Nothing)
                   Try
                       o = (direct_cast(Of ICloneable(Of T))(i).Clone())
                       Return True
                   Catch ex As Exception
                       copy_error(ex)
                       Return False
                   End Try
               End Function,
               Nothing)
        ' DirectCast(i, ICloneable(Of T)).Clone() or DirectCast(i, ICloneable).Clone()
        Public Shared ReadOnly runtime_clone As _do_val_ref(Of T, T, Boolean) = calculate_runtime_clone()
        ' o = i
        Public Shared ReadOnly assignment_clone As _do_val_ref(Of T, T, Boolean) =
            Function(ByVal i As T, ByRef o As T) As Boolean
                o = i
                Return True
            End Function
        ' This function ensures a new object is always created.
        Public Shared ReadOnly new_object_clone As _do_val_ref(Of T, T, Boolean) = calculate_new_object_clone()
        ' The best clone function implemented by T.
        Public Shared ReadOnly dominated_clone As _do_val_ref(Of T, T, Boolean) =
            If(new_object_clone IsNot Nothing, new_object_clone, assignment_clone)

        Private Shared Function calculate_runtime_clone() As _do_val_ref(Of T, T, Boolean)
            If is_object Then
                raise_error(error_type.performance, "copy(Of Object) impacts performance seriously.")
                Return Function(ByVal i As T, ByRef o As T) As Boolean
                           assert(i IsNot Nothing)
                           Using code_block
                               Dim c As ICloneable(Of T) = Nothing
                               c = TryCast(i, ICloneable(Of T))
                               If c IsNot Nothing Then
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
                               If c IsNot Nothing Then
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
            Return Nothing
        End Function

        Private Shared Function calculate_new_object_clone() As _do_val_ref(Of T, T, Boolean)
            If clone_t IsNot Nothing Then
                Return clone_t
            End If
            If clone IsNot Nothing Then
                Return clone
            End If
            If runtime_clone IsNot Nothing Then
                Return runtime_clone
            End If
            If is_valuetype Then
                Return assignment_clone
            End If
            Return Nothing
        End Function

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
