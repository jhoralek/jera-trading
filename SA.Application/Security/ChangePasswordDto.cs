﻿namespace SA.Application.Security
{
    public class ChangePasswordDto
    {
        public string OldPassword { get; set; }
        public string RepeatOldPassword { get; set; }
        public string NewPassword { get; set; }
        public string Token { get; set; }
    }
}
