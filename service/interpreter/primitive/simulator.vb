
Option Strict On

Imports System.IO
Imports System.Text
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation

Namespace primitive
    Public Class simulator
        Implements imitation

        Private ReadOnly _errors As vector(Of executor.error_type)
        Private ReadOnly _instructions As vector(Of instruction)
        Private ReadOnly _stack As vector(Of pointer(Of Byte()))
        Private ReadOnly _extern_functions As extern_functions
        Private _carry_over As Boolean
        Private _diviced_by_zero As Boolean
        Private _imaginary_number As Boolean
        Private _halt As Boolean
        Private _instruction_pointer As UInt64
        Private _do_not_advance_instruction_pointer As Boolean
        Private _stop As Boolean

        Public Sub New(ByVal extern_functions As extern_functions)
            assert(Not extern_functions Is Nothing)
            _errors = New vector(Of executor.error_type)()
            _instructions = New vector(Of instruction)()
            _stack = New vector(Of pointer(Of Byte()))()
            _extern_functions = extern_functions
        End Sub

        Public Sub New()
            Me.New(New extern_functions())
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

        Public Function instruction_pointer() As UInt64 Implements executor.instruction_pointer
            Return _instruction_pointer
        End Function

        Public Function extern_functions() As extern_functions Implements imitation.extern_functions
            Return _extern_functions
        End Function

        Public Sub instruction_pointer(ByVal v As Int64) Implements imitation.instruction_pointer
            If v < 0 OrElse v >= _instructions.size() Then
                executor_stop_error.throw(executor.error_type.instruction_pointer_overflow)
            Else
                _instruction_pointer = CULng(v)
            End If
        End Sub

        Public Sub advance_instruction_pointer(ByVal v As Int64) Implements imitation.advance_instruction_pointer
            If v < 0 AndAlso -v > _instruction_pointer Then
                executor_stop_error.throw(executor.error_type.instruction_pointer_overflow)
            Else
                _instruction_pointer = CULng(CLng(_instruction_pointer) + v)
                If _instruction_pointer >= _instructions.size() Then
                    executor_stop_error.throw(executor.error_type.instruction_pointer_overflow)
                End If
            End If
        End Sub

        Public Sub [stop]() Implements imitation.stop
            _stop = True
        End Sub

        Public Function access_stack(ByVal p As data_ref) As pointer(Of Byte()) Implements executor.access_stack
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
            _stack.emplace_back(New pointer(Of Byte()))
        End Sub

        Public Sub pop_stack() Implements imitation.pop_stack
            If _stack.empty() Then
                executor_stop_error.throw(executor.error_type.stack_access_out_of_boundary)
            Else
                _stack.pop_back()
            End If
        End Sub

        Public Sub do_not_advance_instruction_pointer() Implements imitation.do_not_advance_instruction_pointer
            assert(_do_not_advance_instruction_pointer = False)
            _do_not_advance_instruction_pointer = True
        End Sub

        Private Sub halt(ByVal ex As executor_stop_error)
            assert(Not ex Is Nothing)
            _errors.emplace_back(ex.error_types)
            _halt = True
        End Sub

        Public Sub execute() Implements executor.execute
            If _instructions.empty() Then
                halt(New executor_stop_error(executor.error_type.instruction_pointer_overflow))
                Return
            End If
            _stop = False
            While True
                assert(_instruction_pointer >= 0 AndAlso _instruction_pointer < _instructions.size())
                Try
                    _instructions(CUInt(_instruction_pointer)).execute(Me)
                Catch ex As executor_stop_error
                    halt(ex)
                End Try
                If _stop OrElse _halt Then
                    Exit While
                End If
                If _do_not_advance_instruction_pointer Then
                    _do_not_advance_instruction_pointer = False
                Else
                    ' Detect instruction_pointer_overflow error.
                    advance_instruction_pointer(int64_1)
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

        Public Function import(ByVal i() As Byte, ByRef p As UInt32) As Boolean Implements exportable.import
            If p >= array_size(i) Then
                Return False
            Else
                _instructions.clear()
                While p < array_size(i)
                    Dim ins As instruction_wrapper = Nothing
                    ins = New instruction_wrapper()
                    If ins.import(i, p) Then
                        _instructions.emplace_back(ins)
                    Else
                        Return False
                    End If
                End While
                Return True
            End If
        End Function

        Public Function import(ByVal s As vector(Of String), ByRef p As UInt32) As Boolean Implements exportable.import
            If s.null_or_empty() OrElse p >= s.size() Then
                Return False
            Else
                _instructions.clear()
                While p < s.size()
                    Dim ins As instruction_wrapper = Nothing
                    ins = New instruction_wrapper()
                    If ins.import(s, p) Then
                        _instructions.emplace_back(ins)
                    Else
                        Return False
                    End If
                End While
                Return True
            End If
        End Function
    End Class
End Namespace
