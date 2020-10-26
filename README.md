# QAToolKit.Engine.Bombardier
![.NET Core](https://github.com/qatoolkit/qatoolkit-engine-bombardier-net/workflows/.NET%20Core/badge.svg?branch=main)

`QAToolKit.Engine.Bombardier` is a .NET standard library, which takes `IList<HttpTestRequest>` object and runs load tests with tool called [Bombardier](https://github.com/codesenberg/bombardier).

Library is a thin wrapper, that generates requests and parses results so you can export them to whatever format you prefer.

Major features:

- Library takes `IList<HttpTestRequest>` object, which can be produced in your code or can be imported from other sources. One example can be QAToolKit Swagger library that can produce that object with many options. Check it out [here](https://github.com/qatoolkit/qatoolkit-source-swagger-net).
- Generate a Bombardier report that can be exported to the format you want.

## Sample

```csharp

//Instantiate a Bombardier test generator, by specifying Bombardier options
var bombardierTestsGenerator = new BombardierTestsGenerator(options =>
{
    //if your api endpoints you are testing are protected by Oauth2 access tokens
    options.AddOAuth2Token("eyJhbGciOiJSUzI1N....", AuthenticationType.Customer);
    //if your api endpoints are protected by simple "ApiKey: <apikey>" authentication header
    options.AddApiKey("myAPIKey123423456");
    //if your api endpoints are protected by basic authentication
    options.AddBasicAuthentication("username", "password");
    options.BombardierConcurrentUsers = 1;
    options.BombardierDuration = 1;
    options.BombardierTimeout = 30;
    options.BombardierUseHttp2 = true;
});
//Generate Bombardier Tests
var bombardierTests = await bombardierTestsGenerator.Generate(requests);

//Run Bombardier Tests
var bombardierTestsRunner = new BombardierTestsRunner(bombardierTests.ToList());
var bombardierResults = await bombardierTestsRunner.Run();
```

Results sample `bombardierResults`:

```json
[
    {
        "TestStart": "2020-10-26T14:53:41.9558275+01:00",
        "TestStop": "2020-10-26T14:53:45.371211+01:00",
        "Duration": 3.4153835,
        "Command": "-m GET https://api.demo.com/v1/categories?parent=4 -c 1 -H \"Authorization: Bearer eyJhbGciOiJSUzI1N....\" --http2 --timeout=30s --duration=1s",
        "Counter1xx": 0,
        "Counter2xx": 48,
        "Counter3xx": 0,
        "Counter4xx": 0,
        "Counter5xx": 0,
        "AverageLatency": 63.58,
        "AverageRequestsPerSecond": 15.72,
        "MaxLatency": 215.50,
        "MaxRequestsPerSecond": 53.13,
        "StdevLatency": 25.83,
        "StdevRequestsPerSecond": 18.80
    },
    ...
]
```

As you can see you get a lot of metrics from the tests.

## Description

#### 1. Authentication options

##### 1.1 AddOAuth2Token (prefered)
Use `AddOAuth2Token` if your APIs are protected by Oauth2 Authentication. Pass Bearer token in the method along with the `AuthenticationType`.

##### 1.2 AddApiKey
Use `AddApiKey` if your APIs are protected by simple API Key. Pass Api Key in the method. An `ApiKey` HTTP header will be generated with the value you provided. This is not best practice, but it's here if you need it.

##### 1.3 AddBasicAuthentication
Use `AddBasicAuthentication` if your APIs are protected by basic authentication. Pass username and password in the method. A basic `Authentication` HTTP header will be generated. This is not best practice, but it's here if you need it.

#### 2. Bombardier parameters

You can set 5 Bombardier properties:

- `BombardierConcurrentUsers`: How many concurrent users should be used in Bombardier tests.
- `BombardierDuration`: How long the Bombardier tests should execute in seconds. Use this depending on the type of test you want to perform and should not be used with `BombardierRateLimit`.
- `BombardierTimeout`: What is the Bombardier timeout to wait for the requests to finish.
- `BombardierUseHttp2`: Use HTTP2?
- `BombardierRateLimit`: Rate limit Bombardier tests per second. Use this depending on the type of test you want to perform and should not be used with `BombardierDuration`.

## How to use

TO-DO

## TO-DO

- results obfuscation (authentication headers)

## License

MIT License

Copyright (c) 2020 Miha Jakovac

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.