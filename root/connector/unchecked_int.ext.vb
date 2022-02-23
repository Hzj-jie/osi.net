
Imports System.Runtime.CompilerServices

Public Module _wrapper_unchecked_int_extension
    <Extension()> Public Function increment(ByVal this As ref(Of unchecked_int),
                                            Optional ByVal that As Int32 = 1) As Int32
        assert(this IsNot Nothing)
        this.p += that
        Return this.p
    End Function
End Module