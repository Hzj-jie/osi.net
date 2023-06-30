
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.math

' TODO: Should use big_int to replace big_uint.
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
                    imi.advance_instruction_ref(d0.offset())
                Else
                    imi.instruction_ref(d0.offset())
                End If
                imi.do_not_advance_instruction_ref()
            End Sub
        End Class

        Partial Public NotInheritable Class [add]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim b1 As New big_uint(+p1(imi))
                Dim b2 As New big_uint(+p2(imi))
                p0(imi).set(b1.add(b2).as_bytes())
            End Sub
        End Class

        Partial Public NotInheritable Class [sub]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim b1 As New big_uint(+p1(imi))
                Dim b2 As New big_uint(+p2(imi))
                Dim c As Boolean = False
                p0(imi).set(b1.sub(b2, c).as_bytes())
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
                Dim b1 As New big_uint(+p1(imi))
                Dim b2 As New big_uint(+p2(imi))
                p0(imi).set(b1.multiply(b2).as_bytes())
            End Sub
        End Class

        Partial Public NotInheritable Class [div]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim b2 As New big_uint(+p2(imi))
                Dim b3 As New big_uint(+p3(imi))
                Dim rmd As big_uint = Nothing
                Dim c As Boolean = False
                b2.divide(b3, c, rmd)
                p0(imi).set(b2.as_bytes())
                p1(imi).set(rmd.as_bytes())
                imi.divided_by_zero(c)
            End Sub
        End Class

        Partial Public NotInheritable Class [ext]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim b2 As New big_uint(+p2(imi))
                Dim b3 As New big_uint(+p3(imi))
                Dim rmd As big_uint = Nothing
                Dim c As Boolean = False
                b2.extract(b3, c, rmd)
                p0(imi).set(b2.as_bytes())
                p1(imi).set(rmd.as_bytes())
                imi.divided_by_zero(c)
            End Sub
        End Class

        Partial Public NotInheritable Class [pow]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim b1 As New big_uint(+p1(imi))
                Dim b2 As New big_uint(+p2(imi))
                p0(imi).set(b1.power(b2).as_bytes())
            End Sub
        End Class

        Partial Public NotInheritable Class [jumpif]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                If Not imi.access_as_bool(d1) Then
                    Return
                End If
                If d0.relative() Then
                    imi.advance_instruction_ref(d0.offset())
                Else
                    imi.instruction_ref(d0.offset())
                End If
                imi.do_not_advance_instruction_ref()
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
                Dim b1 As New big_uint(+p1(imi))
                Dim b2 As New big_uint(+p2(imi))
                p0(imi).set(bool_bytes(b1.equal(b2)))
            End Sub
        End Class

        Partial Public NotInheritable Class [less]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim b1 As New big_uint(+p1(imi))
                Dim b2 As New big_uint(+p2(imi))
                p0(imi).set(bool_bytes(b1.less(b2)))
            End Sub
        End Class

        Partial Public NotInheritable Class [app]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim p0 As ref(Of Byte()) = Me.p0(imi)
                p0.set(array_concat(+p0, +p1(imi)))
            End Sub
        End Class

        Partial Public NotInheritable Class [sapp]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim p0 As ref(Of Byte()) = Me.p0(imi)
                Dim b() As Byte = Nothing
                If Not chunk.from_bytes(+p1(imi), b) Then
                    executor_stop_error.throw(executor.error_type.invalid_buffer_size)
                    assert(False)
                    Return
                End If
                p0.set(array_concat(+p0, b))
            End Sub
        End Class

        Partial Public NotInheritable Class [cut]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim p0 As ref(Of Byte()) = Me.p0(imi)
                Dim p1 As ref(Of Byte()) = Me.p1(imi)
                Dim l As UInt32 = imi.access_as_uint32(d2)
                If l >= array_size(+p1) Then
                    p0.clear()
                Else
                    Dim r(CInt(array_size(+p1) - l - uint32_1)) As Byte
                    arrays.copy(r, uint32_0, +p1, l, array_size(+p1) - l)
                    p0.set(r)
                End If
            End Sub
        End Class

        Partial Public NotInheritable Class [cutl]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim p0 As ref(Of Byte()) = Me.p0(imi)
                Dim p1 As ref(Of Byte()) = Me.p1(imi)
                Dim sl() As UInt32 = imi.access_as_uint32(d2, d3)
                assert(array_size(sl) = 2)
                If sl(0) >= array_size(+p1) OrElse sl(1) = uint32_0 Then
                    p0.clear()
                Else
                    Dim r(CInt(min(array_size(+p1) - sl(0), sl(1)) - uint32_1)) As Byte
                    arrays.copy(r, uint32_0, +p1, sl(0), array_size(r))
                    p0.set(r)
                End If
            End Sub
        End Class

        Partial Public NotInheritable Class [int]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                p2(imi).set(imi.interrupts().invoke(imi.access_as_uint32(d0), +p1(imi)))
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
                Dim u As UInt32 = imi.access_as_uint32(d2)
                If chunks.parse(+p1(imi), vs) AndAlso
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
                Dim b1 As New big_uint(+p1(imi))
                Dim b2 As New big_uint(+p2(imi))
                p0(imi).set(b1.and(b2).as_bytes())
            End Sub
        End Class

        Partial Public NotInheritable Class [or]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim b1 As New big_uint(+p1(imi))
                Dim b2 As New big_uint(+p2(imi))
                p0(imi).set(b1.or(b2).as_bytes())
            End Sub
        End Class

        Partial Public NotInheritable Class [not]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim b1 As New big_uint(+p1(imi))
                p0(imi).set(b1.not().as_bytes())
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

        Partial Public NotInheritable Class [fadd]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim b1 As New big_udec(+p1(imi))
                Dim b2 As New big_udec(+p2(imi))
                p0(imi).set(b1.add(b2).as_bytes())
            End Sub
        End Class

        Partial Public NotInheritable Class [fsub]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim b1 As New big_udec(+p1(imi))
                Dim b2 As New big_udec(+p2(imi))
                p0(imi).set(b1.sub(b2).as_bytes())
            End Sub
        End Class

        Partial Public NotInheritable Class [fmul]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim b1 As New big_udec(+p1(imi))
                Dim b2 As New big_udec(+p2(imi))
                p0(imi).set(b1.multiply(b2).as_bytes())
            End Sub
        End Class

        Partial Public NotInheritable Class [fdiv]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim b1 As New big_udec(+p1(imi))
                Dim b2 As New big_udec(+p2(imi))
                Dim c As Boolean = False
                p0(imi).set(b1.divide(b2, c).as_bytes())
                imi.divided_by_zero(c)
            End Sub
        End Class

        Partial Public NotInheritable Class [fext]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim b1 As New big_udec(+p1(imi))
                Dim b2 As New big_udec(+p2(imi))
                Dim c As Boolean = False
                p0(imi).set(b1.extract(b2, c).as_bytes())
                imi.divided_by_zero(c)
            End Sub
        End Class

        Partial Public NotInheritable Class [fpow]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim b1 As New big_udec(+p1(imi))
                Dim b2 As New big_udec(+p2(imi))
                p0(imi).set(b1.power(b2).as_bytes())
            End Sub
        End Class

        Partial Public NotInheritable Class [fequal]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim b1 As New big_udec(+p1(imi))
                Dim b2 As New big_udec(+p2(imi))
                p0(imi).set(bool_bytes(b1.equal(b2)))
            End Sub
        End Class

        Partial Public NotInheritable Class [fless]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim b1 As New big_udec(+p1(imi))
                Dim b2 As New big_udec(+p2(imi))
                p0(imi).set(bool_bytes(b1.less(b2)))
            End Sub
        End Class

        Partial Public NotInheritable Class [lfs]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim b1 As New big_uint(+p1(imi))
                Dim b2 As New big_uint(+p2(imi))
                p0(imi).set(b1.left_shift(b2).as_bytes())
            End Sub
        End Class

        Partial Public NotInheritable Class [rfs]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                assert(Not imi Is Nothing)
                Dim b1 As New big_uint(+p1(imi))
                Dim b2 As New big_uint(+p2(imi))
                p0(imi).set(b1.right_shift(b2).as_bytes())
            End Sub
        End Class

        Partial Public NotInheritable Class [alloc]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                Dim s As UInt32 = imi.access_as_uint32(d1)
                If imi.carry_over() Then
                    executor_stop_error.throw(executor.error_type.out_of_heap_memory)
                    assert(False)
                    Return
                End If
                p0(imi).set(uint64_bytes(imi.alloc(s)))
            End Sub
        End Class

        Partial Public NotInheritable Class [dealloc]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                Dim p As UInt64 = imi.access_as_uint64(d0)
                If imi.carry_over() Then
                    executor_stop_error.throw(executor.error_type.heap_access_out_of_boundary)
                    assert(False)
                    Return
                End If
                imi.dealloc(p)
            End Sub
        End Class

        Partial Public NotInheritable Class [jmpr]
            Implements instruction

            Public Sub execute(ByVal imi As imitation) Implements instruction.execute
                Dim s As Int64 = imi.access_as_int64(d0)
                imi.instruction_ref(s)
                imi.do_not_advance_instruction_ref()
            End Sub
        End Class
    End Namespace
End Namespace
