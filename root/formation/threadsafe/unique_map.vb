
Option Explicit On
Option Infer Off
Option Strict On

#Const USE_DUALLOCK = False

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.lock

#If Not USE_DUALLOCK Then
Imports lock_t = osi.root.lock.slimlock.monitorlock
#End If

Public MustInherit Class unique_map(Of KEY_T As IComparable(Of KEY_T), STORE_T, VALUE_T)
    Private ReadOnly m As unordered_map(Of KEY_T, STORE_T)
#If USE_DUALLOCK Then
    Private l As duallock
#Else
    Private l As lock_t
#End If

    Public Sub New()
        m = New unordered_map(Of KEY_T, STORE_T)()
    End Sub

    Protected MustOverride Function store_value(ByVal i As STORE_T, ByRef o As VALUE_T) As Boolean
    Protected MustOverride Function value_store(ByVal i As VALUE_T) As STORE_T

    Private Function reader_locked(Of T)(ByVal f As Func(Of T)) As T
#If USE_DUALLOCK Then
        Return l.reader_locked(f)
#Else
        Return l.locked(f)
#End If
    End Function

    Private Sub reader_locked(ByVal f As Action)
#If USE_DUALLOCK Then
        l.reader_locked(f)
#Else
        l.locked(f)
#End If
    End Sub

    Private Function writer_locked(Of T)(ByVal f As Func(Of T)) As T
#If USE_DUALLOCK Then
        Return l.writer_locked(f)
#Else
        Return l.locked(f)
#End If
    End Function

    Private Sub writer_locked(ByVal f As Action)
#If USE_DUALLOCK Then
        l.writer_locked(f)
#Else
        l.locked(f)
