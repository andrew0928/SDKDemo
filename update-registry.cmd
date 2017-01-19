for %%v in (10.1 10.2 10.3 10.3.1) do (

copy /y SDKDemo\Demo.SDK.TestConsole\dockerfile output.%%v\Demo.SDK.TestConsole
copy /y SDKDemo\Demo.SDK.TestConsole\start-test.cmd output.%%v\Demo.SDK.TestConsole
docker build -t andrew0928/sdkdemo.testconsole:%%v output.%%v\Demo.SDK.TestConsole
docker push andrew0928/sdkdemo.testconsole:%%v

)