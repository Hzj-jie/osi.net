
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Public Module _html
    'it's a known issue that slimparser cannot work well with script,
    'so remove them before processing is a good work around
    'these two functions have potential risk to wrongly remove content, but it's in an acceptable range
    'since there is no official tag as <style*> or <script*>
    Public Function kick_style(ByRef content As String) As String
        Return kick_between(content, "<style", "</style>", case_sensitive:=False)
    End Function

    Public Function kick_script(ByRef content As String) As String
        Return kick_between(content, "<script", "</script>", case_sensitive:=False)
    End Function

    Public Function kick_style_script(ByRef content As String) As String
        content = kick_script(content)
        Return kick_style(content)
    End Function
End Module
