: docker rmi andrew0928/sdkdemo.testconsole:10.1

: test 10.1
docker rm -f apiurl
docker run -d --name apiurl -p 80 andrew0928/sdkdemo.apiweb:10.1
docker run --rm --link apiurl:apiurl andrew0928/sdkdemo.testconsole:10.1
docker run --rm --link apiurl:apiurl andrew0928/sdkdemo.testconsole:10.2
: docker run --rm --link apiurl:apiurl andrew0928/sdkdemo.testconsole:10.3
docker run --rm --link apiurl:apiurl andrew0928/sdkdemo.testconsole:10.3.1


: test 10.2
docker rm -f apiurl
docker run -d --name apiurl -p 80 andrew0928/sdkdemo.apiweb:10.2
docker run --rm --link apiurl:apiurl andrew0928/sdkdemo.testconsole:10.1
docker run --rm --link apiurl:apiurl andrew0928/sdkdemo.testconsole:10.2
: docker run --rm --link apiurl:apiurl andrew0928/sdkdemo.testconsole:10.3
docker run --rm --link apiurl:apiurl andrew0928/sdkdemo.testconsole:10.3.1


: test 10.3
docker rm -f apiurl
docker run -d --name apiurl -p 80 andrew0928/sdkdemo.apiweb:10.3
docker run --rm --link apiurl:apiurl andrew0928/sdkdemo.testconsole:10.1
docker run --rm --link apiurl:apiurl andrew0928/sdkdemo.testconsole:10.2
: docker run --rm --link apiurl:apiurl andrew0928/sdkdemo.testconsole:10.3
docker run --rm --link apiurl:apiurl andrew0928/sdkdemo.testconsole:10.3.1


: test 10.3.1
docker rm -f apiurl
docker run -d --name apiurl -p 80 andrew0928/sdkdemo.apiweb:10.3.1
docker run --rm --link apiurl:apiurl andrew0928/sdkdemo.testconsole:10.1
docker run --rm --link apiurl:apiurl andrew0928/sdkdemo.testconsole:10.2
: docker run --rm --link apiurl:apiurl andrew0928/sdkdemo.testconsole:10.3
docker run --rm --link apiurl:apiurl andrew0928/sdkdemo.testconsole:10.3.1


