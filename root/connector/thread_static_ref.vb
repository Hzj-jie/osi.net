
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.delegates

'For mono debug purpose, the perf is a little bit worse than .net framework TheadStatic attribute.
' Mono cannot work well with thread-static primary types. But this class uses a set function to make sure even for
' primary types, using this class in Mono is safe.
Public NotInheritable Class thread_static_ref(Of T As Class)
    Private ReadOnly v As thread_static(Of T)

    Public Sub New(ByVal size As Int32)
        v = New thread_static(Of T)(size)
    End Sub

    Public Sub New()
        v = New thread_static(Of T)()
    End Sub

    Public Function [get](ByRef o As T) As Boolean
        Return v.get(o)
    End Function

    Public Function [get]() As T
        Return v.get()
    End Function

    Public Function [set](ByVal i As T) As Boolean
        Return v.set(i)
    End Function

    Public Function or_set(ByVal i As T, ByRef o As T) As Boolean
        Return v.or_set(i, predicates.is_not_null(Of T)(), o)
    End Function

    Public Function or_set(ByVal i As T) As T
        Return v.or_set(i, predicates.is_not_null(Of T)())
    End Function

    Public Function or_set(ByVal i As Func(Of T), ByRef o As T) As Boolean
        Return v.or_set(i, predicates.is_not_null(Of T)(), o)
    End Function

    Public Function or_set(ByVal i As Func(Of T)) As T
        Return v.or_set(i, predicates.is_not_null(Of T)())
    End Function

    Public Function or_new(ByRef o As T) As Boolean
        Return or_set(Function() As T
                          Return alloc(Of T)()
                      End Function,
                      o)
    End Function

    Public Function or_new() As T
        Return or_set(Function() As T
                          Return alloc(Of T)()
                      End Function)
    End Function

    Public Property at() As T
        Get
            Return v.at()
        End Get
        Set(ByVal value As T)
            v.at() = value
        End Set
    End Property

    Public Sub clear()
        v.clear()
    End Sub

    Public Shared Operator +(ByVal this As thread_static_ref(Of T)) As T
        Dim that As thread_static(Of T) = Nothing
        that = If(this Is Nothing, Nothing, this.v)
        Return +that
    End Operator
End Class

