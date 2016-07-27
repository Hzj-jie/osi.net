
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.interpreter.fullstack.executor

Namespace fullstack.executor
    Public Class assignment_test
        Inherits [case]

        Private ReadOnly size As Int32

        Public Sub New()
            size = rnd_int(10, 100)
        End Sub

        Public Overrides Function run() As Boolean
            'TODO: assignment_test
            Return True
        End Function
    End Class
End Namespace
