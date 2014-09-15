echo off
cls
set tobesigned="D:\dev\src\codeplex\Findit\Setup\Setup\Express\SingleImage\DiskImages\DISK1\setup.exe"
set signtoolexe="C:\Program Files (x86)\Windows Kits\8.1\bin\x64\signtool.exe"
%signtoolexe% sign %tobesigned%