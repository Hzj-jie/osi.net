
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class scope
        Inherits scope(Of scope)

        Private ReadOnly incs As includes_t
        Private ReadOnly fc As call_hierarchy_t
        Private ReadOnly d As define_t
        Private ReadOnly ta As New type_alias_t()
        Private ReadOnly s As New struct_t()
        Private ReadOnly v As New variable_t()
        Private ReadOnly f As function_t
        Private ReadOnly vt As New value_target_t()
        Private ReadOnly ps As New params_t()
        Private cf As current_function_t
        Private ReadOnly de As New delegate_t()
        Private ReadOnly t As temp_logic_name_t

        <inject_constructor>
        Public Sub New(ByVal parent As scope)
            MyBase.New(parent)
        End Sub

        Public Sub New()
            Me.New(Nothing)
            incs = New includes_t()
            fc = New call_hierarchy_t()
            d = New define_t()
            f = New function_t()
            t = New temp_logic_name_t()
        End Sub

        Public Function includes() As includes_t
            Return from_root(Function(ByVal i As scope) As includes_t
                                 assert(Not i Is Nothing)
                                 Return i.incs
                             End Function)
        End Function

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

        Public Function defines() As define_t
            Return from_root(Function(ByVal i As scope) As define_t
                                 assert(Not i Is Nothing)
                                 Return i.d
                             End Function)
        End Function

        Public Function delegates() As delegate_proxy
            Return New delegate_proxy(Me,
                                      Function(ByVal s As scope) As delegate_t
                                          assert(Not s Is Nothing)
                                          Return s.de
                                      End Function,
                                      Function(ByVal s As scope, ByVal i As String) As String
                                          assert(Not s Is Nothing)
                                          Return s.type_alias()(i)
                                      End Function)
        End Function
    End Class
End Class
