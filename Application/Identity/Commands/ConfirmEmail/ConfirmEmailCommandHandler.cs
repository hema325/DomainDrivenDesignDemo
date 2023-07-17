﻿using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Identity.Commands.ConfirmEmail
{
    internal class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand>
    {
        private readonly IAuthentication _authentication;

        public ConfirmEmailCommandHandler(IAuthentication authentication)
        {
            _authentication = authentication;
        }

        public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var result = await _authentication.ConfirmEmailAsync(request.UserId, request.Token);

            if (!result.Succeeded)
                throw new Exception(string.Join(",", result.Errors));

            await _authentication.SignInAsync(request.UserId);

            return Unit.Value;
        }
    }
}
