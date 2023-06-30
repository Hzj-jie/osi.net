
Option Explicit On
Option Infer Off
Option Strict On

Imports Microsoft
Imports System.CodeDom.Compiler
Imports osi.root.connector

Namespace dotnet
    Partial Public Class source_executor
        Partial Public NotInheritable Class builder
            Public Function build(ByRef o As source_executor) As Boolean
                Dim result As CompilerResults = Nothing
                If Not sources.empty() Then
                    result = compiler().CompileAssemblyFromSource(compiler_parameters(), +sources)
                ElseIf Not files.empty() Then
                    result = compiler().CompileAssemblyFromFile(compiler_parameters(), +files)
                Else
                    assert(False)
                    Return Nothing
                End If
                If result.Errors().HasErrors() Then
                    Return False
                End If
                o = New source_executor(result.CompiledAssembly())
                Return True
            End Function

            Public Function build() As source_executor
                Dim o As source_executor = Nothing
                assert(build(o))
                Return o
            End Function

            Private Function compiler_parameters() As CompilerParameters
                Dim param As CompilerParameters = Nothing
                param = New CompilerParameters()
                param.GenerateExecutable() = False
                param.GenerateInMemory() = True
                param.ReferencedAssemblies().AddRange(+references)
                Return param
            End Function

            Private Function compiler() As CodeDomProvider
                Select Case l
                    Case language.cs
                        Return New CSharp.CSharpCodeProvider()
                    Case language.vbnet
                        Return New VisualBasic.VBCodeProvider()
                    Case Else
                        assert(False)
                        Return Nothing
                End Select
            End Function
        End Class
    End Class
End Namespace