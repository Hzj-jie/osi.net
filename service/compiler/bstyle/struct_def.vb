
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class struct_def
        Public ReadOnly nesteds As vector(Of builders.parameter)
        Public ReadOnly primitives As vector(Of builders.parameter)

        Public Sub New()
            Me.New(New vector(Of builders.parameter)(), New vector(Of builders.parameter)())
        End Sub

        Public Sub New(ByVal nesteds As vector(Of builders.parameter),
                       ByVal primitives As vector(Of builders.parameter))
            assert(Not nesteds Is Nothing)
            assert(Not primitives Is Nothing)
            Me.nesteds = nesteds
            Me.primitives = primitives
            Me.nesteds.
               stream().
               foreach(Sub(ByVal p As builders.parameter)
                           assert(Not p Is Nothing)
                           assert(Not p.ref)
                       End Sub)
            Me.primitives.
               stream().
               foreach(Sub(ByVal p As builders.parameter)
                           assert(Not p Is Nothing)
                           assert(Not p.ref)
                           assert(Not scope.current().structs().defined(p.type))
                       End Sub)
        End Sub

        Public Shared Function of_primitive(ByVal type As String, ByVal name As String) As struct_def
            Return New struct_def(New vector(Of builders.parameter)(), vector.emplace_of(primitive(type, name)))
        End Function

        Public Function with_nested(ByVal type As String, ByVal name As String) As struct_def
            nesteds.emplace_back(nested(type, name))
            Return Me
        End Function

        ' It can be a struct or just a primitive.
        Public Shared Function nested(ByVal type As String, ByVal name As String) As builders.parameter
            Return builders.parameter.no_ref(scope.current().type_alias()(type), name)
        End Function

        ' It must be a primitive.
        Public Shared Function primitive(ByVal type As String, ByVal name As String) As builders.parameter
            type = scope.current().type_alias()(type)
            assert(Not scope.current().structs().defined(type))
            Return builders.parameter.no_ref(type, name)
        End Function

        Public Shared Function is_primitive(ByVal p As builders.parameter) As Boolean
            assert(Not p Is Nothing)
            Return Not scope.current().structs().defined(p.type)
        End Function

        Private Shared Function append_prefix(ByVal v As vector(Of builders.parameter),
                                              ByVal name As String) As vector(Of builders.parameter)
            assert(Not v Is Nothing)
            assert(Not name.null_or_whitespace())
            Return v.stream().
                     map(Function(ByVal n As builders.parameter) As builders.parameter
                             assert(Not n Is Nothing)
                             Return n.map_name(Function(ByVal nname As String) As String
                                                   Return name + "." + nname
                                               End Function)
                         End Function).
                     collect(Of vector(Of builders.parameter))()
        End Function

        Public Function append_prefix(ByVal name As String) As struct_def
            Return New struct_def(append_prefix(nesteds, name), append_prefix(primitives, name))
        End Function

        Public Sub append(ByVal r As struct_def)
            assert(Not r Is Nothing)
            nesteds.emplace_back(r.nesteds)
            primitives.emplace_back(r.primitives)
        End Sub
    End Class
End Class
