
REM These tools are .Net 2.0 compatible, but by default they are built with .Net 4.0 to provide a better GZipStream compression quality.
csc /out:gen.exe /o+ gen.base64.cs
csc /out:zipgen.exe /o+ zipgen.base64.cs

