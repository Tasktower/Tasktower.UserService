using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Tasktower.UserService.Domain
{
    public abstract class AbstractDomain
    {
        [Column("id")]
        public virtual Guid Id { get; set; }
        [Column("created_at")]
        public virtual DateTime CreatedAt {get; set;}
        [Column("updated_at")]
        public virtual DateTime UpdatedAt { get; set; }

    }
}
