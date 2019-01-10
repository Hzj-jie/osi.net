
Option Explicit On
Option Infer Off
Option Strict On

' TODO: Move to type_info.
#Const cached_cast = True
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.delegates

Public Module _cast
    <Extension()> Public Function cast_to(Of T, IT)(ByVal i As IT, ByRef o As T) As Boolean
        Return cast(Of T, IT)(i, o)
    End Function

    <Extension()> Public Function cast_to(Of T, IT)(ByVal i As IT) As T
        Return cast(Of T, IT)(i)
    End Function

    <Extension()> Public Function cast_to(Of T)(ByVal this As Object, ByRef o As T) As Boolean
        Return cast(Of T)(this, o)
    End Function

    <Extension()> Public Function cast_to(Of T)(ByVal this As Object) As T
        Return cast(Of T)(this)
    End Function

    <Extension()> Public Function direct_cast_to(Of T, IT)(ByVal i As IT, ByRef o As T) As Boolean
        Return direct_cast(Of T, IT)(i, o)
    End Function

    <Extension()> Public Function direct_cast_to(Of T, IT)(ByVal i As IT) As T
        Return direct_cast(Of T, IT)(i)
    End Function

    <Extension()> Public Function direct_cast_to(Of T)(ByVal i As Object, ByRef o As T) As Boolean
        Return direct_cast(Of T)(i, o)
    End Function

    <Extension()> Public Function direct_cast_to(Of T)(ByVal i As Object) As T
        Return direct_cast(Of T)(i)
    End Function

    Private Sub cast_assert(Of T)(ByVal i As Object, ByVal rst As Boolean, ByVal require_assert As Boolean)
        If Not rst AndAlso require_assert Then
            'may not be able to get type from i, if i is nothing
            Try
                'the call GetType() and GetType(t) uses lots of time, so in this way
                assert(False, "cannot convert from ", i.GetType().FullName(), " to ", GetType(T).FullName())
            Catch ex As Exception
                assert(False, "failed to raise assert error ", ex.Message())
            End Try
        End If
    End Sub

    Private Function _direct_cast(Of T)(ByVal i As Object, ByRef o As T) As Boolean
        assert(Not i Is Nothing)
        Try
            o = DirectCast(i, T)
            assert(Not o Is Nothing)
            Return True
        Catch
            Return False
        End Try
    End Function

    Private Function c_nothing(Of T, IT)(ByVal i As IT, ByRef o As T) As Boolean
        If Not i Is Nothing Then
            Return False
        End If
        o = Nothing
        Return True
    End Function

    Private Function change_type(Of T, IT)(ByVal i As IT, ByRef o As T) As Boolean
        'for mono, the CType = Conversions.ToGenericParameter here,
        'but the function does not implement correctly now
        Try
            o = DirectCast(Convert.ChangeType(i, GetType(T)), T)
            Return True
        Catch
            Return False
        End Try
    End Function

    Private Function c_type(Of T)(ByVal i As Object, ByRef o As T) As Boolean
        Try
            o = CType(i, T)
            Return True
        Catch
            Return False
        End Try
    End Function

