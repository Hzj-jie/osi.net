
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation

Namespace dotnet
    Partial Public Class source_executor
        Partial Public NotInheritable Class builder
            Private ReadOnly sources As vector(Of String)
            Private ReadOnly files As vector(Of String)
            Private ReadOnly references As vector(Of String)
            Private l As language

            Public Sub New()
                sources = New vector(Of String)()
                files = New vector(Of String)()
                references = New vector(Of String)()
            End Sub

            Public Function with_language(ByVal l As language) As builder
                Me.l = l
                Return Me
            End Function

            Public Function add_source(ByVal s As String) As builder
                sources.emplace_back(s)
                Return Me
            End Function

            Public Function add_file(ByVal f As String) As builder
                files.emplace_back(f)
                Return Me
            End Function

            Public Function add_reference(ByVal f As String) As builder
                references.emplace_back(f)
                Return Me
            End Function

            Public Function add_sources(ByVal s() As String) As builder
                sources.emplace_back(s)
                Return Me
            End Function

            Public Function add_files(ByVal f() As String) As builder
                files.emplace_back(f)
                Return Me
            End Function

            Public Function add_references(ByVal f() As String) As builder
                references.emplace_back(f)
                Return Me
            End Function
        End Class
    End Class
End Namespace
