using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.Outputs
{
    public enum ImportStatusEnum
    {
        Successfull = 1,
        Already,
        NotFound,
        Error
    }
}
