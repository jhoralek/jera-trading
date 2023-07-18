using System;

namespace SA.Application.Account
{
    public class UserSimpleDto
    {
        public virtual int Id { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual string UserName { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual bool IsAgreementToTerms { get; set; }

        public virtual bool SendingNews { get; set; }

        public virtual string Token { get; set; }

        public virtual string Language { get; set; }

        public virtual int CustomerId { get; set; }

        public virtual bool IsFeePayed { get; set; }

        public virtual string Email { get; set; }

        public virtual string PhoneNumber { get; set; }

        public virtual string BirthNumber { get; set; }

        public virtual string CompanyNumber { get; set; }

        public virtual bool SendEmail { get; set; }
    }
}
