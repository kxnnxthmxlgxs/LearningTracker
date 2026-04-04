using System.ComponentModel.DataAnnotations;

namespace LearningTrackerApp.Models
{
    public class LearningItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You forgot to say what you learned!")]
        [StringLength(100, ErrorMessage = "Keep it brief (under 100 characters).")]
        public string Topic { get; set; } = string.Empty;

        public DateTime DateLearned { get; set; } = DateTime.Now;

        public bool IsCompleted { get; set; }

        public string Category { get; set; } = "General";
    }
}
