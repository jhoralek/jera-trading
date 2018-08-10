﻿using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SA.Application.Customer;
using SA.Core.Model;
using SA.EntityFramework.EntityFramework.Repository;

namespace SA.WebApi.Controllers
{
    [Route("api/Customers")]
    public class CustomersController : BaseController<Customer>
    {
        public CustomersController(IEntityRepository<Customer> repository)
            : base(repository) { }

        [Authorize("read:messages")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Json(await _repository.GetAllAsync<CustomerSimpleDto, string>());

        [Authorize("read:messages")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Customer customer)
        {
            var item = await _repository.GetOneAsync<Customer>(x => x.Id == customer.Id);
            if (customer == null && customer.Id <= 0 && item == null)
            {
                return BadRequest();
            }
            Mapper.Map(customer, item);
            return Json(await _repository.UpdateAsync(item));
        }
    }
}