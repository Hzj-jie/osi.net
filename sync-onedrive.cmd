
for /l %%x in (0,0,1) do (
  robocopy f:\ %UserProfile%\OneDrive /MIR
  ping -n 301 127.0.0.1 > nul
)
