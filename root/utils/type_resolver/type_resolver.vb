
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports lock_t = osi.root.lock.slimlock.monitorlock

Partial Public NotInheritable Class type_resolver(Of T)
    Private ReadOnly s As storage
    Private l As lock_t

    Public Sub New()
        s = New storage()
    End Sub

    Public Function registered(ByVal type As Type) As Boolean
        l.wait()
        Try
            Return s.has(type)
        Finally
            l.release()
        End Try
        l.release()
    End Function

    Public Sub register(ByVal type As Type, ByVal i As T)
        l.wait()
        s.erase(type)
        s.[set](type, i)
        l.release()
    End Sub

    Public Sub assert_first_register(ByVal type As Type, ByVal i As T)
        l.wait()
        s.[set](type, i)
        l.release()
    End Sub

    Public Sub unregister(ByVal type As Type)
        l.wait()
        s.erase(type)
        l.release()
    End Sub

    Public Sub assert_unregister(ByVal type As Type)
        l.wait()
        assert(s.erase(type))
        l.release()
    End Sub

    Public Function from_type(ByVal type As Type, ByRef o As T) As Boolean
        If type Is Nothing Then
            Return False
        End If

        l.wait()
        Try
            Return s.get(type, o)
        Finally
            l.release()
        End Try
    End Function

    Public Function from_type(ByVal type As Type) As T
        Dim o As T = Nothing
        assert(from_type(type, o))
        Return o
    End Function

    Private Function from_types(ByVal types As vector(Of Type), ByRef o As T) As Boolean
        assert(Not types Is Nothing)
        If types.empty() Then
            Return False
        End If

        l.wait()
        Try
            Dim i As UInt32 = 0
            While i < types.size()
                If s.get(types(i), o) Then
                    Return True
                End If
                i += uint32_1
            End While
        Finally
            l.release()
        End Try
        Return False
    End Function

    Public Function from_type_or_base(ByVal type As Type, ByRef o As T) As Boolean
        If type Is Nothing Then
            Return False
        End If

        Dim types As vector(Of Type) = Nothing
        types = New vector(Of Type)()
        While Not type Is Nothing
            types.emplace_back(type)
            type = type.BaseType()
        End While
        Return from_types(types, o)
    End Function

    Public Function from_type_or_base(ByVal type As Type) As T
        Dim o As T = Nothing
        assert(from_type_or_base(type, o))
        Return o
    End Function

    Public Function from_type_or_interfaces(ByVal type As Type, ByRef o As T) As Boolean
        If type Is Nothing Then
            Return False
        End If

        Dim types As vector(Of Type) = Nothing
        types = vector.emplace_of(type)
        types.emplace_back(type.GetInterfaces())
        Return from_types(types, o)
    End Function

    Public Function from_type_or_interfaces(ByVal type As Type) As T
        Dim o As T = Nothing
        assert(from_type_or_interfaces(type, o))
        Return o
    End Function

    Public Function from_interfaces(ByVal type As Type, ByRef o As T) As Boolean
        If type Is Nothing Then
            Return False
        End If

        Dim types As vector(Of Type) = Nothing
        types = New vector(Of Type)()
        types.emplace_back(type.GetInterfaces())
        Return from_types(types, o)
    End Function

    Public Function from_interfaces(ByVal type As Type) As T
        Dim o As T = Nothing
        assert(from_interfaces(type, o))
        Return o
    End Function
End Class
