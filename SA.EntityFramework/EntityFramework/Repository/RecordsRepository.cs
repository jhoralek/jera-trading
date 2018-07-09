﻿using Microsoft.EntityFrameworkCore;
using SA.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SA.EntityFramework.EntityFramework.Repository
{
    public class RecordsRepository : IEntityRepository<Record>
    {
        private readonly SaDbContext _context;
        public RecordsRepository(SaDbContext context)
        {
            _context = context;
        }

        public async Task Add(Record item)
        {
            item.Created = DateTime.Now;
            await _context.Records.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Record>> Find(string key)
            => await GetRecordsInternal()
                .Where(x => x.Name.StartsWith(key))
                .ToListAsync();

        public async Task<IEnumerable<Record>> GetAll()
            => await GetRecordsInternal().ToListAsync();

        public async Task<Record> GetById(int id)
            => await GetRecordsInternal().FirstOrDefaultAsync(x => x.Id == id);

        public async Task Remove(int id)
        {
            var itemToDelte = await GetQueryAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (itemToDelte != null)
            {
                _context.Remove(itemToDelte);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Update(int id, Record item)
        {
            var itemToUpdate = await GetQueryAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (itemToUpdate != null)
            {
                itemToUpdate.Name = item.Name;
                itemToUpdate.MinimumBid = item.MinimumBid;
                itemToUpdate.IsActive = item.IsActive;
                itemToUpdate.Axle = item.Axle;
                itemToUpdate.Body = item.Body;
                itemToUpdate.Colors = item.Colors;
                itemToUpdate.ContactToAppointment = item.ContactToAppointment;
                itemToUpdate.CustomerId = item.CustomerId;
                itemToUpdate.DateOfFirstRegistration = item.DateOfFirstRegistration;
                itemToUpdate.Defects = item.Defects;
                itemToUpdate.Dimensions = item.Dimensions;
                itemToUpdate.Doors = item.Doors;
                itemToUpdate.EngineCapacity = item.EngineCapacity;
                itemToUpdate.Equipment = item.Equipment;
                itemToUpdate.EuroNorm = item.EuroNorm;
                itemToUpdate.Fuel = item.Fuel;
                itemToUpdate.MaximumWeight = item.MaximumWeight;
                itemToUpdate.MaximumWeightOfRide = item.MaximumWeightOfRide;
                itemToUpdate.Mileage = item.Mileage;
                itemToUpdate.MoreDescription = item.MoreDescription;
                itemToUpdate.MostTechnicallyAcceptableWeight = item.MostTechnicallyAcceptableWeight;
                itemToUpdate.MostTechnicallyWeightOfRide = item.MostTechnicallyWeightOfRide;
                itemToUpdate.NumberOfSeets = item.NumberOfSeets;
                itemToUpdate.OperationWeight = item.OperationWeight;
                itemToUpdate.Power = item.Power;
                itemToUpdate.RegistrationCheck = item.RegistrationCheck;
                itemToUpdate.StartingPrice = item.StartingPrice;
                itemToUpdate.State = item.State;
                itemToUpdate.Stk = item.Stk;
                itemToUpdate.ValidFrom = item.ValidFrom;
                itemToUpdate.ValidTo = item.ValidTo;
                itemToUpdate.Vin = item.Vin;

                await _context.SaveChangesAsync();
            }
        }

        public IQueryable<Record> GetQueryAll()
            => _context.Records.AsQueryable();

        private IQueryable<Record> GetRecordsInternal()
            => GetQueryAll()
                .Include(x => x.User)
                .Include(x => x.Customer);
    }
}
