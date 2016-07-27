
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.xml

Public Class xml_component_generation_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        assert_true(strsame(create_start_tag("tag",
                                             emplace_make_pair("key", "value"),
                                             emplace_make_pair("key2", "value2")),
                            "<tag key=""value"" key2=""value2"">"))
        assert_true(strsame(create_start_tag("tag",
                                             True,
                                             emplace_make_pair("key", "value"),
                                             emplace_make_pair("key2", "value2")),
                            "<tag key=""value"" key2=""value2""/>"))
        assert_true(strsame(create_end_tag("tag"), "</tag>"))
        assert_true(strsame(create_text("<a b c>"), "&lt;a b c&gt;"))
        assert_true(strsame(create_loosen_comment("<abc>--<bcd>"), "<!--<abc>- - <bcd>-->"))
        assert_true(strsame(create_loosen_cdata("<abc>&&]]>.."), "<![CDATA[<abc>&&] ]>..]]>"))
        Return True
    End Function
End Class
