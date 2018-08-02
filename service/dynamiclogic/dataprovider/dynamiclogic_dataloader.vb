
Option Explicit On
Option Infer Off
Option Strict On

Imports System
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.dataprovider
Imports osi.service.compiler.dotnet

Public NotInheritable Class dynamiclogic_dataloader
    Implements idataloader(Of Func(Of Object(), Object))

    Private ReadOnly language As source_executor.language
    Private ReadOnly type_name As String
    Private ReadOnly function_name As String

    Public Sub New(ByVal language As source_executor.language,
                   ByVal type_name As String,
                   ByVal function_name As String)
        assert(Not String.IsNullOrWhiteSpace(type_name))
        assert(Not String.IsNullOrWhiteSpace(function_name))
        Me.language = language
        Me.type_name = type_name
        Me.function_name = function_name
    End Sub

    Public Function load(ByVal localfile As String,
                         ByVal result As pointer(Of Func(Of Object(), Object))) As event_comb _
                        Implements idataloader(Of Func(Of Object(), Object)).load
        Return sync_async(Function() As Boolean
                              Dim se As source_executor = Nothing
                              Dim r As Func(Of Object(), Object) = Nothing
                              Return source_executor.[New]().
                                         with_language(language).
                                         add_file(localfile).
                                         build(se) AndAlso
                                     se.functor(type_name, function_name, r) AndAlso
                                     eva(result, r)
                          End Function)
    End Function
End Class
