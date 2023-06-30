
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.constants.counter

Namespace counter
    Public Module _counter_register
        Public Function register(ByVal name As String,
                                 Optional ByVal write_count As Boolean = True,
                                 Optional ByVal write_average As Boolean = False,
                                 Optional ByVal write_last_average As Boolean = False,
                                 Optional ByVal last_average_length As Int64 = default_last_average_length,
                                 Optional ByVal write_rate As Boolean = False,
                                 Optional ByVal write_last_rate As Boolean = False,
                                 Optional ByVal last_rate_length As Int64 = default_last_rate_length,
                                 Optional ByVal interval_scale As Int64 = default_interval_scale,
                                 Optional ByVal sample_rate As Double = default_sample_rate) As Int64
            Dim cr As counter_record = Nothing
            cr = New counter_record(name,
                                    write_count,
                                    write_average,
                                    write_last_average,
                                    last_average_length,
                                    write_rate,
                                    write_last_rate,
                                    last_rate_length,
                                    interval_scale,
                                    sample_rate)
            Dim rtn As Int64 = 0
            rtn = _counter_collection.insert(cr)
            raise_error("register counter ",
                        name,
                        " with interval_scale ",
                        Convert.ToString(interval_scale),
                        ", return id ",
                        Convert.ToString(rtn))

            Return rtn
        End Function

        Public Function register_counter(ByVal name As String) As Int64
            Return register(name)
        End Function

        Public Function register_average(ByVal name As String,
                                         Optional ByVal sample_rate As Double = default_sample_rate) As Int64
            Return register(name, write_count:=False, write_average:=True, sample_rate:=sample_rate)
        End Function

        Public Function register_last_average(ByVal name As String,
                                              Optional ByVal last_average_length As Int64 =
                                                                default_last_average_length,
                                              Optional ByVal sample_rate As Double = default_sample_rate) As Int64
            Return register(name,
                            write_count:=False,
                            write_last_average:=True,
                            last_average_length:=last_average_length,
                            last_rate_length:=last_average_length,
                            sample_rate:=sample_rate)
        End Function

        Public Function register_average_and_last_average(ByVal name As String,
                                                          Optional ByVal last_average_length As Int64 =
                                                                            default_last_average_length,
                                                          Optional ByVal sample_rate As Double =
                                                                            default_sample_rate) _
                                                         As Int64
            Return register(name,
                            write_count:=False,
                            write_average:=True,
                            write_last_average:=True,
                            last_average_length:=last_average_length,
                            last_rate_length:=last_average_length,
                            sample_rate:=sample_rate)
        End Function

        Public Function register_rate(ByVal name As String,
                                      Optional ByVal interval_scale As Int64 = default_interval_scale) As Int64
            Return register(name,
                            write_count:=False,
                            write_rate:=True,
                            interval_scale:=interval_scale)
        End Function

        Public Function register_last_rate(ByVal name As String,
                                           Optional ByVal last_rate_length As Int64 = default_last_rate_length,
                                           Optional ByVal interval_scale As Int64 = default_interval_scale) As Int64
            Return register(name,
                            write_count:=False,
                            write_last_rate:=True,
                            last_average_length:=last_rate_length,
                            last_rate_length:=last_rate_length,
                            interval_scale:=interval_scale)
        End Function

        Public Function register_rate_and_last_rate(ByVal name As String,
                                                    Optional ByVal last_rate_length As Int64 =
                                                                        default_last_rate_length,
                                                    Optional ByVal interval_scale As Int64 = default_interval_scale) _
                                                   As Int64
            Return register(name,
                            write_count:=False,
                            write_rate:=True,
                            write_last_rate:=True,
                            last_average_length:=last_rate_length,
                            last_rate_length:=last_rate_length,
                            interval_scale:=interval_scale)
        End Function
    End Module
End Namespace
