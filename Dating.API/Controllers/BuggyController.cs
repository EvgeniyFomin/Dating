using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dating.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BuggyController : ControllerBase
    {
        /// <summary>
        /// Returns error Bad Request (status code 400)
        /// </summary>
        /// <returns>status code 400</returns>
        [HttpGet("bad-request")]
        public IActionResult GetBadRequest()
        {
            return BadRequest("Just bad request to get");
        }

        /// <summary>
        /// Returns not authorized (status code 401)
        /// </summary>
        /// <returns>status code 401</returns>
        [Authorize]
        [HttpGet("unauthorized")]
        public IActionResult GetUnauthorized()
        {
            return Ok("try to get it haha");
        }

        /// <summary>
        /// Returns not found (status code 404)
        /// </summary>
        /// <returns>status code 404</returns>
        [HttpGet("not-found")]
        public IActionResult GetNotFound()
        {
            return NotFound("Not fount something");
        }

        /// <summary>
        /// Returns internal server error (status code 500)
        /// </summary>
        /// <returns>Status code 500</returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("server-error")]
        public IActionResult GetServerError()
        {
            var result = 1 != 2 ? throw new Exception("here we go status 500") : "good to see it";

            return Ok(result);
        }
    }
}
