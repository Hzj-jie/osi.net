
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports System.Text
Imports osi.root.connector
Imports osi.root.event
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.root.utils

Public Class dataprovider(Of T)
    Implements idataprovider

    Private ReadOnly watcher As idatawatcher
    Private ReadOnly fetcher As idatafetcher
    Private ReadOnly loader As idataloader(Of T)
    Private ReadOnly localfile As String
    Private ReadOnly exp As expiration_controller.settable
    Private ReadOnly on_update As weak_event
    Private v As T
    Private lr As Int64
    Private lu As Int64

    Public Sub New(ByVal watcher As idatawatcher, ByVal fetcher As idatafetcher, ByVal loader As idataloader(Of T))
        assert(Not watcher Is Nothing)
        assert(Not fetcher Is Nothing)
        assert(Not loader Is Nothing)
        Me.watcher = watcher
        Me.fetcher = fetcher
        Me.loader = loader
        Me.v = Nothing
        Me.lr = 0
        Me.lu = 0
        Me.localfile = Path.Combine(temp_folder, guid_str())
        Me.exp = expiration_controller.settable.[New]()
        Me.on_update = New weak_event()
        Me.start()
    End Sub

    Private Sub update(ByVal v As T)
        Me.v = v
        Me.lu = nowadays.ticks()
        Me.on_update.raise()
    End Sub

    Private Sub start()
        Dim w As event_comb = Nothing
        Dim f As event_comb = Nothing
        Dim l As event_comb = Nothing
        Dim r As pointer(Of T) = Nothing
        r = New pointer(Of T)()
        begin_lifetime_event_comb(exp,
                                  Function() As Boolean
                                      w = watcher.watch(exp)
                                      assert(Not w Is Nothing)
                                      assert_waitfor(w)
                                      Return goto_next()
                                  End Function,
                                  Function() As Boolean
                                      If w.end_result() Then
                                          f = fetcher.fetch(localfile)
                                          assert(Not f Is Nothing)
                                          assert_waitfor(f)
                                          Return goto_next()
                                      Else
                                          Return goto_begin()
                                      End If
                                  End Function,
                                  Function() As Boolean
                                      If f.end_result() Then
                                          r.clear()
                                          l = loader.load(localfile, r)
                                          assert(Not l Is Nothing)
                                          assert_waitfor(l)
                                          Return goto_next()
                                      Else
                                          Return goto_begin()
                                      End If
                                  End Function,
                                  Function() As Boolean
                                      If l.end_result() Then
                                          assert(Not +r Is Nothing)
                                          update(+r)
                                      End If
                                      File.Delete(localfile)
                                      Return goto_begin()
                                  End Function)
    End Sub

    Public Function get_object() As Object Implements idataprovider.get_object
        Return [get]()
    End Function

    Public Function last_refered_ticks() As Int64 Implements idataprovider.last_refered_ticks
        Return lr
    End Function

    Public Function last_updated_ticks() As Int64 Implements idataprovider.last_updated_ticks
        Return lu
    End Function

    Public Function [get]() As T
        assert(valid())
        Return v
    End Function

    Public Function valid() As Boolean Implements idataprovider.valid
        lr = nowadays.ticks()
        Return Not Me.v Is Nothing AndAlso
               Me.lu > 0
    End Function

    Public Sub expire() Implements idataprovider.expire
        exp.stop()
    End Sub

    Public Function expired() As Boolean Implements idataprovider.expired
        Return exp.expired()
    End Function

    Public Sub register_update_event(Of OT)(ByVal v As OT,
                                            ByVal d As Action(Of OT)) Implements idataprovider.register_update_event
        assert(on_update.attach(v, d))
    End Sub

    Protected Shared Function name(ByVal t As Type,
                                   ByVal file As String,
                                   ByVal ParamArray p() As pair(Of String, Object)) As String
        Return strcat(t.AssemblyQualifiedName(), "://", file, merge(p))
    End Function

    Protected Shared Function name(Of IMPL)(ByVal file As String,
                                            ByVal ParamArray p() As pair(Of String, Object)) As String
        Return name(GetType(IMPL), file, p)
    End Function

    Private Shared Function merge(ByVal p() As pair(Of String, Object)) As String
        Dim r As StringBuilder = Nothing
        r = New StringBuilder()
        For i As Int32 = 0 To array_size_i(p) - 1
            assert(Not p(i) Is Nothing)
            r.Append("&").
              Append(Convert.ToString(p(i).first)).
              Append("=").
              Append(Convert.ToString(p(i).second))
        Next
        r.Chars(0) = "?"c
        Return Convert.ToString(r)
    End Function

    Protected Shared Function parameter(ByVal name As String, ByVal value As Object) As pair(Of String, Object)
        Return pair.emplace_of(name, value)
    End Function
End Class
