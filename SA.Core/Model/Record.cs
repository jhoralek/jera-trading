using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SA.Core.Model
{
    public class Record : Entity<int>
    {
        [Required]
        public virtual string Name { get; set; }

        [Required]
        [DefaultValue(false)]
        public virtual bool IsActive { get; set; }

        [MaxLength(200)]
        public virtual string Color { get; set; }

        [MaxLength(50)]
        public virtual string Mileage { get; set; }

        [MaxLength(45)]
        public virtual string EngineCapacity { get; set; }

        [MaxLength(100)]
        public virtual string Body { get; set; }

        [MaxLength(45)]
        public virtual string Power { get; set; }

        [MaxLength(10)]
        public virtual string Fuel { get; set; }

        [MaxLength(45)]
        public virtual string Transmission { get; set; }

        [MaxLength(45)]
        public virtual string Axle { get; set; }

        [MaxLength(100)]
        public virtual string RegistrationCheck { get; set; }

        [MaxLength(100)]
        public virtual string ContactToAppointment { get; set; }

        [Required]
        public virtual Decimal MinimumBid { get; set; }

        [Required]
        public virtual Decimal StartingPrice { get; set; }

        [Required]
        public virtual DateTime ValidFrom { get; set; }

        [Required]
        public virtual DateTime ValidTo { get; set; }

        public virtual string Defects { get; set; }

        public virtual string MoreDescription { get; set; }

        [MaxLength(100)]
        public virtual string State { get; set; }


        public virtual string Equipment { get; set; }

        [MaxLength(45)]
        public virtual string Vin { get; set; }

        public virtual int? NumberOfSeets { get; set; }

        [MaxLength(10)]
        public virtual string EuroNorm { get; set; }

        public virtual int? Doors { get; set; }

        public virtual DateTime? DateOfFirstRegistration { get; set; }

        public virtual DateTime? Stk { get; set; }

        [MaxLength(250)]
        public virtual string Dimensions { get; set; }

        [MaxLength(250)]
        public virtual string OperationWeight { get; set; }

        [MaxLength(250)]
        public virtual string MostTechnicallyAcceptableWeight { get; set; }

        [MaxLength(100)]
        public virtual string MaximumWeight { get; set; }

        [MaxLength(100)]
        public virtual string MostTechnicallyWeightOfRide { get; set; }

        [MaxLength(100)]
        public virtual string MaximumWeightOfRide { get; set; }

        [MaxLength(100)]
        public virtual string Place { get; set; }

        [Required]
        public virtual int UserId { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<File> Files { get; set; } = new List<File>();

        public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();

        [Required]
        public virtual int AuctionId { get; set; }

        public virtual Auction Auction { get; set; }

        [Required]
        [DefaultValue(false)]
        public virtual bool WithVat { get; set; }
    }
}
