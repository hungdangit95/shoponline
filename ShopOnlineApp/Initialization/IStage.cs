using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnlineApp.Initialization
{
    public interface IStage
    {
        int Order { get; }
        Task ExecuteAsync();
    }
}
