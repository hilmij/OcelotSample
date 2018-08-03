using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Mime;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ocelot.Middleware;
using Ocelot.Middleware.Multiplexer;

namespace ApiGateway
{
    public class AllAggregator : IDefinedAggregator
    {
        public Task<DownstreamResponse> Aggregate(List<DownstreamResponse> responses)
        {
            JProperty CreateProperty(string name, DownstreamResponse response) => new JProperty(name,
                JArray.FromObject(
                    JsonConvert.DeserializeObject<List<string>>(response.Content.ReadAsStringAsync().Result)));

            return Task.FromResult(new DownstreamResponse(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<JObject>(new JObject(
                    CreateProperty("customers", responses[0]),
                    CreateProperty("products", responses[1])), 
                    new JsonMediaTypeFormatter(), MediaTypeNames.Application.Json)
            }));
        }
    }
}
