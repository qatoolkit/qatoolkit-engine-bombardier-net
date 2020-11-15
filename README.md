# QAToolKit.Engine.Bombardier
![https://github.com/qatoolkit/qatoolkit-engine-bombardier-net/actions](https://github.com/qatoolkit/qatoolkit-engine-bombardier-net/workflows/.NET%20Core/badge.svg?branch=main)
![https://github.com/qatoolkit/qatoolkit-engine-bombardier-net/security/code-scanning](https://github.com/qatoolkit/qatoolkit-engine-bombardier-net/workflows/CodeQL%20Analyze/badge.svg)
![https://sonarcloud.io/dashboard?id=qatoolkit_qatoolkit-engine-bombardier-net](https://github.com/qatoolkit/qatoolkit-engine-bombardier-net/workflows/Sonarqube%20Analyze/badge.svg)
![https://www.nuget.org/packages/QAToolKit.Engine.Bombardier/](https://img.shields.io/nuget/v/QAToolKit.Engine.Bombardier?label=QAToolKit.Engine.Bombardier)

## Description
`QAToolKit.Engine.Bombardier` is a .NET standard library, which takes `IEnumerable<HttpTestRequest>` object and runs load tests with tool called [Bombardier](https://github.com/codesenberg/bombardier).

Supported .NET frameworks and standards: `netstandard2.0`, `netstandard2.1`, `netcoreapp3.1`, `net5.0`

Library is a thin wrapper, that generates requests and parses results to JSON. You can process or import that JSON in other tools. We plan to release an exporter to CSV, HTML and maybe other formats.

Major features:

- Library takes `IEnumerable<HttpTestRequest>` object, which can be produced in your code or can be imported from other sources. One example can be `QAToolKit Swagger` library that can produce that object with many options. Check it out [here](https://github.com/qatoolkit/qatoolkit-source-swagger-net).
- Generate a Bombardier report in JSON format.

Bombardier version [1.2.5](https://github.com/codesenberg/bombardier/releases/tag/v1.2.5) is used for `windows-amd64` and `linux-amd64`.

## Sample

```csharp
//Generate requests from previously stored JSON file:
var content = File.ReadAllText("Assets/getPetById.json");
var httpRequest = JsonConvert.DeserializeObject<IEnumerable<HttpRequest>>(content);

//Create bombardier tests generator
var bombardierTestsGenerator = new BombardierTestsGenerator(httpRequest, options =>
{
    options.BombardierConcurrentUsers = 1;
    options.BombardierDuration = 1;
    options.BombardierTimeout = 30;
    options.BombardierUseHttp2 = true;
});

//Generate bomardier tests
var bombardierTests = await bombardierTestsGenerator.Generate();

//Run Bombardier Tests
var bombardierTestsRunner = new BombardierTestsRunner(bombardierTests.ToList(), options =>
{
    options.ObfuscateAuthenticationHeader = true;
});
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

##### 1.4 AddReplacementValues
When you use `AddReplacementValues` values those can set or replace URL and HTTP body parameters before executing the tests. Replacement values have precedence over the `example` values that are set in Swagger file.

For example, you can add replacement values dictionary to the `BombardierTestsGenerator`.

```csharp
var bombardierTestsGenerator = new BombardierTestsGenerator(options =>
{
    options.AddReplacementValues(new Dictionary<string, object> {
            {"api-version","2"},
            {"bicycleType","1"}
    });
    ...
});
```

#### 2. Bombardier parameters

You can also set those `BombardierGeneratorOptions` options:

- `BombardierConcurrentUsers`: How many concurrent users should be used in Bombardier tests. Default is `10`.
- `BombardierDuration`: How long the Bombardier tests should execute in seconds. Use this depending on the type of test you want to perform and should not be used with `BombardierRateLimit`. Default is `30` seconds.
- `BombardierTimeout`: What is the Bombardier timeout to wait for the requests to finish. Default is `30` seconds.
- `BombardierUseHttp2`: Use HTTP2 protocol. Otherwise HTTP1 is used. By default this is set to `true`.
- `BombardierRateLimit`: Rate limit Bombardier tests per second. Use this depending on the type of test you want to perform and should not be used with `BombardierDuration`. By default rate limit is not set.
- `BombardierNumberOfTotalRequests`: Limit the test to run only certain amount of requests. By default total number of requests is not set.
- `BombardierInsecure`: Instead of HTTPS use HTTP protocol. Default value is `false`.
- `BombardierBodyContentType`: Force only certain HTTP Content type. By default is set to `application/json`.
 
#### 3. Obfuscate Auth tokens for Bombardier output

Output is obfuscated by default, but you can turn it off with `options.ObfuscateAuthenticationHeader = false;` in `BombardierTestsRunner` options.

## How to use

In the sample code above we generate HTTP requests from previously generated object which was serialized to JSON.

If you use Swagger files, you need to check the `QAToolKit.Source.Swagger` NuGet package, where you can generate that object from the Swagger file.

Let's replace

```csharp
//Generate requests from previously stored JSON file:
var content = File.ReadAllText("Assets/getPetById.json");
var httpRequest = JsonConvert.DeserializeObject<IEnumerable<HttpRequest>>(content);
```

with

```csharp
//Setup Swagger source
var urlSource = new SwaggerUrlSource(options =>
{
    options.AddBaseUrl(new Uri("https://qatoolkitapi.azurewebsites.net/"));
    options.AddRequestFilters(new RequestFilter()
    {
        EndpointNameWhitelist = new string[] { "GetAllBikes" }
    });
    options.UseSwaggerExampleValues = true;
});

//Load requests object
var requests = await urlSource.Load(new Uri[] {
    new Uri("https://qatoolkitapi.azurewebsites.net/swagger/v1/swagger.json")
});
```

in the sample code above. Check the [QAToolKit.Source.Swagger](https://github.com/qatoolkit/qatoolkit-source-swagger-net) library for more details.

## To-do

- **This library is an early alpha version**
- Currently tested for GET, POST, PUT and DELETE HTTP methods. Need to extend support.

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