﻿namespace SA.Application.Account
{
    public class AuthResponse
    {
        public int UserId { get; set; }
        public int CustomerId { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public string Language { get; set; }
        public bool IsDealer { get; set; }
        public string Error { get; set; }

    }
}
