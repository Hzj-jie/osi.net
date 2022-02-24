
Imports System.Threading
Imports osi.root.constants
Imports osi.root.utt
Imports osi.root.utils
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.delegates
Imports envs = osi.root.envs

Public Class finalizer_test
    Inherits [case]

    Private Const test_size As Int32 = 100000000
    Private Const magic_number As Int32 = signature_32
    Private Const size As Int32 = 128

    Shared Sub New()
        assert(magic_number <> DirectCast(Nothing, Int32))
    End Sub

    Private MustInherit Class tc(Of T)
        Protected ReadOnly content As T = Nothing
        Protected ReadOnly p As atomic_int = Nothing

        <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")>
        Protected Sub New(ByVal p As atomic_int)
            Me.content = initialize()
            assert(Not p Is Nothing)
            Me.p = p
        End Sub

        Protected MustOverride Function initialize() As T
        Protected MustOverride Function compare() As Boolean

        Protected Overrides Sub Finalize()
            GC.KeepAlive(p)
            MyBase.Finalize()
        End Sub
    End Class

    Private Class tc_array
        Inherits tc(Of Int32())

        Public Sub New(ByVal p As atomic_int)
            MyBase.New(p)
        End Sub

        Protected Overrides Function initialize() As Int32()
            Dim rtn() As Int32 = Nothing
            ReDim rtn(size - 1)
            rtn(0) = magic_number
            Return rtn
        End Function

        Protected Overrides Function compare() As Boolean
            Try
                Return array_size(content) = size AndAlso content(0) = magic_number
            Catch
                Return False
            End Try
        End Function

        Protected Overrides Sub Finalize()
            If Not compare() Then
                p.increment()
            End If
            MyBase.Finalize()
        End Sub
    End Class

    Private Class tc_array_keepalive
        Inherits tc_array

        Public Sub New(ByVal p As atomic_int)
            MyBase.New(p)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            GC.KeepAlive(content)
        End Sub
    End Class

    Private Sub run_case(Of T)(ByVal ctor As _do(Of atomic_int, T), ByVal validate As void(Of atomic_int))
        assert(Not ctor Is Nothing)
        assert(Not validate Is Nothing)
        Dim f As atomic_int = Nothing
        f = New atomic_int(0)
        For i As Int32 = 0 To test_size - 1
            Dim o As T = Nothing
            o = ctor(f)
            If envs.mono Then
                o = Nothing
                garbage_collector.repeat_collect()
            End If
        Next
        validate(f)
    End Sub

    Public Overrides Function run() As Boolean
        run_case(Function(ByRef f) New tc_array(f),
                 Sub(ByRef f As atomic_int)
                     assert(Not f Is Nothing)
                     assertion.more_or_equal(+f, 0)            'never fail
                     assertion.less_or_equal(+f, test_size)
                 End Sub)
        run_case(Function(ByRef f) New tc_array_keepalive(f),
                 Sub(ByRef f As atomic_int)
                     assert(Not f Is Nothing)
                     assertion.equal(+f, 0)
                 End Sub)

        Return True
    End Function
End Class
