
csc importer_gen.cs
importer_gen.exe < ..\definition.txt > importer.definition.vb
move /Y importer.definition.vb ..\

csc builder_gen.cs
builder_gen.exe < ..\definition.txt > builders.implementation.vb
move /Y builders.implementation.vb ..\builder\
