using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Annotations;

namespace RiverLinkReporter.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// Gets some values.
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(string[]), 200)]
        [ProducesResponseType(typeof(NotFoundObjectResult), 400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(OperationId = "GetSomeValues")]
        [HttpGet]
        [Route("api/v1/GetSomeValues", Name = "GetSomeValues")]
        public ActionResult<IEnumerable<string>> GetSomeValues()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet]
        [Route("api/v1/GetById", Name = "GetById")]
        public ActionResult<string> GetById(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
