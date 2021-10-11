using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using ShopOnlineApp.Application.ViewModels.Annoucement;

namespace ShopOnlineApp.SignalR
{
    public class OnlineShopHub : Hub
    {
        //private readonly static ConnectionMapping<string> _connections =
        //    new ConnectionMapping<string>();

        public async Task SendMessage(AnnouncementViewModel message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        //public override Task OnConnected()
        //{
        //    string name = Context.User.Identity.Name;

        //    _connections.Add(name, Context.ConnectionId);

        //    return base.OnConnected();
        //}

        //public override Task OnDisconnected(bool stopCalled)
        //{
        //    string name = Context.User.Identity.Name;

        //    _connections.Remove(name, Context.ConnectionId);

        //    return base.OnDisconnected(stopCalled);
        //}

        //public override Task OnReconnected()
        //{
        //    string name = Context.User.Identity.Name;

        //    if (!_connections.GetConnections(name).Contains(Context.ConnectionId))
        //    {
        //        _connections.Add(name, Context.ConnectionId);
        //    }

        //    return base.OnReconnected();
        //}
    }
}
