using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace NexoRecruiter.Web.Features.Shared.Layouts
{
    public interface ILayoutHeadHost
    {
        void SetPageHead(RenderFragment? fragment);
        void ClearPageHead();
    }
}