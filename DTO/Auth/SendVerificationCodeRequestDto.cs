using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Auth;
public record SendVerificationCodeRequestDto
{
    [EmailAddress]
    public string Email { get; set; } = default!;
}