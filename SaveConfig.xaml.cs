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
using System.Windows.Shapes;
using System.IO;
namespace PromptCombiner
{
    /// <summary>
    /// SaveConfig.xaml 的交互逻辑
    /// </summary>
    public partial class SaveConfig : Window
    {
        string[] prompts=new string[0];
        public SaveConfig()
        {
            InitializeComponent();
        }
        public SaveConfig(string[] prompt)
        {
            InitializeComponent();
            prompts = prompt;
        }

        private void saveConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if(!File.Exists("presets\\"+saveAsNameTextBox.Text+".preset"))File.Create("presets\\"+saveAsNameTextBox.Text+".preset").Close();
            File.WriteAllText("presets\\" + saveAsNameTextBox.Text + ".preset", prompts[0] + '^' + prompts[1]);
            Close();
        }

       
    }
}
