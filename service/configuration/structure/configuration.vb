
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.dataprovider

Public Class configuration
    Public Shared ReadOnly [default] As configuration

    Shared Sub New()
        [default] = New configuration()
    End Sub

    Private ReadOnly c As characters
    Private ReadOnly fs As filter_selector
    Private ReadOnly sv As vector(Of pair(Of String, String))
    Private ReadOnly provider_ctor As Func(Of String, idataloader(Of config), String, dataprovider(Of config))
    Private ReadOnly loader As idataloader(Of config)
    Private ReadOnly loader_name As String

    Public Sub New(Optional ByVal c As characters = Nothing,
                   Optional ByVal fs As filter_selector = Nothing,
                   Optional ByVal static_variants As vector(Of pair(Of String, String)) = Nothing,
                   Optional ByVal provider_ctor As Func(Of String, 
                                                           idataloader(Of config),
                                                           String, 
                                                           dataprovider(Of config)) = Nothing,
                   Optional ByVal loader_ctor As Func(Of istreamdataloader(Of config), 
                                                         idataloader(Of config)) = Nothing,
                   Optional ByVal loader_ctor2 As Func(Of idataloader(Of config)) = Nothing)
        If c Is Nothing Then
            Me.c = characters.default
        Else
            Me.c = c
        End If

        If fs Is Nothing Then
            Me.fs = default_filter_selector()
        Else
            Me.fs = fs
        End If

        Me.sv = combine_static_variants(static_variants)

        If loader_ctor2 Is Nothing Then
            Dim csd As config_streamreader_dataloader = Nothing
            csd = New config_streamreader_dataloader(Me.c, Me.fs, Me.sv)
            If loader_ctor Is Nothing Then
                Me.loader = New filestream_dataloader(Of config)(csd)
            Else
                Me.loader = loader_ctor(csd)
            End If
        Else
            Me.loader = loader_ctor2()
        End If
        assert(Not loader Is Nothing)

        Me.loader_name = Me.loader.GetType().AssemblyQualifiedName()

        If provider_ctor Is Nothing Then
            Me.provider_ctor = AddressOf config_dataprovider.generate
        Else
            Me.provider_ctor = provider_ctor
        End If
    End Sub

    Private Function provider(ByVal file As String) As dataprovider(Of config)
        Dim r As dataprovider(Of config) = Nothing
        r = provider_ctor(file, loader, loader_name)
        assert(Not r Is Nothing)
        Return r
    End Function

    Private Function providers(ByVal files() As String) As dataprovider(Of config)()
        Dim r() As dataprovider(Of config) = Nothing
        ReDim r(array_size(files) - 1)
        For i As Int32 = 0 To array_size(files) - 1
            r(i) = provider(files(i))
        Next
        Return r
    End Function

    Public Sub preload(ByVal file As String)
        provider(file)
    End Sub

    Public Sub unload(ByVal file As String)
        Dim r As dataprovider(Of config) = Nothing
        r = provider_ctor(file, loader, loader_name)
        assert(Not r Is Nothing)
        r.expire()
    End Sub

    Default Public ReadOnly Property v(ByVal file As String) As config
        Get
            Dim o As config = Nothing
            assert([get](file, o))
            Return o
        End Get
    End Property

    Public Function [get](ByVal file As String, ByRef config As config) As Boolean
        Dim p As dataprovider(Of config) = Nothing
        p = provider(file)
        If p.valid() Then
            config = p.get()
            Return True
        Else
            Return False
        End If
    End Function

    Private Function [get](ByVal file As String,
                           ByVal config As pointer(Of config),
                           ByVal w As Func(Of Func(Of Boolean), Boolean)) As event_comb
        assert(Not w Is Nothing)
        Dim p As dataprovider(Of config) = Nothing
        Return New event_comb(Function() As Boolean
                                  p = provider(file)
                                  Return w(Function() p.valid()) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If p.valid() Then
                                      Return eva(config, p.get()) AndAlso
                                             goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Public Function [get](ByVal file As String, ByVal config As pointer(Of config)) As event_comb
        Return [get](file, config, Function(x) waitfor(x))
    End Function

    Public Function [get](ByVal file As String,
                          ByVal config As pointer(Of config),
                          ByVal timeout_ms As Int64) As event_comb
        Return [get](file, config, Function(x) waitfor(x, timeout_ms))
    End Function

    Public Function create_config_object(Of T)(ByVal d As Func(Of T),
                                               ByVal ParamArray files() As String) As config_object(Of T)
        Return New config_object(Of T)(providers(files), d)
    End Function

    Public Function create_config_object(Of T)(ByVal loader As config_object_loader(Of T),
                                               ByVal ParamArray files() As String) As config_object(Of T)
        Return New config_object(Of T)(providers(files), loader)
    End Function
End Class
