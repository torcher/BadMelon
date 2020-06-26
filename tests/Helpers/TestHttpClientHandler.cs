using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BadMelon.Tests.Helpers
{
    public class TestHttpClientHandler : DelegatingHandler
    {
        private readonly CookieContainer cookies = new CookieContainer();

        public TestHttpClientHandler([NotNull] HttpMessageHandler innerHandler)
            : base(innerHandler) { }

        protected override async Task<HttpResponseMessage> SendAsync([NotNull] HttpRequestMessage request, CancellationToken ct)
        {
            Uri requestUri = request.RequestUri;
            request.Headers.Add(HeaderNames.Cookie, this.cookies.GetCookieHeader(requestUri));

            HttpResponseMessage response = await base.SendAsync(request, ct);

            if (response.Headers.TryGetValues(HeaderNames.SetCookie, out IEnumerable<string> setCookieHeaders))
            {
                foreach (SetCookieHeaderValue cookieHeader in SetCookieHeaderValue.ParseList(setCookieHeaders.ToList()))
                {
                    Cookie cookie = new Cookie(cookieHeader.Name.Value, cookieHeader.Value.Value, cookieHeader.Path.Value);
                    if (cookieHeader.Expires.HasValue)
                    {
                        cookie.Expires = cookieHeader.Expires.Value.DateTime;
                    }
                    this.cookies.Add(requestUri, cookie);
                }
            }

            return response;
        }
    }
}