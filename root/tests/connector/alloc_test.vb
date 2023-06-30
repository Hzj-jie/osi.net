
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.lock
Imports osi.root.utils

Public Class alloc_test
    Inherits [case]

    Private Const base_class_msg As String = "base_class"
    Private Const inherit_class_msg As String = "inherit_class"

    Private Class simple_class
        Public Sub New()
        End Sub
    End Class

    Private Class base_class
        Public Sub New(ByVal a As String, ByVal b As String, ByVal c As Int32)
            assertion.is_true(a Is Nothing)
            assertion.is_true(b Is Nothing)
            assertion.is_true(c = 0)
        End Sub

        Public Overridable Function msg() As String
            Return base_class_msg
        End Function
    End Class

    Private Class inherit_class
        Inherits base_class

        Public Sub New()
            MyBase.New(Nothing, Nothing, Nothing)
        End Sub

        Public Overrides Function msg() As String
            Return inherit_class_msg
        End Function
    End Class

    Private Class private_constructor_class
        Inherits base_class

        Private Sub New(ByVal a As String, ByVal b As String, ByVal c As Int32)
            MyBase.New(a, b, c)
        End Sub
    End Class

    Private Class exception_constructor_class
        Public Shared throwed As atomic_int

        Shared Sub New()
            throwed = New atomic_int()
        End Sub

        Public Sub New()
            throwed.increment()
            Throw New Exception("just an exception")
        End Sub
    End Class

    Private Shared Function case1() As Boolean
        Dim o As Object = Nothing
        o = allocate(Nothing)
        assertion.is_true(o Is Nothing)
        Return True
    End Function

    Private Shared Function case2() As Boolean
        Dim a() As base_class = Nothing
        a = allocate(GetType(base_class()))
        assertion.is_true(a Is Nothing)
        assertion.is_true(isemptyarray(a))
        a = alloc(Of base_class())()
        assertion.is_true(a Is Nothing)
        assertion.is_true(isemptyarray(a))
        Return True
    End Function

    Private Shared Function case3() As Boolean
        Dim i As Int32 = 0
        i = allocate(GetType(SByte))
        assertion.is_true(i = 0)
        i = allocate(GetType(Byte))
        assertion.is_true(i = 0)
        i = allocate(GetType(Int16))
        assertion.is_true(i = 0)
        i = allocate(GetType(UInt16))
        assertion.is_true(i = 0)
        i = allocate(GetType(Int32))
        assertion.is_true(i = 0)
        i = allocate(GetType(UInt32))
        assertion.is_true(i = 0)
        i = allocate(GetType(Int64))
        assertion.is_true(i = 0)
        i = allocate(GetType(UInt64))
        assertion.is_true(i = 0)

        i = alloc(Of SByte)()
        assertion.is_true(i = 0)
        i = alloc(Of Byte)()
        assertion.is_true(i = 0)
        i = alloc(Of Int16)()
        assertion.is_true(i = 0)
        i = alloc(Of UInt16)()
        assertion.is_true(i = 0)
        i = alloc(Of Int32)()
        assertion.is_true(i = 0)
        i = alloc(Of UInt32)()
        assertion.is_true(i = 0)
        i = alloc(Of Int64)()
        assertion.is_true(i = 0)
        i = alloc(Of UInt64)()
        assertion.is_true(i = 0)

        Return True
    End Function

    Private Shared Function case4() As Boolean
        Dim o1 As simple_class = Nothing
        o1 = allocate(GetType(simple_class))
        assertion.is_true(Not o1 Is Nothing)
        assertion.is_true(TypeOf o1 Is simple_class)
        Dim o2 As simple_class = Nothing
        o2 = alloc(Of simple_class)()
        assertion.is_true(Not o2 Is Nothing)
        assertion.is_true(TypeOf o2 Is simple_class)
        Dim o3 As simple_class = Nothing
        o3 = allocate_instance_of(o3)
        assertion.is_true(Not o3 Is Nothing)
        assertion.is_true(TypeOf o3 Is simple_class)
        Dim o4 As simple_class = Nothing
        o4 = allocate(Of simple_class)(GetType(simple_class))
        assertion.is_true(Not o4 Is Nothing)
        assertion.is_true(TypeOf o4 Is simple_class)

        assertion.not_equal(object_compare(o1, o2), 0)
        assertion.not_equal(object_compare(o1, o3), 0)
        assertion.not_equal(object_compare(o1, o4), 0)
        assertion.not_equal(object_compare(o2, o3), 0)
        assertion.not_equal(object_compare(o2, o4), 0)
        assertion.not_equal(object_compare(o3, o4), 0)

        Return True
    End Function

    Private Shared Function case5() As Boolean
        Dim o1 As base_class = Nothing
        o1 = allocate(GetType(base_class))
        assertion.is_true(Not o1 Is Nothing)
        assertion.is_true(TypeOf o1 Is base_class)
        Dim o2 As base_class = Nothing
        o2 = alloc(Of base_class)()
        assertion.is_true(Not o2 Is Nothing)
        assertion.is_true(TypeOf o2 Is base_class)
        Dim o3 As base_class = Nothing
        o3 = allocate_instance_of(o3)
        assertion.is_true(Not o3 Is Nothing)
        assertion.is_true(TypeOf o3 Is base_class)
        Dim o4 As base_class = Nothing
        o4 = allocate(Of base_class)(GetType(base_class))
        assertion.is_true(Not o4 Is Nothing)
        assertion.is_true(TypeOf o4 Is base_class)

        assertion.not_equal(object_compare(o1, o2), 0)
        assertion.not_equal(object_compare(o1, o3), 0)
        assertion.not_equal(object_compare(o1, o4), 0)
        assertion.not_equal(object_compare(o2, o3), 0)
        assertion.not_equal(object_compare(o2, o4), 0)
        assertion.not_equal(object_compare(o3, o4), 0)

        Return True
    End Function

    Private Shared Function case6() As Boolean
        Dim o1 As inherit_class = Nothing
        o1 = allocate(GetType(inherit_class))
        assertion.is_true(Not o1 Is Nothing)
        assertion.is_true(TypeOf o1 Is inherit_class)
        Dim o2 As inherit_class = Nothing
        o2 = alloc(Of inherit_class)()
        assertion.is_true(Not o2 Is Nothing)
        assertion.is_true(TypeOf o2 Is inherit_class)
        Dim o3 As inherit_class = Nothing
        o3 = allocate_instance_of(o3)
        assertion.is_true(Not o3 Is Nothing)
        assertion.is_true(TypeOf o3 Is inherit_class)
        Dim o4 As inherit_class = Nothing
        o4 = allocate(Of inherit_class)(GetType(inherit_class))
        assertion.is_true(Not o4 Is Nothing)
        assertion.is_true(TypeOf o4 Is inherit_class)

        assertion.not_equal(object_compare(o1, o2), 0)
        assertion.not_equal(object_compare(o1, o3), 0)
        assertion.not_equal(object_compare(o1, o4), 0)
        assertion.not_equal(object_compare(o2, o3), 0)
        assertion.not_equal(object_compare(o2, o4), 0)
        assertion.not_equal(object_compare(o3, o4), 0)

        Dim o5 As inherit_class = Nothing
        o5 = allocate_instance_of(New inherit_class())
        assertion.is_true(Not o5 Is Nothing)
        assertion.is_true(TypeOf o5 Is inherit_class)

        Return True
    End Function

    Private Shared Function case7() As Boolean
        Dim o1 As private_constructor_class = Nothing
        o1 = allocate(GetType(private_constructor_class))
        assertion.is_true(Not o1 Is Nothing)
        assertion.is_true(TypeOf o1 Is private_constructor_class)
        Dim o2 As private_constructor_class = Nothing
        o2 = alloc(Of private_constructor_class)()
        assertion.is_true(Not o2 Is Nothing)
        assertion.is_true(TypeOf o2 Is private_constructor_class)
        Dim o3 As private_constructor_class = Nothing
        o3 = allocate_instance_of(o3)
        assertion.is_true(Not o3 Is Nothing)
        assertion.is_true(TypeOf o3 Is private_constructor_class)
        Dim o4 As private_constructor_class = Nothing
        o4 = allocate(Of private_constructor_class)(GetType(private_constructor_class))
        assertion.is_true(Not o4 Is Nothing)
        assertion.is_true(TypeOf o4 Is private_constructor_class)

        assertion.not_equal(object_compare(o1, o2), 0)
        assertion.not_equal(object_compare(o1, o3), 0)
        assertion.not_equal(object_compare(o1, o4), 0)
        assertion.not_equal(object_compare(o2, o3), 0)
        assertion.not_equal(object_compare(o2, o4), 0)
        assertion.not_equal(object_compare(o3, o4), 0)

        Dim o5 As private_constructor_class = Nothing
        o5 = allocate_instance_of(o1)
        assertion.is_true(Not o5 Is Nothing)
        assertion.is_true(TypeOf o5 Is private_constructor_class)

        Return True
    End Function

    Private Shared Function case8() As Boolean
        exception_constructor_class.throwed.set(0)
        Dim o1 As exception_constructor_class = Nothing
        o1 = allocate(GetType(exception_constructor_class))
        assertion.is_true(o1 Is Nothing)
        Dim o2 As exception_constructor_class = Nothing
        o2 = alloc(Of exception_constructor_class)()
        assertion.is_true(o2 Is Nothing)
        Dim o3 As exception_constructor_class = Nothing
        o3 = allocate_instance_of(o3)
        assertion.is_true(o3 Is Nothing)
        Dim o4 As exception_constructor_class = Nothing
        o4 = allocate(Of exception_constructor_class)(GetType(exception_constructor_class))
        assertion.is_true(o4 Is Nothing)

        'each alloc will call it twice at least, except for alloc(Of exception_constructor_class) in cache mode
        'while alloc(o3) will have one extra call
        'if not using cached mode, alloc(Of exception_constructor_class) will throw 3 times
        assertion.equal(+exception_constructor_class.throwed, 4 * 2 + 2 - If(use_cached_alloc(), 2, 0))

        Return True
    End Function

    Private Shared Function case9() As Boolean
        Dim o As Action = Nothing
        o = allocate(GetType(Action))
        assertion.is_null(o)
        o = alloc(Of Action)()
        assertion.is_null(o)
        Return True
    End Function

    Private Shared Function case10() As Boolean
        assertion.is_null(allocate(GetType(_alloc)))
        Return True
    End Function

    Public Overrides Function prepare() As Boolean
        If MyBase.prepare() Then
            suppress.alloc_error.inc()
            Return True
        Else
            Return False
        End If
    End Function

    Public Overrides Function run() As Boolean
        Return case1() AndAlso
               case2() AndAlso
               case3() AndAlso
               case4() AndAlso
               case5() AndAlso
               case6() AndAlso
               case7() AndAlso
               case8() AndAlso
               case9() AndAlso
               case10()
    End Function

    Public Overrides Function finish() As Boolean
        suppress.alloc_error.dec()
        Return MyBase.finish()
    End Function
End Class
