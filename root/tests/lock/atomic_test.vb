
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.lock
Imports osi.root.utt

Public Class atomic_test
    Inherits chained_case_wrapper

    Private Shared ReadOnly thread_count As Int32
    Private Shared ReadOnly repeat_times As Int32
    Private Shared ReadOnly run_times As Int32

    Shared Sub New()
        thread_count = (Environment.ProcessorCount() << 2)
        repeat_times = (1024 * 1024)
        run_times = thread_count * repeat_times
    End Sub

    Private Shared Function create(Of T As {New, [case]})() As [case]
        Return multithreading(repeat(New T(), repeat_times), thread_count)
    End Function

    Public Sub New()
        MyBase.New(create(Of int_case)(),
                   create(Of long_case)(),
                   create(Of double_case)(),
                   create(Of object_case)(),
                   create(Of string_case)(),
                   create(Of structure_case)(),
                   create(Of atomic_int_case)(),
                   create(Of atomic_long_case)(),
                   create(Of atomic_double_case)(),
                   create(Of atomic_object_case)(),
                   create(Of atomic_string_case)())
    End Sub

    Private MustInherit Class atomic_case(Of T)
        Inherits [case]

        Private ReadOnly sc As atomic_int
        Private ReadOnly dc As atomic_int
        Private ReadOnly exp_sc_min As Int32
        Private ReadOnly exp_sc_max As Int32
        Private ReadOnly exp_dc_min As Int32
        Private ReadOnly exp_dc_max As Int32
        Private b As Boolean
        Protected MustOverride Function first() As T
        Protected MustOverride Function second() As T
        Protected MustOverride Sub write(ByVal v As T)
        Protected MustOverride Function read() As T
        Protected MustOverride Function same(ByVal v1 As T, ByVal v2 As T) As Boolean

        Protected Sub New(ByVal exp_sc_min As Int32,
                          ByVal exp_sc_max As Int32,
                          ByVal exp_dc_min As Int32,
                          ByVal exp_dc_max As Int32)
            Me.sc = New atomic_int()
            Me.dc = New atomic_int()
            Me.exp_sc_min = exp_sc_min
            Me.exp_sc_max = exp_sc_max
            Me.exp_dc_min = exp_dc_min
            Me.exp_dc_max = exp_dc_max
        End Sub

        Protected Sub New(ByVal exp_sc As Int32, ByVal exp_dc As Int32)
            Me.New(exp_sc, exp_sc, exp_dc, exp_dc)
        End Sub

        Protected Sub New()
            Me.New(thread_count * repeat_times, 0)
        End Sub

        Protected Sub New(ByVal atomic As Boolean)
            Me.New(If(atomic, run_times, 0),
                   If(atomic, run_times, run_times),
                   If(atomic, 0, 0),
                   If(atomic, 0, run_times))
        End Sub

        Public Overrides Function run() As Boolean
            b = Not b
            write(If(b, first(), second()))
            Dim v As T = Nothing
            v = read()
            If same(v, first()) OrElse same(v, second()) Then
                sc.increment()
            Else
                dc.increment()
            End If
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            'the case has been run
            If (+sc) > 0 OrElse (+dc) > 0 Then
                assertion.more_or_equal_and_less_or_equal(+sc, exp_sc_min, exp_sc_max, "same count of ", name)
                assertion.more_or_equal_and_less_or_equal(+dc, exp_dc_min, exp_dc_max, "difference count of ", name)
            End If
            Return MyBase.finish()
        End Function
    End Class

    Private Class int_case
        Inherits atomic_case(Of Int32)

        Protected v As Int32

        Public Sub New()
            MyBase.New()
        End Sub

        Protected Overrides Function first() As Int32
            Return -1
        End Function

        Protected Overrides Function read() As Int32
            Return v
        End Function

        Protected Overrides Function same(ByVal v1 As Int32, ByVal v2 As Int32) As Boolean
            Return v1 = v2
        End Function

        Protected Overrides Function second() As Int32
            Return 0
        End Function

        Protected Overrides Sub write(ByVal v As Int32)
            Me.v = v
        End Sub
    End Class

    Private Class atomic_int_case
        Inherits int_case

        Protected Overrides Function read() As Int32
            Return atomic.read(v)
        End Function

        Protected Overrides Sub write(ByVal v As Int32)
            atomic.eva(Me.v, v)
        End Sub
    End Class

    Private Class long_case
        Inherits atomic_case(Of Int64)

        Protected v As Int64

        Public Sub New()
            MyBase.New(x64_cpu)
        End Sub

        Protected Sub New(ByVal atomic As Boolean)
            MyBase.New(atomic)
        End Sub

        Protected Overrides Function first() As Int64
            Return -1
        End Function

        Protected Overrides Function read() As Int64
            Return v
        End Function

        Protected Overrides Function same(ByVal v1 As Int64, ByVal v2 As Int64) As Boolean
            Return v1 = v2
        End Function

        Protected Overrides Function second() As Int64
            Return 0
        End Function

        Protected Overrides Sub write(ByVal v As Int64)
            Me.v = v
        End Sub
    End Class

    Private Class atomic_long_case
        Inherits long_case

        Public Sub New()
            MyBase.New(True)
        End Sub

        Protected Overrides Function read() As Int64
            Return atomic.read(v)
        End Function

        Protected Overrides Sub write(ByVal v As Int64)
            atomic.eva(Me.v, v)
        End Sub
    End Class

    Private Class double_case
        Inherits atomic_case(Of Double)

        Protected v As Double

        Public Sub New()
            MyBase.New()
        End Sub

        Protected Overrides Function first() As Double
            Return -4.42330604244772E-305
        End Function

        Protected Overrides Function read() As Double
            Return v
        End Function

        Protected Overrides Function same(ByVal v1 As Double, ByVal v2 As Double) As Boolean
            Return v1 = v2
        End Function

        Protected Overrides Function second() As Double
            Return 0D
        End Function

        Protected Overrides Sub write(ByVal v As Double)
            Me.v = v
        End Sub
    End Class

    Private Class atomic_double_case
        Inherits double_case

        Protected Overrides Function read() As Double
            Return atomic.read(v)
        End Function

        Protected Overrides Sub write(ByVal v As Double)
            atomic.eva(Me.v, v)
        End Sub
    End Class

    Private Class object_case
        Inherits atomic_case(Of Object)

        Private Shared ReadOnly v1 As Object
        Private Shared ReadOnly v2 As Object
        Protected v As Object

        Shared Sub New()
            v1 = New Object()
            v2 = New Object()
        End Sub

        Public Sub New()
            MyBase.New()
        End Sub

        Protected Overrides Function first() As Object
            Return v1
        End Function

        Protected Overrides Function read() As Object
            Return v
        End Function

        Protected Overrides Function same(ByVal v1 As Object, ByVal v2 As Object) As Boolean
            Return object_compare(v1, v2) = 0
        End Function

        Protected Overrides Function second() As Object
            Return v2
        End Function

        Protected Overrides Sub write(ByVal v As Object)
            Me.v = v
        End Sub
    End Class

    Private Class atomic_object_case
        Inherits object_case

        Protected Overrides Function read() As Object
            Return atomic.read(v)
        End Function

        Protected Overrides Sub write(ByVal v As Object)
            atomic.eva(Me.v, v)
        End Sub
    End Class

    Private Class string_case
        Inherits atomic_case(Of String)

        Protected v As String

        Public Sub New()
            MyBase.New()
        End Sub

        Protected Overrides Function first() As String
            Return "abc"
        End Function

        Protected Overrides Function read() As String
            Return v
        End Function

        Protected Overrides Function same(ByVal v1 As String, ByVal v2 As String) As Boolean
            Return strsame(v1, v2)
        End Function

        Protected Overrides Function second() As String
            Return "bcd"
        End Function

        Protected Overrides Sub write(ByVal v As String)
            copy(Me.v, v)
        End Sub
    End Class

    Private Class atomic_string_case
        Inherits string_case

        Protected Overrides Function read() As String
            Return atomic.read(v)
        End Function

        Protected Overrides Sub write(ByVal v As String)
            atomic.eva(Me.v, v)
        End Sub
    End Class

    Private Class structure_case
        Inherits atomic_case(Of s)

        Public Structure s
            Dim i As Int64
            Dim j As Byte
            Dim k As Boolean

            Public Shared Operator =(ByVal this As s, ByVal that As s) As Boolean
                Return this.i = that.i AndAlso this.j = that.j AndAlso this.k = that.k
            End Operator

            Public Shared Operator <>(ByVal this As s, ByVal that As s) As Boolean
                Return Not (this = that)
            End Operator
        End Structure

        Private Shared ReadOnly v1 As s
        Private Shared ReadOnly v2 As s
        Private v As s

        Public Sub New()
            MyBase.New(False)
        End Sub

        Shared Sub New()
            v1 = New s() With {.i = 0, .j = max_uint8, .k = False}
            v2 = New s() With {.i = -1, .j = 0, .k = True}
        End Sub

        Protected Overrides Function first() As s
            Return v1
        End Function

        Protected Overrides Function read() As s
            Return v
        End Function

        Protected Overrides Function same(ByVal v1 As s, ByVal v2 As s) As Boolean
            Return v1 = v2
        End Function

        Protected Overrides Function second() As s
            Return v2
        End Function

        Protected Overrides Sub write(ByVal v As s)
            Me.v = v
        End Sub
    End Class
End Class
