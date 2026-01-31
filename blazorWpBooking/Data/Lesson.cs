using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace blazorWpBooking.Data
{
    public class Lesson
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        // Foreign key to a LessonType (another entity)
        public int? TypeId { get; set; }
        public LessonType? Type { get; set; }

        // Foreign key to a Schedule (another entity)
        public int? ScheduleId { get; set; }
        public Schedule? Schedule { get; set; }

        public bool IsPublished { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }

}
