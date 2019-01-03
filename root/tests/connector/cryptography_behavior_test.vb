
Imports System.Security.Cryptography
Imports System.Threading
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt

' Conclusion, thread unsafe, and create performance is ~30x - 60x worse than thread static. So using thread static is a
' good choice.
Public Class cryptography_behavior_test
    Inherits chained_case_wrapper

    'Unfortunately, this test won't pass, HashAlgorithms are not implemented thread-safe.
#If 0 Then
    Private Class thread_safety_case
        Inherits [case]

        Private ReadOnly size As UInt32
        Private ReadOnly thread_count As UInt32
        Private ReadOnly h As HashAlgorithm
        Private ReadOnly type_name As String
        Private i()() As Byte = Nothing
        Private o()() As Byte = Nothing

        Private Sub New(ByVal h As HashAlgorithm, ByVal size As UInt32, ByVal thread_count As UInt32)
            assert(Not h Is Nothing)
            assert(size > uint32_0)
            assert(thread_count > uint32_1)
            Me.h = h
            Me.type_name = h.GetType().Name()
            Me.size = size
            Me.thread_count = thread_count
        End Sub

        Public Overrides Function prepare() As Boolean
            If MyBase.prepare() Then
                ReDim i(size - uint32_1)
                ReDim o(size - uint32_1)
                For j As UInt32 = uint32_0 To size - uint32_1
                    i(j) = rnd_bytes(rnd_uint(1024, 2048))
                    o(j) = h.ComputeHash(i(j))
                Next
                Return True
            Else
                Return False
            End If
        End Function

        Public Overrides Function run() As Boolean
            Dim id As Int32 = 0
            id = multithreading_case_wrapper.thread_id()
            assert(id >= 0)
            Dim t As UInt32 = uint32_0
            If id = thread_count - uint32_1 Then
                t = size
            Else
                t = (id + uint32_1) * (size \ thread_count)
            End If
            For l As UInt32 = id * (size \ thread_count) To t - uint32_1
                assertion.array_equal(o(l), h.ComputeHash(i(l)), type_name)
            Next
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            Erase i
            Erase o
            Return MyBase.finish()
        End Function

        Protected Overrides Sub Finalize()
            h.Clear()
            MyBase.Finalize()
        End Sub

        Private Shared Function create(ByVal h As HashAlgorithm) As [case]
            Return multithreading(New thread_safety_case(h, 10000, 8), 8)
        End Function

        Public Shared Function cases() As [case]()
            Return {
                create(MD5.Create()),
                create(RIPEMD160.Create()),
                create(SHA1.Create()),
                create(SHA256.Create()),
                create(SHA384.Create()),
                create(SHA512.Create())
            }
        End Function
    End Class
#End If

    Private Class create_perf_case
        Inherits [case]

        Private ReadOnly f As Func(Of HashAlgorithm)

        Private Sub New(ByVal f As Func(Of HashAlgorithm))
            assert(Not f Is Nothing)
            Me.f = f
        End Sub

        Public Overrides Function run() As Boolean
            Dim h As HashAlgorithm = Nothing
            h = f()
            Return True
        End Function

        Private Shared Function create(ByVal f As Func(Of HashAlgorithm)) As [case]
            Return performance(repeat(New create_perf_case(f), 1000000))
        End Function

        Public Shared Function cases() As [case]()
            Return {
                create(AddressOf MD5.Create),
                create(AddressOf RIPEMD160.Create),
                create(AddressOf SHA1.Create),
                create(AddressOf SHA256.Create),
                create(AddressOf SHA384.Create),
                create(AddressOf SHA512.Create)
            }
        End Function
    End Class

    Private Class reusable_case
        Inherits [case]

        Private ReadOnly h As HashAlgorithm
        Private b() As Byte
        Private r() As Byte

        Private Sub New(ByVal h As HashAlgorithm)
            assert(Not h Is Nothing)
            Me.h = h
        End Sub

        Public Overrides Function prepare() As Boolean
            If MyBase.prepare() Then
                b = rnd_bytes(rnd_uint(1024, 2048))
                r = h.ComputeHash(b)
                Return True
            Else
                Return False
            End If
        End Function

        Public Overrides Function run() As Boolean
            assertion.array_equal(r, h.ComputeHash(b))
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            Erase b
            Erase r
            Return MyBase.finish()
        End Function

        Protected Overrides Sub Finalize()
            h.Clear()
            MyBase.Finalize()
        End Sub

        Private Shared Function create(ByVal h As HashAlgorithm) As [case]
            Return repeat(New reusable_case(h), 1000)
        End Function

        Public Shared Function cases() As [case]()
            Return {
                create(MD5.Create()),
                create(RIPEMD160.Create()),
                create(SHA1.Create()),
                create(SHA256.Create()),
                create(SHA384.Create()),
                create(SHA512.Create())
            }
        End Function
    End Class

    Private Class keyed_reusable_case
        Inherits [case]

        Private ReadOnly h As KeyedHashAlgorithm
        Private k() As Byte
        Private b() As Byte
        Private r() As Byte

        Private Sub New(ByVal h As KeyedHashAlgorithm)
            assert(Not h Is Nothing)
            Me.h = h
        End Sub

        Public Overrides Function prepare() As Boolean
            If MyBase.prepare() Then
                k = rnd_bytes(rnd_uint(100, 200))
                b = rnd_bytes(rnd_uint(1024, 2048))
                h.Key() = k
                r = h.ComputeHash(b)
                Return True
            Else
                Return False
            End If
        End Function

        Public Overrides Function run() As Boolean
            h.Key() = rnd_bytes(rnd_uint(100, 200))
            h.Key() = k
            assertion.array_equal(r, h.ComputeHash(b))
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            Erase k
            Erase b
            Erase r
            Return MyBase.finish()
        End Function

        Protected Overrides Sub Finalize()
            h.Clear()
            MyBase.Finalize()
        End Sub

        Private Shared Function create(ByVal h As KeyedHashAlgorithm) As [case]
            Return repeat(New keyed_reusable_case(h), 1000)
        End Function

        Public Shared Function cases() As [case]()
            Return {
                create(MACTripleDES.Create()),
                create(HMACMD5.Create()),
                create(HMACRIPEMD160.Create()),
                create(HMACSHA1.Create()),
                create(HMACSHA256.Create()),
                create(HMACSHA384.Create()),
                create(HMACSHA512.Create())
            }
        End Function
    End Class

    Public Sub New()
        MyBase.New(array_concat(create_perf_case.cases(),
                                reusable_case.cases(),
                                keyed_reusable_case.cases()))
    End Sub
End Class
