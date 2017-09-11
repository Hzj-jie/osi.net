
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utt
Imports osi.tests.root.connector

Public Class equal_test
    Inherits [case]

    Private Shared Function basic_cases() As Boolean
        assert_true(equal(1, 1))
        assert_false(equal(1, 10))
        assert_true(equal(True, True))
        assert_true(equal([default](Of String).null, [default](Of String).null))
        assert_true(equal([default](Of Object).null, [default](Of String).null))
        assert_true(equal(1, 1.0))
        Return True
    End Function

    Private Class test_class
        Implements IEquatable(Of test_class)

        Public ReadOnly v As Int32

        Public Sub New(ByVal v As Int32)
            Me.v = v
        End Sub

        Public Overloads Function Equals(ByVal other As test_class) As Boolean _
                                        Implements IEquatable(Of test_class).Equals
            If other Is Nothing Then
                Return False
            Else
                Return v = other.v
            End If
        End Function
    End Class

    Private Class test_class2
        Implements IEquatable(Of Object), IEquatable(Of test_class)

        Public ReadOnly v As Int32

        Public Sub New(ByVal v As Int32)
            Me.v = v
        End Sub

        Public Overloads Function Equals(ByVal other As test_class) As Boolean Implements IEquatable(Of test_class).Equals
            If other Is Nothing Then
                Return False
            Else
                Return v = other.v
            End If
        End Function

        Public Overloads Function Equals(ByVal obj As Object) As Boolean Implements IEquatable(Of Object).Equals
            If TypeOf obj Is test_class2 Then
                Return v = direct_cast(Of test_class2)(obj).v
            ElseIf TypeOf obj Is test_class Then
                Return v = direct_cast(Of test_class)(obj).v
            Else
                Return False
            End If
        End Function
    End Class

    Private Shared Function iequality_case() As Boolean
        assert_true(equal(New test_class(1), New test_class(1)))
        assert_false(equal(New test_class(10), New test_class(2)))
        assert_false(equal(Of test_class, test_class)(New test_class(10), Nothing))
        assert_true(equal(New test_class(10), New test_class2(10)))
        assert_false(equal(New test_class(10), New test_class2(101)))
        assert_true(equal(New test_class2(10), New test_class(10)))
        assert_false(equal(New test_class2(101), New test_class(10)))
        assert_true(equal(direct_cast(Of Object)(New test_class2(10)), New test_class(10)))
        assert_true(equal(direct_cast(Of Object)(New test_class2(10)), direct_cast(Of Object)(New test_class(10))))
        assert_true(equal(New test_class2(10), direct_cast(Of Object)(New test_class(10))))
        assert_false(equal(direct_cast(Of Object)(New test_class2(101)), New test_class(10)))
        assert_false(equal(direct_cast(Of Object)(New test_class2(101)), direct_cast(Of Object)(New test_class(10))))
        assert_false(equal(New test_class2(10), direct_cast(Of Object)(New test_class(101))))
        Return True
    End Function

    Private Class test_class3
        Private ReadOnly v As Int32

        Public Sub New(ByVal v As Int32)
            Me.v = v
        End Sub

        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            If TypeOf obj Is test_class3 Then
                Return v = direct_cast(Of test_class3)(obj).v
            Else
                Return False
            End If
        End Function
    End Class

    Private Shared Function object_equals_case() As Boolean
        assert_true(equal(New test_class3(10), New test_class3(10)))
        assert_false(equal(New test_class3(10), New test_class3(101)))
        Return True
    End Function

    Private Structure test_structure
        Public a As Int32
        Public b As Int32
        Public c As Boolean

        Public Sub New(ByVal a As Int32, ByVal b As Int32, ByVal c As Boolean)
            Me.a = a
            Me.b = b
            Me.c = c
        End Sub
    End Structure

    Private Shared Function value_type_equals_case() As Boolean
        assert_true(equal(New test_structure(1, 1, True), New test_structure(1, 1, True)))
        assert_true(equal(New test_structure(-1, -1, False), New test_structure(-1, -1, False)))
        assert_false(equal(New test_structure(-1, -1, False), New test_structure(-1, -1, True)))
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return basic_cases() AndAlso
               iequality_case() AndAlso
               object_equals_case() AndAlso
               value_type_equals_case()
    End Function
End Class
