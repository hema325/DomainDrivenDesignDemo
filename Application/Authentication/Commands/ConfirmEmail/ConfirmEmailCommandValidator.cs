﻿namespace Application.Authentication.ConfirmEmail
{
    public class ConfirmEmailCommandValidator: AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidator()
        {
            RuleFor(c => c.UserId).NotEmpty().NotNull();
            RuleFor(c => c.Token).NotEmpty().NotNull();
        }
    }
}
