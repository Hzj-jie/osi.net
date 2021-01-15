
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.resource

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

            Private Shared Sub train_line(ByVal line As String,
                                          ByVal len As UInt32,
                                          ByVal sel As Func(Of String, String, Boolean),
                                          ByVal result As unordered_map(Of String, unordered_map(Of Char, UInt32)))
                assert(Not line Is Nothing)
                assert(len >= uint32_2)
                assert(Not sel Is Nothing)
                If line.null_or_whitespace() Then
                    Return
                End If
                line.strsep(AddressOf _character.cjk,
                            Sub(ByVal start As UInt32, ByVal [end] As UInt32)
                                If [end] - start < len Then
                                    Return
                                End If
                                For j As UInt32 = start To [end] - len
                                    Dim l As String = Nothing
                                    Dim r As Char = Nothing
                                    l = line.strmid(j, len - uint32_1)
                                    r = line.char_at(j + len - uint32_1)
                                    If sel(l, r) Then
                                        result(l)(r) += uint32_1
                                    End If
                                Next
                            End Sub)
            End Sub

            Private Shared Function process(ByVal f As String,
                                            ByVal len As UInt32,
                                            ByVal sel As Func(Of String, String, Boolean)) _
                                           As unordered_map(Of String, unordered_map(Of Char, UInt32))
                Dim v As unordered_map(Of String, unordered_map(Of Char, UInt32)) =
                    New unordered_map(Of String, unordered_map(Of Char, UInt32))()
                For Each line As String In IO.File.ReadLines(f)
                    train_line(line, len, sel, v)
                Next
                Return v
            End Function

            Private Shared Function process(ByVal t As tar.reader,
                                            ByVal len As UInt32,
                                            ByVal sel As Func(Of String, String, Boolean)) _
                                           As unordered_map(Of String, unordered_map(Of Char, UInt32))
                assert(Not t Is Nothing)
                Dim v As unordered_map(Of String, unordered_map(Of Char, UInt32)) =
                    New unordered_map(Of String, unordered_map(Of Char, UInt32))()
                t.foreach(Sub(ByVal name As String, ByVal m As MemoryStream)
                              Using r As StreamReader = New StreamReader(m, m.guess_encoding())
                                  Dim line As String = r.ReadLine()
                                  While Not line Is Nothing
                                      train_line(line, len, sel, v)
                                      line = r.ReadLine()
                                  End While
                              End Using
                          End Sub)
                Return v
            End Function

            Private Function train(ByVal process As Func(Of UInt32, Func(Of String, String, Boolean),
                                   unordered_map(Of String, unordered_map(Of Char, UInt32)))) _
                                  As vector(Of unordered_map(Of String, UInt32))
                assert(Not process Is Nothing)
                Dim r As vector(Of unordered_map(Of String, UInt32)) = New vector(Of unordered_map(Of String, UInt32))()
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
                    process(i, sel).
                        stream().
                        map(Function(ByVal e As first_const_pair(Of String, unordered_map(Of Char, UInt32))) _
                                As first_const_pair(Of String, vector(Of tuple(Of Char, UInt32)))
                                assert(Not e Is Nothing)
                                Dim s As vector(Of tuple(Of Char, UInt32)) = Nothing
                                s = e.second.
                                      stream().
                                      map(AddressOf tuple(Of Char, UInt32).from_first_const_pair).
                                      collect(Of vector(Of tuple(Of Char, UInt32)))()
                                Return first_const_pair.of(e.first,
                                                           s.stream().
                                                             filter(ml.percentile.descent.filter(s, percentile)).
                                                             collect(Of vector(Of tuple(Of Char, UInt32)))())
                            End Function).
                        foreach(Sub(ByVal e As first_const_pair(Of String, vector(Of tuple(Of Char, UInt32))))
                                    e.second.
                                      stream().
                                      foreach(Sub(ByVal t As tuple(Of Char, UInt32))
                                                  r.back().emplace(strcat(e.first, t.first()), t.second())
                                              End Sub)
                                End Sub)
                Next
                Return r
            End Function

            Public Function train(ByVal f As String) As vector(Of unordered_map(Of String, UInt32))
                Return train(Function(ByVal len As UInt32,
                                      ByVal sel As Func(Of String, String, Boolean)) _
                                     As unordered_map(Of String, unordered_map(Of Char, UInt32))
                                 Return process(f, len, sel)
                             End Function)
            End Function

            Public Function train(ByVal t As tar.reader) As vector(Of unordered_map(Of String, UInt32))
                Return train(Function(ByVal len As UInt32,
                                      ByVal sel As Func(Of String, String, Boolean)) _
                                     As unordered_map(Of String, unordered_map(Of Char, UInt32))
                                 Return process(t, len, sel)
                             End Function)
            End Function
        End Class
    End Class
End Class
