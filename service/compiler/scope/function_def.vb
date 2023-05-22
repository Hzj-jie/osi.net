
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports builders = osi.service.compiler.logic.builders

Partial Public Class scope(Of WRITER As {lazy_list_writer, New},
                              __BUILDER As func_t(Of String, WRITER, Boolean),
                              __CODE_GENS As func_t(Of code_gens(Of WRITER)),
                              T As scope(Of WRITER, __BUILDER, __CODE_GENS, T))
    Partial Public Class class_def
        Public NotInheritable Class function_def
            Implements IComparable(Of function_def), IEquatable(Of function_def)

            Public Enum type_t
                pure
                [overridable]
                override
            End Enum

            ' TODO: A better way to check the return type.
            Private Shared ReadOnly void_type As String =
                Function() As String
                    Dim p As builders.parameter_type = normalized_type.of("void")
                    assert(Not p.ref)
                    Return p.type
                End Function()
            Private ReadOnly class_def As class_def
            Private ReadOnly return_type As name_with_namespace
            Private ReadOnly signature As vector(Of name_with_namespace)
            Private ReadOnly type As type_t
            Public ReadOnly content As String

            ' TODO: Using names with global namespace does not provide extra benefit, may consider to use string and
            ' always prefix with global namespace.
            Public Function name() As name_with_namespace
                Return signature(0)
            End Function

            Public Sub New(ByVal class_def As class_def,
                           ByVal return_type As name_with_namespace,
                           ByVal signature As vector(Of name_with_namespace),
                           ByVal type As type_t,
                           ByVal content As String)
                assert(Not class_def Is Nothing)
                assert(Not signature.null_or_empty())
                assert(Not content.null_or_whitespace())
                Me.class_def = class_def
                Me.return_type = return_type
                Me.signature = signature
                Me.type = type
                Me.content = content
            End Sub

            Public Sub New(ByVal class_def As class_def,
                           ByVal return_type As name_with_namespace,
                           ByVal name As name_with_namespace,
                           ByVal type As type_t,
                           ByVal content As String)
                Me.New(class_def, return_type, vector.emplace_of(name), type, content)
            End Sub

            Public Function with_content(ByVal content As String) As function_def
                Return New function_def(class_def, return_type, signature, type, content)
            End Function

            Public Function with_class(ByVal class_def As class_def) As function_def
                Return New function_def(class_def, return_type, signature, type, content)
            End Function

            Public Function is_virtual() As Boolean
                Return type <> type_t.pure
            End Function

            Public Function is_override() As Boolean
                Return type = type_t.override
            End Function

            Private Function default_param_names() As vector(Of String)
                Return streams.range(0, signature.size() - uint32_1).
                               map(Function(ByVal index As Int32) As String
                                       Return "i" + index.ToString()
                                   End Function).
                               collect_to(Of vector(Of String))()
            End Function

            Public Function forward_to(ByVal other As class_def) As String
                assert(Not other Is Nothing)
                Dim content As New StringBuilder()
                content.Append("reinterpret_cast(this,").
                        Append(other.name.in_global_namespace()).
                        Append(");")
                If Not return_type.in_global_namespace().Equals(void_type) Then
                    content.Append("return ")
                End If
                content.Append(name().in_global_namespace()).
                        Append("(this").
                        Append(default_param_names().stream().
                                                     map(Function(ByVal s As String) As String
                                                             Return "," + s
                                                         End Function).
                                                     collect_by(stream(Of String).collectors.to_str()))
                content.Append(");")
                Return content.ToString()
            End Function

            Public Function declaration(ByVal param_names As vector(Of String),
                                        ByVal ignored_types As unordered_set(Of String)) As String
                assert(Not param_names Is Nothing)
                assert(param_names.size() = signature.size() - uint32_1)
                ' No namespace is necessary, the first parameter contains namespace.
                Dim content As New StringBuilder()
                Dim get_type As Func(Of name_with_namespace, String) =
                        Function(ByVal x As name_with_namespace) As String
                            If ignored_types Is Nothing OrElse ignored_types.find(x.name()) = ignored_types.end() Then
                                Return x.in_global_namespace()
                            End If
                            Return x.name()
                        End Function
                content.Append(get_type(return_type)).
                        Append(" ").
                        Append(name().in_global_namespace()).
                        Append("(").
                        Append(class_def.name.in_global_namespace()).
                        Append("& this")
                Dim i As UInt32 = 1
                While i < signature.size()
                    content.Append(",").
                            Append(get_type(signature(i))).
                            Append(" ").
                            Append(param_names(i - uint32_1))
                    i += uint32_1
                End While
                content.Append(")")
                Return content.ToString()
            End Function

            Public Function declaration() As String
                Return declaration(default_param_names(), Nothing)
            End Function

            Public Overrides Function Equals(ByVal obj As Object) As Boolean
                Return Equals(direct_cast(Of function_def)(obj, False))
            End Function

            Public Overloads Function Equals(ByVal other As function_def) As Boolean _
                                            Implements IEquatable(Of function_def).Equals
                If other Is Nothing Then
                    Return False
                End If
                Return signature.Equals(other.signature)
            End Function

            Public Function CompareTo(ByVal other As function_def) As Int32 _
                                     Implements IComparable(Of function_def).CompareTo
                If other Is Nothing Then
                    Return 1
                End If
                Return signature.CompareTo(other.signature)
            End Function

            Public Overrides Function GetHashCode() As Int32
                Return signature.GetHashCode()
            End Function

            Public Overrides Function ToString() As String
                Return strcat("{", signature, ": ", return_type, "}")
            End Function
        End Class
    End Class
End Class
