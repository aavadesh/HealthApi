using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using HealthApi.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace HealthApi.Models.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(HealthDbContext dbContext, IWebHostEnvironment env)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password).MinimumLength(6);

            RuleFor(x => x.ConfirmPassword).Equal(e => e.Password);

            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    var emailInUse = dbContext.Users.Any(u => u.Email == value);
                    if (emailInUse)
                    {
                        context.AddFailure("Email", "That email is taken");
                    }

                    if (env.IsDevelopment())
                    {
                        context.AddFailure("Email", "That email is taken");
                    }
                    else
                    {
                        context.AddFailure("Email", "");
                    }
                });
        }
    }
}
