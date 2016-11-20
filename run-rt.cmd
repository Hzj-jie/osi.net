
REM This script should be started from osi.root.utt folder, such as c;\deploys\apps\osi.root.utt\.

for /l %%x in (0, 0, 1) do (
  robocopy "W:\documents-t43\Program Files\osi\root\utt\bin\Release" . /MIR /XF run-rt.cmd
  osi.root.utt
)
