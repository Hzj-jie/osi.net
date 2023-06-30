
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utils

Namespace primitive
    Public NotInheritable Class loaded_method
        Private ReadOnly m As map(Of const_array(Of Byte), Func(Of Byte(), Byte()))
        Private last As Func(Of Byte(), Byte())

        Public Sub New()
            m = New map(Of const_array(Of Byte), Func(Of Byte(), Byte()))()
        End Sub

        Public Sub load(ByVal i() As Byte)
            Dim n As const_array(Of Byte) = Nothing
            n = const_array.of(i)
            Dim it As map(Of const_array(Of Byte), Func(Of Byte(), Byte())).iterator = Nothing
            it = m.find(n)
            If it = m.end() Then
                last = load_method(i)
                assert(m.emplace(n, last).second)
                Return
            End If
            last = (+it).second
        End Sub

        Private Function load_method(ByVal i() As Byte) As Func(Of Byte(), Byte())
            If isemptyarray(i) Then
                raise_error(error_type.user, "Empty method name to load.")
                executor_stop_error.throw(executor.error_type.interrupt_failure)
            End If
            Dim s As String = Nothing
            s = bytes_str(i)
            If s.null_or_whitespace() Then
                raise_error(error_type.user, "Cannot parse method name.")
                executor_stop_error.throw(executor.error_type.interrupt_failure)
            End If
            Dim f As invoker(Of Func(Of Byte(), Byte())) = Nothing
            If Not typeless_invoker.of(Of Func(Of Byte(), Byte())).
                       for_static_methods().
                       with_fully_qualifed_name(s).
                       build(f) Then
                raise_error(error_type.user, "Cannot find method ", s)
                executor_stop_error.throw(executor.error_type.interrupt_failure)
            End If
            Dim bf As Func(Of Byte(), Byte()) = Nothing
            If Not f.pre_bind(bf) Then
                raise_error(error_type.user, "Method ", s, " is not pre-bindable.")
                executor_stop_error.throw(executor.error_type.interrupt_failure)
            End If
            Return bf
        End Function

        Public Function execute(ByVal i() As Byte) As Byte()
            If last Is Nothing Then
                raise_error(error_type.user, "No previously loaded method.")
                executor_stop_error.throw(executor.error_type.interrupt_failure)
            End If
            Try
                Return last(i)
            Catch ex As Exception
                raise_error(error_type.user, "Failed to execute loaded method ", last, " against ", i)
                executor_stop_error.throw(executor.error_type.interrupt_failure)
                Return Nothing
            End Try
        End Function
    End Class
End Namespace
