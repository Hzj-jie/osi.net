
Imports osi.root.connector
Imports osi.root.utils

Partial Public Class cluster
    'cluster id
    Private ReadOnly _id As Int64
    'offset in the virtdisk
    Private ReadOnly _offset As Int64
    'length of data
    Private ReadOnly _length As Int64
    'virtdisk
    Private ReadOnly _vd As virtdisk
    'used bytes of data
    Private _used As Int64
    'cluster id of prev cluster
    Private _prev_id As Int64
    'cluster id of next cluster
    Private _next_id As Int64
End Class
