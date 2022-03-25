namespace Movies.Client.HttpHandlers
{
    public class AuthenticationDelegateHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken);
        }

        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.Send(request, cancellationToken);
        }
    }
}
