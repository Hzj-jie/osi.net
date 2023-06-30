
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.xml

Public Class xml_component_generation_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        assertion.is_true(strsame(create_start_tag("tag",
                                             pair.emplace_of("key", "value"),
                                             pair.emplace_of("key2", "value2")),
                            "<tag key=""value"" key2=""value2"">"))
        assertion.is_true(strsame(create_start_tag("tag",
                                             True,
                                             pair.emplace_of("key", "value"),
                                             pair.emplace_of("key2", "value2")),
                            "<tag key=""value"" key2=""value2""/>"))
        assertion.is_true(strsame(create_end_tag("tag"), "</tag>"))
        assertion.is_true(strsame(create_text("<a b c>"), "&lt;a b c&gt;"))
        assertion.is_true(strsame(create_loosen_comment("<abc>--<bcd>"), "<!--<abc>- - <bcd>-->"))
        assertion.is_true(strsame(create_loosen_cdata("<abc>&&]]>.."), "<![CDATA[<abc>&&] ]>..]]>"))
        Return True
    End Function
End Class
