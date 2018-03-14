
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation

Partial Public NotInheritable Class type_resolver(Of T)
    Private NotInheritable Class unordered_map_storage
        Private ReadOnly m As unordered_map(Of comparable_type, T)

        Public Sub New()
            m = New unordered_map(Of comparable_type, T)()
        End Sub

        Public Function [set](ByVal type As comparable_type, ByVal i As T) As Boolean
            Return m.emplace(type, i).second
        End Function

        Public Function [erase](ByVal type As comparable_type) As Boolean
            Return m.erase(type)
        End Function

        Public Function has(ByVal type As comparable_type) As Boolean
            Return m.find(type) <> m.end()
        End Function

        Public Function [get](ByVal type As comparable_type, ByRef o As T) As Boolean
            Dim it As unordered_map(Of comparable_type, T).iterator = Nothing
            it = m.find(type)
            If it = m.end() Then
                Return False
            End If
            o = (+it).second
            Return True
        End Function
    End Class
End Class
