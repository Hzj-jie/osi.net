
Imports osi.root.template
Imports osi.root.constants
Imports osi.root.connector

Namespace sign
    <global_init(global_init_level.services)>
    Public NotInheritable Class md5_merge_hasher
        Inherits merge_hasher(Of _New)

        Public NotInheritable Class _New
            Inherits __do(Of merge_method, merge_hasher(Of _New))

            Public Overrides Function at(ByRef k As merge_hasher(Of _New).merge_method) As merge_hasher(Of _New)
                Return New md5_merge_hasher(k)
            End Function
        End Class

        Private Sub New(ByVal m As merge_method)
            MyBase.New(m)
        End Sub

        Protected Overrides Function compute(ByVal i() As Byte) As Byte()
            Return md5calculator().ComputeHash(i)
        End Function

        Private Shared Sub init()
            register("md5")
        End Sub
    End Class
End Namespace
