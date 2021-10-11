using System.Collections.Generic;
using System.Threading.Tasks;
using ShopOnlineApp.Application.ViewModels.Footer;
using ShopOnlineApp.Application.ViewModels.Slide;
using ShopOnlineApp.Application.ViewModels.SystemConfig;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface ICommonService
    {
        Task<FooterViewModel> GetFooter();
        Task<List<SlideViewModel>> GetSlides(string groupAlias);
        Task<SystemConfigViewModel> GetSystemConfig(string code);
    }
}
