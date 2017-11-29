
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector

Public NotInheritable Class invokers(Of delegate_t)
    Private ReadOnly m() As invoker(Of delegate_t) = Nothing

    Private Sub New(ByVal t As Type,
                    ByVal bindingFlags As BindingFlags,
                    ByVal obj As Object,
                    ByVal names() As String,
                    ByVal ctor As Func(Of Type, BindingFlags, Object, String, invoker(Of delegate_t)))
        assert(Not ctor Is Nothing)
        If Not names Is Nothing AndAlso Not isemptyarray(names) Then
            ReDim m(array_size_i(names) - 1)
            For i As Int32 = 0 To array_size_i(names) - 1
                m(i) = ctor(t, bindingFlags, obj, names(i))
            Next
        End If
    End Sub

    Public Sub New(ByVal t As Type,
                   ByVal bindingFlags As BindingFlags,
                   ByVal obj As Object,
                   ByVal ParamArray names() As String)
        Me.New(t, bindingFlags, obj, names, Function(a, b, c, d) New invoker(Of delegate_t)(a, b, c, d))
    End Sub

    Public Sub New(ByVal t As Type, ByVal bindingFlags As BindingFlags, ByVal ParamArray names() As String)
        Me.New(t, bindingFlags, Nothing, names, Function(a, b, c, d) New invoker(Of delegate_t)(a, b, d))
    End Sub

    Public Sub New(ByVal t As Type, ByVal ParamArray names() As String)
        Me.New(t, Nothing, Nothing, names, Function(a, b, c, d) New invoker(Of delegate_t)(a, d))
    End Sub

    Public Sub New(ByVal t As Type, ByVal obj As Object, ByVal ParamArray names() As String)
        Me.New(t, Nothing, obj, names, Function(a, b, c, d) New invoker(Of delegate_t)(a, c, d))
    End Sub

    Public Sub New(ByVal obj As Object, ByVal ParamArray names() As String)
        Me.New(Nothing, Nothing, obj, names, Function(a, b, c, d) New invoker(Of delegate_t)(c, d))
    End Sub

    Public Sub New(ByVal obj As Object, ByVal bindingFlags As BindingFlags, ByVal ParamArray names() As String)
        Me.New(Nothing, bindingFlags, obj, names, Function(a, b, c, d) New invoker(Of delegate_t)(b, c, d))
    End Sub

    Public Sub New(ByVal bindingFlags As BindingFlags, ByVal obj As Object, ByVal ParamArray names() As String)
        Me.New(Nothing, bindingFlags, obj, names, Function(a, b, c, d) New invoker(Of delegate_t)(b, c, d))
    End Sub

    Default Public ReadOnly Property at(ByVal i As UInt32) As delegate_t
        Get
            Return +m(CInt(i))
        End Get
    End Property

    Public Function size() As UInt32
        Return array_size(m)
    End Function
End Class
