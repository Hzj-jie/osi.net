
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.template
Imports osi.root.connector

Public Class accumulate_segment_tree(Of T As IComparable(Of T))
    Inherits segment_tree(Of T, _true)

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal min As Int64, ByVal max As Int64)
        MyBase.New(min, max)
    End Sub

    Public Shared Shadows Function move(ByVal v As accumulate_segment_tree(Of T)) As accumulate_segment_tree(Of T)
        Dim r As accumulate_segment_tree(Of T) = Nothing
        r = New accumulate_segment_tree(Of T)()
        move_to(v, r)
        Return r
    End Function

    Public Shadows Function accumulate(ByVal v As Int64, ByRef o As T) As Boolean
        If root Is Nothing OrElse
           Not root.cover(v) Then
            Return False
        Else
            Return segment_tree(Of T, _true).accumulate(root, v, o)
        End If
    End Function
End Class

Public Class segment_tree(Of T As IComparable(Of T))
    Inherits segment_tree(Of T, _false)

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal min As Int64, ByVal max As Int64)
        MyBase.New(min, max)
    End Sub

    Public Shared Shadows Function move(ByVal v As segment_tree(Of T)) As segment_tree(Of T)
        Dim r As segment_tree(Of T) = Nothing
        r = New segment_tree(Of T)()
        move_to(v, r)
        Return r
    End Function

    Public Shadows Function find(ByVal min As Int64,
                                 ByVal max As Int64) As vector(Of pair(Of iterator, pair(Of Int64, Int64)))
        If min > max OrElse
           root Is Nothing Then
            Return Nothing
        Else
            If min < root_range_min Then
                min = root_range_min
            End If
            If max < root_range_min Then
                max = root_range_min
            End If
            If min > root_range_max Then
                min = root_range_max
            End If
            If max > root_range_max Then
                max = root_range_max
            End If
            Dim r As vector(Of pair(Of iterator, pair(Of Int64, Int64))) = Nothing
            r = New vector(Of pair(Of iterator, pair(Of Int64, Int64)))()
            segment_tree(Of T, _false).find(root, min, max, r)
            Return r
        End If
    End Function

    Public Shadows Function find(ByVal v As Int64) As iterator
        If root Is Nothing OrElse
           Not root.cover(v) Then
            Return Nothing
        Else
            Return segment_tree(Of T, _false).find(root, v)
        End If
    End Function
End Class

