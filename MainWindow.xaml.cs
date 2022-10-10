using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
namespace PromptCombiner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, Dictionary<string,(string,string)>> promptTypeDict = new Dictionary<string, Dictionary<string, (string, string)>>();
        Dictionary<string, (string, string)> currentSelectedTypeDictionary;
        //prompt类型,prompt的中文译名,原名及介绍
        Dictionary<string, string[]> presetDict = new Dictionary<string, string[]>();
        public MainWindow()
        {
            InitializeComponent();
            initializePromptTypeDict();
            initializePromptTypeComboBox();
            initializePresets();
        }
        private void initializePromptTypeDict()
        {
            promptTypeDict.Clear();
            if (!Directory.Exists("prompts")) Directory.CreateDirectory("prompts");
            DirectoryInfo Foder = new DirectoryInfo("prompts");
            foreach (FileInfo nextFile in Foder.GetFiles())
            {
                if (nextFile.Extension.Equals(".plist"))
                {
                    foreach (string lines in File.ReadLines(nextFile.FullName))
                    {
                        string[] field = lines.Split('^');
                        if (!promptTypeDict.ContainsKey(field[0])) promptTypeDict.Add(field[0], new Dictionary<string, (string, string)>());//创建不存在的类型     
                        if (!promptTypeDict[field[0]].ContainsKey(field[1])) promptTypeDict[field[0]].Add(field[1], (field[2], field[3]));
                    }
                }
            }
        }
        private void initializePresets()
        {
            presetsComboBox.Items.Clear();
            presetDict.Clear();
            if (!Directory.Exists("presets")) Directory.CreateDirectory("presets");
            DirectoryInfo Foder = new DirectoryInfo("presets");
            foreach (FileInfo nextFile in Foder.GetFiles())
            {
                if (nextFile.Extension.Equals(".preset"))
                {
                    presetsComboBox.Items.Add(nextFile.Name);
                    string allText = File.ReadAllText(nextFile.FullName);
                    presetDict.Add(nextFile.Name, new string[] { allText.Split('^')[0], allText.Split('^')[1] });
                }
            }
        }

        private void initializePromptTypeComboBox()
        {
            foreach (string field in promptTypeDict.Keys)if(!field.Equals("")) promptTypeComboBox.Items.Add(field);
        }
        
        private void promptTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            promptComboBox.Items.Clear();
            currentSelectedTypeDictionary = promptTypeDict[(string)promptTypeComboBox.SelectedValue];
            foreach (KeyValuePair<string,(string,string)> field in currentSelectedTypeDictionary) promptComboBox.Items.Add(field.Key);
        }

        private void promptComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if((string)promptComboBox.SelectedValue != null) introductionLabel.Content = currentSelectedTypeDictionary[(string)promptComboBox.SelectedValue].Item2;
        }

        private void addToPromptButton_Click(object sender, RoutedEventArgs e)
        {
            if (promptComboBox.SelectedIndex != -1&& !currentSelectedTypeDictionary[(string)promptComboBox.SelectedValue].Item1.Equals("")) promptTextBox.AppendText(currentSelectedTypeDictionary[(string)promptComboBox.SelectedValue].Item1 + ',');
        }

        private void addToNegativeButton_Click(object sender, RoutedEventArgs e)
        {
            if (promptComboBox.SelectedIndex != -1 && !currentSelectedTypeDictionary[(string)promptComboBox.SelectedValue].Item1.Equals("")) negativePromptTextBox.AppendText(currentSelectedTypeDictionary[(string)promptComboBox.SelectedValue].Item1 + ',');
        }

        private void saveConfigButton_Click(object sender, RoutedEventArgs e)
        {
            new SaveConfig(new string[] { promptTextBox.Text, negativePromptTextBox.Text}).ShowDialog();
            initializePresets();
        }

        private void loadConfigButton_Click(object sender, RoutedEventArgs e)
        {
            if (presetsComboBox.SelectedIndex != -1)
            {
                promptTextBox.Text = presetDict[(string)presetsComboBox.SelectedItem][0];
                negativePromptTextBox.Text = presetDict[(string)presetsComboBox.SelectedItem][1];
            }
        }

        private void copyToCopyboard(object sender, RoutedEventArgs e)
        {
            if (promptComboBox.SelectedIndex != -1) negativePromptTextBox.AppendText(currentSelectedTypeDictionary[(string)promptComboBox.SelectedValue].Item1+',');
        }

    }
}
