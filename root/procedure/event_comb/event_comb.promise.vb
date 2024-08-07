
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Partial Public Class event_comb
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Widening Operator CType(ByVal this As promise) As event_comb
        Return from(Of Object)(this)
    End Operator

    Public Shared Function from(Of T)(ByVal this As promise,
                                      Optional ByVal result As ref(Of T) = Nothing) As event_comb
        If this Is Nothing Then
            Return Nothing
        End If
        Dim succeeded As Boolean = False
        Dim restore As Action = Nothing
        Return New event_comb(Function() As Boolean
                                  restore = event_comb.wait()
                                  this.then(Sub(ByVal r As Object)
                                                If Not result Is Nothing Then
                                                    result.set(direct_cast(Of T)(r))
                                                End If
                                                succeeded = True
                                                restore()
                                            End Sub,
                                            Sub()
                                                If Not result Is Nothing Then
                                                    result.set([default](Of T).null)
                                                End If
                                                succeeded = False
                                                restore()
                                            End Sub)
                                  Return goto_next()
                              End Function,
                                  Function() As Boolean
                                      Return succeeded AndAlso
                                             goto_end()
                                  End Function)
    End Function
End Class
