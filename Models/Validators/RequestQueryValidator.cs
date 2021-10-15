using FluentValidation;
using RentItAPI.Entities;
using System;
using System.Linq;

namespace RentItAPI.Models.Validators
{
    public class ItemQueryValidator : AbstractValidator<ItemQuery>
    {
        private int[] allowedPageSizes = new[] { 5, 10, 15 };
        private string[] allowedSortByColumnNames = new[] { nameof(Item.Name), nameof(Item.Price), nameof(Item.Description) };

        public ItemQueryValidator()
        {
            RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(r => r.PageSize).Custom((value, context) =>
            {
                if (!allowedPageSizes.Contains(value))
                {
                    context.AddFailure("PageSize", $"PageSize must be equal to one of following values: [{string.Join(",", allowedPageSizes)}]");
                }
            });
            RuleFor(r => r.SortBy)
                .Must(value => string.IsNullOrEmpty(value) || allowedSortByColumnNames.Contains(value))
                .WithMessage($"Sorting is optional but if used, it must be by one of the following values:[{string.Join(",", allowedSortByColumnNames)}]");
        }
    }
}