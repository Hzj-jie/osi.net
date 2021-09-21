
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

    Private Shared Function read_argument_value(ByVal class_type As Type,
                                                ByVal field_type As Type,
                                                ByVal field As FieldInfo,
                                                ByVal [default] As var) As Object
        If field_type.Equals(GetType(vector(Of String))) AndAlso
           (field.Name().Equals("others") OrElse
            field.Name().Equals("other_values")) Then
            Return [default].other_values()
        End If

        Dim arg_names As vector(Of String) = vector.emplace_of(
            argument_name(class_type, field),
            argument_name(class_type, field).Replace("_"c, "-"c),
            field.Name(),
            field.Name().Replace("_"c, "-"c))
        If field_type.Equals(GetType(Boolean)) Then
            For i As UInt32 = 0 To arg_names.size() - uint32_1
                Dim o As Boolean = False
                If [default].switch(arg_names(i), o) Then
                    Return o
                End If
            Next
            Return False
        End If

        static_constructor.once_execute(field_type)
        For i As UInt32 = 0 To arg_names.size() - uint32_1
            Dim o As String = Nothing
            If Not [default].value(arg_names(i), o) Then
                Continue For
            End If
            Dim assert_msgs() As Object = {
                arg_names(i),
                ": ",
                o,
                " for ",
                class_type.AssemblyQualifiedName(),
                ".",
                field.Name()
            }
            If field_type.IsEnum() Then
                Try
                    Return [Enum].Parse(field_type, o, True)
                Catch ex As Exception
                    assert(False, assert_msgs, ", ex ", ex)
                End Try
            End If
            Dim v As Object = Nothing
            assert(type_string_serializer.r.from_str(field_type, False, o, v) OrElse
                   type_json_serializer.r.from_str(field_type, False, o, v),
                   assert_msgs)
            Return v
        Next

        Return Nothing
    End Function

    ' VisibleForTesting
    Public Shared Sub process_type(ByVal class_type As Type, ByVal [default] As var)
        If class_type.IsGenericType() OrElse class_type.IsGenericTypeDefinition() Then
            Return
        End If

        For Each field As FieldInfo In class_type.GetFields(binding_flags.static_all)
            get_argument_type(field).if_present(
                Sub(ByVal field_type As Type)
                    Dim v As Object = read_argument_value(class_type, field_type, field, [default])
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
