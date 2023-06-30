
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class case2
    Private NotInheritable Class random_run_case
        Inherits [case]

        Private ReadOnly r As random_run(Of Boolean)

        Public Sub New(ByVal t As Type,
                       ByVal prepare As Func(Of Object, Boolean),
                       ByVal finish As Func(Of Object, Boolean),
                       ByVal reserved_processors As Int16,
                       ByVal randoms As vector(Of random_function_info))
            MyBase.New(t, Nothing, prepare, Nothing, finish, reserved_processors)
            r = New random_run(Of Boolean)()
            assert(Not randoms.null_or_empty())
            For i As UInt32 = 0 To randoms.size() - uint32_1
                Dim f As random_function_info = Nothing
                f = randoms(i)
                assert(Not f Is Nothing)
                assert(Not f.f Is Nothing)
                r.insert_call(f.percentage,
                              Function() As Boolean
                                  Return f.f(obj)
                              End Function)
            Next
        End Sub

        Public Overrides Function run() As Boolean
            Return r.select()
        End Function
    End Class
End Class
