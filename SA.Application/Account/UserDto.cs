using System;

namespace SA.Application.Account
{
    public class UserDto
    {
        public virtual int Id { get; set; }

        public virtual string UserName { get; set; }

        public virtual string Password { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual bool IsAgreeementToTerms { get; set; }

        public virtual bool SendingNews { get; set; }

        public virtual string Token { get; set; }

        public virtual string Language { get; set; }

        public virtual int CustomerId { get; set; }
        public virtual DateTime Created { get; set; }
    }
}
