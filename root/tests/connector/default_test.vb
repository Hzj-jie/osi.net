
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.envs
Imports osi.root.utt

' Performance of default(Of T).null, default.null(Of T)() and DirectCast(Nothing, T) are on-par.
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
                       repeat(New default_without_shared_new_null_case(), size),
                       repeat(New direct_case_case(), size),
                       repeat(New generic_function_case(), size))
        End Sub

        Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
            If isdebugbuild() Then
                Return loosen_bound({9714, 7902, 5539, 7005}, i, j)
            Else
                If os.windows_major <= os.windows_major_t._5 Then
                    Return loosen_bound({1095, 100, 125, 208}, i, j)
                Else
                    Return loosen_bound({3850, 795, 833, 754}, i, j)
                End If
            End If
        End Function

        Private Class default_null_case
            Inherits [case]

            Private Class [default](Of T)
                Public Shared ReadOnly null As T

                Shared Sub New()
                    null = Nothing
                End Sub
            End Class

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

        Private Class default_without_shared_new_null_case
            Inherits [case]

            Private Class [default](Of T)
                Public Shared ReadOnly null As T = Nothing
            End Class

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
