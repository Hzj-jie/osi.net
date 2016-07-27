
Imports System.Security.Cryptography
Imports osi.root.connector
Imports osi.root.template

Namespace sign
    Public Class hash_algorithm_merge_hasher(Of T As __do(Of HashAlgorithm))
        Inherits merge_hasher(Of [New])

        Public Class [New]
            Inherits __do(Of merge_method, merge_hasher(Of [New]))

            Public Overrides Function at(ByRef k As merge_method) As merge_hasher(Of [New])
                Return New hash_algorithm_merge_hasher(Of T)(k)
            End Function
        End Class

        Private Shared ReadOnly create As Func(Of HashAlgorithm)
        <ThreadStatic> Private Shared h As HashAlgorithm

        Shared Sub New()
            create = -alloc(Of T)()
            assert(Not create Is Nothing)
        End Sub

        Protected Sub New(ByVal m As merge_method)
            MyBase.New(m)
        End Sub

        Protected Overrides Function compute(ByVal i() As Byte) As Byte()
            If h Is Nothing Then
                h = create()
            End If
            assert(Not h Is Nothing)
            Return h.ComputeHash(i)
        End Function
    End Class
End Namespace
