using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using NexoRecruiter.Web.Features.Shared.Layouts;

namespace NexoRecruiter.Web.Features.Shared.Components.LayoutHead
{
    public partial class LayoutHead : ComponentBase, IDisposable
    {
        [CascadingParameter]
        public ILayoutHeadHost? Layout { get; set; }

        [Parameter]
        public RenderFragment? ChildContent { get; set; }
        protected override void OnParametersSet()
        {
            Layout?.SetPageHead(ChildContent);
        }

        public void Dispose()
        {
            Layout?.ClearPageHead();
        }
    }
}