
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class logic
    Partial Public NotInheritable Class scope
        Public Class typed_ref
            Public ReadOnly type As String

            Protected Sub New(ByVal type As String)
                assert(Not type.null_or_whitespace())
                Me.type = type
            End Sub

            Protected Sub New(ByVal other As typed_ref)
                assert(Not other Is Nothing)
                Me.type = other.type
            End Sub
        End Class

        Public NotInheritable Class ref
            Inherits typed_ref
            'Starts from 1 to allow size()-top.offset=0.
            Public ReadOnly offset As UInt64

            Public Sub New(ByVal offset As UInt64, ByVal type As String)
                MyBase.New(type)
                Me.offset = offset
            End Sub

            Public Function [with](ByVal type As String) As ref
                Return New ref(offset, type)
            End Function
        End Class

        Public NotInheritable Class exported_ref
            Inherits typed_ref

            Public ReadOnly data_ref As data_ref

            Public Sub New(ByVal r As ref, ByVal data_ref As data_ref)
                MyBase.New(r)
                assert(Not data_ref Is Nothing)
                Me.data_ref = data_ref
            End Sub
        End Class

        Private NotInheritable Class variable_t
            ' This stack can be a real stack, but using map with offset can provide a better lookup performance.
            Private ReadOnly stack As New unordered_map(Of String, ref)()
            Private undefined As UInt32 = 0

            Public Function find(ByVal name As String, ByRef o As ref) As Boolean
                Return stack.find(name, o)
            End Function

            ' Removing items from the stack will cause the calculation of location of the references to be off, since
            ' variable_proxy.export function uses variable_t.size to decide the depth of stacks. So besides removing
            ' the items from the map, the count of removed items also needs to be tracked.
            Public Function undefine(ByVal name As String) As Boolean
                undefined += uint32_1
                Return stack.erase(name)
            End Function

            Public Function size() As UInt32
                Return stack.size() + undefined
            End Function

            Public Function define(ByVal name As String, ByVal type As String) As Boolean
                assert(Not name.null_or_whitespace())
                assert(Not type.null_or_whitespace())
                If stack.find(name) <> stack.end() Then
                    errors.redefine(name, type, stack(name).type)
                    Return False
                End If
                Return assert(stack.emplace(name, New ref(size() + uint32_1, type)).second())
            End Function

            Public Function redefine(ByVal name As String, ByVal type As String) As Boolean
                Dim it As unordered_map(Of String, ref).iterator = stack.find(name)
                If it = stack.end() Then
                    Return False
                End If
                stack(name) = (+it).second.with(type)
                Return True
            End Function
        End Class

        Public Structure variable_proxy
            Private ReadOnly s As scope

            Public Sub New(ByVal s As scope)
                assert(Not s Is Nothing)
                Me.s = s
            End Sub

            Public Function define(ByVal name As String, ByVal type As String) As Boolean
                Return s.v.define(name, type)
            End Function

            Public Function defined_in_current_scope(ByVal name As String) As Boolean
                Return s.v.find(name, Nothing)
            End Function

            Public Function undefine(ByVal name As String) As Boolean
                Dim s As scope = Me.s
                While Not s Is Nothing
                    If s.v.undefine(name) Then
                        Return True
                    End If
                    s = s.parent
                End While
                raise_error(error_type.user, "Cannot find variable ", name)
                Return False
            End Function

            Public Function redefine(ByVal name As String, ByVal type As String) As Boolean
                Dim s As scope = Me.s
                While Not s Is Nothing
                    If s.v.redefine(name, type) Then
                        Return True
                    End If
                    s = s.parent
                End While
                raise_error(error_type.user, "Cannot find variable ", name)
                Return False
            End Function

            Public Function export(ByVal name As String, ByRef o As exported_ref) As Boolean
                Dim size As UInt64 = 0
                Dim s As scope = Me.s
                While Not s Is Nothing
                    Dim r As ref = Nothing
                    If Not s.v.find(name, r) Then
                        size += s.v.size()
                        s = s.parent
                        Continue While
                    End If
                    Dim d As data_ref = Nothing
                    If s.is_root() Then
                        ' To allow a callee to access global variables.
                        If Not data_ref.abs(CLng(r.offset - 1), d) Then
                            Return False
                        End If
                    Else
                        If Not data_ref.rel(CLng(s.v.size() - r.offset + size), d) Then
                            Return False
                        End If
                    End If
                    o = New exported_ref(r, d)
                    Return True
                End While
                Return False
            End Function

            Public Function export(ByVal name As String) As exported_ref
                Dim o As exported_ref = Nothing
                assert(export(name, o))
                Return o
            End Function

            Public Function unique_name() As String
                Return strcat("@scope_", s.v.GetHashCode(), "_unique_name_", s.v.size() + uint32_1)
            End Function
        End Structure

        Public Function variables() As variable_proxy
            Return New variable_proxy(Me)
        End Function
    End Class
End Class
