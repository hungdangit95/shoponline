using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Footer;
using ShopOnlineApp.Application.ViewModels.Slide;
using ShopOnlineApp.Application.ViewModels.SystemConfig;
using ShopOnlineApp.Data.IRepositories;
using ShopOnlineApp.Infrastructure.Interfaces;
using ShopOnlineApp.Utilities.Constants;

namespace ShopOnlineApp.Application.Implementation
{
    public class CommonService : ICommonService
    {

        #region private method
        private readonly IFooterRepository _footerRepository;
        private readonly ISystemConfigRepository _systemConfigRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISlideRepository _slideRepository;

        #endregion

        #region constructor
        public CommonService(IFooterRepository footerRepository,
            ISystemConfigRepository systemConfigRepository,
            IUnitOfWork unitOfWork,
            ISlideRepository slideRepository)
        {
            _footerRepository = footerRepository;
            _unitOfWork = unitOfWork;
            _systemConfigRepository = systemConfigRepository;
            _slideRepository = slideRepository;
        }

        #endregion

        #region public Method
        public async Task<FooterViewModel> GetFooter()
        {
            return new FooterViewModel().Map(await _footerRepository.FindSingle(x => x.Id == CommonConstants.DefaultFooterId));
        }
        public async Task<List<SlideViewModel>> GetSlides(string groupAlias)
        {
            return new SlideViewModel().Map(await _slideRepository.FindAll(x => x.Status && x.GroupAlias == groupAlias)).ToList();
        }

        public async Task<SystemConfigViewModel> GetSystemConfig(string code)
        {
            return new SystemConfigViewModel().Map(await _systemConfigRepository.FindSingle(x => x.Id == code));
        }

        #endregion

    }
}
