using System.Web.Http;
using Newtonsoft.Json.Linq;

namespace PBManager.Web.ControllersApi
{
    public class ChartsApiController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            var datasets = @"
             [
                    {
                        label: 'Electronics',
                        fillColor: 'rgba(210, 214, 222, 1)',
                        strokeColor: 'rgba(210, 214, 222, 1)',
                        pointColor: 'rgba(210, 214, 222, 1)',
                        pointStrokeColor: '#c1c7d1',
                        pointHighlightFill: '#fff',
                        pointHighlightStroke: 'rgba(220,220,220,1)',
                        data: [65, 59, 80, 81, 56, 55, 40]
                    },
                    {
                        label: 'Digital Goods',
                        fillColor: 'rgba(60,141,188,0.9)',
                        strokeColor: 'rgba(60,141,188,0.8)',
                        pointColor: '#3b8bba',
                        pointStrokeColor: 'rgba(60,141,188,1)',
                        pointHighlightFill: '#fff',
                        pointHighlightStroke: 'rgba(60,141,188,1)',
                        data: [28, 48, 40, 19, 86, 27, 90]
                    }
                ]
            ";
            // var jo = JObject.Parse(datasets);

            var jo = JArray.Parse(datasets);

            return Ok(jo);
        }
    }
}