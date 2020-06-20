﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreAccessControl.DataAccess.Ef.Models
{
    public partial class Space
    {
        public Space()
        {
            DeviceSpace = new HashSet<DeviceSpace>();
            KeyholderSpace = new HashSet<KeyholderSpace>();
        }

        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public long LocationId { get; set; }
        public int? State { get; set; }
        public long? StatusId { get; set; }
        public long CreatedBy { get; set; }
        [Column(TypeName = "datetime2(0)")]
        public DateTime CreatedOn { get; set; }
        public long? LastUpdatedBy { get; set; }
        [Column(TypeName = "datetime2(0)")]
        public DateTime? LastUpdatedOn { get; set; }

        [ForeignKey(nameof(LocationId))]
        [InverseProperty("Space")]
        public virtual Location Location { get; set; }
        [ForeignKey(nameof(StatusId))]
        [InverseProperty(nameof(SpaceStatus.Space))]
        public virtual SpaceStatus Status { get; set; }
        [InverseProperty("Space")]
        public virtual ICollection<DeviceSpace> DeviceSpace { get; set; }
        [InverseProperty("Space")]
        public virtual ICollection<KeyholderSpace> KeyholderSpace { get; set; }
    }
}