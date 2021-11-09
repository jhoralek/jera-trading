using SA.Core.Enums;
using System;

namespace SA.Application.Records
{
    public class GdprRecordDto
    {
        public virtual int Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }        
        public virtual string CompanyNumber { get; set; }        
        public virtual string BirthNumber { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual string PhoneNumber { get; set; }        
        public virtual string Street { get; set; }        
        public virtual string City { get; set; }        
        public virtual string PostCode { get; set; }        
        public virtual int CountryId { get; set; }        
        public virtual string Description { get; set; }
        public virtual GdprApplicationType Type { get; set; }
        public virtual bool IsProcessed { get; set; }
        public virtual DateTime Created { get; set; }
    }
}
