using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskApi.BLL.Dto;
using TaskApi.BLL.Interfaces;
using TaskApi.Json;
using TaskApi.Models;

namespace TaskApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private const long MaxFileSize = 10L * 1024L * 1024L * 1024L;

        private readonly ITicketService _taskService;
        private readonly IMapper _mapper;

        public TicketController(ITicketService taskService, IMapper mapper)
        {
            _taskService = taskService;
            _mapper = mapper;
        }

        // GET api/<TicketController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketGetResponse>> GetAsync(int id)
        {
            var result = await _taskService.GetTaskAsync(id);
            if(result == null)
            {
                return NotFound();
            }
            else
            {
                return _mapper.Map<TicketGetResponse>(result);
            }
        }


        // POST api/<TicketController>
        [HttpPost]
        [DisableFormValueModelBinding]
        [RequestSizeLimit(MaxFileSize)]
        [RequestFormLimits(MultipartBodyLengthLimit = MaxFileSize)]
        public async Task<ActionResult<int>> PostAsync(TicketPostRequest value)
        {
            try
            {
                if (FileNameValidation(value?.Files?.Select(x => x.FileName)))
                {
                    return BadRequest("Task files contain duplicate names");
                }
                return Ok(await _taskService.CreateTaskAsync(_mapper.Map<TicketDto>(value)));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
        
        // PUT api/<TicketController>/5
        [HttpPut("{id}")]
        [DisableFormValueModelBinding]
        [RequestSizeLimit(MaxFileSize)]
        [RequestFormLimits(MultipartBodyLengthLimit = MaxFileSize)]
        public async Task<ActionResult<int>> PutAsync(int id, TicketPutRequest value) {
            if (FileNameValidation(value?.Files?.Select(x => x.FileName)))
            {
                return BadRequest("Task files contain duplicate names");
            }
            var ticketDto = _mapper.Map<TicketDto>(value);
            ticketDto.Id = id;
            return await _taskService.UpdateTaskAsync(ticketDto);
        }
            
        // DELETE api/<TicketController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id) =>
             await _taskService.DeleteTaskAsync(id) ? Ok(id) : NotFound(id);

        /// <summary>
        /// Files validation. Files must contains unique names
        /// </summary>
        /// <param name="fileNames"></param>
        /// <returns></returns>
        private bool FileNameValidation(IEnumerable<string> fileNames)
        {
            if(fileNames != null && fileNames.Count() != fileNames.Distinct().Count())
            {
                return true;
            }
            return false;
        }
    }
}