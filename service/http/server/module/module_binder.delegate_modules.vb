
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.procedure

Partial Public NotInheritable Class module_binder
    Private NotInheritable Class delegate_module
        Implements module_handle.module

        Private ReadOnly filter As context_filter
        Private ReadOnly action As Func(Of server.context, Boolean)

        Private Sub New(ByVal filter As context_filter, ByVal action As Func(Of server.context, Boolean))
            assert(Not filter Is Nothing)
            assert(Not action Is Nothing)
            Me.filter = filter
            Me.action = action
        End Sub

        Public Shared Function [New](ByVal filter As context_filter,
                                     ByVal action As Func(Of server.context, Boolean)) As module_handle.module
            Return New delegate_module(filter, action)
        End Function

        Public Function context_received(ByVal context As server.context) As Boolean _
                                        Implements module_handle.module.context_received
            Return filter.select(context) AndAlso action(context)
        End Function
    End Class

    Private NotInheritable Class delegate_procedure_module
        Inherits procedure_module

        Private ReadOnly filter As context_filter
        Private ReadOnly action As _do_val_ref(Of server.context, event_comb, Boolean)

        Private Sub New(ByVal filter As context_filter,
                        ByVal action As _do_val_ref(Of server.context, event_comb, Boolean))
            assert(Not filter Is Nothing)
            assert(Not action Is Nothing)
            Me.filter = filter
            Me.action = action
        End Sub

        Public Shared Function [New](ByVal filter As context_filter,
                                     ByVal action As _do_val_ref(Of server.context, event_comb, Boolean)) _
                                    As delegate_procedure_module
            Return New delegate_procedure_module(filter, action)
        End Function

        Protected Overrides Function process(ByVal context As server.context, ByRef ec As event_comb) As Boolean
            Return filter.select(context) AndAlso action(context, ec)
        End Function
    End Class

    Private NotInheritable Class delegate_filtered_module
        Inherits filtered_module

        Private ReadOnly action As Action(Of server.context)

        Private Sub New(ByVal filter As context_filter, ByVal action As Action(Of server.context))
            MyBase.New(filter)
            assert(Not action Is Nothing)
            Me.action = action
        End Sub

        Public Shared Function [New](ByVal filter As context_filter,
                                     ByVal action As Action(Of server.context)) As module_handle.module
            Return New delegate_filtered_module(filter, action)
        End Function

        Protected Overrides Sub process(ByVal context As server.context)
            action(context)
        End Sub
    End Class

    Private NotInheritable Class delegate_filtered_procedure_module
        Inherits filtered_procedure_module

        Private ReadOnly action As Func(Of server.context, event_comb)

        Private Sub New(ByVal filter As context_filter, ByVal action As Func(Of server.context, event_comb))
            MyBase.New(filter)
            assert(Not action Is Nothing)
            Me.action = action
        End Sub

        Public Shared Function [New](ByVal filter As context_filter,
                                     ByVal action As Func(Of server.context, event_comb)) As module_handle.module
            Return New delegate_filtered_procedure_module(filter, action)
        End Function

        Protected Overrides Function execute(ByVal context As server.context) As event_comb
            Return action(context)
        End Function
    End Class
End Class
