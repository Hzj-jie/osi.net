﻿
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.interpreter.primitive

Namespace primitive
    Public Class data_ref_exportable_test
        Inherits exportable_test(Of data_ref)

        Protected Overrides Function create() As data_ref
            Dim i As Int64 = 0
            Dim r As data_ref = Nothing
            r = data_ref.random(i)
            assertion.is_true((r.offset() < 0) = (i < 0))
            assertion.equal(r.export(), i)
            If r.relative() Then
                assertion.equal(data_ref.rel(r.offset()), r)
            Else
                assertion.equal(data_ref.abs(r.offset()), r)
            End If
            Return r
        End Function
    End Class

    Public Class data_ref_exportable_test2
        Inherits [case]

        Public Overrides Function run() As Boolean
            Dim r As data_ref = Nothing
            r = New data_ref()
            assertion.is_true(r.import("abs0"))
            assertion.equal(r.offset(), int64_0)
            assertion.is_true(r.absolute())

            assertion.is_true(r.import(strcat("abs", data_ref.max_value)))
            assertion.equal(r.offset(), data_ref.max_value)
            assertion.is_true(r.absolute())

            assertion.is_true(r.import(strcat("abs", data_ref.abs_min_value)))
            assertion.equal(r.offset(), data_ref.abs_min_value)
            assertion.is_true(r.absolute())

            assertion.is_true(r.import("rel0"))
            assertion.equal(r.offset(), int64_0)
            assertion.is_true(r.relative())

            assertion.is_true(r.import(strcat("rel", data_ref.max_value)))
            assertion.equal(r.offset(), data_ref.max_value)
            assertion.is_true(r.relative())

            assertion.is_true(r.import(strcat("rel", data_ref.rel_min_value)))
            assertion.equal(r.offset(), data_ref.rel_min_value)
            assertion.is_true(r.relative())
            Return True
        End Function
    End Class
End Namespace
