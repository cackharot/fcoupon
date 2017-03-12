FROM microsoft/dotnet:core
MAINTAINER cackharot <cackharot@gmail.com>


RUN mkdir -p /opt/fcoupon

ADD ./bin/Release/netcoreapp1.1/publish /opt/fcoupon

WORKDIR /opt/fcoupon

EXPOSE 5000:5500

CMD dotnet fcoupon.dll