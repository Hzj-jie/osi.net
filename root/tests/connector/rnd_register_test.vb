
Imports osi.root.connector
Imports osi.root.utt

Public Class rnd_register_test
    Inherits [case]

    Private Class test_class
    End Class

    Public Overrides Function run() As Boolean
        rnd_register(Of test_class).register(Function() New test_class())

        rnd(Of Boolean)()
        rnd(Of Byte)()
        rnd(Of SByte)()
        rnd(Of UInt16)()
        rnd(Of Int16)()
        rnd(Of UInt32)()
        rnd(Of Int32)()
        rnd(Of UInt64)()
        rnd(Of Int64)()
        rnd(Of Double)()
        rnd(Of String)()

        assert_not_nothing(rnd(Of test_class)())
        Return True
    End Function
End Class
