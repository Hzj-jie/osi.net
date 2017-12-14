
Option Explicit On
Option Infer Off
Option Strict On

Partial Public Class cluster
    'cluster id, must >= 0.
    Private ReadOnly _id As Int64
    'offset in the virtdisk.
    Private ReadOnly _offset As UInt64
    'length of data, must > 0.
    Private ReadOnly _length As UInt64
    'virtdisk, is not null.
    Private ReadOnly _vd As virtdisk
    'used bytes of data, -1 means unused (a free cluster), otherwise >= 0.
    Private _used As Int64
    'cluster id of prev cluster, < 0 means no previous id.
    Private _prev_id As Int64
    'cluster id of next cluster, < 0 means no next id.
    Private _next_id As Int64
End Class
