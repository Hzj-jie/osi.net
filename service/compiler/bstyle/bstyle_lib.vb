
Option Explicit On
Option Infer Off
Option Strict On

'this file is generated by /osi/service/resource/zipgen/zipgen.exe with
'stdio_h cstdio bstyle_h bstyle_types_h bstyle_constants_h
'so change /osi/service/resource/gen/gen.cs or resource files instead of this file

Imports System.IO
Imports System.IO.Compression
Imports osi.root.connector

Friend Module bstyle_lib
    Public ReadOnly stdio_h() As Byte
    Public ReadOnly cstdio() As Byte
    Public ReadOnly bstyle_h() As Byte
    Public ReadOnly bstyle_types_h() As Byte
    Public ReadOnly bstyle_constants_h() As Byte

    Sub New()
        stdio_h = Convert.FromBase64String(strcat_hint(CUInt(212), _
        "H4sIAAAAAAAEAHu/ez8vl3JmWl5KapqCU3BIpI9rvI+nU3xwiIunf7wHUA4okZmXilUOpDMvOac0JVXBJqm4pDInVS/DDiScmVei4OrvpmCrABGOj0/NT9PQtIbJpaeWJGckFmloKlTzcikoFKWWlBblwdXCZYHqa2FaCkohgiB2JnZ9cCVwjcqpeSmZaQoK+vpY3A8AZ8/+9PsAAAA="))

        assert(stdio_h.ungzip(stdio_h))
        cstdio = Convert.FromBase64String(strcat_hint(CUInt(208), _
        "H4sIAAAAAAAEAHu/ez8vl3JmWl5KapqCU3BIpI9rvI+nU7xzcIiLpz9QCiiemZeKTYqXS19fIcTfxd9KwTe/LFWhJF8hyai4pDInVSE/LxnIz0hVyMxLzilNSVVITsxTSEpVyEjMS8lJTVFIqoQrTSxWKE/NydEDmacMU25TXJKSma+XYQcWTc1LyUxTUADahuEKAMBNPVW/AAAA"))

        assert(cstdio.ungzip(cstdio))
        bstyle_h = Convert.FromBase64String(strcat_hint(CUInt(700), _
        "H4sIAAAAAAAEALVVwW7bMAy9B8g/EO0lA7p253YojAIFNqC3bYedBEehExWOJEgUGm/Yl+2wT9ovjJLl1k6VbsAa5CKLfI+PfHT8++ev+exUNXqFDdx8+vz17lbcfbwR+fiBgxxRGsvBiNWyDSuE90tPXYuCOov+fHNdCEmjPdWa+vB8dnEBXzzCyoRli28Da3BeGodABuq2NQ/QA9OzlOg90AYZ0QQtSTEbPCja5KzLS9D1Fr2tJWcYt63pfD5TmnJciDWS3NRu8Qa+z2cAMeSu4qk1ayXhhC/QuWAJciZUlXXc/g59Rbi1vvLklF6DO0k4hxScTiQ/YkPjYjb0xeKdyhWf1clJoA5VmtRRpTpomoMNSWM7mHTxaEHFuL83IYNzqEls/aQG+tBSeXJPiMPDS/Bp8YExKch5gwh+jKsja1rkiDqDfLqfTra2FvWKx3lfHty/MC87wv/kJROpF4lp8D6n+DG+pOOMEQPtUq3D2A7mzVeTtdpX8wKsNbHJA7gUHIHi84uFJoBGkVC6B6XAbjpDGUj4VvHLueNfcSe5lnhXDnn1DYVpEv3Eg11pb7OWYVBHUcIpZSHJ9j0l8a7fh6NoicxlMUVPhwV50sJJ3VVRVveaZnUls1hVNGpfVDwfQdO+bV3RtvjKRMvGglLCERQ9M+9R0in/6agGgD+Uxc/vH/xintO+BwAA"))

        assert(bstyle_h.ungzip(bstyle_h))
        bstyle_types_h = Convert.FromBase64String(strcat_hint(CUInt(256), _
        "H4sIAAAAAAAEAH2PPQqDQBCFe8E7DHoAky5gJxgiWAQ0hZVodnYZkNmga8CzpciRcoXs5oeYEKzeMO9j3rzb5ep7IUkWKCEpyipP6zxL6tdYVvu0qHcWsT4xLiG+Z6YTujsZG1TYA7GJfa/Tio4QOBM6zQo2QfxhE607bBhaqz9wOxmE9RdM6sADKUbxzmhJjc+cP9C2042BUTqZEYXpyf4xPGS2d7qCsyYRuzohsiAJEEVLte9fnGrDQgEAAA=="))

        assert(bstyle_types_h.ungzip(bstyle_types_h))
        bstyle_constants_h = Convert.FromBase64String(strcat_hint(CUInt(360), _
        "H4sIAAAAAAAEAJ2UwUoDMRCG74W+Q2jP0gaKCF7iimCh1MOuB0+h3Z2EgXVm2aRgfTUPPpKvYJotsiLY3Zwyh/9jkvmGfH18TidzNFSBEVlevGwe9Gad6XN5/7TNi7ttkevHEAsZJLgUm05qtliK2TmuVNOG6g2cKpmc35F3CsnrpViTBwvt7HYwI0cyDt9Bs9GBTSRrJpuI7pnrVPToIRE1Ne/GPhbY9IkfpuTmqGPsP4247LUZgkiBcgTSl4irBDA6xJsEMipMumw0mER2AlFej2BPAvFK/pL3V7iH18apPdrDaZQZ2mdyaAmqIdvSwc63GIaZx6NrNweq0AixWFz6Gr4BsTySzW4EAAA="))

        assert(bstyle_constants_h.ungzip(bstyle_constants_h))
    End Sub
End Module
