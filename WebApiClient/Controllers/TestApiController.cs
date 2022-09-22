using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiClient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestApiController : ControllerBase
    {

        public TestApiController()
        {
            
        }

        [HttpGet]
        [Route("Numeros")]
        public IEnumerable<int> GetNumerosRandom()
        {
            Random rand = new Random();
            var ints = Enumerable.Range(0, 10)
                                         .Select(i => new Tuple<int, int>(rand.Next(10), i))
                                         .OrderBy(i => i.Item1)
                                         .Select(i => i.Item2);
            return ints;

        }
        [HttpPost]
        [Route("Palabra")]
        public IEnumerable<string> GetPalabraPorInicial(string inicial)
        {
            List<string> words = new List<string>() { "man", "rat", "cow", "chicken", "north", "south", "east", "west" };

            return words.Where(s => s.StartsWith(inicial));

        }
    }
}
