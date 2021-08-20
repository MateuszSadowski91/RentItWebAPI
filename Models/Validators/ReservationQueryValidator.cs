using FluentValidation;
using RentItAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentItAPI.Models.Validators
{
    public class ReservationQueryValidator : AbstractValidator<ReservationQuery>
    {
        private int[] allowedPageSizes = new[] { 5, 10, 15 };
        private string[] allowedSortByColumnNames = new[] { nameof(Reservation.Item.Name), nameof(Reservation.FirstName), nameof(Reservation.LastName)};
        public ReservationQueryValidator()
        {
            RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(r => r.PageSize).Custom((value, context) =>
                {
                    if (!allowedPageSizes.Contains(value))
                    {
                        context.AddFailure("PageSize", $"PageSize must be within range of [{string.Join(",", allowedSortByColumnNames)}");
                    }
                });
            RuleFor(r => r.SortBy)
                .Must(value => string.IsNullOrEmpty(value) || allowedSortByColumnNames.Contains(value))
                .WithMessage($"Sorting is optional but if used, it must be by one of the following values:[{string.Join(",", allowedSortByColumnNames)}]");
        }
    }
}
