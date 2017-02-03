
#Const USE_WORK_ON = False

Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.convertor
Imports osi.service.math

Namespace primitive
    Namespace instructions
        Partial Public NotInheritable Class [push]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                imi.push_stack()
            End Sub
        End Class

        Partial Public NotInheritable Class [pop]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                imi.pop_stack()
            End Sub
        End Class

        Partial Public NotInheritable Class [jump]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                If d0.relative() Then
                    imi.advance_instruction_pointer(d0.offset())
                    imi.do_not_advance_instruction_pointer()
                Else
                    imi.instruction_pointer(d0.offset())
                End If
            End Sub
        End Class

        Partial Public NotInheritable Class [add]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim b1 As big_uint = Nothing
                Dim b2 As big_uint = Nothing
#If USE_WORK_ON Then
                b1 = New big_uint()
                If Not b1.work_on(+p1(imi)) Then
                    executor_stop_error.throw(executor.error_type.invalid_buffer_size)
                End If
                b2 = New big_uint()
                If Not b2.work_on(+p2(imi)) Then
                    executor_stop_error.throw(executor.error_type.invalid_buffer_size)
                End If
                p0(imi).set(b1.add(b2).release_as_bytes())
#Else
                b1 = New big_uint(+p1(imi))
                b2 = New big_uint(+p2(imi))
                p0(imi).set(b1.add(b2).as_bytes())
#End If
            End Sub
        End Class

        Partial Public NotInheritable Class [sub]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim b1 As big_uint = Nothing
                Dim b2 As big_uint = Nothing
                Dim c As Boolean = False
#If USE_WORK_ON Then
                b1 = New big_uint()
                If Not b1.work_on(+p1(imi)) Then
                    executor_stop_error.throw(executor.error_type.invalid_buffer_size)
                End If
                b2 = New big_uint()
                If Not b2.work_on(+p2(imi)) Then
                    executor_stop_error.throw(executor.error_type.invalid_buffer_size)
                End If
                p0(imi).set(b1.sub(b2, c).release_as_bytes())
#Else
                b1 = New big_uint(+p1(imi))
                b2 = New big_uint(+p2(imi))
                p0(imi).set(b1.sub(b2, c).as_bytes())
#End If
                imi.carry_over(c)
            End Sub
        End Class

        Partial Public NotInheritable Class [cpc]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                p0(imi).set(d1.as_bytes())
            End Sub
        End Class

        Partial Public NotInheritable Class [mov]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                p0(imi).set(+p1(imi))
                p1(imi).clear()
            End Sub
        End Class

        Partial Public NotInheritable Class [cp]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                p0(imi).set(copy(+p1(imi)))
            End Sub
        End Class

        Partial Public NotInheritable Class [mul]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim b1 As big_uint = Nothing
                Dim b2 As big_uint = Nothing
#If USE_WORK_ON Then
                b1 = New big_uint()
                If Not b1.work_on(+p1(imi)) Then
                    executor_stop_error.throw(executor.error_type.invalid_buffer_size)
                End If
                b2 = New big_uint()
                If Not b2.work_on(+p2(imi)) Then
                    executor_stop_error.throw(executor.error_type.invalid_buffer_size)
                End If
                p0(imi).set(b1.multiply(b2).release_as_bytes())
#Else
                b1 = New big_uint(+p1(imi))
                b2 = New big_uint(+p2(imi))
                p0(imi).set(b1.multiply(b2).as_bytes())
#End If
            End Sub
        End Class

        Partial Public NotInheritable Class [div]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim b2 As big_uint = Nothing
                Dim b3 As big_uint = Nothing
                Dim rmd As big_uint = Nothing
                Dim c As Boolean = False
#If USE_WORK_ON Then
                b2 = New big_uint()
                If Not b2.work_on(+p2(imi)) Then
                    executor_stop_error.throw(executor.error_type.invalid_buffer_size)
                End If
                b3 = New big_uint()
                If Not b3.work_on(+p3(imi)) Then
                    executor_stop_error.throw(executor.error_type.invalid_buffer_size)
                End If
                b2.divide(b3, c, rmd)
                p0(imi).set(b2.release_as_bytes())
                p1(imi).set(rmd.release_as_bytes())
#Else
                b2 = New big_uint(+p2(imi))
                b3 = New big_uint(+p3(imi))
                b2.divide(b3, c, rmd)
                p0(imi).set(b2.as_bytes())
                p1(imi).set(rmd.as_bytes())
#End If
                imi.divided_by_zero(c)
            End Sub
        End Class

        Partial Public NotInheritable Class [ext]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim b2 As big_uint = Nothing
                Dim b3 As big_uint = Nothing
                Dim rmd As big_uint = Nothing
                Dim c As Boolean = False
