
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

' It's safe to execute this global_init multiple times, especially for newly loaded modules.
<global_init(False, global_init_level.arguments)>
Public NotInheritable Class argument_setter
    ' init once.
    <global_init(global_init_level.arguments)>
    Private NotInheritable Class argument_reporter
        Private Shared Sub init()
            raise_error(error_type.user,
                        "process arguments from ",
                        Microsoft.VisualBasic.Command(),
                        ", parsed arguments ",
                        var.[default])
        End Sub

        Private Sub New()
        End Sub
    End Class

    Private Shared Sub init()
        concurrency_runner.execute(AddressOf process_assembly, AppDomain.CurrentDomain().GetAssemblies())
    End Sub

    Private Shared Sub process_assembly(ByVal assembly As Assembly)
        For Each type As Type In assembly.GetTypes()
            process_type(type, var.[default])
        Next
    End Sub

    Private Shared Function argument_name(ByVal type As Type, ByVal field As FieldInfo) As String
        Return strcat(type.Name(), ".", field.Name()).strrplc("+", ".")
    End Function

    ' VisibleForTesting
    Public Shared Sub process_type(ByVal type As Type, ByVal [default] As var)
        If type.IsGenericType() OrElse type.IsGenericTypeDefinition() Then
            Return
        End If

        For Each field As FieldInfo In type.GetFields(binding_flags.static_all)
            get_argument_type(field).if_present(
                    Sub(ByVal t As Type)
                        Dim v As Object = Nothing
                        If t.Equals(GetType(vector(Of String))) AndAlso
                                                       (field.Name().Equals("others") OrElse
                                                        field.Name().Equals("other_values")) Then
                            v = [default].other_values()
                        Else
                            static_constructor.once_execute(t)
                            Dim arg_names As vector(Of String) = vector.emplace_of(
                                argument_name(type, field),
                                argument_name(type, field).Replace("_"c, "-"c),
                                field.Name(),
                                field.Name().Replace("_"c, "-"c))
                            If t.Equals(GetType(Boolean)) Then
                                For i As UInt32 = 0 To arg_names.size() - uint32_1
                                    Dim o As Boolean = False
                                    If [default].switch(arg_names(i), o) Then
                                        v = o
                                        Exit For
                                    End If
                                Next
                            Else
                                For i As UInt32 = 0 To arg_names.size() - uint32_1
                                    Dim o As String = Nothing
                                    If [default].value(arg_names(i), o) Then
                                        If t.IsEnum() Then
                                            v = [Enum].Parse(t, o)
                                        Else
                                            assert(type_string_serializer.r.from_str(t, False, o, v) OrElse
                                                   type_json_serializer.r.from_str(t, False, o, v))
                                        End If
                                        Exit For
                                    End If
                                Next
                            End If
                        End If
                        Try
                            field.SetValue(Nothing, field.FieldType().parameters_constructor()({v}))
                        Catch ex As TargetInvocationException
                            assert(False, ex)
                        End Try
                    End Sub)
        Next
    End Sub

    Private Shared Function get_argument_type(ByVal field As FieldInfo) As [optional](Of Type)
        If Not field.FieldType().IsGenericType() Then
            Return [optional].empty(Of Type)()
        End If
        If Not field.FieldType().GetGenericTypeDefinition().Equals(GetType(root.delegates.argument(Of ))) Then
            Return [optional].empty(Of Type)()
        End If
        If field.FieldType().GetGenericArguments().Length() <> 1 Then
            Return [optional].empty(Of Type)()
        End If
        If Not field.GetValue(Nothing) Is Nothing Then
            Return [optional].empty(Of Type)()
        End If
        Return [optional].of(field.FieldType().GetGenericArguments(0))
    End Function

    Private Sub New()
    End Sub
End Class
