@echo off
echo off
cls

c:
cd \mksoftwares\libs\MKSoft.PIX
path c:\Windows\Microsoft.NET\Framework\v4.0.30319
rem if exist *.xml (del *.xml /q)
if exist MKSoft.PIX.tlb (del MKSoft.PIX.tlb)

regasm /tlb MKSoft.PIX.dll MKSoft.PIX.tlb /codebase

if not exist MKSoft.PIX.tlb (goto fim)

echo.
echo.
echo.
echo ********************************************************************************
echo                     MKSoft.PIX.DLL REGISTRADA COM EXITO
echo ********************************************************************************

:fim
pause