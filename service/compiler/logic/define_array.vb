
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Namespace logic
    ' Define an array with @name, @type and @size
    Public NotInheritable Class define_array
        Implements exportable

        Public Function export(ByVal scope As scope,
                               ByVal o As vector(Of String)) As Boolean Implements exportable.export
            Return assert(False)
        End Function
    End Class
End Namespace
