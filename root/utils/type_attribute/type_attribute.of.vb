
Imports osi.root.connector

Partial Public Class type_attribute
    Private Shared Function [of](Of T)(ByVal check_only As Boolean,
                                       Optional ByRef o As type_attribute = Nothing) As Boolean
        static_constructor(Of T).execute()
        Return [of](check_only, GetType(T), o)
    End Function

    Private Shared Function [of](Of T As signal)(ByVal check_only As Boolean,
                                                 ByVal i As T,
                                                 Optional ByRef o As type_attribute = Nothing) As Boolean
        If i Is Nothing Then
            Return [of](Of T)(check_only, o)
        Else
            Return [of](check_only, DirectCast(i, signal), o)
        End If
    End Function

    Private Shared Function [of](ByVal check_only As Boolean,
                                 ByVal i As signal,
                                 Optional ByRef o As type_attribute = Nothing) As Boolean
        If i Is Nothing Then
            Return False
        Else
            o = i.attribute()
            assert(Not o Is Nothing)
            Return True
        End If
    End Function

    Private Shared Function [of](ByVal check_only As Boolean,
                                 ByVal t As Type,
                                 Optional ByRef o As type_attribute = Nothing) As Boolean
        If t.custom_attribute(o) Then
            assert(o.s Is Nothing)
            If check_only Then
                Return store.exist(t)
            Else
                o.s = store.get(t)
                assert(Not o.s Is Nothing)
                Return True
            End If
        Else
            Return False
        End If
    End Function

    Private Shared Function [of](ByVal check_only As Boolean,
                                 ByVal obj As Object,
                                 Optional ByRef o As type_attribute = Nothing) As Boolean
        If obj Is Nothing Then
            Return False
        Else
            Dim s As signal = Nothing
            s = TryCast(obj, signal)
            If s Is Nothing Then
                Return [of](check_only, obj.GetType(), o)
            Else
                Return [of](check_only, s, o)
            End If
        End If
    End Function

    Public Shared Function [of](Of T)(ByRef o As type_attribute) As Boolean
        Return [of](Of T)(False, o)
    End Function

    Public Shared Function [of](Of T As signal)(ByVal i As T, ByRef o As type_attribute) As Boolean
        Return [of](Of T)(False, i, o)
    End Function

    Public Shared Function [of](ByVal i As signal, ByRef o As type_attribute) As Boolean
        Return [of](False, i, o)
    End Function

    Public Shared Function [of](ByVal t As Type, ByRef o As type_attribute) As Boolean
        Return [of](False, t, o)
    End Function

    Public Shared Function [of](ByVal obj As Object, ByRef o As type_attribute) As Boolean
        Return [of](False, obj, o)
    End Function

    Public Shared Function [of](Of T)() As type_attribute
        Dim o As type_attribute = Nothing
        assert([of](Of T)(o))
        Return o
    End Function

    Public Shared Function [of](Of T As signal)(ByVal i As T) As type_attribute
        Dim o As type_attribute = Nothing
        assert([of](Of T)(i, o))
        Return o
    End Function

    Public Shared Function [of](ByVal i As signal) As type_attribute
        Dim o As type_attribute = Nothing
        assert([of](i, o))
        Return o
    End Function

    Public Shared Function [of](ByVal t As Type) As type_attribute
        Dim o As type_attribute = Nothing
        assert([of](t, o))
        Return o
    End Function

    Public Shared Function [of](ByVal obj As Object) As type_attribute
        Dim o As type_attribute = Nothing
        assert([of](obj, o))
        Return o
    End Function
End Class
