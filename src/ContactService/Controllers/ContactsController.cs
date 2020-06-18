using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactService.Models;
using ContactService.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.Extensions.Logging;

namespace ContactService.Controllers
{
    //[Authorize] No authorization mechanism added as part of the implemetation. This must be done for production
    [Route("[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        #region Constructor
        public ContactsController(ILogger<ContactsController> logger)
        {
            _logger = logger;
            //Since there are no logging specifications in the scope of work, I am not implementing it here.
            //In production, audit logging should be considered and should meet with the industry and application standards
            _repository = new ContactRepository();
        }
        #endregion

        #region Methods
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _repository.List();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var contact = await _repository.Find(id);
            
            if (contact == null)
            {
                _logger.LogWarning("User searched for contact that does not exist: ID: {0}", id);
                return NotFound();
            }
            return Ok(contact);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Contact contact)
        {
            try
            {
                var id = await _repository.Insert(contact);
                return Ok(id);
            }
            catch (Exception e)
            {
                //Logger is not implemented yet. This error should be logged
                _logger.LogError("ContactsController.Post Exception '{0}'", e);
                return BadRequest("Bad Request");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Contact contact)
        {
            try
            {
                await _repository.Update(id, contact);
                return Ok();
            }
            catch (Exception e)
            {
                //Logger is not implemented yet. This error should be logged
                _logger.LogError("ContactsController.Put Exception '{0}'", e);
                return BadRequest("Bad Request");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _repository.Delete(id))
            {
                return Ok();
            }
            else
            {
                //Logger is not implemented yet. This error should be logged
                _logger.LogError("ContactsController.Delete Record with ID '{0}' was not deleted");
                return NotFound();
            }
        }
        #endregion

        #region Members
        private readonly ContactRepository _repository;
        private readonly ILogger _logger;
        #endregion
    }
}