#End If
    End Sub

    Public Function size() As UInt32
        Static f As Func(Of UInt32) = Function() m.size()
        Return reader_locked(f)
    End Function

    Public Function empty() As Boolean
        Static f As Func(Of Boolean) = Function() m.empty()
        Return reader_locked(f)
    End Function

    Public Sub clear()
        Static f As Action = Sub() m.clear()
        writer_locked(f)
    End Sub

    Private Function unlocked_erase(ByVal key As KEY_T, ByRef v As VALUE_T) As Boolean
        If key Is Nothing Then
            Return False
        End If
        Dim it As unordered_map(Of KEY_T, STORE_T).iterator = Nothing
        it = m.find(key)
        If it = m.end() Then
            Return False
        End If
        store_value((+it).second, v)
        Return assert(m.erase(it))
    End Function

    Public Function [erase](ByVal key As KEY_T, Optional ByRef v As VALUE_T = Nothing) As Boolean
        Dim t As VALUE_T = Nothing
        If writer_locked(Function() As Boolean
                             Return unlocked_erase(key, t)
                         End Function) Then
            v = t
            Return True
        End If
        Return False
    End Function

    Public Function [erase](ByVal keys As vector(Of KEY_T),
                            Optional ByVal vs As vector(Of VALUE_T) = Nothing) As Boolean
        If keys Is Nothing Then
            Return False
        End If
        If keys.empty() Then
            Return True
        End If
        If Not vs Is Nothing Then
            vs.clear()
        End If
        Return writer_locked(Function() As Boolean
                                 Dim r As Boolean = False
                                 r = True
                                 For i As UInt32 = uint32_0 To keys.size() - uint32_1
                                     Dim v As VALUE_T = Nothing
                                     If unlocked_erase(keys(i), v) Then
                                         If Not vs Is Nothing Then
                                             vs.emplace_back(v)
                                         End If
                                     Else
                                         r = False
                                     End If
                                 Next
                                 Return r
                             End Function)
    End Function

    Public Function [get](Of T)(ByVal key As KEY_T, ByVal action As Func(Of VALUE_T, T)) As T
        If action Is Nothing OrElse key Is Nothing Then
            Return Nothing
        End If
        Return reader_locked(Function() As T
                                 Dim v As VALUE_T = Nothing
                                 Dim it As unordered_map(Of KEY_T, STORE_T).iterator = Nothing
                                 it = m.find(key)
                                 If it <> m.end() AndAlso
                                    store_value((+it).second, v) Then
                                     Return action(v)
                                 End If
                                 Return Nothing
                             End Function)
    End Function

    Public Function [get](ByVal key As KEY_T, ByVal action As Action(Of VALUE_T)) As Boolean
        If action Is Nothing Then
            Return False
        End If
        Return [get](key,
                     Function(ByVal x As VALUE_T) As Boolean
                         action(x)
                         Return True
                     End Function)
    End Function

    Public Function [get](ByVal key As KEY_T, ByRef v As VALUE_T) As Boolean
        Dim v2 As VALUE_T = Nothing
        If [get](key, Sub(x) v2 = x) Then
            v = v2
            Return True
        End If
        Return False
    End Function

    Public Function [get](ByVal key As KEY_T) As VALUE_T
        Dim o As VALUE_T = Nothing
        assert([get](key, o))
        Return o
    End Function

    Public Function [get](ByVal keys() As KEY_T, ByRef o As vector(Of VALUE_T)) As Boolean
        Dim r As Boolean = False
        r = True
        o.renew()
        For i As Int32 = 0 To array_size_i(keys) - 1
            Dim t As VALUE_T = Nothing
            If [get](keys(i), t) Then
                o.emplace_back(t)
            Else
                r = False
            End If
        Next
        Return r
    End Function

    Public Function [get](ByVal ParamArray names() As KEY_T) As vector(Of VALUE_T)
        Dim r As vector(Of VALUE_T) = Nothing
        assert([get](names, r))
        Return r
    End Function

    'v is IN and OUT parameter
    'v is always the value in the collection
    'return true if the value has been changed to v
    'otherwise v has been changed to the value in the collection
    Private Function get_set(ByVal k As KEY_T, ByRef v As VALUE_T) As Boolean
        If k Is Nothing Then
            Return False
        End If
        Dim o As VALUE_T = Nothing
        o = v
        Dim result As Boolean = False
        result = writer_locked(Function() As Boolean
                                   Dim it As unordered_map(Of KEY_T, STORE_T).iterator = Nothing
                                   Dim r As VALUE_T = Nothing
                                   it = m.find(k)
                                   If it = m.end() OrElse Not store_value((+it).second, r) Then
                                       m.insert(k, value_store(o))
                                       Return True
                                   End If
                                   o = r
                                   Return False
                               End Function)
        v = o
        Return result
    End Function

    Public Function [set](ByVal key As KEY_T, ByVal v As VALUE_T) As Boolean
        Return get_set(key, v)
    End Function

    Public Function replace(ByVal key As KEY_T, ByVal v As VALUE_T) As Boolean
        If key Is Nothing Then
            Return False
        End If
        Return writer_locked(Function() As Boolean
                                 m(key) = value_store(v)
                                 Return True
                             End Function)
    End Function

    Public Function exist(ByVal key As KEY_T) As Boolean
        Dim r As VALUE_T = Nothing
        Return [get](key, r)
    End Function

    Public Function generate(ByVal key As KEY_T, ByVal ctor As Func(Of VALUE_T)) As VALUE_T
        assert(Not key Is Nothing)
        assert(Not ctor Is Nothing)
        Dim v As VALUE_T = Nothing
        If [get](key, v) Then
            Return v
        End If
        v = ctor()
        get_set(key, v)
        Return v
    End Function

    Public Function generate(ByVal key As KEY_T) As VALUE_T
        Return generate(key, AddressOf alloc(Of VALUE_T))
    End Function

    Public Function generate(Of VT As {VALUE_T, New})(ByVal key As KEY_T) As VALUE_T
        Return generate(key, Function() New VT())
    End Function

    Public Sub foreach(ByVal f As Action(Of KEY_T, VALUE_T))
        assert(Not f Is Nothing)
        reader_locked(Sub()
                          m.stream().foreach(m.on_pair(Sub(ByVal k As KEY_T, ByVal s As STORE_T)
                                                           Dim v As VALUE_T = Nothing
                                                           If store_value(s, v) Then
                                                               f(k, v)
                                                           End If
                                                       End Sub))
                      End Sub)
    End Sub
End Class
