
Option Explicit On
Option Infer Off
Option Strict On

#Const USE_UNORDERED_MAP = True

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class type_resolver(Of T)
    Private NotInheritable Class storage
#If USE_UNORDERED_MAP Then
        Private ReadOnly m As unordered_map_storage
#Else
        Private ReadOnly m As dictionary_storage
#End If

        Public Sub New()
#If USE_UNORDERED_MAP Then
            m = New unordered_map_storage()
#Else
            m = New dictionary_storage()
#End If
        End Sub

        ' Asserts "set" happened.
        Public Sub [set](ByVal type As Type, ByVal i As T)
            assert(m.set(New comparable_type(type), copy_no_error(i)))
        End Sub

        ' Returns true if "erase" happened.
        Public Function [erase](ByVal type As Type) As Boolean
            Return m.erase(New comparable_type(type))
        End Function

        ' Returns true if type is found.
        Public Function has(ByVal type As Type) As Boolean
            Return m.has(New comparable_type(type))
        End Function

        ' Returns true if type is found and the T associated with it has been assigned to the result.
        Public Function [get](ByVal type As Type, ByRef o As T) As Boolean
            Return m.get(New comparable_type(type), o)
        End Function
    End Class
End Class
