
Imports System.Runtime.CompilerServices
Imports osi.root.connector

Public Module _extension
    <Extension()> Public Function free(ByVal i As ref(Of forks)) As Boolean
        assert(i IsNot Nothing)
        Return i.p.free()
    End Function

    <Extension()> Public Function not_free(ByVal i As ref(Of forks)) As Boolean
        assert(i IsNot Nothing)
        Return i.p.not_free()
    End Function

    <Extension()> Public Function mark_as(ByVal i As ref(Of forks), ByVal s As Int32) As Boolean
        assert(i IsNot Nothing)
        Return i.p.mark_as(s)
    End Function

    <Extension()> Public Sub mark_free(ByVal i As ref(Of forks))
        assert(i IsNot Nothing)
        i.p.mark_free()
    End Sub

    <Extension()> Public Sub release(ByVal i As ref(Of forks))
        assert(i IsNot Nothing)
        i.p.release()
    End Sub

    <Extension()> Public Function null_or_not_in_use(ByVal i As ref(Of singleentry)) As Boolean
        Return i Is Nothing OrElse
               i.p.not_in_use()
    End Function

    <Extension()> Public Function not_null_and_in_use(ByVal i As ref(Of singleentry)) As Boolean
        Return i IsNot Nothing AndAlso
               i.p.in_use()
    End Function

    <Extension()> Public Function null_or_in_use(ByVal i As ref(Of singleentry)) As Boolean
        Return i Is Nothing OrElse
               i.p.in_use()
    End Function

    <Extension()> Public Function not_null_and_not_in_use(ByVal i As ref(Of singleentry)) As Boolean
        Return i IsNot Nothing AndAlso
               i.p.not_in_use()
    End Function

    <Extension()> Public Function in_use(ByVal i As ref(Of singleentry)) As Boolean
        assert(i IsNot Nothing)
        Return i.p.in_use()
    End Function

    <Extension()> Public Function not_in_use(ByVal i As ref(Of singleentry)) As Boolean
        assert(i IsNot Nothing)
        Return i.p.not_in_use()
    End Function

    <Extension()> Public Function mark_in_use(ByVal i As ref(Of singleentry)) As Boolean
        assert(i IsNot Nothing)
        Return i.p.mark_in_use()
    End Function

    <Extension()> Public Sub release(ByVal i As ref(Of singleentry))
        assert(i IsNot Nothing)
        i.p.release()
    End Sub

    <Extension()> Public Sub mark_not_in_use(ByVal i As ref(Of singleentry))
        assert(i IsNot Nothing)
        i.p.mark_not_in_use()
    End Sub
End Module
