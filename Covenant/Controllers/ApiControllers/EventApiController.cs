﻿// Author: Ryan Cobb (@cobbr_io)
// Project: Covenant (https://github.com/cobbr/Covenant)
// License: GNU GPLv3

using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Covenant.Core;
using Covenant.Models;
using Covenant.Models.Covenant;

namespace Covenant.Controllers
{
    [Authorize(Policy = "RequireJwtBearer")]
    [ApiController]
    [Route("api/events")]
    public class EventApiController : Controller
    {
        private readonly CovenantContext _context;

        public EventApiController(CovenantContext context)
        {
            _context = context;
        }

        // GET: api/events
        // <summary>
        // Get a list of Events
        // </summary>
        [HttpGet(Name = "GetEvents")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            return Ok(await _context.GetEvents());
        }

        // GET api/events/{id}
        // <summary>
        // Get an Event by id
        // </summary>
        [HttpGet("{id}", Name = "GetEvent")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            try
            {
                return await _context.GetEvent(id);
            }
            catch (ControllerNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (ControllerBadRequestException e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/events/time
        // <summary>
        // Get Covenant's current DateTime
        // </summary>
        [HttpGet("time", Name = "GetEventTime")]
        public ActionResult<long> GetEventTime()
        {
            try
            {
                return _context.GetEventTime();
            }
            catch (ControllerNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (ControllerBadRequestException e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/events/range/{fromdate}
        // <summary>
        // Get a list of Events that occurred after the specified DateTime
        // </summary>
        [HttpGet("range/{fromdate}", Name = "GetEventsAfter")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventsAfter(long fromdate)
        {
            try
            {
                return Ok(await _context.GetEventsAfter(fromdate));
            }
            catch (ControllerNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (ControllerBadRequestException e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/events/range/{fromdate}/{todate}
        // <summary>
        // Get a list of Events that occurred between the range of specified DateTimes
        // </summary>
        [HttpGet("range/{fromdate}/{todate}", Name = "GetEventsRange")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventsRange(long fromdate, long todate)
        {
            try
            {
                return Ok(await _context.GetEventsRange(fromdate, todate));
            }
            catch (ControllerNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (ControllerBadRequestException e)
            {
                return BadRequest(e.Message);
            }
        }

		// POST api/events
		// <summary>
		// Create an Event
		// </summary>
		[HttpPost(Name = "CreateEvent")]
		[ProducesResponseType(typeof(Event), 201)]
		public async Task<ActionResult<Event>> CreateEvent([FromBody]Event anEvent)
		{
            try
            {
                Event createdEvent = await _context.CreateEvent(anEvent);
                return CreatedAtRoute(nameof(GetEvent), new { id = createdEvent.Id }, createdEvent);
            }
            catch (ControllerNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (ControllerBadRequestException e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/events/download/{id}
        // <summary>
        // Get a DownloadEvent
        // </summary>
        [HttpGet("download/{id}", Name = "GetDownloadEvent")]
        public async Task<ActionResult<DownloadEvent>> GetDownloadEvent(int id)
        {
            try
            {
                return await _context.GetDownloadEvent(id);
            }
            catch (ControllerNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (ControllerBadRequestException e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/events/download/{id}/content
        // <summary>
        // Get a downloaded file
        // </summary>
        [HttpGet("download/{id}/content", Name = "GetDownloadContent")]
        public async Task<ActionResult<string>> GetDownloadContent(int id)
        {
            try
            {
                return await _context.GetDownloadContent(id);
            }
            catch (ControllerNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (ControllerBadRequestException e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST api/events/download
        // <summary>
        // Post a downloaded file or portion of a downloaded file
        // </summary>
        [HttpPost("download", Name = "CreateDownloadEvent")]
        [ProducesResponseType(typeof(Event), 201)]
        public async Task<ActionResult> CreateDownloadEvent([FromBody]DownloadEvent downloadEvent)
        {
            try
            {
                DownloadEvent createdEvent = await _context.CreateDownloadEvent(downloadEvent);
                return CreatedAtRoute(nameof(GetEvent), new { id = createdEvent.Id }, createdEvent);
            }
            catch (ControllerNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (ControllerBadRequestException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
