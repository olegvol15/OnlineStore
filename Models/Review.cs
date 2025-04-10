using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class Review : IValidatableObject
    {
        public int Id { get; set; }

        [Range(1, 5, ErrorMessage = "Оцінка має бути від 1 до 5")]
        public int? Rating { get; set; }

        [MinLength(5, ErrorMessage = "Коментар має містити щонайменше 5 символів")]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Comment) && Rating == null)
            {
                yield return new ValidationResult("Потрібно вказати або оцінку, або коментар (або обидва)");
            }

            if (!string.IsNullOrWhiteSpace(Comment) && Comment.Length < 5)
            {
                yield return new ValidationResult("Коментар має містити щонайменше 5 символів");
            }
        }
    }
}