#If cached_cast Then
    Private Structure runtime_casting_cache(Of T, IT)
        Private Shared ReadOnly c As _do_val_ref(Of IT, T, Boolean)

        Private Shared Function select_casting(ByVal ms() As MethodInfo,
                                               ByVal itt As Type,
                                               ByVal n As String,
                                               ByRef c As _do_val_ref(Of IT, T, Boolean)) As Boolean
            assert(Not itt Is Nothing)
            If isemptyarray(ms) Then
                Return False
            Else
                Dim ott As Type = Nothing
                ott = GetType(T)
                For j As Int32 = 0 To array_size_i(ms) - 1
                    If Not ms(j) Is Nothing AndAlso
                       ms(j).Name() = n AndAlso
                       array_size(ms(j).GetParameters()) = 1 AndAlso
                       itt.is(ms(j).GetParameters()(0).ParameterType()) AndAlso
                       ms(j).ReturnType().is(ott) Then
                        Dim m As MethodInfo = Nothing
                        m = ms(j)
                        'closure
                        c = Function(i As IT, ByRef o As T) As Boolean
                                o = DirectCast(m.Invoke(Nothing, New Object() {i}), T)
                                Return True
                            End Function
                        Return True
                    End If
                Next
                Return False
            End If
        End Function

        Private Shared Function select_casting(ByVal ms() As MethodInfo,
                                               ByVal itt As Type,
                                               ByRef c As _do_val_ref(Of IT, T, Boolean)) As Boolean
            Return select_casting(ms, itt, implicit_cast_operator, c) OrElse
                   select_casting(ms, itt, explicit_cast_operator, c)
        End Function

        Private Shared Function select_casting(ByVal ms() As MethodInfo,
                                               ByRef c As _do_val_ref(Of IT, T, Boolean)) As Boolean
            Return select_casting(ms, GetType(IT), c)
        End Function

        Private Shared Function select_casting(ByVal ct As Type,
                                               ByVal it As Type,
                                               ByRef c As _do_val_ref(Of IT, T, Boolean)) As Boolean
            assert(Not ct Is Nothing)
            Return select_casting(ct.GetMethods(cast_operator_binding_flags), it, c)
        End Function

        Private Shared Function select_casting(Of CT)(ByVal it As Type,
                                                      ByRef c As _do_val_ref(Of IT, T, Boolean)) As Boolean
            assert(Not it Is Nothing)
            assert(GetType(T) Is GetType(CT) OrElse GetType(IT) Is GetType(CT))
            Return select_casting(GetType(CT), it, c)
        End Function

        Private Shared Function select_casting(Of CT)(ByRef c As _do_val_ref(Of IT, T, Boolean)) As Boolean
            assert(GetType(T) Is GetType(CT) OrElse GetType(IT) Is GetType(CT))
            Return select_casting(Of CT)(GetType(IT), c)
        End Function

        Private Shared Function select_casting(ByRef c As _do_val_ref(Of IT, T, Boolean)) As Boolean
            c = Function(i As IT, ByRef o As T) As Boolean
                    assert(Not i Is Nothing)
                    Dim it As Type = Nothing
                    it = i.GetType()
                    Dim v As _do_val_ref(Of IT, T, Boolean) = Nothing
                    'for object
                    If select_casting(it, it, v) OrElse select_casting(Of T)(it, v) Then
                        Return v(i, o)
                    Else
                        Return False
                    End If
                End Function
            Return True
        End Function

        Shared Sub New()
            Dim t As Boolean = False
            t = select_casting(Of IT)(c) OrElse
                select_casting(Of T)(c) OrElse
                select_casting(c)
            assert(Not c Is Nothing)
        End Sub

        Public Shared Function cast(ByVal i As IT, ByRef o As T) As Boolean
            Return c(i, o)
        End Function
    End Structure

    Private Function runtime_casting(Of T, IT)(ByVal i As IT, ByRef o As T) As Boolean
        Return runtime_casting_cache(Of T, IT).cast(i, o)
    End Function
#Else
    Private Function runtime_casting(Of T, IT)(ByVal i As IT,
                                               ByRef o As T,
                                               ByVal ms() As MethodInfo,
                                               ByVal n As String) As Boolean
        assert(Not isemptyarray(ms))
        Dim itt As Type = Nothing
        Dim ott As Type = Nothing
        itt = If(i Is Nothing, GetType(IT), i.GetType())
        ott = GetType(T)
        For j As Int32 = 0 To array_size(ms) - 1
            If Not ms(j) Is Nothing AndAlso
               ms(j).Name() = n AndAlso
               array_size(ms(j).GetParameters()) = 1 AndAlso
               itt.is(ms(j).GetParameters()(0).ParameterType()) AndAlso
               ms(j).ReturnType().is(ott) Then
                Try
                    o = DirectCast(ms(j).Invoke(Nothing, New Object() {i}), T)
                    Return True
                Catch
                    Return False
                End Try
            End If
        Next
        Return False
    End Function

    Private Function runtime_casting(Of T, IT)(ByVal i As IT, ByRef o As T, ByVal ms() As MethodInfo) As Boolean
        If isemptyarray(ms) Then
            Return False
        Else
            Return runtime_casting(i, o, ms, implicit_cast_operator) OrElse
                   runtime_casting(i, o, ms, explicit_cast_operator)
        End If
    End Function

    Private Function runtime_casting(Of T, IT, CT)(ByVal i As IT, ByRef o As T) As Boolean
#If DEBUG Then
        assert(GetType(T) Is GetType(CT) OrElse
                 GetType(IT) Is GetType(CT))
#End If
        Return runtime_casting(i, o, GetType(CT).GetMethods(cast_operator_binding_flags))
    End Function

    Private Function runtime_casting_a_b(Of T, IT)(ByVal i As IT, ByRef o As T) As Boolean
        Return If(i Is Nothing,
                  False,
                  runtime_casting(i, o, i.GetType().GetMethods(cast_operator_binding_flags))) OrElse
               runtime_casting(Of T, IT, IT)(i, o)
    End Function

    Private Function runtime_casting_b_a(Of T, IT)(ByVal i As IT, ByRef o As T) As Boolean
        Return runtime_casting(Of T, IT, T)(i, o)
    End Function

    Private Function runtime_casting(Of T, IT)(ByVal i As IT, ByRef o As T) As Boolean
        Return runtime_casting_a_b(i, o) OrElse
               runtime_casting_b_a(i, o)
    End Function
