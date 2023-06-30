
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.constants

<global_init(global_init_level.functor)>
Friend NotInheritable Class string_serializer_registry2
    Shared Sub New()
        string_serializer.register(Sub(ByVal i As String, ByVal o As StringWriter)
                                       assert(Not o Is Nothing)
                                       o.Write(i)
                                   End Sub,
                                   Function(ByVal i As StringReader, ByRef o As String) As Boolean
                                       assert(Not i Is Nothing)
                                       o = i.ReadToEnd()
                                       Return True
                                   End Function)
        json_serializer.register(Sub(ByVal i As String, ByVal o As StringWriter)
                                     assert(Not o Is Nothing)
                                     o.Write("""")
                                     o.Write(i)
                                     o.Write("""")
                                 End Sub)
        json_serializer.register(Sub(ByVal i As Char, ByVal o As StringWriter)
                                     assert(Not o Is Nothing)
                                     o.Write("""")
                                     o.Write(i)
                                     o.Write("""")
                                 End Sub)
    End Sub

    Private Shared Sub init()
    End Sub

    Private Sub New()
    End Sub
End Class
