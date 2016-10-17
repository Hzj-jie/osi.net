
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.envs
Imports osi.root.utt

Public Class default_test
    Inherits chained_case_wrapper

    Public Sub New()
        MyBase.New(New default_null_perf())
    End Sub

    ' [default](Of T).null has an almost same behavior as DirectCast(T, Nothing).
    Private Class default_null_perf
        Inherits performance_comparison_case_wrapper

        Private Class temp_class
        End Class

        Private Const size As Int64 = 1024 * 1024 * 128

        Public Sub New()
            MyBase.New(repeat(New default_null_case(), size),
                       repeat(New direct_case_case(), size),
                       repeat(New generic_function_case(), size))
        End Sub

        Protected Overrides Function min_rate_table() As Double(,)
            If isdebugbuild() Then
                If os.windows_major <= os.windows_major_t._5 Then
                    Return {{0, 2.5, 1.2},
                            {0.6, 0, 0.5},
                            {1.5, 2.8, 0}}
                End If
                Return {{0, 1.8, 1.2},
                        {0.9, 0, 0.8},
                        {1.5, 2.3, 0}}
            Else
                Return {{0, 6, 2.5},
                        {0.4, 0, 0.8},
                        {0.6, 2, 0}}
            End If
        End Function

        Private Class default_null_case
            Inherits [case]

            Private Shared Function null(Of T)() As T
                Return [default](Of T).null
            End Function

            Public Overrides Function run() As Boolean
                Dim x As String = [default](Of String).null
                Dim y As temp_class = [default](Of temp_class).null
                Dim z As Int32 = [default](Of Int32).null
                Dim i As String = null(Of String)()
                Dim j As temp_class = null(Of temp_class)()
                Dim k As Int32 = null(Of Int32)()
                Return True
            End Function
        End Class

        Private Class direct_case_case
            Inherits [case]

            Private Shared Function null(Of T)() As T
                Return DirectCast(Nothing, T)
            End Function

            Public Overrides Function run() As Boolean
                Dim x As String = DirectCast(Nothing, String)
                Dim y As temp_class = DirectCast(Nothing, temp_class)
                Dim z As Int32 = DirectCast(Nothing, Int32)
                Dim i As String = null(Of String)()
                Dim j As temp_class = null(Of temp_class)()
                Dim k As Int32 = null(Of Int32)()
                Return True
            End Function
        End Class

        Private Class generic_function_case
            Inherits [case]

            Private Shared Function null(Of T)() As T
                Return Nothing
            End Function

            Public Overrides Function run() As Boolean
                Dim x As String = null(Of String)()
                Dim y As temp_class = null(Of temp_class)()
                Dim z As Int32 = null(Of Int32)()
                Dim i As String = null(Of String)()
                Dim j As temp_class = null(Of temp_class)()
                Dim k As Int32 = null(Of Int32)()
                Return True
            End Function
        End Class
    End Class
End Class
