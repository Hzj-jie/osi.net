
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utils

Public Class time_test
    Inherits [case]

    Private Shared Function equal(ByVal d As Date,
                                  ByVal exp As Date,
                                  ByVal _1 As Func(Of Date, Date),
                                  ByVal _2 As Func(Of Int64, Int64)) As Boolean
        assert(Not _1 Is Nothing)
        assert(Not _2 Is Nothing)
        assertion.equal(_1(d), exp)
        assertion.equal(_2(d.Ticks()), exp.Ticks())
        Return True
    End Function

    Private Shared Function this() As Boolean
        Const year As Int32 = 2001
        Const month As Int32 = 2
        Const day As Int32 = 3
        Const hour As Int32 = 4
        Const minute As Int32 = 5
        Const second As Int32 = 6
        Const milliseconds As Int32 = 7

        Dim d As Date = Nothing
        d = New Date(year, month, day, hour, minute, second, milliseconds)
        Return equal(d,
                     New Date(year, month, day, hour, minute, second, milliseconds),
                     AddressOf thismillisecond,
                     AddressOf thismillisecond) AndAlso
               equal(d,
                     New Date(year, month, day, hour, minute, second, 0),
                     AddressOf thissecond,
                     AddressOf thissecond) AndAlso
               equal(d,
                     New Date(year, month, day, hour, minute, 0),
                     AddressOf thisminute,
                     AddressOf thisminute) AndAlso
               equal(d,
                     New Date(year, month, day, hour, 0, 0),
                     AddressOf thishour,
                     AddressOf thishour) AndAlso
               equal(d,
                     New Date(year, month, day, 0, 0, 0),
                     AddressOf thisday,
                     AddressOf thisday) AndAlso
               equal(d,
                     New Date(year, month, 1, 0, 0, 0),
                     AddressOf thismonth,
                     AddressOf thismonth) AndAlso
               equal(d,
                     New Date(year, 1, 1, 0, 0, 0, 0),
                     AddressOf thisyear,
                     AddressOf thisyear)
    End Function

    Public Overrides Function run() As Boolean
        Return this()
    End Function
End Class
