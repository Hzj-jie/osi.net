
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class b2style
    Partial Public NotInheritable Class class_def
        Public NotInheritable Class function_def
            Implements IEquatable(Of function_def)

            Public Enum type_t
                pure
                virtual
                override
            End Enum

            Private ReadOnly c As class_def
            Private ReadOnly return_type As name_with_namespace
            ' TODO: Move name out of signature
            Private ReadOnly signature As vector(Of name_with_namespace)
            Private ReadOnly type As type_t
            Public ReadOnly content As String

            ' TODO: Using names with global_namespace does not provide extra benefit, may consider to use string.
            Public Function name() As name_with_namespace
                Return signature(0)
            End Function

            Public Shared Function name_of(ByVal name As String) As name_with_namespace
                Return name_with_namespace.of_global_namespace(name)
            End Function

            Public Shared Function type_of(ByVal type As String) As name_with_namespace
                Return name_with_namespace.of(type)
            End Function

            Public Sub New(ByVal c As class_def,
                           ByVal return_type As name_with_namespace,
                           ByVal signature As vector(Of name_with_namespace),
                           ByVal type As type_t,
                           ByVal content As String)
                assert(Not c Is Nothing)
                assert(Not signature.null_or_empty())
                assert(Not content.null_or_whitespace())
                Me.c = c
                Me.return_type = return_type
                Me.signature = signature
                Me.type = type
                Me.content = content
            End Sub

            Public Sub New(ByVal c As class_def,
                           ByVal return_type As name_with_namespace,
                           ByVal name As name_with_namespace,
                           ByVal type As type_t,
                           ByVal content As String)
                Me.New(c, return_type, vector.emplace_of(name), type, content)
            End Sub

            Public Function with_content(ByVal content As String) As function_def
                Return New function_def(c, return_type, signature, type, content)
            End Function

            Public Function with_class(ByVal c As class_def) As function_def
                Return New function_def(c, return_type, signature, type, content)
            End Function

            Public Function with_name(ByVal name As String) As function_def
                Dim signature As vector(Of name_with_namespace) = Me.signature.CloneT()
                signature(0) = name_of(name)
                Return New function_def(c, return_type, signature, type, content)
            End Function

            Private Function default_param_names() As vector(Of String)
                Return streams.range(0, signature.size() - uint32_1).
                               map(Function(ByVal index As Int32) As String
                                       Return "i" + index.ToString()
                                   End Function).
                               collect(Of vector(Of String))()
            End Function

            Public Function invoke(ByVal param_names As vector(Of String)) As String
                assert(Not param_names Is Nothing)
                assert(param_names.size() = signature.size() - uint32_1)
                Dim content As New StringBuilder()
                ' TODO: A better way to check the return type.
                If Not return_type.name().Equals("void") Then
                    content.Append("return ")
                End If
                content.Append(name().in_global_namespace()).
                        Append("(this")
                Dim i As UInt32 = 0
                While i < param_names.size()
                    content.Append(",").
                            Append(param_names(i))
                    i += uint32_1
                End While
                content.Append(");")
                Return content.ToString()
            End Function

            Public Function forward_to(ByVal other As class_def) As String
                assert(Not other Is Nothing)
                Return "reinterpret_cast(this," +
                       other.name.in_global_namespace() +
                       ");" +
                       invoke(default_param_names())
            End Function

            Public Function declaration() As String
                Return declaration(default_param_names())
            End Function

            Public Function declaration(ByVal param_names As vector(Of String)) As String
                assert(Not param_names Is Nothing)
                assert(param_names.size() = signature.size() - uint32_1)
                ' No namespace is necessary, the first parameter contains namespace.
                Dim content As New StringBuilder()
                content.Append(return_type.in_global_namespace()).
                        Append(" ").
                        Append(name().in_global_namespace()).
                        Append("(").
                        Append(c.name.in_global_namespace()).
                        Append("& this")
                For i As Int32 = 1 To CInt(signature.size()) - 1
                    content.Append(",").
                            Append(signature(CUInt(i)).in_global_namespace()).
                            Append(" ").
                            Append(param_names(CUInt(i - 1)))
                Next
                content.Append(")")
                Return content.ToString()
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

            Public Overrides Function GetHashCode() As Int32
                Return signature.GetHashCode()
            End Function

            Public Overrides Function ToString() As String
                Return strcat("{", signature, ": ", return_type, "}")
            End Function
        End Class
    End Class
End Class
