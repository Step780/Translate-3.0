using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;
using System.Net;

namespace Translate3._0
{
    public partial class MainWindow : Window
    {
        public static List<History> histories;
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(List<History>));
        private const string SaveFileName = "history.xml";

        private readonly StringDictionary _languages = new StringDictionary
        {
            {"en", "Английский" },
            {"ru", "Русский" },
            {"fr", "Французский" },
            {"deu", "Немецкий" },
            {"spa", "Испанский" },
            {"ita", "Итальянский" },
            {"zho", "Китайский" }
        };

        public MainWindow()
        {
            InitializeComponent();
            languageBox.ItemsSource = _languages;

            Closing += (sender, args) =>
                Serializer.Serialize(File.OpenWrite(SaveFileName), histories);

            if (File.Exists(SaveFileName))
            {
                histories = (List<History>)Serializer.Deserialize(File.OpenRead(SaveFileName));
                storyBox.ItemsSource = histories;
                storyBox.SelectedValuePath = "To";
                storyBox.DisplayMemberPath = "From";
            }
            else
            {
                histories = new List<History>();
            }
        }

        public bool check1 = false;

        public bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    check1 = true;
                    return true;
                    
                }
            }
            catch
            {
                check1 = false;
                return false;
            }
        }


        private async void Translate_Click(object sender, RoutedEventArgs e)
        {
            CheckForInternetConnection();

            if (check1 == false)
            {
                MessageBox.Show("Отсутствует подключение к интернету");
            }

            else {

                if (textFrom.Text == "")
                {
                    MessageBox.Show("Вы не ввели текст");
                }

                else
                {

                    try
                    {
                        progressBar.IsIndeterminate = true;
                        var client = new HttpClient();
                        var request = new HttpRequestMessage
                        {
                            Method = HttpMethod.Get,
                            RequestUri = new Uri("https://localhost:44329/api/Accounts")
                            
                        };


                        using (var response = await client.SendAsync(request))
                        {
                            response.EnsureSuccessStatusCode();
                            var body = await response.Content.ReadAsStringAsync();
                            dynamic obj = JsonConvert.DeserializeObject(body);
                            
                        }

                        History history = new History(textFrom.Text, textTo.Text);
                        histories.Add(history);
                        progressBar.IsIndeterminate = false;
                    }

                    catch

                    {
                        progressBar.IsIndeterminate = false;
                        MessageBox.Show("У вас закончились попытки");

                    }
                }
            }
        }

        private void languageBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show("Вы выбрали " + languageBox.SelectedValue);
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {

                    
                       
                var url = "https://drive.google.com/drive/u/0/my-drive";
                Process.Start(url);
                
                return;
                //var browser = new CefSharp.Wpf.ChromiumWebBrowser(url)
                //{

                //    Height = 540,
                //    Width = 960

                //};


                //var browserWindow = new Window();
                //browserWindow.Content = browser;
                //browserWindow.Show();
                
            
        }

       

        private void storyBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            textFrom.Text = ((History)storyBox.SelectedItem).From;

            textTo.Text = storyBox.SelectedValue.ToString();
        }

        private void progressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
