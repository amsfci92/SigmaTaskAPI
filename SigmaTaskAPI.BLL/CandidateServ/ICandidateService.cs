using SigmaTaskAPI.BLL.DtoModels;

namespace SigmaTaskAPI.BLL.CandidateServ
{
    public interface ICandidateService
    {
        Task<Result> InsertOrUpdate(CandidateModel candidateModel);
    }
}