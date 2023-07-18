using AutoMapper;
using SA.Application.Account;
using SA.Application.Bid;
using SA.Application.Country;
using SA.Application.Customer;
using SA.Application.Records;
using SA.Core.Model;
using System.Linq;

namespace SA.Web.Models
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Record, RecordTableDto>()
                   .ForMember(dto => dto.CurrentPrice, dto => dto.MapFrom(x => x.Bids.Any()
                       ? x.Bids.OrderByDescending(y => y.Price).FirstOrDefault().Price
                       : x.StartingPrice))
                   .ForMember(dto => dto.WinningUserId, dto => dto.MapFrom(x => x.Bids.Any()
                       ? x.Bids.OrderByDescending(y => y.Price).FirstOrDefault().UserId
                       : 0))
                   .ForMember(dto => dto.BiddingUserIds, dto => dto.MapFrom(x => x.Bids.Select(y => y.UserId)))
                   .ForMember(dto => dto.NumberOfBids, dto => dto.MapFrom(x => x.Bids.Count()))
                   .ForMember(dto => dto.RegistrationYear, dto => dto.MapFrom(x => x.DateOfFirstRegistration.HasValue ? x.DateOfFirstRegistration.Value.Year as int? : null))
                   .ForMember(dto => dto.AuctionName, dto => dto.MapFrom(x => x.Auction.Name));
            CreateMap<Record, RecordDetailDto>()
                .ForMember(dto => dto.CurrentPrice, dto => dto.MapFrom(x => x.Bids.Any()
                    ? x.Bids.OrderByDescending(y => y.Price).FirstOrDefault().Price
                    : x.StartingPrice))
                .ForMember(dto => dto.NumberOfBids, dto => dto.MapFrom(x => x.Bids.Count()));
            CreateMap<Record, RecordMinimumDto>()
                .ForMember(dto => dto.CurrentPrice, dto => dto.MapFrom(x => x.Bids.Any()
                    ? x.Bids.OrderByDescending(y => y.Price).FirstOrDefault().Price
                    : x.StartingPrice))
                .ForMember(dto => dto.WinningUserId, dto => dto.MapFrom(x => x.Bids.Any()
                    ? x.Bids.OrderByDescending(y => y.Price).FirstOrDefault().UserId
                    : 0))
                .ForMember(dto => dto.BiddingUserIds, dto => dto.MapFrom(x => x.Bids
                    .Select(y => y.UserId)));

            CreateMap<File, FileSimpleDto>();

            CreateMap<Bid, BidSimpleDto>()
                .ForMember(dto => dto.UserName, dto => dto.MapFrom(x => x.User.UserName))
                .ForMember(dto => dto.RecordValidTo, dto => dto.MapFrom(x => x.Record.ValidTo));

            CreateMap<Customer, CustomerSimpleDto>();

            CreateMap<User, UserDto>();
            CreateMap<User, UserSimpleDto>()
                .ForMember(dto => dto.IsFeePayed, dto => dto.MapFrom(x => x.Customer.IsFeePayed))
                .ForMember(dto => dto.PhoneNumber, dto => dto.MapFrom(x => x.Customer.PhoneNumber))
                .ForMember(dto => dto.Email, dto => dto.MapFrom(x => x.Customer.Email))
                .ForMember(dto => dto.BirthNumber, dto => dto.MapFrom(x => x.Customer.BirthNumber))
                .ForMember(dto => dto.CompanyNumber, dto => dto.MapFrom(x => x.Customer.CompanyNumber))
                .ForMember(dto => dto.SendEmail, dto => dto.MapFrom(x => false));
            CreateMap<User, UserShortDto>()
                .ForMember(dto => dto.Email, dto => dto.MapFrom(x => x.Customer.Email))
                .ForMember(dto => dto.PhoneNumber, dto => dto.MapFrom(x => x.Customer.PhoneNumber))
                .ForMember(dto => dto.FullName, dto => dto.MapFrom(x => $"{x.Customer.TitleBefore} {x.Customer.FirstName} {x.Customer.LastName} {x.Customer.TitleAfter}"))
                .ForMember(dto => dto.CompanyNumber, dto => dto.MapFrom(x => x.Customer.CompanyNumber))
                .ForMember(dto => dto.CompanyName, dto => dto.MapFrom(x => x.Customer.CompanyName))
                .ForMember(dto => dto.Street, dto => dto.MapFrom(x => x.Customer.Address.Street))
                .ForMember(dto => dto.City, dto => dto.MapFrom(x => x.Customer.Address.City))
                .ForMember(dto => dto.PostCode, dto => dto.MapFrom(x => x.Customer.Address.PostCode));

            CreateMap<Country, CountryLookupDto>();
            CreateMap<Country, CountryDto>();
            CreateMap<Country, Country>();

            CreateMap<GdprRecord, GdprRecordTableDto>()
                .ForMember(dto => dto.FullName, dto => dto.MapFrom(x => $"{x.FirstName} {x.LastName}"));
            CreateMap<GdprRecord, GdprRecordDto>();

            CreateMap<Auction, AuctionDto>();
            CreateMap<Auction, AuctionTableDto>()
                .ForMember(dto => dto.NumberOfRecords, dto => dto.MapFrom(x => x.Records.Count()));
            CreateMap<Auction, AuctionLookupDto>()
                .ForMember(dto => dto.Name, dto => dto.MapFrom(x => $"{x.Name} - [{x.ValidFrom.ToString("dd.MM.yyyy HH:mm")} - {x.ValidTo.ToString("dd.MM.yyyy HH:mm")}] - {(x.IsActive ? "Aktivní" : "Neaktivní")}"));

            // reverse mapping
            CreateMap<UserDto, User>();
            CreateMap<UserSimpleDto, User>();
            CreateMap<RecordDetailDto, Record>()
                .ForMember(x => x.Auction, x => x.Ignore())
                .ForMember(x => x.Files, x => x.Ignore())
                .ForMember(x => x.Bids, x => x.Ignore())
                .ForMember(x => x.User, x => x.Ignore());
            CreateMap<RecordTableDto, Record>()
                .ForMember(x => x.User, x => x.Ignore())
                .ForMember(x => x.Files, x => x.Ignore());
            CreateMap<AuctionDto, Auction>()
                .ForMember(x => x.Records, x => x.Ignore());

            // update mapping
            CreateMap<User, User>();
            CreateMap<Record, Record>()
                .ForMember(x => x.User, x => x.Ignore())
                .ForMember(x => x.Auction, x => x.Ignore())
                .ForMember(x => x.Files, x => x.Ignore())
                .ForMember(x => x.Bids, x => x.Ignore());
            CreateMap<Customer, Customer>();
            CreateMap<Address, Address>();
            CreateMap<UserActivation, UserActivation>();
            CreateMap<Auction, Auction>()
                .ForMember(x => x.Records, x => x.Ignore());
        }
    }
}
