
Option Explicit On
Option Infer Off
Option Strict On

'this file is generated by /osi/service/resource/zipgen/zipgen.exe with
'priority_activity_exe priority_activity_pdb
'so change /osi/service/resource/gen/gen.cs or resource files instead of this file

Imports System.IO
Imports System.IO.Compression
Imports osi.root.connector

Friend Module _priority_activity
    Public ReadOnly priority_activity_exe() As Byte
    Public ReadOnly priority_activity_pdb() As Byte

    Sub New()
        priority_activity_exe = Convert.FromBase64String(strcat_hint(CUInt(2852), _
        "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"))

        assert(priority_activity_exe.ungzip(priority_activity_exe))
        priority_activity_pdb = Convert.FromBase64String(strcat_hint(CUInt(1492), _
        "H4sIAAAAAAAEAO2ZXWwUVRSAzwyL7W5/6FBcAU0dEBKhuH+AtjWxJC3EIhvATU0wky7LdCqztDNkZrZSX9zEH4whyoMPivBA+oL2xURDDIjpgynRJwPqk08+mGiIiNGoMel6zszd7Wy7wZS0brp7v92bc+fOmXvPnTvnzj13krpqmbY54sh90b7OTjmZ2ic/EYnFWkIb+1OAiO4fHsB0CjzuB06tMF3g1DNd/Amoa5q+igtdNBG0sYS0QqEQQFkorKrexMT5X6g0/mv4+NcN70TiwkftbUcEzA9eSG94/40/9lzUY2+eObf/g6KOwJKf7w/vnF/EWYFU8n96L/j9n44rSc7Kh49/ffPXscO9Mw03pbMpmD44KV78Yu8z7/304w3pbK933HTh9eelkZvSte2o2zhw7VZ4fWHow7bk6T/NAfWVXUfXsXq+zT70SGHq3OWhJ/+ZPP/Ns6+Fra8vVbVjnEXRM09y6oPZ27O3aSE3Swd9PcqgrVm28vRL2XRW15R+U82NaYZjK8/pdi4zKqec3LBuyolYfLdyyDKzmornXtDGdENXTFtXLNN0FEezsTTn6KO2ctLSTUt3JtIZ1dHHKaOd0haWRlQbQO1Rcm7zx1nzw6Xmx73mbV/zJ5e4eXrjvYwpz+4M3ZXaj38e/CEm5FF+nr08MXP9hrDYRT3pi1AeI1B9j7K6/+46rSnsXPHuaktgN2dpoKc+Atslyt9iZf53AI7bfUwCJDO6gSIMqyS67lOfLp0PwRZpMDVhO9oY6mAeXTJCLhlRTcNAXzUtWFtWrhnj6PflZa7jQgfIEqtXKtpSdFvKi2hWBAKBL3t/nvn4yHcDn20NilenPjnqmgn9kKC+CV7/mt16Au41v2H+1ZLNQsn+IMx932jCx1T0rsuHULagXAu0LwL5MLjL5PxmcA3LbwOyH/I7Ubaj7EaJa6J8EGSsM4rXPoyJ8gnwyp7CtB5Woy2/o24jzPkN5WVMuxY9ivcOreU3BqN3aOnfjJ3exOzZDQtj/ntFvss5vomwMmiutgGcZaEDWovzLM7zfQeTnem9hmNNpA+ZuuHQ3LbVPR9wdbwXQBi2+cpij8eIePX6sIw0VtsADmc5kUXPj+dDgUBxbUBO0ChW/g5Q49S8/9/t+88UlkUP6MaJAWPEhKiRGdNsiNqWGj2uZYY169ioqZ7wCkb0Uc2OVj+Ap1iFutLBukfvqQZMtFynsQwxHWI1k3X9KYt8/MVg3I37Me55qxneHsJ4ByhmlzHtwHTGp0/xzwY398v0XB1zdfmPPULwK8udh4Xzx4KRhO7EvkS8uxsDuMeuRO4U9SvtS4r4a3EVhFLbpEdzVbk9hYLgm72qvM1V2nNjppcPhkfQl6/Q86WjFbxxbkA76DvQJJPEZsHbr6FngfZz3sXjHXhv92A6AN7uGMXF5FDt4DkaORg+R66zUUxNcUMLa4eckeLMNcvaI85iWPffKhwOp0b5F3MqNYYALgAA"))

        assert(priority_activity_pdb.ungzip(priority_activity_pdb))
    End Sub
End Module
