
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class wordtracer
    Partial Public NotInheritable Class cjk
        Public NotInheritable Class tracerall
            Private ReadOnly percentile As Double
            Private ReadOnly max_len As UInt32

            Public Sub New(ByVal percentile As Double, ByVal max_len As UInt32)
                assert(percentile > 0 AndAlso percentile < 1)
                assert(max_len >= 2)
                Me.percentile = percentile
                Me.max_len = max_len
            End Sub

            Private Shared Function process(ByVal f As String,
                                            ByVal len As UInt32,
                                            ByVal sel As Func(Of String, String, Boolean)) _
                                        As unordered_map(Of String, unordered_map(Of String, UInt32))
                assert(len >= uint32_2)
                Dim v As unordered_map(Of String, unordered_map(Of String, UInt32)) = Nothing
                v = New unordered_map(Of String, unordered_map(Of String, UInt32))()
                assert(Not sel Is Nothing)
                For Each line As String In IO.File.ReadLines(f)
                    If line.null_or_whitespace() Then
                        Continue For
                    End If
                    line.strsep(AddressOf _character.cjk,
                                Sub(ByVal start As UInt32, ByVal [end] As UInt32)
                                    If [end] - start < len Then
                                        Return
                                    End If
                                    For j As UInt32 = start To [end] - len
                                        Dim l As String = Nothing
                                        Dim r As String = Nothing
                                        l = line.strmid(j, len - uint32_1)
                                        r = line.char_at(j + len - uint32_1)
                                        If sel(l, r) Then
                                            v(l)(r) += uint32_1
                                        End If
                                    Next
                                End Sub)
                Next
                Return v
            End Function

            Public Function train(ByVal f As String) As vector(Of unordered_map(Of String, UInt32))
                Dim r As vector(Of unordered_map(Of String, UInt32)) = Nothing
                r = New vector(Of unordered_map(Of String, UInt32))()
                For i As UInt32 = 2 To max_len
                    Dim sel As Func(Of String, String, Boolean) = Nothing
                    If i = 2 Then
                        sel = Function(ByVal x As String, ByVal y As String) As Boolean
                                  Return True
                              End Function
                    Else
                        Dim last As unordered_map(Of String, UInt32) = Nothing
                        last = r.back()
                        sel = Function(ByVal x As String, ByVal y As String) As Boolean
                                  Return last.find(x) <> last.end()
                              End Function
                    End If

                    r.emplace_back(New unordered_map(Of String, UInt32)())
                    process(f, i, sel).
                        stream().
                        map(Function(ByVal e As first_const_pair(Of String, unordered_map(Of String, UInt32))) _
                                As first_const_pair(Of String, vector(Of tuple(Of String, UInt32)))
                                assert(Not e Is Nothing)
                                Dim s As vector(Of tuple(Of String, UInt32)) = Nothing
                                s = e.second.
                                      stream().
                                      map(AddressOf tuple(Of String, UInt32).from_first_const_pair).
                                      collect(Of vector(Of tuple(Of String, UInt32)))()
                                Return first_const_pair.of(e.first,
                                                           s.stream().
                                                             filter(ml.percentile.descent.filter(s, percentile)).
                                                             collect(Of vector(Of tuple(Of String, UInt32)))())
                            End Function).
                        foreach(Sub(ByVal e As first_const_pair(Of String, vector(Of tuple(Of String, UInt32))))
                                    e.second.
                                      stream().
                                      foreach(Sub(ByVal t As tuple(Of String, UInt32))
                                                  r.back().emplace(strcat(e.first, t.first()), t.second())
                                              End Sub)
                                End Sub)
                Next
                Return r
            End Function
        End Class
    End Class
End Class
