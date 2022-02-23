
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
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
            Private ReadOnly signature As vector(Of name_with_namespace)
            Private ReadOnly type As type_t
            Public ReadOnly content As String

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
                assert(c IsNot Nothing)
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

            Public Function forward_to(ByVal other As class_def) As String
                assert(other IsNot Nothing)
                Dim content As New StringBuilder()
                content.Append("reinterpret_cast(this,").
                        Append(other.name.in_global_namespace()).
                        Append(");")
                ' TODO: A better way to check the return type.
                If Not return_type.name().Equals("void") Then
                    content.Append("return ")
                End If
                content.Append(name().in_global_namespace()).
                    Append("(this")
                For i As Int32 = 2 To CInt(signature.size()) - 1
                    content.Append(",").
                            Append("i").
                            Append(i - 2)
                Next
                content.Append(");")
                Return content.ToString()
            End Function

            ' TODO: Add parameter names to avoid duplicating with class_def.
            Public Function declaration() As String
                ' No namespace is necessary, the first parameter contains namespace.
                Dim content As New StringBuilder()
                content.Append(return_type.in_global_namespace()).
                        Append(" ").
                        Append(name().in_global_namespace()).
                        Append("(").
                        Append(c.name.in_global_namespace()).
                        Append("& this")
                For i As Int32 = 2 To CInt(signature.size()) - 1
                    content.Append(",").
                            Append(signature(CUInt(i)).in_global_namespace()).
                            Append("i").
                            Append(i - 2)
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
