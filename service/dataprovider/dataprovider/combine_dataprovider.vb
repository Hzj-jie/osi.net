

'actually it should be dataprovider_dataprovider
Public Class combine_dataprovider(Of T)
    Inherits dataprovider(Of T)

    Private Const default_trigger_rule As dataprovider_datawatcher.trigger_rule = _
                        dataprovider_datawatcher.trigger_rule.one

    Public Sub New(ByVal rule As dataprovider_datawatcher.trigger_rule,
                   ByVal loader As dataprovider_dataloader(Of T),
                   ByVal ParamArray dps() As idataprovider)
        MyBase.New(New dataprovider_datawatcher(rule, dps),
                   empty_datafetcher.instance,
                   loader)
    End Sub

    Public Sub New(ByVal rule As dataprovider_datawatcher.trigger_rule,
                   ByVal loader As Func(Of idataprovider(), T),
                   ByVal ParamArray dps() As idataprovider)
        Me.New(rule, New dataprovider_dataloader(Of T)(loader, dps), dps)
    End Sub

    Public Sub New(ByVal rule As dataprovider_datawatcher.trigger_rule,
                   ByVal loader_ctor As Func(Of idataprovider(), dataprovider_dataloader(Of T)),
                   ByVal ParamArray dps() As idataprovider)
        Me.New(rule, loader_ctor(dps), dps)
    End Sub

    Public Sub New(ByVal loader As dataprovider_dataloader(Of T),
                   ByVal ParamArray dps() As idataprovider)
        Me.New(default_trigger_rule, loader, dps)
    End Sub

    Public Sub New(ByVal loader As Func(Of idataprovider(), T),
                   ByVal ParamArray dps() As idataprovider)
        Me.New(default_trigger_rule, loader, dps)
    End Sub

    Public Sub New(ByVal loader_ctor As Func(Of idataprovider(), dataprovider_dataloader(Of T)),
                   ByVal ParamArray dps() As idataprovider)
        Me.New(default_trigger_rule, loader_ctor, dps)
    End Sub
End Class
