echo Build solution
"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe" Oxard.XControls\Oxard.XControls.csproj /t:build /p:Configuration=Release
"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe" Oxard.XControls.Android\Oxard.XControls.Android.csproj /t:build /p:Configuration=Release
"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe" Oxard.XControls.UWP\Oxard.XControls.UWP.csproj /t:build /p:Configuration=Release
echo Build x64 UWP
"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe" Oxard.XControls.UWP\Oxard.XControls.UWP.csproj /t:build /p:Configuration=Release /p:Platform="x64"
echo Build ARM UWP
"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe" Oxard.XControls.UWP\Oxard.XControls.UWP.csproj /t:build /p:Configuration=Release /p:Platform="ARM"

echo Generate nuget package
nuget pack Oxard.XControls.nuspec
pause