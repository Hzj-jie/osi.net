
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class typeless_invoker
    Public NotInheritable Class builder(Of delegate_t)
        Inherits invoker.builder_base(Of builder(Of delegate_t))

        Private type_name As String
        Private assembly_name As String

        Public Function with_type_name(ByVal type_name As String) As builder(Of delegate_t)
            Me.type_name = type_name
            Return Me
        End Function

        Public Function with_assembly_name(ByVal assembly_name As String) As builder(Of delegate_t)
            Me.assembly_name = assembly_name
            Return Me
        End Function

        Public Function build(ByRef o As invoker(Of delegate_t)) As Boolean
            Return typeless_invoker(Of delegate_t).[New](type_name,
                                                         assembly_name,
                                                         binding_flags,
                                                         name,
                                                         suppress_error,
                                                         o)
        End Function

        Public Function build() As invoker(Of delegate_t)
            Return typeless_invoker(Of delegate_t).[New](type_name,
                                                         assembly_name,
                                                         binding_flags,
                                                         name,
                                                         suppress_error)
        End Function
    End Class
End Class
