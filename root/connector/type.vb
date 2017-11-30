
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _type
    Private Function pure_assembly_name(ByVal name As String) As String
        assert(Not String.IsNullOrEmpty(name))
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

    Private Function create(ByVal type_name As String, ByRef o As Type) As Boolean
        Try
            o = Type.GetType(type_name)
        Catch ex As Exception
            raise_error(error_type.warning, "Type.GetType(", type_name, ") throws exception ", ex.details())
            Return False
        End Try
        Return Not o Is Nothing
    End Function

    <Extension()> Public Function [New](ByRef o As Type, ByVal type_name As String) As Boolean
        If String.IsNullOrEmpty(type_name) Then
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

        Return create(type_name, o)
    End Function

    <Extension()> Public Function [New](ByRef o As Type,
                                        ByVal type_name As String,
                                        ByVal assembly_name As String) As Boolean
        If String.IsNullOrEmpty(type_name) Then
            Return False
        End If

        If String.IsNullOrEmpty(assembly_name) Then
            Return [New](o, type_name)
        End If

        Dim full_type_name As String = Nothing
        If strstartwith(type_name, character.dot) Then
            full_type_name = strcat(pure_assembly_name(assembly_name), type_name)
        Else
            full_type_name = type_name
        End If

        Return create(merge_type_name_with_assembly(full_type_name, assembly_name), o)
    End Function
End Module