#If USE_WORK_ON Then
                b2 = New big_uint()
                If Not b2.work_on(+p2(imi)) Then
                    executor_stop_error.throw(executor.error_type.invalid_buffer_size)
                End If
                b3 = New big_uint()
                If Not b3.work_on(+p3(imi)) Then
                    executor_stop_error.throw(executor.error_type.invalid_buffer_size)
                End If
                b2.extract(b3, c, rmd)
                p0(imi).set(b2.release_as_bytes())
                p1(imi).set(rmd.release_as_bytes())
#Else
                b2 = New big_uint(+p2(imi))
                b3 = New big_uint(+p3(imi))
                b2.extract(b3, c, rmd)
                p0(imi).set(b2.as_bytes())
                p1(imi).set(rmd.as_bytes())
#End If
                imi.divided_by_zero(c)
            End Sub
        End Class

        Partial Public NotInheritable Class [pow]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim b1 As big_uint = Nothing
                Dim b2 As big_uint = Nothing
#If USE_WORK_ON Then
                b1 = New big_uint()
                If Not b1.work_on(+p1(imi)) Then
                    executor_stop_error.throw(executor.error_type.invalid_buffer_size)
                End If
                b2 = New big_uint()
                If Not b2.work_on(+p2(imi)) Then
                    executor_stop_error.throw(executor.error_type.invalid_buffer_size)
                End If
                p0(imi).set(b1.power(b2).release_as_bytes())
#Else
                b1 = New big_uint(+p1(imi))
                b2 = New big_uint(+p2(imi))
                p0(imi).set(b1.power(b2).as_bytes())
#End If
            End Sub
        End Class

        Partial Public NotInheritable Class [jumpif]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim b As Boolean = False
                b = imi.access_stack_as_bool(d1)
                If b Then
                    If d0.relative() Then
                        imi.advance_instruction_pointer(d0.offset())
                        imi.do_not_advance_instruction_pointer()
                    Else
                        imi.instruction_pointer(d0.offset())
                    End If
                End If
            End Sub
        End Class

        Partial Public NotInheritable Class [cpco]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                p0(imi).set(bool_bytes(imi.carry_over()))
            End Sub
        End Class

        Partial Public NotInheritable Class [cpdbz]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                p0(imi).set(bool_bytes(imi.divided_by_zero()))
            End Sub
        End Class

        Partial Public NotInheritable Class [cpin]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                p0(imi).set(bool_bytes(imi.imaginary_number()))
            End Sub
        End Class

        Partial Public NotInheritable Class [stop]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                imi.stop()
            End Sub
        End Class

        Partial Public NotInheritable Class [equal]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim b1 As big_uint = Nothing
                Dim b2 As big_uint = Nothing
#If USE_WORK_ON Then
                b1 = New big_uint()
                If Not b1.work_on(+p1(imi)) Then
                    executor_stop_error.throw(executor.error_type.invalid_buffer_size)
                End If
                b2 = New big_uint()
                If Not b2.work_on(+p2(imi)) Then
                    executor_stop_error.throw(executor.error_type.invalid_buffer_size)
                End If
                p0(imi).set(bool_bytes(b1.equal(b2)))
#Else
                b1 = New big_uint(+p1(imi))
                b2 = New big_uint(+p2(imi))
                p0(imi).set(bool_bytes(b1.equal(b2)))
#End If
            End Sub
        End Class

        Partial Public NotInheritable Class [less]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim b1 As big_uint = Nothing
                Dim b2 As big_uint = Nothing
#If USE_WORK_ON Then
                b1 = New big_uint()
                If Not b1.work_on(+p1(imi)) Then
                    executor_stop_error.throw(executor.error_type.invalid_buffer_size)
                End If
                b2 = New big_uint()
                If Not b2.work_on(+p2(imi)) Then
                    executor_stop_error.throw(executor.error_type.invalid_buffer_size)
                End If
                p0(imi).set(bool_bytes(b1.less(b2)))
#Else
                b1 = New big_uint(+p1(imi))
                b2 = New big_uint(+p2(imi))
                p0(imi).set(bool_bytes(b1.less(b2)))
