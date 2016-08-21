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

namespace payworks_pirate_translator
{
    //TODO: queue for input
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string PreviousTextInInputBox;

        public MainWindow()
        {
            PreviousTextInInputBox = "";
            InitializeComponent();
        }

        private void InputBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox) sender;
            PreviousTextInInputBox = box.GetLineText(0);
            box.Text = string.Empty;
            box.GotFocus -= InputBox_OnGotFocus;
        }

        private void TranslateButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.outputBox.Text = TranslatorLogic.RunTranslation(this.inputBox.Text);
        }
    }
}
