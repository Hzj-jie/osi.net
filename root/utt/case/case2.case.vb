
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Partial Public Class case2
    Private Class [case]
        Inherits utt.case

        Protected ReadOnly obj As Object
        Private ReadOnly _prepare As Func(Of Object, Boolean)
        Private ReadOnly _run As Func(Of Object, Boolean)
        Private ReadOnly _finish As Func(Of Object, Boolean)
        Private ReadOnly _reserved_processors As Int16

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
            If Not String.IsNullOrEmpty(method_name) Then
                Return strcat(base_name, character.dot, method_name)
            Else
                Return base_name
            End If
        End Function

        Public Sub New(ByVal t As Type,
                       ByVal method_name As String,
                       ByVal prepare As Func(Of Object, Boolean),
                       ByVal run As Func(Of Object, Boolean),
                       ByVal finish As Func(Of Object, Boolean),
                       ByVal reserved_processors As Int16)
            MyBase.New(append_method_name(t, method_name, 0),
                       append_method_name(t, method_name, 1),
                       append_method_name(t, method_name, 2))
            ' Allow Me.obj to be null: the test cases can be static methods.
            Me.obj = t.alloc()
            Me._prepare = prepare
            Me._run = run
            Me._finish = finish
            Me._reserved_processors = reserved_processors
        End Sub

        Private Function run_or_null(ByVal f As Func(Of Object, Boolean)) As Boolean
            Return f Is Nothing OrElse f(obj)
        End Function

        Public Overrides Function prepare() As Boolean
            Return MyBase.prepare() AndAlso
               run_or_null(_prepare)
        End Function

        Public Overrides Function run() As Boolean
            assert(Not _run Is Nothing)
            Return _run(obj)
        End Function

        Public Overrides Function finish() As Boolean
            Return run_or_null(_finish) And
               MyBase.finish()
        End Function

        Public Overrides Function reserved_processors() As Int16
            Return _reserved_processors
        End Function
    End Class
End Class
