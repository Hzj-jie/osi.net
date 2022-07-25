
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class scope
        Public Function current_function() As current_function_proxy
            Return New current_function_proxy(Me,
                                              current_function_t.ctor(Function(ByVal i As String) As String
                                                                          Return scope.current().type_alias()(i)
                                                                      End Function),
                                              Sub(ByVal i As scope, ByVal j As current_function_t)
                                                  assert(Not i Is Nothing)
                                                  i.cf = j
                                              End Sub,
                                              Function(ByVal i As scope) As current_function_t
                                                  assert(Not i Is Nothing)
                                                  Return i.cf
                                              End Function,
                                              Function(ByVal i As scope, ByVal j As String) As Boolean
                                                  assert(Not i Is Nothing)
                                                  Return i.structs().types().defined(j)
                                              End Function)
        End Function
    End Class
End Class
