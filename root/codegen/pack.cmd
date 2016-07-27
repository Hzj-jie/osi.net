
for /f %%x in ('where zip.exe ^| findstr zip.exe ^| linew /-') do set containsZip=%%x
if containsZip==0 path %path%;C:\Program Files\WinRAR\;

del %1
zip -9 -T -n "exx mxx" %1 *.*


