
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _type
    Private Function pure_assembly_name(ByVal name As String) As String
        assert(Not name.null_or_empty())
        Dim index As Int32 = 0
        index = name.IndexOf(character.comma)
        If index = npos Then
            Return name
        Else
            Return strleft(name, CUInt(index))
        End If
    End Function

    Private Function merge_type_name_with_assembly(ByVal type_name As String, ByVal assembly_name As String) As String
        Return strcat(type_name, character.comma, character.blank, assembly_name)
    End Function

    Private Function create(ByVal type_name As String, ByVal assembly As Assembly, ByRef o As Type) As Boolean
        Try
            If assembly Is Nothing Then
                o = Type.GetType(type_name)
            Else
                o = assembly.GetType(type_name)
            End If
        Catch ex As Exception
            raise_error(error_type.warning, "Type.GetType(", type_name, ") throws exception ", ex.details())
            Return False
        End Try
        Return Not o Is Nothing
    End Function

    <Extension()> Public Function [New](ByRef o As Type,
                                        ByVal assembly As Assembly,
                                        ByVal type_name As String) As Boolean
        If type_name.null_or_empty() Then
            Return False
        End If

        If Not strcontains(type_name, character.comma) Then
            Dim index As Int32 = 0
            index = type_name.LastIndexOf(character.dot)
            If index <> npos Then
                Dim assembly_name As String = Nothing
                assembly_name = strleft(type_name, CUInt(index))
                type_name = merge_type_name_with_assembly(type_name, assembly_name)
            End If
        End If

        Return create(type_name, assembly, o)
    End Function

    <Extension()> Public Function [New](ByRef o As Type, ByVal type_name As String) As Boolean
        Return [New](o, [default](Of Assembly).null, type_name)
    End Function

    <Extension()> Public Function [New](ByRef o As Type,
                                        ByVal assembly As Assembly,
                                        ByVal type_name As String,
                                        ByVal assembly_name As String) As Boolean
        If type_name.null_or_empty() Then
            Return False
        End If

        If assembly_name.null_or_empty() Then
            Return [New](o, assembly, type_name)
        End If

        Dim full_type_name As String = Nothing
        If strstartwith(type_name, character.dot) Then
            full_type_name = strcat(pure_assembly_name(assembly_name), type_name)
        Else
            full_type_name = type_name
        End If

        Return create(merge_type_name_with_assembly(full_type_name, assembly_name), assembly, o)
    End Function

    <Extension()> Public Function [New](ByRef o As Type,
                                        ByVal type_name As String,
                                        ByVal assembly_name As String) As Boolean
        Return [New](o, Nothing, type_name, assembly_name)
    End Function
End Module
