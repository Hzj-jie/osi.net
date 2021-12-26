
Option Explicit On
Option Infer Off
Option Strict On

'this file is generated by /osi/service/resource/zipgen/zipgen.exe with
'nlexer_rule syntaxer_rule
'so change /osi/service/resource/gen/gen.cs or resource files instead of this file

Imports System.IO
Imports System.IO.Compression
Imports osi.root.connector

Friend Module b2style_rules
    Public ReadOnly nlexer_rule() As Byte
    Public ReadOnly syntaxer_rule() As Byte

    Sub New()
        nlexer_rule = Convert.FromBase64String(strcat_hint(CUInt(424), _
        "H4sIAAAAAAAEAFWRsU7DMBRF9/cVr0J0gDrdq7hLYUBCMDGRUpn4tbFw7GA7tEj8GQOfxC+QOE5LF+ueK9vvWP79/oG7h9X9080tZtncV8KRnBtNB3Ib12rKwiEAvO2ZETX5RpSEx9TXpRbeY1wBLnBl60Y45a0BTd4z6xi9t0JjzmHnSARyp27JYQicg7EhtRM+HA2VMJgfT0Vc9jMeG3IiWAdCSiyuoVam9cigbnVQjf7E4gqk+lCScA61lXgJjd2Twxd4VYEJI3Eak3VYfEHkKUTosBPBCXjSW6ZM2d3fTYgkqUTGOrdtYL5S24B5Dk7tqhGXnV3cOXjxAZLdSCfH1Iym44beN+Vknejo/o+jdOLhHQnSaxKdOafu3JwD9N+Kz8V+tpktFus+FDLmWba+gj+GzB0jKQIAAA=="))

        assert(nlexer_rule.ungzip(nlexer_rule))
        syntaxer_rule = Convert.FromBase64String(strcat_hint(CUInt(944), _
        "H4sIAAAAAAAEALVWTW/cIBC9I/U/IPVWmc29lx6aHipVzSHtybIqbI9tFAIuH9nsv+8Yg78TbaT0sobhATNv5g1Lvv/8+uP37Td6Ot3Yjhuob+xFOf4M5o/xEk7u2RFitHbMXXqgeeNV5YRWGX3i0gOroRFKDBZ2Fq5jFh4Fq7QcEB3wHgGV5IYfI9IZr0AUfwTb8wpYRoWqpK8ho+FW/IpG4ZCdDe97qDMqdSuq/RmD6wG3XbDO+MplFO+3tiAGGjCgKqClcIyrmsy304czm2bBK9zNjWM9ut6iAx3NJ56KTxRUPS8RC8oNB29doLlolTbADFgvHQuUjV6xRDWruJQY6wq4WdwmY0/+AdnJhAZvkU0Dzhs1TTHg0gB/iLQiWyAbtt4Smc1oj67pHmIWAwiN2rqttfhC5/BJOHJEaEPzMOV1He96FMrbNMaoRS8vcVqLJzFUwrim045en8HEcczhYqbT0myeTBIax2wnGhcNRrRdtBQfyC525M4IXkpgYy2sAglA0mjDpNb9wCSOY72UhlcP4Gi+z9kuQyuy4o4j25SPgxS9T4G9Jb9D6acoZwkYfh4RNH/FiV1dL0geGoCDdkhwKVqPk4z6RmqO31JrGfQsVFuQkYFBa9rvGA8SnLzGsk+OLffNAJp7xc1lH30pjuwFOYTT/HoGyQGU5kqnysQ2mFQAVRGL7SO9x2BlTa3ve23c4dE0nnVwDDnEL5J2tGd36TEn6dqg7CjqWc9JykHFUcCTdpNsw2QYSLCWuY5j4lrsTg5MnIUFVBz89VzOi7MlfpDH0ZKYe8HnRejv4/fcXxat5X8EtO5MK7WNMgqWcTj8FmQLmRG01i6AyJV9hL64Qq7bs3haN8KVwrp1eyFYgb/ubu8+0/tYgLjRiWpqIyS87EMDHgfH7/YVf0PSgduXffmY/QOfsT+9TQkAAA=="))

        assert(syntaxer_rule.ungzip(syntaxer_rule))
    End Sub
End Module
