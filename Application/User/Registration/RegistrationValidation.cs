using System;
using System.Linq.Expressions;
using Application.Validators;
using FluentValidation;

namespace Application.User.Registration
{
	public class RegistrationValidation : AbstractValidator<RegistrationCommand>
	{
		public RegistrationValidation()
		{
			RuleFor(x => x.DisplayName).NotEmpty().Matches(@"[A-Za-zА-Яа-яЁё]+(\s+[A-Za-zА-Яа-яЁё]+)?").OnFailure(e => throw new ValidationException("Please provide human name"));
			RuleFor(x => x.UserName).Empty();
			RuleFor(x => x.Email).NotEmpty().EmailAddress();
			RuleFor(x => x.Password).NotEmpty().Password();
			RuleFor(x => x.Password2).NotEmpty().Password();
			RuleFor(x => x.Password).Equal(x => x.Password2).OnFailure(e => throw new ValidationException("Passwords don't match"));
		}
	}
}