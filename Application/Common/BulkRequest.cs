using System;
using System.Collections.Generic;

namespace Application.Common
{
    public record BulkRequest(List<Guid> Ids);
}