#End If

    Private Function runtime_casting_proxy(Of T, IT)(ByVal i As IT, ByRef o As T) As Boolean
        Try
            Return runtime_casting(i, o)
        Catch
            Return False
        End Try
    End Function

    Public NotInheritable Class cast_type_inferrer(Of T)
        Public Shared ReadOnly instance As cast_type_inferrer(Of T)

        Shared Sub New()
            instance = New cast_type_inferrer(Of T)()
        End Sub

        Public Function from(Of IT)(ByVal i As IT, Optional ByVal require_assert As Boolean = True) As T
            Return cast(Of T, IT)(i, require_assert)
        End Function

        Public Function from(Of IT)(ByVal i As IT, ByRef o As T) As Boolean
            Return into(i, o)
        End Function

        ' To avoid conflicting with cast(Of Boolean)().from("str", b).
        Public Function into(Of IT)(ByVal i As IT, ByRef o As T) As Boolean
            Return cast(i, o)
        End Function

        Private Sub New()
        End Sub
    End Class

    Public Structure cast_cache(Of T)
        Private ReadOnly i As T

        Public Sub New(ByVal i As T)
            Me.i = i
        End Sub

        ' To avoid conflicting with cast_from("str").to(b).
        Public Function into(Of OT)(ByRef o As OT) As Boolean
            Return cast(i, o)
        End Function

        Public Function [to](Of OT)(ByRef o As OT) As Boolean
            Return into(o)
        End Function

        Public Function [to](Of OT)(Optional ByVal require_assert As Boolean = True) As OT
            Return cast(Of OT, T)(i, require_assert)
        End Function
    End Structure

    Public Function cast(Of T, IT)(ByVal i As IT, ByRef o As T) As Boolean
        If c_nothing(i, o) Then
            Return True
        End If
        If caster(Of IT, T).defined() Then
            o = caster(Of IT, T).cast(i)
            Return True
        End If
        If _direct_cast(i, o) Then
            Return True
        End If
        If on_mono() AndAlso change_type(i, o) Then
            Return True
        End If
        If c_type(i, o) Then
            Return True
        End If
        If runtime_casting_proxy(i, o) Then
            Return True
        End If

        Return False
    End Function

    Public Function cast(Of T)() As cast_type_inferrer(Of T)
        Return cast_type_inferrer(Of T).instance
    End Function

    ' To avoid conflicting with Dim r as T = cast(Of T)(a).
    Public Function cast_from(Of T)(ByVal i As T) As cast_cache(Of T)
        Return New cast_cache(Of T)(i)
    End Function

    Public Function cast(Of T)(ByVal i As Object, ByRef o As T) As Boolean
        If Not is_suppressed.by("cast(Of T)(Object v)") AndAlso
           typed_once_action(Of cast_type_inferrer(Of T)).first() Then
            raise_error(error_type.performance,
                    "cast(Of ",
                    GetType(T).Name(),
                    ")(i) seriously impacts performance. cast(Of T)().from(i) or cast_from(i).to(o) is preferred: ",
                    backtrace("_cast"))
        End If
        Return cast(Of T, Object)(i, o)
    End Function

    Public Function direct_cast(Of T, IT)(ByVal i As IT, ByRef o As T) As Boolean
        Return c_nothing(i, o) OrElse
               _direct_cast(i, o)
    End Function

    Public Function direct_cast(Of T)(ByVal i As Object, ByRef o As T) As Boolean
        Return direct_cast(Of T, Object)(i, o)
    End Function

    Public Function cast(Of T, IT)(ByVal i As IT, Optional ByVal require_assert As Boolean = True) As T
        Dim o As T = Nothing
        cast_assert(Of T)(i, cast(Of T, IT)(i, o), require_assert)
        Return o
    End Function

    Public Function cast(Of T)(ByVal i As Object, Optional ByVal require_assert As Boolean = True) As T
        Dim o As T = Nothing
        cast_assert(Of T)(i, cast(Of T)(i, o), require_assert)
        Return o
    End Function

    Public Function direct_cast(Of T, IT)(ByVal i As IT, Optional ByVal require_assert As Boolean = True) As T
        Dim o As T = Nothing
        cast_assert(Of T)(i, direct_cast(Of T, IT)(i, o), require_assert)
        Return o
    End Function

    Public Function direct_cast(Of T)(ByVal i As Object, Optional ByVal require_assert As Boolean = True) As T
        Dim o As T = Nothing
        cast_assert(Of T)(i, direct_cast(Of T)(i, o), require_assert)
        Return o
    End Function
End Module
