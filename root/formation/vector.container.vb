
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class vector(Of T)
    Shared Sub New()
        container_operator(Of vector(Of T), T).register(Function(ByVal i As vector(Of T)) As UInt32
                                                            Return i.size_or_0()
                                                        End Function)
        container_operator.register(Function(ByVal i As vector(Of T), ByVal j As T) As Boolean
                                        assert(Not i Is Nothing)
                                        i.emplace_back(j)
                                        Return True
                                    End Function)
        container_operator.register(
                Function(ByVal i As vector(Of T)) As container_operator(Of vector(Of T), T).enumerator
                    Return New enumerator(i)
                End Function)
        bytes_serializer(Of vector(Of T)).container(Of T).register()
    End Sub

    Private Class enumerator
        Implements container_operator(Of vector(Of T), T).enumerator

        Private ReadOnly v As vector(Of T)
        Private index As UInt32

        Public Sub New(ByVal v As vector(Of T))
            assert(Not v Is Nothing)
            Me.v = v
        End Sub

        Public Sub [next]() Implements container_operator(Of vector(Of T), T).enumerator.next
            index += uint32_1
        End Sub

        Public Function current() As T Implements container_operator(Of vector(Of T), T).enumerator.current
            Return v(index)
        End Function

        Public Function [end]() As Boolean Implements container_operator(Of vector(Of T), T).enumerator.end
            Return index = v.size()
        End Function
    End Class
End Class
