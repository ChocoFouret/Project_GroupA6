using Application;
using Infrastructure.EF.Has;
using Service.UseCases.Has.Dtos;

namespace Service.UseCases.Has;

public class UseCaseFetchHasByFunctions
{
    private readonly IHasRepository _hasRepository;

    public UseCaseFetchHasByFunctions(IHasRepository hasRepository)
    {
        _hasRepository = hasRepository;
    }
    
    public IEnumerable<DtoOutputHas> Execute(int id)
    {
        var dbHas = _hasRepository.FetchByFunctions(id);
        return Mapper.GetInstance().Map<IEnumerable<DtoOutputHas>>(dbHas);
    }
}