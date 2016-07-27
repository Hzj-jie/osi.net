
Imports System.Reflection
Imports osi.root.constants

Public Module _event
    Private Const event_magic_suffix As String = "Event"

    'test purpose only
    Public Function attached_event_count(ByVal T As Type,
                                         ByVal i As Object,
                                         ByVal name As String) As Int32
        If T Is Nothing Then
            Return npos
        Else
            If Not strendwith(name, event_magic_suffix) Then
                name += event_magic_suffix
            End If
            Dim fi As FieldInfo = Nothing
            fi = T.GetField(name,
                            BindingFlags.NonPublic Or
                            If(i Is Nothing, BindingFlags.Static, BindingFlags.Instance))
            If fi Is Nothing Then
                Return npos
            Else
                Dim d As [Delegate] = Nothing
                Return If(cast(Of [Delegate])(fi.GetValue(i), d), attached_delegate_count(d), npos)
            End If
        End If
    End Function

    Public Function attached_event_count(Of T)(ByVal i As T,
                                               ByVal name As String) As Int32
        Return attached_event_count(GetType(T), i, name)
    End Function

    Public Function attached_event_count(ByVal T As Type,
                                         ByVal name As String) As Int32
        Return attached_event_count(T, Nothing, name)
    End Function

    Public Function attached_event_count(Of T)(ByVal name As String) As Int32
        Return attached_event_count(GetType(T), name)
    End Function

    Public Function attached_delegate_count(ByVal d As [Delegate]) As UInt32
        If d Is Nothing Then
            Return 0
        Else
            Return array_size(d.GetInvocationList())
        End If
    End Function

    Public Function event_attached(ByVal d As [Delegate]) As Boolean
        Return attached_delegate_count(d) > uint32_0
    End Function
End Module
