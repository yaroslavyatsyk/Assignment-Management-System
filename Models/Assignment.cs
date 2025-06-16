using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDo_Web_App.Models
{
    public class Assignment
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter a name for the assignment.")]
        public string? Name { get; set; }

 

        [Required(ErrorMessage = "Please enter a due date for the assignment.")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Please choose the type of assignment")]
        public string? Type { get; set; }

        [ForeignKey("ApplicationUser")]
        public string? UserId { get; set; }

        public ApplicationUser?  User { get; set; }
        public string? Description { get; set; }
    }
}
