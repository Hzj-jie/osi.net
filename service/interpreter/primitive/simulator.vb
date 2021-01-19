
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports System.Text
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation

Namespace primitive
    Public Class simulator
        Implements imitation

        Private ReadOnly _errors As vector(Of executor.error_type)
        Private ReadOnly _instructions As vector(Of instruction)
        Private ReadOnly _stack As vector(Of ref(Of Byte()))
        Private ReadOnly _states As vector(Of executor.state)
        Private ReadOnly _interrupts As interrupts
        Private _carry_over As Boolean
        Private _diviced_by_zero As Boolean
        Private _imaginary_number As Boolean
        Private _halt As Boolean
        Private _instruction_ref As UInt64
        Private _do_not_advance_instruction_ref As Boolean
        Private _stop As Boolean

        Public Sub New(ByVal interrupts As interrupts)
            assert(Not interrupts Is Nothing)
            _errors = New vector(Of executor.error_type)()
            _instructions = New vector(Of instruction)()
            _stack = New vector(Of ref(Of Byte()))()
            _states = New vector(Of executor.state)()
            _interrupts = interrupts
        End Sub

        Public Sub New()
            Me.New(New interrupts())
        End Sub

        Public Function carry_over() As Boolean Implements executor.carry_over
            Return _carry_over
        End Function

        Public Sub carry_over(ByVal v As Boolean) Implements imitation.carry_over
            _carry_over = v
        End Sub

        Public Function divided_by_zero() As Boolean Implements executor.divided_by_zero
            Return _diviced_by_zero
        End Function

        Public Sub divided_by_zero(ByVal v As Boolean) Implements imitation.divided_by_zero
            _diviced_by_zero = v
        End Sub

        Public Function imaginary_number() As Boolean Implements executor.imaginary_number
            Return _imaginary_number
        End Function

        Public Sub imaginary_number(ByVal v As Boolean) Implements imitation.imaginary_number
            _imaginary_number = v
        End Sub

        Public Function errors() As vector(Of executor.error_type) Implements executor.errors
            Return _errors
        End Function

        Public Function halt() As Boolean Implements executor.halt
            Return _halt
        End Function

        Public Function stack_size() As UInt64 Implements executor.stack_size
            Return _stack.size()
        End Function

        Public Function instruction_ref() As UInt64 Implements executor.instruction_ref
            Return _instruction_ref
        End Function

        Public Function access_states(ByVal p As UInt64) As executor.state Implements executor.access_states
            If p >= _states.size() Then
                executor_stop_error.throw(executor.error_type.stack_access_out_of_boundary)
                assert(False)
                Return executor.state.empty
            Else
                Return _states(CUInt(p))
            End If
        End Function

        Public Function states_size() As UInt64 Implements executor.states_size
            Return CULng(_states.size())
        End Function

        Public Function interrupts() As interrupts Implements imitation.interrupts
            Return _interrupts
        End Function

        Public Sub instruction_ref(ByVal v As Int64) Implements imitation.instruction_ref
            If v < 0 OrElse v >= _instructions.size() Then
                executor_stop_error.throw(executor.error_type.instruction_ref_overflow)
            Else
                _instruction_ref = CULng(v)
            End If
        End Sub

        Public Sub advance_instruction_ref(ByVal v As Int64) Implements imitation.advance_instruction_ref
            If v < 0 AndAlso -v > _instruction_ref Then
                executor_stop_error.throw(executor.error_type.instruction_ref_overflow)
            Else
                _instruction_ref = CULng(CLng(_instruction_ref) + v)
                If _instruction_ref >= _instructions.size() Then
                    executor_stop_error.throw(executor.error_type.instruction_ref_overflow)
                End If
            End If
        End Sub

        Public Sub [stop]() Implements imitation.stop
            _stop = True
        End Sub

        Public Function access_stack(ByVal p As data_ref) As ref(Of Byte()) Implements executor.access_stack
            assert(Not p Is Nothing)
            ' data_ref has only 62 bits, so using int64 instead of uint64 is safe.
            Dim l As Int64 = 0
            If p.relative() Then
                l = _stack.size() - p.offset() - 1
            Else
                l = p.offset()
            End If
            If l < 0 OrElse l >= _stack.size() Then
                executor_stop_error.throw(executor.error_type.stack_access_out_of_boundary)
                Return Nothing
            Else
                Return _stack(CUInt(l))
            End If
        End Function

        Public Sub push_stack() Implements imitation.push_stack
            _stack.emplace_back(New ref(Of Byte()))
        End Sub

        Public Sub pop_stack() Implements imitation.pop_stack
            If _stack.empty() Then
                executor_stop_error.throw(executor.error_type.stack_access_out_of_boundary)
            Else
                _stack.pop_back()
            End If
        End Sub

        Public Sub store_state() Implements imitation.store_state
            _states.emplace_back(current_state())
        End Sub

        Public Sub restore_state() Implements imitation.restore_state
            If _states.empty() Then
                executor_stop_error.throw(executor.error_type.stack_access_out_of_boundary)
            Else
                Dim r As executor.state = Nothing
                r = _states.back()
                _states.pop_back()
                assert(r.stack_size <= _stack.size())
                _stack.resize(CUInt(r.stack_size))
                instruction_ref(r.instruction_ref)
            End If
        End Sub

        Public Sub do_not_advance_instruction_ref() Implements imitation.do_not_advance_instruction_ref
            assert(_do_not_advance_instruction_ref = False)
            _do_not_advance_instruction_ref = True
        End Sub

        Private Sub halt(ByVal ex As executor_stop_error)
            assert(Not ex Is Nothing)
            _errors.emplace_back(ex.error_types)
            _halt = True
        End Sub

        Public Sub execute() Implements executor.execute
            If _instructions.empty() Then
                halt(New executor_stop_error(executor.error_type.instruction_ref_overflow))
                Return
            End If
            _stop = False
            While True
                assert(_instruction_ref >= 0 AndAlso _instruction_ref < _instructions.size())
                Try
                    _instructions(CUInt(_instruction_ref)).execute(Me)
                Catch ex As executor_stop_error
                    halt(ex)
                End Try
                If _stop OrElse _halt Then
                    Exit While
                End If
                If _do_not_advance_instruction_ref Then
                    _do_not_advance_instruction_ref = False
                Else
                    ' Detect instruction_ref_overflow error.
                    advance_instruction_ref(int64_1)
                End If
            End While
        End Sub

        Public Function bytes_size() As UInt32 Implements exportable.bytes_size
            ' No one should use a simulator as its parameter.
            assert(False)
            Return uint32_0
        End Function

        Public Function export(ByRef b() As Byte) As Boolean Implements exportable.export
            If _instructions.empty() Then
                b = Nothing
                Return True
            Else
                Dim ms As MemoryStream = Nothing
                ms = New MemoryStream()
                For i As UInt32 = 0 To _instructions.size() - uint32_1
                    If _instructions(i).export(b) Then
                        assert(Not isemptyarray(b))
                        ms.Write(b, 0, array_size_i(b))
                    Else
                        Return False
                    End If
                Next
                b = ms.fit_buffer()
                Return True
            End If
        End Function

        Public Function export(ByRef s As String) As Boolean Implements exportable.export
            If _instructions.empty() Then
                s = Nothing
                Return True
            Else
                Dim ss As StringBuilder = Nothing
                ss = New StringBuilder()
                For i As UInt32 = 0 To _instructions.size() - uint32_1
                    If _instructions(i).export(s) Then
                        assert(Not s.null_or_empty())
                        ss.Append(s)
                        ss.Append(newline.incode())
                    Else
                        Return False
                    End If
                Next
                s = Convert.ToString(ss)
                Return True
            End If
        End Function

        Private Function import(Of T)(ByVal i As T,
                                      ByRef p As UInt32,
                                      ByVal imp As _do_val_val_ref(Of instruction_wrapper, T, UInt32, Boolean),
                                      ByVal size_of As Func(Of T, UInt32)) As Boolean
            assert(Not imp Is Nothing)
            assert(Not size_of Is Nothing)
            If p >= size_of(i) Then
                Return False
            Else
                _instructions.clear()
                While p < size_of(i)
                    Dim ins As instruction_wrapper = Nothing
                    ins = New instruction_wrapper()
                    If imp(ins, i, p) Then
                        _instructions.emplace_back(ins)
                    Else
                        Return False
                    End If
                End While
                _instructions.emplace_back(New instructions.stop())
                Return True
            End If
        End Function

        Public Function import(ByVal i() As Byte, ByRef p As UInt32) As Boolean Implements exportable.import
            Return import(i,
                          p,
                          Function(ByVal ins As instruction_wrapper, ByVal x() As Byte, ByRef pos As UInt32) As Boolean
                              assert(Not ins Is Nothing)
                              Return ins.import(x, pos)
                          End Function,
                          Function(ByVal x() As Byte) As UInt32
                              Return array_size(i)
                          End Function)
        End Function

        Public Function import(ByVal s As vector(Of String), ByRef p As UInt32) As Boolean Implements exportable.import
            Return import(s,
                          p,
                          Function(ByVal ins As instruction_wrapper,
                                   ByVal x As vector(Of String),
                                   ByRef pos As UInt32) As Boolean
                              assert(Not ins Is Nothing)
                              Return ins.import(x, pos)
                          End Function,
                          Function(ByVal x As vector(Of String)) As UInt32
                              Return x.size_or_0()
                          End Function)
        End Function
    End Class
End Namespace
