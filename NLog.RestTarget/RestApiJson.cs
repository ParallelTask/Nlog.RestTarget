using Newtonsoft.Json;
using NLog.Config;
using NLog.Layouts;
using NLog.RestTarget.Models;
using NLog.Targets;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLog.RestTarget
{
    [Target("RestService")]
    public sealed class RestApiJson : TargetWithLayout
    {
        private readonly RestClient _client;

        public RestApiJson()
        {
            Host = String.Empty;
            Headers = new List<MethodCallParameter>();
            _client = new RestClient();

            // Todo: Add Attributes
            // Attributes = new List<JsonAttribute>();
        }

        [RequiredParameter]
        public string Host { get; private set; }

        [ArrayParameter(typeof(MethodCallParameter), "header")]
        public IList<MethodCallParameter> Headers { get; private set; }

        // Todo: Add Attributes
        // [ArrayParameter(typeof(JsonAttribute), "attribute")]
        // public IList<JsonAttribute> Attributes { get; private set; }

        private LayoutWithHeaderAndFooter LHF
        {
            get => (LayoutWithHeaderAndFooter)base.Layout;
            set => base.Layout = value;
        }

        public override Layout Layout
        {
            get => LHF.Layout;

            set
            {
                if (value is LayoutWithHeaderAndFooter)
                {
                    base.Layout = value;
                }
                else if (LHF == null)
                {
                    LHF = new LayoutWithHeaderAndFooter()
                    {
                        Layout = value
                    };
                }
                else
                {
                    LHF.Layout = value;
                }
            }
        }

        protected override void Write(LogEventInfo logEvent)
        {
            var httpRequest = new RestHttpRequest();

            Headers
                .ToList()
                .ForEach(x => httpRequest.Headers.Add(x.Name, RenderLogEvent(x.Layout, logEvent)));

            httpRequest.Body = JsonConvert.DeserializeObject<ExpandoObject>(RenderLogEvent(LHF.Layout, logEvent));

            SendTheMessageToRemoteHost(Host, httpRequest);

            // Render Log Event, not required currently
            // string logMessage = this.Layout.Render(logEvent);
        }

        private void SendTheMessageToRemoteHost(string host, RestHttpRequest httpRequest)
        {
            var request = new RestRequest();

            if (host == String.Empty)
            {
                throw new NullReferenceException("Host property not set");
            }

            _client.BaseUrl = new Uri(host);
            request.Method = Method.POST;
            request.RequestFormat = DataFormat.Json;

            foreach (KeyValuePair<string, string> header in httpRequest.Headers)
            {
                request.AddHeader(header.Key, header.Value);
            }

            request.AddJsonBody(httpRequest.Body);

            Task.Factory
                .StartNew(() => _client.ExecuteTaskAsync(request))
                .ContinueWith((t) =>
                {
                    throw new NLogRuntimeException(JsonConvert.SerializeObject(t));

                }, TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}
