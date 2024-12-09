namespace Challenge_Blip.WebApi.Tests.TestUtils;
public class MockHttpMessageHandler(Func<HttpRequestMessage, Task<HttpResponseMessage>> responseFunction) : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, Task<HttpResponseMessage>> _responseFunction = responseFunction ?? throw new ArgumentNullException(nameof(responseFunction));

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return _responseFunction(request);
    }
}

