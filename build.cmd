@echo off

SET R_HOME=D:\GCModeller\src\R-sharp\App\net5.0

%R_HOME%/Rscript.exe --build ./ /save ./MSImaging.zip
%R_HOME%/R#.exe --install.packages ./MSImaging.zip

pause