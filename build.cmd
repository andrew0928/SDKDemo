"c:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" DEMO.sln /t:clean
rd /s /q output


"c:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" DEMO.sln /p:config=release;outdir=.\output
md output
xcopy /e /i Demo.ApiWeb\output          output\Demo.ApiWeb
xcopy /e /i Demo.SDK\output             output\Demo.SDK
xcopy /e /i Demo.SDK.TestConsole\output output\Demo.SDK.TestConsole

cd output

cd Demo.SDK.TestConsole
docker build -t andrew0928/sdkdemo.testconsole:10.3.1 .
cd ..

cd Demo.ApiWeb
docker build -t andrew0928/sdkdemo.apiweb:10.3.1 .
cd ..


cd ..




: start /min start-web.cmd
: choice /t 10
: call start-test.cmd