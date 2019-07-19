set MSBUILD_PATH="C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin"

echo Build solution
%MSBUILD_PATH%\MSBuild.exe Oxard.XControls\Oxard.XControls.csproj /t:build /p:Configuration=Release
%MSBUILD_PATH%\MSBuild.exe Oxard.XControls.Android\Oxard.XControls.Android.csproj /t:build /p:Configuration=Release
%MSBUILD_PATH%\MSBuild.exe Oxard.XControls.UWP\Oxard.XControls.UWP.csproj /t:build /p:Configuration=Release
echo Build x86 UWP
%MSBUILD_PATH%\MSBuild.exe Oxard.XControls.UWP\Oxard.XControls.UWP.csproj /t:build /p:Configuration=Release /p:Platform="x86"
echo Build x64 UWP
%MSBUILD_PATH%\MSBuild.exe Oxard.XControls.UWP\Oxard.XControls.UWP.csproj /t:build /p:Configuration=Release /p:Platform="x64"
echo Build ARM UWP
%MSBUILD_PATH%\MSBuild.exe Oxard.XControls.UWP\Oxard.XControls.UWP.csproj /t:build /p:Configuration=Release /p:Platform="ARM"

echo Generate nuget package
nuget pack Oxard.XControls.nuspec
pause