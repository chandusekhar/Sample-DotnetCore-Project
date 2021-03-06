﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreAccessControl.DataAccess.Ef.Models
{
    public partial class ChangeEmailRequest
    {
        [Key]
        public long Id { get; set; }
        public long UserId { get; set; }
        [Required]
        [StringLength(256)]
        public string Email { get; set; }
        [Column(TypeName = "datetime2(0)")]
        public DateTime RequestedOn { get; set; }
        [StringLength(45)]
        public string VerificationToken { get; set; }
        [Column(TypeName = "datetime2(0)")]
        public DateTime? VerificationTokenExpiry { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("ChangeEmailRequest")]
        public virtual User User { get; set; }
    }
}