
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt

Partial Public Class vector_test
    Partial Private Class vector_case
        Private Sub size()
            If validation Then
                Dim oldsize As Int64
                oldsize = v.size()

                Dim i As Int64
                Dim till As Int64
                till = rnd_int(0, 1024)
                For i = 0 To till - 1
                    v.push_back(guid_str())
                Next

                assertion.equal(v.size() - oldsize, till,
                             "cannot update size property after ", till, " push_back operations.")
            Else
                Dim t As UInt32 = 0
                t = v.size()
            End If
        End Sub

        Private Sub clear()
            Dim old_size As UInt32 = 0
            old_size = v.size()
            v.clear()
            assertion.equal(v.size(), uint32_0, "v.size() <> 0 after clear.")
            If validation Then
                For i As Int32 = 0 To CInt(v.capacity()) - 1
                    assertion.is_null(v.data()(i), "cannot clear ", i, " after clear operation.")
                Next
            End If
        End Sub

        Private Sub data()
            If validation Then
                Dim a() As String = Nothing
                rndarray(a)

                Dim oldsize As UInt32 = 0
                oldsize = v.size()
                assertion.is_true(v.push_back(a))

                assertion.equal(oldsize + array_size(a), v.size())
                assertion.is_true(memcmp(v.data(), oldsize, a, 0, array_size(a)) = 0, "caught v.data <> a.data.")
            Else
                v.data()
            End If
        End Sub

        Private Sub push_back()
            Dim s As String = Nothing
            s = guid_str()

            Dim oldsize As Int64 = 0
            oldsize = v.size()
            v.push_back(s)

            assertion.equal(v.size(), oldsize + 1, "v.size() - oldsize <> 1 after one push_back operation.")
            assertion.equal(v.back(), s, "v.back() <> s.")
        End Sub

        Private Sub pop_back()
            Dim oldsize As Int64 = 0
            oldsize = v.size()

            Dim s As String = Nothing
            s = guid_str()
            v.push_back(s)

            v.pop_back()
            If validation Then
                assertion.equal(v.find(s), npos)
                assertion.equal(v.size(), oldsize)
            End If
        End Sub

        Private Sub insert()
            'make sure there is at least one element, to avoid breaking v.back()
            v.push_back(guid_str())
            Dim oldsize As UInt32 = 0
            oldsize = v.size()
            Dim last_back As String = Nothing
            last_back = v.back()

            Dim s As String = Nothing
            s = guid_str()
            Dim p As UInt32 = 0
            If rnd_bool() Then
                p = oldsize
            Else
                p = rnd_uint(0, oldsize)
            End If
            v.insert(p, s)

            If validation Then
                assertion.equal(v.find(s), CInt(p))
                If p = oldsize Then
                    assertion.equal(v.size(), p + uint32_1)
                    assertion.equal(v.find(last_back), CInt(oldsize - uint32_1))
                Else
                    'do not allow nothing to be inserted in to the vector to avoid breaking erase case
                    assert(p < oldsize)
                    assertion.equal(oldsize, v.size() - 1)
                    'oldsize + 1 - 1 = v.size() - 1
                    assertion.equal(v.find(last_back), CInt(oldsize))
                End If
            End If
        End Sub

        Private Sub find()
            If validation Then
                Dim a() As String = Nothing
                rndarray(a)

                Dim oldsize As Int32 = 0
                oldsize = CInt(v.size())
                assertion.is_true(v.push_back(a))

                For i As Int32 = 0 To a.Length() - 1
                    assertion.equal(v.find(a(i)), oldsize + i, "caught a(", i, ") is not at the place expected.")
                Next
            Else
                v.find(guid_str())
            End If
        End Sub

        Private Sub fill()
            'confirm there is at lease one element in vector
            v.push_back(guid_str())

            Dim a() As String = Nothing
            v.fill(a)

            assertion.is_not_null(a, "found a is nothing")
            assertion.equal(CUInt(a.Length()), v.size(), "a.length()<>v.size()")
            If validation Then
                assertion.is_true(memcmp(v.data(), 0, a, 0, array_size(a)) = 0, "found v is not same as a.")
            End If
        End Sub

        Private Sub empty()
            v.clear()
            assertion.is_true(v.empty(), "v is not empty after clear.")
        End Sub

        Private Sub front()
            v.clear()
            Dim s As String = Nothing
            s = guid_str()
            v.push_back(s)
            assertion.equal(v.front(), s, "v.front <> s.")
        End Sub

        Private Sub back()
            Dim s As String = Nothing
            s = guid_str()
            v.push_back(s)
            assertion.equal(v.back(), s, "v.back <> s.")
        End Sub

        Private Sub [erase]()
            'make sure there is at least one element in the vector
            v.push_back(guid_str())
            Dim a() As String = Nothing
            Dim i As UInt32 = 0
            Dim j As UInt32 = 0
            i = rnd_uint(0, v.size())
            j = rnd_uint(0, v.size())
            If i > j Then
                swap(i, j)
            End If

            If validation Then
                ReDim a(CInt(j - i + uint32_1))
                arrays.copy(a, 0, v.data(), i, j - i)
            End If

            v.erase(i, j)

            'confirm at least one element here.
            v.push_back(guid_str())
            i = rnd_uint(0, v.size())
            If validation Then
                copy(a(a.Length() - 2), v(i))
            End If
            v.erase(i)

            'confirm at least one element here.
            v.push_back(guid_str())
            i = rnd_uint(0, v.size())
            If validation Then
                copy(a(a.Length() - 1), v(i))
            End If
            assertion.is_true(v.erase(v(i)))

            If validation Then
                For k As Int32 = 0 To a.Length() - 1
                    assertion.equal(v.find(a(k)), npos, "still find ", a(k), " in vector.")
                Next
            End If
        End Sub

        Private Sub shrink_to_fit()
            Dim a() As String = Nothing
            ReDim a(CInt(v.size()) - 1)
            arrays.copy(a, v.data(), array_size(a))
            v.shrink_to_fit()
            assertion.equal(v.capacity(), v.size(), "v.capacity() <> v.size() after shrink_to_fit.")
            If validation Then
                assertion.is_true(memcmp(v.data(), 0, a, 0, array_size(a)) = 0, "v.data <> a after shrink_to_fit.")
            End If
        End Sub

        Private Sub clone()
            Dim v2 As vector(Of String) = Nothing
            assertion.is_true(copy(v2, v))
            assertion.equal(object_compare(v2, v), object_compare_undetermined)
            If validation Then
                assertion.is_true(compare(v, v2) = 0, "v<>v2")
            End If
        End Sub
    End Class
End Class
