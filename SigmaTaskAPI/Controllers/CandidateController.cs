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

        [HttpGet(Name = "InsertUpdate")]
        public async Task<ActionResult> InsertOrUpdate(CandidateModel candidateModel)
        {
            var result = new Result();
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
            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                result.Errors.Add(new Error { Message = ex.Message });
            }

            if (result.Succeeded)
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