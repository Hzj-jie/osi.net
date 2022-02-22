
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class struct_def
        Private ReadOnly _nesteds As vector(Of builders.parameter)
        Private ReadOnly _primitives As vector(Of builders.parameter)

        Public Sub New()
            Me.New(New vector(Of builders.parameter)(), New vector(Of builders.parameter)())
        End Sub

        Private Sub New(ByVal nesteds As vector(Of builders.parameter),
                        ByVal primitives As vector(Of builders.parameter))
            assert(Not nesteds Is Nothing)
            assert(Not primitives Is Nothing)
            Me._nesteds = nesteds
            Me._primitives = primitives
            Me._nesteds.
               stream().
               foreach(Sub(ByVal p As builders.parameter)
                           assert(Not p Is Nothing)
                           assert(Not p.ref)
                       End Sub)
            Me._primitives.
               stream().
               foreach(Sub(ByVal p As builders.parameter)
                           assert(Not p Is Nothing)
                           assert(Not p.ref)
                           assert(Not scope.current().structs().defined(p.type))
                       End Sub)
        End Sub

        Public Function nesteds() As stream(Of builders.parameter)
            Return _nesteds.stream()
        End Function

        Public Function primitives() As stream(Of builders.parameter)
            Return _primitives.stream()
        End Function

        Public Function primitive_count() As UInt32
            Return _primitives.size()
        End Function

        Public Shared Function of_primitive(ByVal type As String, ByVal name As String) As struct_def
            Return New struct_def().with_primitive(type, name)
        End Function

        Public Function with_nested(ByVal type As String, ByVal name As String) As struct_def
            _nesteds.emplace_back(nested(type, name))
            Return Me
        End Function

        ' It can be a struct or just a primitive.
        Public Shared Function nested(ByVal type As String, ByVal name As String) As builders.parameter
            Return builders.parameter.no_ref(scope.current().type_alias()(type), name)
        End Function

        Public Shared Function nested(ByVal p As builders.parameter) As builders.parameter
            assert(Not p Is Nothing)
            assert(Not p.ref)
            Return nested(p.type, p.name)
        End Function

        ' It must be a primitive.
        Public Function with_primitive(ByVal type As String, ByVal name As String) As struct_def
            Dim r As builders.parameter = nested(type, name)
            assert(Not scope.current().structs().defined(r.type))
            _primitives.emplace_back(r)
            Return Me
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
                     collect_to(Of vector(Of builders.parameter))()
        End Function

        Public Function append_prefix(ByVal name As String) As struct_def
            Return New struct_def(append_prefix(_nesteds, name), append_prefix(_primitives, name))
        End Function

        Public Sub append(ByVal r As struct_def)
            assert(Not r Is Nothing)
            _nesteds.emplace_back(r._nesteds)
            _primitives.emplace_back(r._primitives)
        End Sub
    End Class
End Class
