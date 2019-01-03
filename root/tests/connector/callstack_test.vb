
Imports osi.root.constants
Imports osi.root.utt
Imports osi.root.connector

Public Class callstack_test
    Inherits [case]

    Private Structure test_structure2
        <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Public Function func6(ByVal ignores() As String) As String
            Return backtrace(ignores)
        End Function
    End Structure

    Private Class test_class2
        <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Public Function func5(ByVal ignores() As String) As String
            Dim s As test_structure2 = Nothing
            Return s.func6(ignores)
        End Function
    End Class

    Private Structure test_structure1
        <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Public Shared Function func4(ByVal ignores() As String) As String
            Dim c As test_class2 = Nothing
            c = New test_class2()
            Return c.func5(ignores)
        End Function
    End Structure

    Private Class test_class1
        <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Public Shared Function func3(ByVal ignores() As String) As String
            Return test_structure1.func4(ignores)
        End Function
    End Class

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Private Shared Function func2(ByVal ignores() As String) As String
        Return test_class1.func3(ignores)
    End Function

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Private Shared Function func1(ByVal ignores() As String) As String
        Return func2(ignores)
    End Function

    Private Shared Function [case](ByVal ParamArray ignores() As String) As Boolean
        Dim s As String = Nothing
        s = func1(ignores)
        assertion.equal(strindexof(s, "func2"), npos)
        assertion.equal(strindexof(s, "func3"), npos)
        assertion.equal(strindexof(s, "func4"), npos)
        assertion.equal(strindexof(s, "func5"), npos)
        assertion.equal(strindexof(s, "func6"), npos)
        assertion.equal(strindexof(s, "test_class1"), npos)
        assertion.equal(strindexof(s, "test_structure1"), npos)
        assertion.equal(strindexof(s, "test_class2"), npos)
        assertion.equal(strindexof(s, "test_structure2"), npos)
        assertion.not_equal(strindexof(s, "func1"), npos)
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return [case]("func2", "func3", "func4", "func5", "func6") AndAlso
               [case]("func2", "test_class1", "test_class2", "test_structure1", "test_structure2")
    End Function
End Class
