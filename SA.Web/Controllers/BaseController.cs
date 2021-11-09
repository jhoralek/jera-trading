using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SA.EntityFramework.EntityFramework.Repository;

namespace SA.WebApi.Controllers
{
    public class BaseController<T> : Controller
    {
        protected IEntityRepository<T> _repository { get; set; }
        protected IMapper _mapper;

        public BaseController(IEntityRepository<T> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
    }
}
