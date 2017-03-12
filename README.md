# fcoupon
Coupon API micro services used by fbeazt app. Written in .NET Core 1.1 WebAPI

## Setup and Run
* Install .NET Core 1.1 SDK
* cd to this project src/fcoupon
* `dotnet restore`
* `dotnet run` or `dotnet watch run`
* Open [http://localhost:5000/api/coupon](http://localhost:5000/api/coupon)

## Sample cURLs
* Create a new coupon resource
```
curl ‘http://localhost:5000/api/coupon’ -H ‘Origin: http://localhost:4000’ -H ‘Content-Type: application/json;charset=UTF-8’ --data-binary ‘{“status”:true,”code”:”DISC25”,”description”:”flat discount”,”start”:”2017-03-08T05:00:00.000Z”,”end”:”2017-04-01T04:00:00.000Z”,”type”:”FlatDiscountWithCap”,”discount”:25,”maxDiscountAmount”:50}’ --compressed
```

* Search the coupon resource
`curl ‘http://localhost:5000/api/coupon?code=DISC25’`

* Fetch coupon resource by identifier
`curl ‘http://localhost:5000/api/coupon/58c49d9f17d24408e51eda20’`

* Update a given coupon resource
```
curl ‘http://localhost:5000/api/coupon/58c49d9f17d24408e51eda20’ -XPUT -H ‘Content-Type: application/json;charset=UTF-8’ --data-binary ‘{“status”:true,”code”:”DISC25”,”description”:”flat discount updated”,”start”:”2017-03-08T05:00:00.000Z”,”end”:”2017-04-01T04:00:00.000Z”,”type”:”FlatDiscountWithCap”,”discount”:25,”maxDiscountAmount”:100}’ --compressed
```
