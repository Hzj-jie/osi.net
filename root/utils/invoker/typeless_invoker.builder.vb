
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class typeless_invoker
    Public MustInherit Class typeless_invoker_builder(Of RT)
        Inherits invoker.builder_base(Of RT, typeless_invoker_builder(Of RT))

        Protected assembly As Assembly
        Protected type_name As String
        Protected assembly_name As String

        Public Function with_assembly(ByVal assembly As Assembly) As typeless_invoker_builder(Of RT)
            Me.assembly = assembly
            Return Me
        End Function

        Public Function with_type_name(ByVal type_name As String) As typeless_invoker_builder(Of RT)
            Me.type_name = type_name
            Return Me
        End Function

        Public Function with_assembly_name(ByVal assembly_name As String) As typeless_invoker_builder(Of RT)
            Me.assembly_name = assembly_name
            Return Me
        End Function

        Public Function with_fully_qualifed_name(ByVal name As String) As typeless_invoker_builder(Of RT)
            If name.null_or_whitespace() Then
                Return Me
            End If
            Dim i As Int32 = 0
            i = name.LastIndexOf(character.colon)
            If i = npos OrElse i = name.Length() - 1 Then
                Return with_type_name(name)
            End If
            with_type_name(name.Substring(0, i))
            Return with_name(name.Substring(i + 1))
        End Function
    End Class

    Public NotInheritable Class builder(Of delegate_t)
        Inherits typeless_invoker_builder(Of invoker(Of delegate_t))

        Public Overrides Function build(ByRef o As invoker(Of delegate_t)) As Boolean
            Return typeless_invoker(Of delegate_t).[New](assembly,
                                                         type_name,
                                                         assembly_name,
                                                         binding_flags,
                                                         name,
                                                         suppress_error,
                                                         o)
        End Function
    End Class

    Public NotInheritable Class builder
        Inherits typeless_invoker_builder(Of invoker)

        Public Overrides Function build(ByRef o As invoker) As Boolean
            Return typeless_invoker.[New](assembly,
                                          type_name,
                                          assembly_name,
                                          binding_flags,
                                          name,
                                          suppress_error,
                                          o)
        End Function
    End Class

    Public Shared Function [of]() As builder
        Return New builder()
    End Function

    Public Shared Function [of](ByVal i As invoker) As builder
        Return [of]()
    End Function

    Public Shared Function [of](Of delegate_t)() As builder(Of delegate_t)
        Return New builder(Of delegate_t)()
    End Function

    Public Shared Function [of](Of delegate_t)(ByVal invoker As invoker(Of delegate_t)) As builder(Of delegate_t)
        Return [of](Of delegate_t)()
    End Function
End Class
