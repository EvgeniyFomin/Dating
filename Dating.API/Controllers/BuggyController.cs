using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dating.API.Controllers
{
    public class BuggyController : ControllerBase
    {
        [HttpGet("bad-request")]
        public IActionResult GetBadRequest()
        {
            return BadRequest("Just bad request to get");
        }

        [Authorize]
        [HttpGet("unauthorized")]
        public IActionResult GetUnauthorized()
        {
            return Ok("try to get it haha");
        }

        [HttpGet("not-found")]
        public IActionResult GetNotFound()
        {
            return NotFound("Not fount something");
        }

        [HttpGet("server-error")]
        public IActionResult GetServerError()
        {
            var result = 1 != 2 ? throw new Exception("here we go status 500") : "good to see it";

            return Ok(result);
        }
    }
}
