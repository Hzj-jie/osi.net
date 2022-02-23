
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class vector(Of T)
    Shared Sub New()
        container_operator(Of vector(Of T), T).size(Function(ByVal i As vector(Of T)) As UInt32
                                                        Return i.size_or_0()
                                                    End Function)
        container_operator.emplace(Function(ByVal i As vector(Of T), ByVal j As T) As Boolean
                                       assert(i IsNot Nothing)
                                       i.emplace_back(j)
                                       Return True
                                   End Function)
        container_operator.enumerate(
                Function(ByVal i As vector(Of T)) As container_operator(Of T).enumerator
                    Return New enumerator(i)
                End Function)
        container_operator(Of vector(Of T), T).clear(Sub(ByVal i As vector(Of T))
                                                         assert(i IsNot Nothing)
                                                         i.clear()
                                                     End Sub)
        bytes_serializer(Of vector(Of T)).container(Of T).register()
        json_serializer(Of vector(Of T)).container(Of T).register_as_array()
        string_serializer(Of vector(Of T)).container(Of T).register()
    End Sub
End Class
