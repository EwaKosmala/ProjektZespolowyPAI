using lab1_gr1.Models;
using AutoMapper;

namespace lab1_gr1.Services;

public abstract class BaseService
{
    protected readonly MyDBContext _dbContext;
    protected readonly IMapper _mapper;

    public BaseService(MyDBContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
}
