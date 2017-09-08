
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

' Use attributes instead of inheritance to execute test case.
Partial Public NotInheritable Class case2
    Inherits [case]

    Private ReadOnly obj As Object
    Private ReadOnly _prepare As Func(Of Object, Boolean)
    Private ReadOnly _run As Func(Of Object, Boolean)
    Private ReadOnly _finish As Func(Of Object, Boolean)
    Private ReadOnly _reserved_processor_count As Int16

    Private Shared Function append_method_name(ByVal t As Type,
                                               ByVal method_name As String,
                                               ByVal id As Int32) As String
        assert(Not t Is Nothing)
        Dim base_name As String = Nothing
        If id = 0 Then
            base_name = t.FullName()
        ElseIf id = 1 Then
            base_name = t.AssemblyQualifiedName()
        Else
            assert(id = 2)
            base_name = t.Name()
        End If
        Return strcat(base_name, character.dot, method_name)
    End Function

    Private Sub New(ByVal t As Type,
                    ByVal method_name As String,
                    ByVal prepare As Func(Of Object, Boolean),
                    ByVal run As Func(Of Object, Boolean),
                    ByVal finish As Func(Of Object, Boolean),
                    ByVal reserved_processor_count As Int16)
        MyBase.New(append_method_name(t, method_name, 0),
                   append_method_name(t, method_name, 1),
                   append_method_name(t, method_name, 2))
        ' Allow Me.obj to be null: the test cases can be static methods.
        Me.obj = t.alloc()
        Me._prepare = prepare
        assert(Not run Is Nothing)
        Me._run = run
        Me._finish = finish
        Me._reserved_processor_count = reserved_processor_count
    End Sub

    Public Shared Function create(ByVal t As Type) As case2()
        assert(Not t Is Nothing)
        If t.has_custom_attribute(Of attributes.test)() Then
            Return New case2() {}
        Else
            Return Nothing
        End If
    End Function

    Private Function run_or_null(ByVal f As Func(Of Object, Boolean)) As Boolean
        Return f Is Nothing OrElse f(obj)
    End Function

    Public Overrides Function prepare() As Boolean
        Return MyBase.prepare() AndAlso
               run_or_null(_prepare)
    End Function

    Public Overrides Function run() As Boolean
        Return _run(obj)
    End Function

    Public Overrides Function finish() As Boolean
        Return run_or_null(_finish) And
               MyBase.finish()
    End Function

    Public Overrides Function reserved_processors() As Int16
        Return _reserved_processor_count
    End Function
End Class
