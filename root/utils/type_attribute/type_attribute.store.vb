
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public Class type_attribute
    Private NotInheritable Class store
        Public Shared ReadOnly m As unique_strong_map(Of comparable_type, store) =
            New unique_strong_map(Of comparable_type, store)()
        Private Shared ReadOnly ctor As Func(Of store) = Function() As store
                                                             Return New store()
                                                         End Function

        Public Shared Function exist(ByVal t As Type) As Boolean
            Return m.exist(New comparable_type(t))
        End Function

        Public Shared Function [get](ByVal t As Type) As store
            Return m.generate(New comparable_type(t), ctor)
        End Function

        Public Sub New()
        End Sub

        Private v As Object

        Private Shared Function check_before_get(ByVal init_mode As init_mode) As Boolean
            Return init_mode = type_attribute.init_mode.exactly_once OrElse
                   init_mode = type_attribute.init_mode.must_be_set
        End Function

        Private Shared Function check_before_set(ByVal init_mode As init_mode) As Boolean
            Return init_mode = type_attribute.init_mode.exactly_once OrElse
                   init_mode = type_attribute.init_mode.once
        End Function

        Public Function [get](ByVal init_mode As init_mode, ByVal forward_mode As forward_mode) As Object
            Dim o As Object = Nothing
            If forward_signal.get(v, o, forward_mode) Then
                Return o
            Else
                If check_before_get(init_mode) Then
                    assert(v IsNot Nothing)
                End If
                Return v
            End If
        End Function

        Public Function [get](Of T)(ByVal init_mode As init_mode, ByVal forward_mode As forward_mode) As T
            Dim o As Object = Nothing
            o = [get](init_mode, forward_mode)
            Return cast(Of T)(o)
        End Function

        Public Sub [set](ByVal i As Object, ByVal init_mode As init_mode, ByVal forward_mode As forward_mode)
            If Not forward_signal.set(v, i, forward_mode) Then
                If check_before_set(init_mode) Then
                    assert(i IsNot Nothing)
                    assert(v Is Nothing)
                End If
                v = i
            End If
        End Sub
    End Class
End Class
