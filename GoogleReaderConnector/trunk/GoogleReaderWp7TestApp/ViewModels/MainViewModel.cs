using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using CodeClimber.GoogleReaderConnector;
using CodeClimber.GoogleReaderConnector.Exceptions;
using CodeClimber.GoogleReaderConnector.Services;


namespace GoogleReaderWp7TestApp
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }

        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            IUriBuilder urlBuilder = new GoogleReaderUrlBuilder("test");
            IHttpService httpService = new HttpService();
            IClientLoginService loginService = new GoogleReaderClientLogin("", "",
                                                                           httpService, urlBuilder);
            httpService.ClientLogin = loginService;

            ReaderServiceAsync rdr = new ReaderServiceAsync(urlBuilder, httpService);
            //loginService.LoginAsync(
            //    loggedin => this.Items.Add(new ItemViewModel { LineOne = "w00t!!!", LineThree = loggedin.ToString(), LineTwo = "so cool!!" }),
            //        ex =>
            //            {
            //                this.Items.Add(new ItemViewModel
            //                                   {LineOne = "noooo", LineTwo = ex.Message, LineThree = "so not cool!!"});
            //            },
            //        () => this.IsDataLoaded = true
            //    );

            rdr.GetFeedAsync("http://feeds.feedburner.com/codeclimber",
                             new ReaderParameters {Direction = ItemDirection.Descending, MaxItems = 20},
                             items =>
                                 {
                                     foreach (var item in items)
                                     {
                                         this.Items.Add(new ItemViewModel()
                                                            {
                                                                LineOne = item.Title,
                                                                LineTwo = "by " + item.Author,
                                                                LineThree = item.Summary.Content
                                                            });
                                     }
                                 },
                             ex =>
                                 {
                                    this.Items.Add(new ItemViewModel{LineOne = "Error", LineTwo = ex.Message, LineThree = "so not cool!!"});
                                 },
                             () => this.IsDataLoaded = true);

        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}