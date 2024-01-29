﻿using Product_Management.Models.DomainModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product_Management.Models.DTO
{
    public class AddProductRequestDTO
    {
        [Required]
        public string ProductName { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string ProductDesc { get; set; }

        [Required]
        public int ProductPrice { get; set; }

        public DateTime ProductCreatedAt { get; set; } = DateTime.Now;

        public int CategoryId { get; set; }

        // navigation property
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

    }
}