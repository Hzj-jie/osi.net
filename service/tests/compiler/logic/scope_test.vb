
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.compiler.logic
Imports osi.service.interpreter.primitive

Namespace logic
    Public Class scope_test
        Inherits [case]

        Private Shared Function rnd_name() As String
            While True
                Dim s As String = Nothing
                s = rnd_chars(rnd_int(1, 10))
                If Not s.str_empty_or_whitespaces() Then
                    Return s
                End If
            End While
            assert(False)
            Return Nothing
        End Function

        Private Shared Sub execute(ByVal s As scope,
                                   ByVal stack As vector(Of pair(Of String, String)),
                                   ByVal depth As Int32)
            assert(Not s Is Nothing)
            assert(Not stack Is Nothing)
            Dim c As Int32 = 0
            c = rnd_int(0, 100)
            For i As Int32 = 0 To c - 1
                Dim name As String = Nothing
                Dim type As String = Nothing
                Do
                    name = rnd_name()
                    type = rnd_name()
                Loop Until s.define(name, type)
                stack.emplace_back(pair.emplace_of(name, type))
            Next

            For i As Int32 = 0 To CInt(stack.size()) - 1
                Dim offset As data_ref = Nothing
                Dim type As String = Nothing
                If assertion.is_true(s.export(stack(CUInt(i)).first, offset)) AndAlso
                   assertion.is_true(offset.to_rel(stack.size(), offset)) AndAlso
                   assertion.more_or_equal(offset.offset(), uint32_0) AndAlso
                   assertion.less(offset.offset(), stack.size()) AndAlso
                   assertion.is_true(s.type(stack(CUInt(i)).first, type)) Then
                    If stack.size() - i - 1 > offset.offset() Then
                        assertion.equal(stack(stack.size() - CUInt(offset.offset()) - uint32_1).first,
                                     stack(CUInt(i)).first)
                        assertion.equal(type, stack(stack.size() - CUInt(offset.offset()) - uint32_1).second)
                    Else
                        assertion.equal(offset.offset(), stack.size() - i - uint32_1)
                        assertion.equal(type, stack(CUInt(i)).second)
                    End If
                End If
            Next

            For i As Int32 = 0 To 1000
                Dim name As String = Nothing
                name = rnd_name()
                Dim offset As data_ref = Nothing
                Dim type As String = Nothing
                If s.export(name, offset) AndAlso
                   offset.to_rel(stack.size(), offset) AndAlso
                   assertion.less(offset.offset(), stack.size()) AndAlso
                   assertion.is_true(s.type(name, type)) Then
                    assertion.equal(stack(stack.size() - CUInt(offset.offset()) - uint32_1).first, name)
                    assertion.equal(stack(stack.size() - CUInt(offset.offset()) - uint32_1).second, type)
                End If
            Next

            If depth < 100 Then
                Dim new_scope As scope = Nothing
                new_scope = s.start_scope()
                execute(new_scope, stack, depth + 1)
#If 0 Then
                assertion.reference_equal(new_scope.end_scope(), s)
#End If
            End If
            stack.resize(stack.size() - CUInt(c))
        End Sub

        Public Overrides Function run() As Boolean
            Dim stack As vector(Of pair(Of String, String)) = Nothing
            stack = New vector(Of pair(Of String, String))()
            Dim scope As scope = Nothing
            scope = New scope()
            execute(scope, stack, 0)
            Return True
        End Function
    End Class
End Namespace
