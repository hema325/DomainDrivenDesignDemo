﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authentication.CreateEmailConfirmationToken
{
    public class SendEmailConfirmationCommand : IRequest
    {
        public string Email { get; set; }
        public string CallbackUrl { get; set; }
    }
}
