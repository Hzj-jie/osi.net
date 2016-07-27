
Imports sf = osi.service.configuration.constants.static_filter
Imports fis = osi.service.configuration.constants.filter_selector

Friend Module _default_filter_selector
    Public Function default_filter_selector() As filter_selector
        Dim fs As filter_selector = Nothing
        fs = New filter_selector()
        fs.set(sf.version, fis.string_compare)
        fs.set(sf.short_version, fis.string_compare)
        fs.set(sf.machine_name, fis.string)
        fs.set(sf.computer_name, fis.string)
        fs.set(sf.domain_name, fis.string)
        fs.set(sf.user_name, fis.string)
        fs.set(sf.working_directory, fis.string)
        fs.set(sf.service_name, fis.string)
        fs.set(sf.build, fis.string)
        fs.set(sf.running_mode, fis.string)

        fs.set(sf.os_full_name, fis.string_pattern)
        fs.set(sf.os_version, fis.string_interval)
        fs.set(sf.os_platform, fis.string_pattern)

        fs.set(sf.available_physical_memory, fis.int_compare)
        fs.set(sf.available_virtual_memory, fis.int_compare)
        fs.set(sf.total_physical_memory, fis.int_compare)
        fs.set(sf.total_virtual_memory, fis.int_compare)
        fs.set(sf.processor_count, fis.int_compare)

        Return fs
    End Function
End Module
