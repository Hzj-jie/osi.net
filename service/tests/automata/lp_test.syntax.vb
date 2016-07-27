
Imports System.IO
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils

Partial Public Class lp_test
#If OUTPUT_AS_FOLLOWING Then
default_separators ----
	use ignore
method_type -----------
	osi.tests.service.automata.lp_test, osi.tests.service.automata
key_words -------------
	verb	like	run	go	come	stay
	preposition	to
	noun	i	you	he	she	it	apple	orange
	-separator	.	,	;	:	'	"	/	\	[	]	{	}	(	)	*	&	^	%	$	#	@	!	~	`
transitions -----------
	start	noun	after_subject	start_to_after_subject
	after_subject	verb	after_predicate	after_subject_to_after_predicate
	after_predicate	preposition	after_predicate	after_predicate_to_after_predicate
	after_predicate	noun	after_object	after_predicate_to_after_object
	after_predicate	unknown	after_object	after_predicate_to_after_object_unknown
    after_object	end	end	after_object_to_end
#End If
    Private Const tab As Char = character.tab
    Private Const nl As Char = character.newline
    Private Shared ReadOnly syntax_file_name As String = Path.Combine(temp_folder, "lp_test.syntax.txt")
    Private Shared ReadOnly syntax As String = strcat(
        "default_separators ----", nl,
        tab, "use ignore", nl,
        "method_type -----------", nl,
        tab, "osi.tests.service.automata.lp_test, osi.tests.service.automata", nl,
        "key_words -------------", nl,
        tab, "verb", tab, "like", tab, "run", tab, "go", tab, "come", tab, "stay", nl,
        tab, "preposition", tab, "to", nl,
        tab, "noun", tab, "i", tab, "you", tab, "he", tab, "she", tab, "it", tab, "apple", tab, "orange", nl,
        tab, "-separator", tab, ".", tab, ",", tab, ";", tab, ":", tab, "'", tab, """", tab, "/", tab, "\", tab,
                                "[", tab, "]", tab, "{", tab, "}", tab, "(", tab, ")", tab, "*", tab, "&", tab,
                                "^", tab, "%", tab, "$", tab, "#", tab, "@", tab, "!", tab, "~", tab, "`", nl,
        "transitions -----------", nl,
        tab, "start", tab, "noun", tab, "after_subject", tab,
             "start_to_after_subject", nl,
        tab, "after_subject", tab, "verb", tab, "after_predicate", tab,
             "after_subject_to_after_predicate", nl,
        tab, "after_predicate", tab, "preposition", tab, "after_predicate", tab,
             "after_predicate_to_after_predicate", nl,
        tab, "after_predicate", tab, "noun", tab, "after_object", tab,
             "after_predicate_to_after_object", nl,
        tab, "after_predicate", tab, "unknown", tab, "after_object", tab,
             "after_predicate_to_after_object_unknown", nl,
        tab, "after_object", tab, "end", tab, "end", tab,
             "after_object_to_end", nl)

    Private Shared Function write_syntax_file() As Boolean
        Dim o As TextWriter = Nothing
        Try
            o = New StreamWriter(syntax_file_name)
        Catch ex As Exception
            raise_error(error_type.warning,
                        "failed to create syntax file ",
                        syntax_file_name,
                        ", ex ",
                        ex.Message())
            Return False
        End Try

        Using o
            o.WriteLine(syntax)
            Return True
        End Using
    End Function
End Class
