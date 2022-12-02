using Microsoft.AspNetCore.Mvc;
using SigmaTaskAPI.BLL.CandidateServ;
using SigmaTaskAPI.BLL.DtoModels;

namespace SigmaTaskAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CandidateController : ControllerBase
    {
        private ICandidateService _candidateService;

        private readonly ILogger<CandidateController> _logger;

        public CandidateController(ILogger<CandidateController> logger, ICandidateService candidateService)
        {
            _logger = logger;
            _candidateService = candidateService;
        }


        /// <summary>
        /// Insert ot update candidate
        /// </summary>
        /// <param name="candidateModel">The candidate data.</param>
        /// <response code="400">BadRequest - returned if any validation issue happened.</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="200">Candidate created or updated successfully</response>
        [Route("InsertOrUpdate")]
        [ResponseCache(Duration = 30)]
        [Produces(typeof(Result))]
        [HttpPost]
        public async Task<ActionResult<Result>> InsertOrUpdate(CandidateModel candidateModel)
        {
            var result = new Result();
            var isInternalServerError = false;

            if (candidateModel == null || !ModelState.IsValid)
            {
                result.Succeeded = false;
                result.Errors = ModelState.Select(m => new Error { Message = $"{m.Key} => {m.Value}" }).ToList();
                
                return BadRequest(result);
            }
            try
            {
                result = await _candidateService.InsertOrUpdate(candidateModel);

            }
            catch (IOException io)
            {
                result.Succeeded = false;
                result.Errors.Add(new Error { Message = $"Close the CSV file. {io.Message }" });
                isInternalServerError = true;
                _logger.Log(LogLevel.Error, io.Message);
            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                result.Errors.Add(new Error { Message = ex.Message });
                isInternalServerError = true;
                _logger.Log(LogLevel.Error, ex.Message);
            }

            if(isInternalServerError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            else if (result.Succeeded)
            {
                return Ok(result);
            }
            else
            {

                return BadRequest(result);
            }

        }
    }
}