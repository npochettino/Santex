using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.Outputs
{
    public enum ImportStatusEnum
    {
        Successfully = 1,
        Already,
        NotFound,
        Error
    }
}
