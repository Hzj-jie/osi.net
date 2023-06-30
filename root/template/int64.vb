
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Public MustInherit Class _int64
    Inherits __do(Of Int64)
End Class

Public Class _N1
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return -1
    End Function
End Class

Public Class _NPOS
    Inherits _N1
End Class

Public Class _0
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 0
    End Function
End Class

Public Class _1
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 1
    End Function
End Class

Public Class _2
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 2
    End Function
End Class

Public Class _3
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 3
    End Function
End Class

Public Class _4
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 4
    End Function
End Class

Public Class _8
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 8
    End Function
End Class

Public Class _10
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 10
    End Function
End Class

Public Class _15
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 15
    End Function
End Class

Public Class _16
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 16
    End Function
End Class

Public Class _31
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 31
    End Function
End Class

Public Class _32
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 32
    End Function
End Class

Public Class _48
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 48
    End Function
End Class

Public Class _63
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 63
    End Function
End Class

Public Class _64
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 64
    End Function
End Class

Public Class _100
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 100
    End Function
End Class

Public Class _120
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 120
    End Function
End Class

Public Class _max_int8
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return max_int8
    End Function
End Class

Public Class _127
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 127
    End Function
End Class

Public Class _128
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 128
    End Function
End Class

Public Class _max_uint8
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return max_uint8
    End Function
End Class

Public Class _256
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 256
    End Function
End Class

Public Class _500
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 500
    End Function
End Class

Public Class _512
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 512
    End Function
End Class

Public Class _1000
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 1000
    End Function
End Class

Public Class _1023
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 1023
    End Function
End Class

Public Class _1024
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 1024
    End Function
End Class

Public Class _1025
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 1025
    End Function
End Class

Public Class _1200
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 1200
    End Function
End Class

Public Class _1500
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 1500
    End Function
End Class

Public Class _2048
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 2048
    End Function
End Class

Public Class _3371
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 3371
    End Function
End Class

Public Class _3600
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 3600
    End Function
End Class

Public Class _4096
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 4096
    End Function
End Class

Public Class _8192
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 8192
    End Function
End Class

Public Class _10000
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 10000
    End Function
End Class

Public Class _16381
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 16381
    End Function
End Class

Public Class _16383
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 16383
    End Function
End Class

Public Class _max_int16
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return max_int16
    End Function
End Class

Public Class _max_uint16
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return max_uint16
    End Function
End Class

Public Class _99991
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 99991
    End Function
End Class

Public Class _131071
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 131071
    End Function
End Class

Public Class _500000
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 500000
    End Function
End Class

Public Class _1000000
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 1000000
    End Function
End Class

Public Class _5000000
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 5000000
    End Function
End Class

Public Class _10000000
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 10000000
    End Function
End Class

Public Class _50000000
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 50000000
    End Function
End Class

Public Class _134217728
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return 134217728
    End Function
End Class

Public Class _max_int64
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return max_int64
    End Function
End Class
