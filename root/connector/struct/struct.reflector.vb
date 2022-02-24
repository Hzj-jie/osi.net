
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection

Partial Public Class struct(Of T)
    Private NotInheritable Class reflector
        Inherits struct(Of T)

        Public NotInheritable Class definition
            Inherits struct.definition

            Public ReadOnly getter As Func(Of Object, Object)
            Public ReadOnly setter As Action(Of Object, Object)

            Public Sub New(ByVal fs As FieldInfo)
                MyBase.New(assert_which.of(fs).is_not_null().FieldType(), assert_which.of(fs).is_not_null().Name())
                getter = AddressOf fs.GetValue
                setter = AddressOf fs.SetValue
                assert(Not getter Is Nothing)
                assert(Not setter Is Nothing)
            End Sub

            Public Function variable_of(ByVal i As Object, ByRef o As struct.variable) As Boolean
                Try
                    o = New struct.variable(type, name, getter(i))
                    Return True
                Catch ex As Exception
                    log_unhandled_exception("definition.variable_of", ex)
                    Return False
                End Try
            End Function

            Private Shared Function set_to(Of RT)(ByVal vs() As Object,
                                                  ByVal definitions() As definition,
                                                  ByVal o As RT) As Boolean
                assert(array_size(vs) = array_size(definitions))
                assert(Not o Is Nothing)
                For i As Int32 = 0 To array_size_i(definitions) - 1
                    Try
                        definitions(i).setter(o, vs(i))
                    Catch ex As Exception
                        log_unhandled_exception("definition.set_to", ex)
                        Return False
                    End Try
                Next
                Return True
            End Function

            Public Shared Function set_to(ByVal vs() As Object,
                                          ByVal definitions() As definition,
                                          ByVal o As T) As Boolean
                Return set_to(Of T)(vs, definitions, o)
            End Function

            Public Shared Function value_type_set_to(ByVal vs() As Object,
                                                     ByVal definitions() As definition,
                                                     ByRef o As T) As Boolean
                Dim x As ValueType = Nothing
                x = direct_cast(Of ValueType)(o)
                If Not set_to(Of ValueType)(vs, definitions, x) Then
                    Return False
                End If
                o = direct_cast(Of T)(x)
                Return True
            End Function
        End Class

        Public Shared ReadOnly instance As reflector = New reflector()
        Private Shared ReadOnly defs() As definition = calculate_defs()

        ' Do not initialize until the default implementation is used.
        Private Shared Function calculate_defs() As definition()
            Dim fs() As FieldInfo = Nothing
            fs = GetType(T).GetFields()
            Dim defs(array_size_i(fs) - 1) As definition
            For i As Int32 = 0 To array_size_i(fs) - 1
                defs(i) = New definition(fs(i))
            Next
            Return defs
        End Function

        Public Overrides Function definitions() As struct.definition()
            Return defs
        End Function

        Public Overrides Function disassemble(ByVal i As T, ByRef o() As struct.variable) As Boolean
            If array_size(o) <> array_size(defs) Then
                ReDim o(array_size_i(defs) - 1)
            End If
            For j As Int32 = 0 To array_size_i(defs) - 1
                If Not defs(j).variable_of(i, o(j)) Then
                    Return False
                End If
            Next
            Return True
        End Function

        Public Overrides Function assemble(ByVal vs() As Object, ByRef o As T) As Boolean
            If array_size(vs) <> array_size(defs) Then
                Return False
            End If
            If type_info(Of T).is_valuetype Then
                ' Support struct types by using boxing.
                Return definition.value_type_set_to(vs, defs, o)
            End If
            o = alloc(Of T)()
            Return definition.set_to(vs, defs, o)
        End Function

        Private Sub New()
        End Sub
    End Class
End Class
