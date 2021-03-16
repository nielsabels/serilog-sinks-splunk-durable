using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Serilog.Sinks.Splunk.Plus
{
    internal class EventCollectorClient : HttpClient, IDisposable
    {
        private const string AUTH_SCHEME = "Splunk";
        private const string SPLUNK_REQUEST_CHANNEL = "X-Splunk-Request-Channel";

        public EventCollectorClient(string eventCollectorToken) : base()
        {
            SetHeaders(eventCollectorToken);
        }

        public EventCollectorClient(string eventCollectorToken, HttpMessageHandler messageHandler) : base(messageHandler)
        {
            SetHeaders(eventCollectorToken);
        }

        private void SetHeaders(string eventCollectorToken)
        {
            DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AUTH_SCHEME, eventCollectorToken);
            if (!this.DefaultRequestHeaders.Contains(SPLUNK_REQUEST_CHANNEL))
            {
                this.DefaultRequestHeaders.Add(SPLUNK_REQUEST_CHANNEL, Guid.NewGuid().ToString());
            }
        }
    }

}