Public Class segment_tree(Of T As IComparable(Of T), is_acc As _boolean)
    Inherits bt(Of segment)

    Public Shared ReadOnly boa As binder(Of Func(Of T, T, T), binary_operator_add_protector)
    Private Shared ReadOnly acc As Boolean

    Shared Sub New()
        acc = +(alloc(Of is_acc)())
        If acc Then
            boa = New binder(Of Func(Of T, T, T), binary_operator_add_protector)()
            If isdebugmode() AndAlso
               Not boa.has_value() AndAlso
               Not accumulatable(Of T).v Then
                assert(Not accumulatable(Of T).ex Is Nothing)
                raise_error(error_type.warning,
                              "cannot add a T to another, T = ",
                              GetType(T).FullName(),
                              ", accumulate function is disabled, ex ",
                              accumulatable(Of T).ex.Message())
            End If
        End If
    End Sub

    Public Class segment
        Implements IComparable(Of segment), IComparable, ICloneable

        Private hv As Boolean
        Private v As T

        Friend Sub New(ByVal has_value As Boolean, ByVal value As T)
            Me.hv = has_value
            Me.v = value
        End Sub

        Public Function has_value() As Boolean
            Return hv
        End Function

        Public Function value() As T
            Return v
        End Function

        Friend Sub set_value(ByVal v As T)
            Me.hv = True
            If acc Then
                Me.v = add(Me.v, v)
            Else
                Me.v = v
            End If
        End Sub

        Friend Sub clear()
            Me.hv = False
            Me.v = Nothing
        End Sub

        Friend Sub spread(ByVal n As node)
            assert(Not n Is Nothing)
            If has_value() Then
                n.value().set_value(value())
            End If
        End Sub

        Public Function Clone() As Object Implements ICloneable.Clone
            Return If(Me.has_value(),
                      New segment(True, copy_no_error(Me.value())),
                      New segment(False, Nothing))
        End Function

        Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
            Return CompareTo(cast(Of segment)(obj, False))
        End Function

        Public Function CompareTo(ByVal other As segment) As Int32 Implements IComparable(Of segment).CompareTo
            Dim c As Int32 = 0
            c = object_compare(Me, other)
            If c = object_compare_undetermined Then
                assert(Not other Is Nothing)
                If Me.has_value() = other.has_value() Then
                    If Me.has_value() Then
                        Return connector.compare(Me.value(), other.value())
                    Else
                        Return 0
                    End If
                Else
                    Return If(Me.has_value(), 1, -1)
                End If
            Else
                Return c
            End If
        End Function

        Public Overrides Function ToString() As String
            Return If(has_value(), strcat("has-value = ", value()), "has-no-value")
        End Function
    End Class

    Public Shared Shadows Function move(ByVal v As segment_tree(Of T, is_acc)) As segment_tree(Of T, is_acc)
        Dim r As segment_tree(Of T, is_acc) = Nothing
        r = New segment_tree(Of T, is_acc)()
        move_to(v, r)
        Return r
    End Function

    Private Shared Function add(ByVal i As T, ByVal j As T) As T
        assert(boa.has_value() OrElse accumulatable(Of T).v)
        If boa.has_value() Then
            Return (+boa)(i, j)
        Else
            Return inc(i, j)
        End If
    End Function

    Private Shared Function create_node(ByVal min As Int64,
                                        ByVal max As Int64,
                                        ByVal has_value As Boolean,
                                        ByVal value As T) As node
        Return New node(New range_box(min, max, New segment(has_value, value)))
    End Function

    Private Shared Function create_node(ByVal min As Int64,
                                        ByVal max As Int64) As node
        Return create_node(min, max, False, Nothing)
    End Function

    Protected ReadOnly root_range_min As Int64
    Protected ReadOnly root_range_max As Int64

    Public Sub New()
        Me.New(min_int64, max_int64)
    End Sub

    Public Sub New(ByVal min As Int64, ByVal max As Int64)
        assert(min <= max)
        Me.root_range_min = min
        Me.root_range_max = max
    End Sub

    Public Function insert(ByVal min As Int64, ByVal max As Int64, ByVal value As T) As Boolean
        Return emplace(min, max, If(acc, value, copy_no_error(value)))
    End Function

    Public Function emplace(ByVal min As Int64, ByVal max As Int64, ByVal value As T) As Boolean
        If root Is Nothing Then
            root = create_node(root_range_min, root_range_max)
        End If
        If min <= max AndAlso
           root.cover(min, max) Then
            emplace(root, min, max, value)
            Return True
        Else
        Return False
        End If
    End Function

    Public Function insert(ByVal position As Int64, ByVal value As T) As Boolean
        Return insert(position, position, value)
    End Function

    Public Function emplace(ByVal position As Int64, ByVal value As T) As Boolean
        Return emplace(position, position, value)
    End Function

    Protected Shared Function accumulate(ByVal n As node, ByVal v As Int64, ByRef o As T) As Boolean
        assert(acc)
        assert(Not n Is Nothing)
        assert(n.cover(v))
        Dim r As Boolean = False
        If n.value().has_value() Then
            o = add(o, n.value().value())
            r = True
        End If
        assert(n.has_left_child() = n.has_right_child())
        If n.has_left_child() Then
            If n.left_child().cover(v) Then
                r = accumulate(n.left_child(), v, o) OrElse r
            Else
                assert(n.right_child().cover(v))
                r = accumulate(n.right_child(), v, o) OrElse r
            End If
        End If
        Return r
    End Function

    Protected Shared Sub find(ByVal n As node,
                              ByVal min As Int64,
                              ByVal max As Int64,
                              ByVal r As vector(Of pair(Of iterator, pair(Of Int64, Int64))))
        assert(Not acc)
        assert(min <= max)
        assert(Not n Is Nothing)
        assert(n.cover(min, max))
        assert(Not r Is Nothing)
        If n.value().has_value() OrElse
           n.is_leaf() Then
            r.emplace_back(emplace_make_pair(New iterator(n),
                                             emplace_make_pair(n.range_min(), n.range_max())))
        Else
            assert(n.has_left_child())
            assert(n.has_right_child())
            If min >= n.right_child_range_min() Then
                find(n.right_child(), min, max, r)
            ElseIf max <= n.left_child_range_max() Then
                find(n.left_child(), min, max, r)
            Else
                find(n.left_child(), min, n.left_child_range_max(), r)
                find(n.right_child(), n.right_child_range_min(), max, r)
            End If
        End If
    End Sub

    Protected Shared Function find(ByVal n As node, ByVal v As Int64) As iterator
        assert(Not acc)
        assert(Not n Is Nothing)
        assert(n.cover(v))
        If n.value().has_value() OrElse
           n.is_leaf() Then
            Return New iterator(n)
        Else
            assert(n.has_left_child())
            assert(n.has_right_child())
            If n.left_child().cover(v) Then
                Return find(n.left_child(), v)
            Else
                assert(n.right_child().cover(v))
                Return find(n.right_child(), v)
            End If
        End If
    End Function

    Private Shared Sub emplace(ByVal n As node,
                               ByVal min As Int64,
                               ByVal max As Int64,
                               ByVal value As T)
        assert(Not n Is Nothing)
        assert(min <= max)
        assert(n.cover(min, max))
        If n.equal(min, max) Then
            n.value().set_value(value)
        Else
            If n.is_leaf() Then
                n.replace_left(create_node(n.left_child_range_min(), n.left_child_range_max()))
                n.replace_right(create_node(n.right_child_range_min(), n.right_child_range_max()))
            Else
                assert(n.has_left_child())
                assert(n.has_right_child())
            End If
            If Not acc Then
                n.value().spread(n.left_child())
                n.value().spread(n.right_child())
            End If
            If min >= n.right_child_range_min() Then
                emplace(n.right_child(),
                        min,
                        max,
                        value)
            ElseIf max <= n.left_child_range_max() Then
                emplace(n.left_child(),
                        min,
                        max,
                        value)
            Else
                emplace(n.left_child(),
                        min,
                        n.left_child_range_max(),
                        value)
                emplace(n.right_child(),
                        n.right_child_range_min(),
                        max,
                        value)
            End If
            If Not acc Then
                If n.left_child().value().has_value() AndAlso
                   n.right_child().value().has_value() AndAlso
                   n.subnodes_equal() Then
                    n.value().set_value(n.left_child().value().value())
                Else
                    n.value().clear()
                End If
            End If
        End If
    End Sub
End Class
