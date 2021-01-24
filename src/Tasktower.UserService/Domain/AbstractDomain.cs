using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasktower.UserService.Domain
{
    public abstract class AbstractDomain
    {
        public virtual Guid Id { get; set; }
        public virtual DateTime CreatedAt {get; set;}
        public virtual DateTime UpdatedAt { get; set; }

        public abstract class AbstractDomainMap<TDomain> : ClassMap<TDomain> where TDomain : AbstractDomain {
            public AbstractDomainMap() {
                Id(x => x.Id).Column("id"); // constraint accounts_pkey
                Map(x => x.CreatedAt).Column("created_at");
                Map(x => x.UpdatedAt).Column("updated_at");
            }
        }

    }
}
