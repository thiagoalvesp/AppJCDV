using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppJCDV.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedPageNavigation : TabbedPage
    {
        public TabbedPageNavigation ()
        {
            var sendPositionPage = new SendPositionPage();
            sendPositionPage.Title = "Send Position";

            var getPositionPage = new GetPositionPage();
            getPositionPage.Title = "Get Position";

            Children.Add(sendPositionPage);
            Children.Add(getPositionPage);

            InitializeComponent();
        }
    }
}