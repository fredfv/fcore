using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pressF.API.Authentication;
using pressF.API.Enums;
using pressF.API.Model;
using pressF.API.Repository.Interfaces;
using pressF.API.ViewModel;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace pressF.API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonRepository _personRepository;
        private readonly IUnitOfWork _uow;

        public PersonController(IPersonRepository personRepository, IUnitOfWork uow)
        {
            _personRepository = personRepository;
            _uow = uow;
        }

        [Authorize(Roles = "admin")]
        [HttpGet("getall")]
        public async Task<ActionResult<IEnumerable<Person>>> GetAll()
        {
            if(await _personRepository.IsBlocked(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)) return StatusCode(StatusCodes.Status403Forbidden, new { message = "User is excluded" });

            var products = await _personRepository.GetAll();

            return Ok(products);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("get")]
        public async Task<ActionResult<Person>> Get(string id)
        {
            var product = await _personRepository.GetById(id);

            return Ok(product);
        }
        
        [Authorize(Roles = "admin")]
        [HttpPost("insert")]
        public async Task<ActionResult<Person>> Insert([FromBody] PersonViewModel value)
        {
            var person = new Person(value);
            _personRepository.Add(person);

            await _uow.Commit();

            return Ok(person);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("update")]
        public async Task<ActionResult<Person>> Update(string id, [FromBody] PersonViewModel value)
        {
            var person = new Person(value, _personRepository.GetById(id).Result);

            _personRepository.Update(person);

            await _uow.Commit();

            return Ok(await _personRepository.GetById(id));
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("delete")]
        public async Task<ActionResult> Delete(string id)
        {
            _personRepository.Remove(id);

            await _uow.Commit();

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("auth")]
        public async Task<ActionResult<AuthorizedToken>> Auth([FromBody] AuthRequest request)
        {
            Log.Write(Serilog.Events.LogEventLevel.Information, JsonSerializer.Serialize(request));
            var authresponse = await _personRepository.Auth(request.Password, request.Username);

            if (authresponse.Status == StatusAuthResponse.NotFound)
                return StatusCode(StatusCodes.Status404NotFound, new { message = authresponse.Message });

            if (authresponse.Status == StatusAuthResponse.Excluded)
                return StatusCode(StatusCodes.Status403Forbidden, new { message = authresponse.Message });

            if (authresponse.Status == StatusAuthResponse.Error)
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = authresponse.Message });

            var token = TokenService.GenerateToken(authresponse.AuthoredPerson);
            authresponse.AuthoredPerson.Password = "";

            return Ok(new AuthorizedToken { Person = authresponse.AuthoredPerson, Token = token });
        }

        [AllowAnonymous]
        [HttpPost("findnew")]
        public async Task<ActionResult<IEnumerable<Person>>> FindNew([FromBody] DateTimeOffset date)
        {
            var product = await _personRepository.New(date);

            return Ok(product);
        }
    }
}