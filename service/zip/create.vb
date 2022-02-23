
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.argument
Imports osi.service.device
Imports osi.service.transmitter

Public Enum zip_mode
    bypass
    bypass2
    gzip
    deflate
End Enum

<global_init(global_init_level.services)>
Public Module _create
    Public ReadOnly bypass_mode As String = zip_mode.bypass.ToString()

    Public Function create(ByVal mode_str As String,
                           ByVal parameters As var,
                           ByRef o As zipper) As Boolean
        Dim mode As zip_mode = Nothing
        If enum_def.from(mode_str, mode) Then
            Select Case mode
                Case zip_mode.bypass
                    Return bypass.create(parameters, o)
                Case zip_mode.bypass2
                    Return bypass2.create(parameters, o)
                Case zip_mode.gzip
                    Return gzip.create(parameters, o)
                Case zip_mode.deflate
                    Return deflate.create(parameters, o)
                Case Else
                    Return False
            End Select
        Else
            Return False
        End If
    End Function

    Public Function create(ByVal parameters As var, ByRef o As zipper) As Boolean
        Return parameters IsNot Nothing AndAlso
               Not parameters.other_values().empty() AndAlso
               create(parameters.other_values()(0), parameters, o)
    End Function

    Public Function create(ByVal parameter As String, ByRef o As zipper) As Boolean
        Return create(New var(parameter), o)
    End Function

    Public Function create(ByVal mode_str As String, ByVal parameters As var) As zipper
        Dim o As zipper = Nothing
        assert(create(mode_str, parameters, o))
        Return o
    End Function

    Public Function create(ByVal parameter As String) As zipper
        Dim o As zipper = Nothing
        assert(create(parameter, o))
        Return o
    End Function

    Public Function create(ByVal parameters As var) As zipper
        Dim o As zipper = Nothing
        assert(create(parameters, o))
        Return o
    End Function

    Public Function create_bytes_transformer_block_wrapper(ByVal mode As String,
                                                           ByVal v As var,
                                                           ByVal i As block,
                                                           ByVal compress As Boolean,
                                                           ByRef o As bytes_transformer_block_wrapper) As Boolean
        Dim z As zipper = Nothing
        If create(mode, v, z) Then
            Return bytes_transformer_block_wrapper.create(i,
                                                          If(compress,
                                                             z.as_zip_bytes_transformer(),
                                                             z.as_unzip_bytes_transformer()),
                                                          If(compress,
                                                             z.as_unzip_bytes_transformer(),
                                                             z.as_zip_bytes_transformer()),
                                                          o)
        Else
            Return False
        End If
    End Function

    Public Function create_zip_bytes_transformer_block_wrapper(ByVal mode As String,
                                                               ByVal v As var,
                                                               ByVal i As block,
                                                               ByRef o As bytes_transformer_block_wrapper) As Boolean
        Return create_bytes_transformer_block_wrapper(mode, v, i, True, o)
    End Function

    Public Function create_unzip_bytes_transformer_block_wrapper(ByVal mode As String,
                                                                 ByVal v As var,
                                                                 ByVal i As block,
                                                                 ByRef o As bytes_transformer_block_wrapper) As Boolean
        Return create_bytes_transformer_block_wrapper(mode, v, i, False, o)
    End Function

    Private Sub init()
        assert(wrapper.register(wrapper.parameter(
                   "zipper",
                   Function(mode As String,
                            v As var,
                            i As block,
                            ByRef o As block) As Boolean
                       Dim r As bytes_transformer_block_wrapper = Nothing
                       If Not create_zip_bytes_transformer_block_wrapper(mode, v, i, r) Then
                           Return False
                       End If
                       o = r
                       Return True
                   End Function)))
        assert(wrapper.register(wrapper.parameter(
                   "unzipper",
                   Function(mode As String,
                            v As var,
                            i As block,
                            ByRef o As block) As Boolean
                       Dim r As bytes_transformer_block_wrapper = Nothing
                       If Not create_unzip_bytes_transformer_block_wrapper(mode, v, i, r) Then
                           Return False
                       End If
                       o = r
                       Return True
                   End Function)))
    End Sub
End Module
