
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.interpreter.primitive

Namespace primitive
    Public Class data_block_exportable_test
        Inherits exportable_test(Of data_block)

        Protected Overrides Function create() As data_block
            Dim c As Char = Nothing
            Dim d1 As data_block = Nothing
            d1 = data_block.random(c)
            Dim s As String = Nothing
            If rnd_bool() Then
                Select Case c
                    Case data_block.prefix.int
                        assert(d1.str(data_block.prefix.int, s))
                    Case data_block.prefix.long
                        assert(d1.str(data_block.prefix.long, s))
                    Case data_block.prefix.boolean
                        assert(d1.str(data_block.prefix.boolean, s))
                    Case data_block.prefix.array
                        assert(d1.str(data_block.prefix.array, s))
                    Case data_block.prefix.double
                        assert(d1.str(data_block.prefix.double, s))
                    Case data_block.prefix.string, data_block.prefix.c_escaped_string
                        If rnd_bool() Then
                            assert(d1.str(data_block.prefix.string, s))
                        Else
                            assert(d1.str(data_block.prefix.c_escaped_string, s))
                        End If
                    Case Else
                        assert(False)
                End Select
            Else
                assert(d1.str(data_block.prefix.array, s))
            End If
            Dim d2 As data_block = Nothing
            d2 = New data_block()
            assert_true(d2.import(s))
            assert_equal(d1, d2)
            Return d2
        End Function
    End Class
End Namespace
