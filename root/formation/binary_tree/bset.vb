
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with bset.vbp ----------
'so change bset.vbp instead of this file



Imports osi.root.connector

Public Class bset(Of T)
    Inherits bbst(Of T)
    Implements ICloneable, ICloneable(Of bset(Of T))

    Shared Sub New()
        container_operator(Of bset(Of T), T).size(Function(ByVal i As bset(Of T)) As UInt32
                                                          assert(Not i Is Nothing)
                                                          Return i.size()
                                                      End Function)
        container_operator(Of bset(Of T), T).emplace(Function(ByVal i As bset(Of T), ByVal j As T) As Boolean
                                                             assert(Not i Is Nothing)
                                                             Return i.emplace(j).second
                                                         End Function)
        container_operator(Of bset(Of T), T).enumerate(Function(ByVal i As bset(Of T)) _
                                                                   As container_operator(Of bset(Of T), T).enumerator
                                                               Return New enumerator(i)
                                                           End Function)
        container_operator(Of bset(Of T), T).clear(Sub(ByVal i As bset(Of T))
                                                           assert(Not i Is Nothing)
                                                           i.clear()
                                                       End Sub)
        bytes_serializer(Of bset(Of T)).container(Of T).register()
    End Sub

    Private NotInheritable Class enumerator
        Implements container_operator(Of bset(Of T), T).enumerator

        Private it As iterator

        Public Sub New(ByVal s As bset(Of T))
            assert(Not s Is Nothing)
            it = s.begin()
        End Sub

        Public Sub [next]() Implements container_operator(Of bset(Of T), T).enumerator.next
            it += 1
        End Sub

        Public Function current() As T Implements container_operator(Of bset(Of T), T).enumerator.current
            Return +it
        End Function

        Public Function [end]() As Boolean Implements container_operator(Of bset(Of T), T).enumerator.end
            Return it.is_end()
        End Function
    End Class

    Public Shared Shadows Function move(ByVal v As bset(Of T)) As bset(Of T)
        If v Is Nothing Then
            Return Nothing
        Else
            Dim r As bset(Of T) = Nothing
            r = New bset(Of T)()
            move_to(v, r)
            Return r
        End If
    End Function

    Public Shadows Function clone() As bset(Of T)
        Dim r As bset(Of T) = Nothing
        r = New bset(Of T)()
        clone_to(Me, r)
        Return r
    End Function

    Public Function CloneT() As bset(Of T) Implements ICloneable(Of bset(Of T)).Clone
        Return clone()
    End Function

    Public Function ICloneable_Clone() As Object Implements ICloneable.Clone
        Return clone()
    End Function
End Class

'finish bset.vbp --------
