using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SA.Core.Model;
using SA.EntityFramework.EntityFramework.Repository;
using System.Threading.Tasks;

namespace SA.WebApi.Controllers
{
    [Route("api/Addresses")]
    public class AddressesController : BaseController<Address>
    {
        public AddressesController(IEntityRepository<Address> repository, IMapper mapper)
            : base(repository, mapper) { }

        [Authorize("read:messages")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Address address)
        {
            var item = await _repository.GetOneAsync<Address>(x => x.Id == address.Id);
            if (address == null && address.Id <= 0 && item == null)
            {
                return BadRequest();
            }
            //Mapper.Map(address, item);
            return Json(await _repository.UpdateAsync(item));
        }
    }
}