using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.Enums
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "PaymentRecieved")]
        PaymentRecieved,
        [EnumMember(Value = "PaymentFailed")]
        PaymentFailed
    }
}
