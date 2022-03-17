﻿using System.ComponentModel.DataAnnotations;
using DesafioStone.Enums;
using DesafioStone.CustomAttributes;

namespace DesafioStone.DataContracts
{
    public class InvoiceCreateRequest
    {
        [RequiredEnumField]
        public Month ReferenceMonth { get; set; }

        [Required]
        [Range(1900, int.MaxValue)]
        public int ReferenceYear { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{11}$|^[0-9]{14}$")]
        public string Document { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int? Amount { get; set; }
    }
}
