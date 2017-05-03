using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.ModelView;
using FishOn.PlatformInterfaces;
using FishOn.Repositories;
using FishOn.Services;
using Xamarin.Forms;

namespace FishOn.Pages_MVs.ProvisioningPages.Settings
{
    public class SettingsListViewModel : BaseModelView
    {
        private ProvisioningPageDefinitions _provisioningPagesDefinitions;

        public SettingsListViewModel(FishOnNavigationService navigation, IFishOnService fishOnService):base(navigation, fishOnService) { }

        public List<ProvisioningPageDefinition> PageDefinitions
        {
            get { return _provisioningPagesDefinitions.GetList(); }
        }

        public async Task DisplayProvisioningPage(string pageTitle)
        {
            var pageDefinition = _provisioningPagesDefinitions.GetDefinition(pageTitle);

            var page = await pageDefinition.CreatePage(_navigation, _fishOnService);

            await _navigation.PushAsync(page);

        }

    }
}
