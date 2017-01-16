
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.compiler.logic

Namespace logic
    Public Class scope_test
        Inherits [case]

        Private Shared Function rnd_name() As String
            Return rnd_chars(rnd_int(1, 10))
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
                name = rnd_name()
                Dim type As String = Nothing
                type = rnd_name()
                If s.define(name, type) Then
                    stack.emplace_back(emplace_make_pair(name, type))
                End If
            Next

            For i As Int32 = 0 To stack.size() - 1
                Dim offset As UInt32 = 0
                Dim type As String = Nothing
                If assert_true(s.export(stack(i).first, offset)) AndAlso
                   assert_less(offset, stack.size()) AndAlso
                   assert_true(s.type(stack(i).first, type)) Then
                    If stack.size() - i - 1 > offset Then
                        assert_equal(stack(stack.size() - offset - 1).first, stack(i).first)
                        assert_equal(type, stack(stack.size() - offset - 1).second)
                    Else
                        assert_equal(offset, stack.size() - i - 1)
                        assert_equal(type, stack(i).second)
                    End If
                End If
            Next

            For i As Int32 = 0 To 1000
                Dim name As String = Nothing
                name = rnd_name()
                Dim offset As UInt32 = 0
                Dim type As String = Nothing
                If s.export(name, offset) AndAlso
                   assert_less(offset, stack.size()) AndAlso
                   assert_true(s.type(name, type)) Then
                    assert_equal(stack(stack.size() - offset - 1).first, name)
                    assert_equal(stack(stack.size() - offset - 1).second, type)
                End If
            Next

            If depth < 100 Then
                Dim new_scope As scope = Nothing
                new_scope = s.start_scope()
                execute(new_scope, stack, depth + 1)
#If 0 Then
                assert_reference_equal(new_scope.end_scope(), s)
#End If
            End If
            stack.resize(stack.size() - c)
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
