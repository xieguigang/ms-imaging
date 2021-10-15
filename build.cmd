@echo off

SET drive=%~d0
SET R_HOME=%drive%/GCModeller\src\R-sharp\App\net5.0

%R_HOME%/Rscript.exe --build ./ /save ./MSImaging.zip
%R_HOME%/R#.exe --install.packages ./MSImaging.zip

pause