#End If
            End Sub
        End Class

        Partial Public NotInheritable Class [app]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim p0 As pointer(Of Byte()) = Nothing
                p0 = Me.p0(imi)
                p0.set(array_concat(+p0, +p1(imi)))
            End Sub
        End Class

        Partial Public NotInheritable Class [sapp]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim p0 As pointer(Of Byte()) = Nothing
                p0 = Me.p0(imi)
                p0.set(array_concat(+p0, to_chunk(+p1(imi))))
            End Sub
        End Class

        Partial Public NotInheritable Class [cut]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim p0 As pointer(Of Byte()) = Nothing
                Dim p1 As pointer(Of Byte()) = Nothing
                p0 = Me.p0(imi)
                p1 = Me.p1(imi)
                Dim l As UInt32 = 0
                l = imi.access_stack_as_uint32(d2)
                If l >= array_size(+p1) Then
                    p0.clear()
                Else
                    Dim r() As Byte = Nothing
                    ReDim r(CInt(array_size(+p1) - l - uint32_1))
                    memcpy(r, uint32_0, +p1, l, array_size(+p1) - l)
                    p0.set(r)
                End If
            End Sub
        End Class

        Partial Public NotInheritable Class [cutl]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim p0 As pointer(Of Byte()) = Nothing
                Dim p1 As pointer(Of Byte()) = Nothing
                p0 = Me.p0(imi)
                p1 = Me.p1(imi)
                Dim sl() As UInt32 = Nothing
                sl = imi.access_stack_as_uint32(d2, d3)
                assert(array_size(sl) = 2)
                If sl(0) >= array_size(+p1) OrElse sl(1) = uint32_0 Then
                    p0.clear()
                Else
                    Dim r() As Byte = Nothing
                    ReDim r(CInt(min(array_size(+p1) - sl(0), sl(1)) - uint32_1))
                    memcpy(r, uint32_0, +p1, sl(0), array_size(r))
                    p0.set(r)
                End If
            End Sub
        End Class

        Partial Public NotInheritable Class [extern]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                p2(imi).set(imi.extern_functions().invoke(imi.access_stack_as_uint32(d0), +p1(imi)))
            End Sub
        End Class

        Partial Public NotInheritable Class [clr]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                p0(imi).clear()
            End Sub
        End Class

        Partial Public NotInheritable Class [scut]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                Dim vs As vector(Of Byte()) = Nothing
                Dim u As UInt32 = 0
                u = imi.access_stack_as_uint32(d2)
                If bytes_vector_bytes(+p1(imi), vs) AndAlso
                   u < vs.size() Then
                    p0(imi).set(vs(u))
                Else
                    p0(imi).clear()
                End If
            End Sub
        End Class

        Partial Public NotInheritable Class [sizeof]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                p0(imi).set(uint32_bytes(array_size(+p1(imi))))
            End Sub
        End Class

        Partial Public NotInheritable Class [empty]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                p0(imi).set(bool_bytes(p1(imi).empty()))
            End Sub
        End Class

        Partial Public NotInheritable Class [and]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim b1 As big_uint = Nothing
                Dim b2 As big_uint = Nothing
#If USE_WORK_ON Then
                b1 = New big_uint()
                If Not b1.work_on(+p1(imi)) Then
                    executor_stop_error.throw(executor.error_type.invalid_buffer_size)
                End If
                b2 = New big_uint()
                If Not b2.work_on(+p2(imi)) Then
                    executor_stop_error.throw(executor.error_type.invalid_buffer_size)
                End If
                p0(imi).set(b1.and(b2).release_as_bytes())
#Else
                b1 = New big_uint(+p1(imi))
                b2 = New big_uint(+p2(imi))
                p0(imi).set(b1.and(b2).as_bytes())
#End If
            End Sub
        End Class

        Partial Public NotInheritable Class [or]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim b1 As big_uint = Nothing
                Dim b2 As big_uint = Nothing
#If USE_WORK_ON Then
                b1 = New big_uint()
                If Not b1.work_on(+p1(imi)) Then
                    executor_stop_error.throw(executor.error_type.invalid_buffer_size)
                End If
                b2 = New big_uint()
                If Not b2.work_on(+p2(imi)) Then
                    executor_stop_error.throw(executor.error_type.invalid_buffer_size)
                End If
                p0(imi).set(b1.or(b2).release_as_bytes())
#Else
                b1 = New big_uint(+p1(imi))
                b2 = New big_uint(+p2(imi))
                p0(imi).set(b1.or(b2).as_bytes())
#End If
            End Sub
        End Class

        Partial Public NotInheritable Class [not]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim b1 As big_uint = Nothing
#If USE_WORK_ON Then
                b1 = New big_uint()
                If Not b1.work_on(+p1(imi)) Then
                    executor_stop_error.throw(executor.error_type.invalid_buffer_size)
                End If
                p0(imi).set(b1.not().release_as_bytes())
#Else
                b1 = New big_uint(+p1(imi))
                p0(imi).set(b1.not().as_bytes())
#End If
            End Sub
        End Class

        Partial Public NotInheritable Class [stst]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                imi.store_state()
            End Sub
        End Class

        Partial Public NotInheritable Class [rest]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                imi.restore_state()
            End Sub
        End Class
    End Namespace
End Namespace
