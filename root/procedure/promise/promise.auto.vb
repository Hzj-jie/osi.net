
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class promise
    Private NotInheritable Class auto
        Inherits thenable

        Public Sub New(ByVal executor As Action(Of Action(Of Object), Action(Of Object)))
            assert(Not executor Is Nothing)
            Try
                executor(AddressOf resolve, AddressOf reject)
            Catch ex As Exception
                log_unhandled_exception(ex)
                reject(ex)
            End Try
        End Sub
    End Class
End Class
