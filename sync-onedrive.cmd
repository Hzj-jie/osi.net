
for /l %%x in (0,0,1) do (
  robocopy f:\osi-root-utt-log\1-core %UserProfile%\OneDrive\osi-root-utt-log\1-core /MIR
  robocopy f:\osi-root-utt-log\2-cores %UserProfile%\OneDrive\osi-root-utt-log\2-cores /MIR
  robocopy f:\osi-root-utt-log\4-cores %UserProfile%\OneDrive\osi-root-utt-log\4-cores /MIR
  robocopy f:\osi-root-utt-log\12-cores %UserProfile%\OneDrive\osi-root-utt-log\12-cores /MIR
  robocopy f:\osi-root-utt-log\12-cores-deploys %UserProfile%\OneDrive\osi-root-utt-log\12-cores-deploys /MIR
  ping -n 301 127.0.0.1 > nul
)
