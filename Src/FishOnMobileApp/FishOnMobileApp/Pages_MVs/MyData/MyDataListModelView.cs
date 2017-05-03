using System.Collections.Generic;
using System.Threading.Tasks;
using FishOn.Services;
using FishOn.Utils;
using Xamarin.Forms;

namespace FishOn.ModelView
{
    public class MyDataListModelView : BaseModelView
    {
        private List<Page> _pages = new List<Page>();

        public MyDataListModelView(FishOnNavigationService navigation, IFishOnService fishOnService) : base(navigation, fishOnService) { }

        public async Task InitializeNewPageContextAsync(string pageTitle)
        {
            //Don't iniitalize every content page on the tab
            //wait until the user wants to view that content page.
            var p = _pages.FindPage(pageTitle);

            if (p != null && p.BindingContext != null)
            {
                var pageVM = (BaseModelView) p.BindingContext;
                await pageVM.InitializeAsync();
            }
        }

        //Unable to bind the TabbedPage.Children to "{Binding ChildPages}"
        public List<Page> GetContentPages()
        {
            _pages = new List<Page>();

           
            return _pages;
        }
    }
}
