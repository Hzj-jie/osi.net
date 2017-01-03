#!/bin/sh

for ((;;))
do
    tskill OneDrive
    cmd /c "$USERPROFILE\AppData\Local\Microsoft\OneDrive\OneDrive.exe"
    sleep 1h
done
