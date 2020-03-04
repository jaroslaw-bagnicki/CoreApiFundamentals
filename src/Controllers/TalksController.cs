using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace CoreCodeCamp.Controllers
{
    [ApiController]
    [Route("api/camps/{moniker}/talks")]
    public class TalksController : ControllerBase
    {
        private readonly ICampRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public TalksController(ICampRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<TalkModel[]>> GetAll(string moniker)
        {
            try
            {
                var results = await _repository.GetTalksByMonikerAsync(moniker);

                return _mapper.Map<TalkModel[]>(results);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{talkId:int}")]
        public async Task<ActionResult<TalkModel>> GetOne(string moniker, int talkId)
        {
            try
            {
                var result = await _repository.GetTalkByMonikerAsync(moniker, talkId);
                if (result == null) return NotFound(new {message = $"Talk with id: {talkId} not found."});

                return _mapper.Map<TalkModel>(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TalkModel>> Add(string moniker, [FromBody] TalkModel model)
        {
            try
            {
                var camp = await _repository.GetCampAsync(moniker);
                if (camp == null) return BadRequest(new {message = $"Camp with {moniker} moniker not exists."});

                var speaker = await _repository.GetSpeakerAsync(model.Speaker.SpeakerId);
                if (speaker == null) return BadRequest(new { message = $"Speaker with {model.Speaker.SpeakerId} id not exists." });

                var talk = _mapper.Map<Talk>(model);
                talk.Camp = camp;
                _repository.Add(talk);
                if (await _repository.SaveChangesAsync())
                {

                    var location = _linkGenerator.GetPathByAction("GetOne", "Talks", new { moniker, talkId = talk.TalkId });
                    return Created(location, _mapper.Map<TalkModel>(talk));
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return StatusCode(StatusCodes.Status500InternalServerError, "Something goes wrong");
        }
    }
}
