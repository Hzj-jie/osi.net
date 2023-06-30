
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.lock.slimlock

Public Module _autolock
    ' Just in case we can have value type reference one day
    Private Structure ref_autolock(Of T As {islimlock, Class})
        Implements IDisposable

        Private ReadOnly i As T

        Public Sub New(ByRef i As T)
            assert(Not i Is Nothing)
            Me.i = i
            Me.i.wait()
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Me.i.release()
        End Sub
    End Structure

    Private Structure wrapper_autolock(Of T As {islimlock, Structure})
        Implements IDisposable

        Private ReadOnly i As ref(Of T)

        Public Sub New(ByRef i As ref(Of T))
            assert(Not i Is Nothing)
            Me.i = i
            Me.i.p.wait()
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Me.i.p.release()
        End Sub
    End Structure

    Public Function make_autolock(Of T As {islimlock, Class})(ByRef i As T) As IDisposable
        Return New ref_autolock(Of T)(i)
    End Function

    Public Function make_autolock(Of T As {islimlock, Structure})(ByRef i As ref(Of T)) As IDisposable
        Return New wrapper_autolock(Of T)(i)
    End Function
End Module
