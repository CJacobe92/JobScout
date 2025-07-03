using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace JobScout.Domain.SeedWork
{
    public interface IDomainEvent : INotification
    {
        DateTime OccuredOn { get; set; }
    }
}
