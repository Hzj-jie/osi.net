
Imports System.Runtime.CompilerServices

Public Module _inherit
    <Extension()> Public Function inherit(ByVal t As Type, ByVal base As Type) As Boolean
        If t Is Nothing AndAlso base Is Nothing Then
            Return True
        ElseIf t Is Nothing OrElse base Is Nothing Then
            Return False
        ElseIf t Is base Then
            Return True
        Else
            If t.IsGenericType() AndAlso base.IsGenericTypeDefinition() Then
                'object is not a generic type
                While t.IsGenericType() 'AndAlso Not t Is GetType(Object)
                    If t.GetGenericTypeDefinition() Is base Then
                        Return True
                    End If
                    t = t.BaseType()
                End While
            End If
            Return t.IsSubclassOf(base)
        End If
    End Function

    <Extension()> Public Function inherit(Of base)(ByVal t As Type) As Boolean
        Return inherit(t, GetType(base))
    End Function

    <Extension()> Public Function implement(ByVal t As Type, ByVal base As Type) As Boolean
        If t Is Nothing AndAlso base Is Nothing Then
            Return True
        ElseIf t Is Nothing OrElse base Is Nothing Then
            Return False
        ElseIf base.IsGenericTypeDefinition() Then
            Dim ints() As Type = Nothing
            ints = t.GetInterfaces()
            Dim i As UInt32 = 0
            While i < array_size(ints)
                If ints(i).IsGenericType() AndAlso ints(i).GetGenericTypeDefinition() Is base Then
                    Return True
                End If
                i += 1
            End While
            Return False
        Else
            Try
                t.GetInterfaceMap(base)
            Catch
                Return False
            End Try
            Return True
        End If
    End Function

    <Extension()> Public Function implement(Of base)(ByVal t As Type) As Boolean
        Return implement(t, GetType(base))
    End Function
End Module
