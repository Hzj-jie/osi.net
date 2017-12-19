
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.procedure
Imports osi.root.utils
Imports bind_module_type = osi.service.http.bind_module(Of
                                    osi.root.delegates._do_val_ref(Of osi.service.http.server.context,
                                                                      osi.root.procedure.event_comb,
                                                                      Boolean))

Public NotInheritable Class prebind_module
    Inherits bind_module_type

    Private ReadOnly i As _do_val_ref(Of server.context, event_comb, Boolean)

    Private Sub New(ByVal name As String, ByVal i As _do_val_ref(Of server.context, event_comb, Boolean))
        MyBase.New(name)
        assert(Not i Is Nothing)
        Me.i = i
    End Sub

    Public Shared Shadows Function [New](ByVal type As String,
                                         ByVal assembly As String,
                                         ByVal binding_flags As BindingFlags,
                                         ByVal function_name As String) As prebind_module
        Return bind_module_type.[New](type,
                                      assembly,
                                      binding_flags,
                                      function_name,
                                      Function(ByVal invoker) As prebind_module
                                          assert(Not invoker Is Nothing)
                                          Dim i As _do_val_ref(Of server.context, event_comb, Boolean) = Nothing
                                          If invoker.pre_bind(i) Then
                                              Return New prebind_module(invoker.identity(), i)
                                          Else
                                              Return Nothing
                                          End If
                                      End Function)
    End Function

    Protected Overrides Function execute(ByVal context As server.context, ByRef o As event_comb) As Boolean
        Return i(context, o)
    End Function
End Class
