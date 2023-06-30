
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils
Imports osi.service.argument
Imports osi.service.device

<global_init(global_init_level.services)>
Public Class gzip
    Implements zipper

    'do not support any parameters for now, so the gzip instance can be shared
    Public Shared ReadOnly instance As gzip

    Shared Sub New()
        instance = New gzip()
    End Sub

    Private Sub New()
    End Sub

    Public Shared Function create(ByVal v As var, ByRef o As zipper) As Boolean
        o = instance
        Return True
    End Function

    Public Function unzip(ByVal i() As Byte,
                          ByVal offset As UInt32,
                          ByVal count As UInt32,
                          ByRef o() As Byte) As Boolean Implements zipper.unzip
        Return _compress.ungzip(i, offset, count, o)
    End Function

    Public Function zip(ByVal i() As Byte,
                        ByVal offset As UInt32,
                        ByVal count As UInt32,
                        ByRef o() As Byte) As Boolean Implements zipper.zip
        Return _compress.gzip(i, offset, count, o)
    End Function

    Private Shared Sub init()
        assert(constructor.register("gzip",
                                    Function(v As var, ByRef o As zipper) As Boolean
                                        Return create(v, o)
                                    End Function))
    End Sub
End Class
