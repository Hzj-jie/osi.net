
for %%x in (root,service,production) do (
    cd %%x
    call ..\%1\%%x.sln
    cd .. )

