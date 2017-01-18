"c:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" DEMO.sln /t:clean
rd /s /q output


"c:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" DEMO.sln /p:config=release;outdir=.\output
md output
xcopy /e /i Demo.ApiWeb\output          output\Demo.ApiWeb
xcopy /e /i Demo.SDK\output             output\Demo.SDK
xcopy /e /i Demo.SDK.TestConsole\output output\Demo.SDK.TestConsole



: start /min start-web.cmd
: choice /t 10
: call start-test.cmd