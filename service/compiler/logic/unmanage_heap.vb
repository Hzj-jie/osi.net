﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Namespace logic
    Public NotInheritable Class _unmanage_heap
        Inherits ptr_type_operator

        Public Sub New(ByVal name As String)
            MyBase.New(name)
        End Sub

        Protected Overrides Function process(ByVal ptr As variable, ByVal o As vector(Of String)) As Boolean
            assert(scope.current().variables().undefine(ptr.name))
            Return True
        End Function
    End Class
End Namespace
