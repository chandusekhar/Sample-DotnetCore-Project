using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreAccessControl.Domain.RequestModels
{
    public class UpdateSecurityQuestionReqModel
    {
        [Required(ErrorMessage = "QuestionQuestion is required.")]
        [MaxLength(200, ErrorMessage = "SecurityQuestion max length sould be 250 character.")]
        public string Question { get; set; }
        [Required(ErrorMessage = "Answer is required.")]
        [MaxLength(200, ErrorMessage = "Answer max length sould be 250 character.")]
        public string Answer { get; set; }
    }
}
