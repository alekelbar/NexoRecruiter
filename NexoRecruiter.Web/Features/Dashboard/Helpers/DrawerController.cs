using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexoRecruiter.Web.Features.Dashboard.Helpers
{
public class DrawerController
{
    public bool IsOpen { get; private set; } = true;

    public event Action? OnChange;

    public void Open()
    {
        IsOpen = true;
        OnChange?.Invoke();
    }

    public void Close()
    {
        IsOpen = false;
        OnChange?.Invoke();
    }

    public void Toggle()
    {
        IsOpen = !IsOpen;
        OnChange?.Invoke();
    }
}
}