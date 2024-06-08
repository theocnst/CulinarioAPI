using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CulinarioAPI.Models.RecipeModels
{
    public class Instruction
    {
        [Key]
        public int InstructionId { get; set; }

        [ForeignKey("Recipe")]
        public int RecipeId { get; set; }
        public int StepNumber { get; set; }
        public string Description { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
