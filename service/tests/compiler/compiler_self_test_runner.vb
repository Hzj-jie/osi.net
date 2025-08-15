
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.resource
Imports envs = osi.root.envs

Public MustInherit Class compiler_self_test_runner
    Private Shared filter As argument(Of String)
    Private ReadOnly data() As Byte

    Protected Sub New(ByVal data() As Byte)
        assert(Not data.null_or_empty())
        Me.data = data
    End Sub

    <test>
    Private Sub run()
        Dim a As New vector(Of Action)()
        tar.gen.reader_of(data).foreach(
            Sub(ByVal name As String,
                ByVal enc_precision As Double,
                ByVal content As StreamReader)
                Dim text As String = content.ReadToEnd()
                a.emplace_back(host.forward_current_case(
                    Sub()
                        If Not name.match_pattern(filter Or "*") AndAlso
                           Not name.match_pattern(filter Or "*" + ".txt") Then
                            If envs.deploys.dev_env Then
                                raise_error(error_type.user, "Ignore test case ", name, " through the filter.")
                            End If
                            Return
                        End If
                        If ignore_case(name) Then
                            If envs.deploys.dev_env Then
                                raise_error(error_type.user, "Ignore test case ", name)
                            End If
                            Return
                        End If
                        If envs.deploys.dev_env Then
                            raise_error(error_type.user, "Execute test case ", name)
                        End If
                        Using with_current_file(name)
                            catch_assertion_failure(
                                Sub()
                                    Try
                                        execute(name, text)
                                    Catch ex As Exception
                                        ignore_assertion_break(ex)
                                        raise_error("Exception caught when running ", name, ", ", msg)
                                    End Try
                                End Sub,
                                Sub(ByVal msg As String)
                                    raise_error("Assertion failure happened when running ", name, ", ", msg)
                                End Sub)
                        End Using
                    End Sub))
            End Sub)
        assertions.of(a).not_empty()
        concurrency_runner.execute(+a)
    End Sub

    Protected MustOverride Sub execute(ByVal name As String, ByVal text As String)
    Protected MustOverride Function with_current_file(ByVal filename As String) As IDisposable

    Protected Overridable Function ignore_case(ByVal name As String) As Boolean
        Return False
    End Function
End Class
