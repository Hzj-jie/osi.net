
for %%x in (root,service) do (
    cd %%x
    call ..\%1 %%x.sln
    cd .. )

