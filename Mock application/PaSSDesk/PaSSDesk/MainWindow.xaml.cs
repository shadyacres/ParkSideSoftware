using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PaSSDesk
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HelpDeskGroup.DataContext = this;
            List<HelpDeskItem> HelpDeskItems = new List<HelpDeskItem>();
            HelpDeskItems.Add(new HelpDeskItem
            {
                HelpDeskNumber = "10124",
                Name = "PC won't turn on",
                Reported = new DateTime(2014, 5,2),
                Location = "Park",
                Urgency = 5,
                Flag = "On"
            });
            HelpDeskItems.Add(new HelpDeskItem
            {
                HelpDeskNumber = "10125",
                Name = "Smartboard pen not working",
                Reported = new DateTime(2014, 5,2),
                Location = "FCH",
                Urgency = 3,
                Flag = "Off"
            });
            HelpDeskItems.Add(new HelpDeskItem
            {
                HelpDeskNumber = "10126",
                Name = "Projector dull",
                Reported = new DateTime(2014, 5,2),
                Location = "Halls",
                Urgency = 2,
                Flag = "Off"
            });
            HelpDeskItems.Add(new HelpDeskItem
            {
                HelpDeskNumber = "10127",
                Name = "Printer out of toner",
                Reported = new DateTime(2014, 5, 3),
                Location = "Park",
                Urgency = 5,
                Flag = "Off"
            });
            HelpDeskItems.Add(new HelpDeskItem
            {
                HelpDeskNumber = "10131",
                Name = "No sound",
                Reported = new DateTime(2014, 5, 3),
                Location = "Oxstalls",
                Urgency = 1,
                Flag = "On"
            });
            HelpDeskItems.Add(new HelpDeskItem
            {
                HelpDeskNumber = "10133",
                Name = "Virus",
                Reported = new DateTime(2014, 5, 4),
                Location = "Park",
                Urgency = 2,
                Flag = "On"
            });

            HelpDeskGroup.ItemsSource = HelpDeskItems;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddHelpdesk hd = new AddHelpdesk();
            hd.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ViewHelpDesk vd = new ViewHelpDesk();
            vd.Show();
        }
    }
}
