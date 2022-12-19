using System;
using System.Reactive.Linq;
using System.Windows.Threading;
using GlazeWM.Domain.Common.Enums;
using GlazeWM.Domain.Containers;
using GlazeWM.Domain.Containers.Events;
using GlazeWM.Domain.UserConfigs;
using GlazeWM.Infrastructure;
using GlazeWM.Infrastructure.Bussing;

namespace GlazeWM.Bar.Components
{
  public class TilingDirectionComponentViewModel : ComponentViewModel
  {
    private Dispatcher _dispatcher => _parentViewModel.Dispatcher;
    private readonly Bus _bus = ServiceLocator.GetRequiredService<Bus>();
    private readonly ContainerService _containerService =
     ServiceLocator.GetRequiredService<ContainerService>();

    /// <summary>
    /// The layout of the currently focused container. Can be null on app startup when
    /// workspaces haven't been created yet.
    /// </summary>
    private Layout? _tilingDirection =>
      (_containerService.FocusedContainer as SplitContainer)?.Layout ??
      (_containerService.FocusedContainer.Parent as SplitContainer)?.Layout;

    public string TilingDirectionString =>
      _tilingDirection == Layout.VERTICAL ? "vertical" : "horizontal";

    public TilingDirectionComponentViewModel(
      BarViewModel parentViewModel,
      TilingDirectionComponentConfig config) : base(parentViewModel, config)
    {
      _bus.Events.OfType<LayoutChangedEvent>().Subscribe(_ =>
        _dispatcher.Invoke(() => OnPropertyChanged(nameof(TilingDirectionString)))
      );
    }
  }
}