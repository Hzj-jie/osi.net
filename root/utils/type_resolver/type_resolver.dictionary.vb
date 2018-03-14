
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Collections.Generic
Imports osi.root.formation

Partial Public NotInheritable Class type_resolver(Of T)
    Private NotInheritable Class dictionary_storage
        Private ReadOnly m As Dictionary(Of comparable_type, T)

        Public Sub New()
            m = New Dictionary(Of comparable_type, T)()
        End Sub

        Public Function [set](ByVal type As comparable_type, ByVal i As T) As Boolean
            Try
                m.Add(type, i)
            Catch ex As ArgumentException
                Return False
            End Try
            Return True
        End Function

        Public Function [erase](ByVal type As comparable_type) As Boolean
            Return m.Remove(type)
        End Function

        Public Function has(ByVal type As comparable_type) As Boolean
            Return m.ContainsKey(type)
        End Function

        Public Function [get](ByVal type As comparable_type, ByRef o As T) As Boolean
            Return m.TryGetValue(type, o)
        End Function
    End Class
End Class
