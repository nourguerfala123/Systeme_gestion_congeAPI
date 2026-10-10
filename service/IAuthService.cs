using gestion_congeAPI.DOTs;

namespace gestion_congeAPI.service
{
    public interface IAuthService
    {
        Task<string> EmpCreation(Creation dto);
    }
}

