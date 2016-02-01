cd IOEx
nuget.exe pack -Prop Configuration=Release
xcopy *.nupkg "D:\dev\nuget" /F /Y
cd ..
