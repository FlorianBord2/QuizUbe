using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using QuizUbeXamarin.Classes;

namespace QuizUbeXamarin
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

		async void OnButtonClicked(object sender, EventArgs args)
		{
			//127.0.0.1 = localhost, 10.0.2.2  Special alias to your host loopback interface
			string url = @"http://10.0.2.2:5000/search_channel?q=Misterpixel";
			string data = Get(url);

			IEnumerable<ChannelResult> channelInfos = JsonConvert.DeserializeObject<IEnumerable<ChannelResult>>(data);
		
			foreach (var ci in channelInfos)
			{
				if (!string.IsNullOrEmpty(ci.URL))
				{
					using (var client = new WebClient())
					{
						try
						{
							string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
							string filename = System.IO.Path.Combine(path, ci.Title + ".jpg");
							client.DownloadFile(new Uri(ci.URL), filename);
							Image im = new Image { Source = filename };
							Label lab = new Label();
							lab.Text = ci.Title;
							stack.Children.Add(im);
							stack.Children.Add(lab);
							stack.Orientation = StackOrientation.Vertical;
							stack.HorizontalOptions = LayoutOptions.FillAndExpand;
						}
						catch (Exception ex)
						{
							while (ex != null)
							{
								Console.WriteLine(ex.Message);
								ex = ex.InnerException;
							}
						}
					}
				}
			}
		}

		public string Get(string uri)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
			request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

			using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
			using (Stream stream = response.GetResponseStream())
			using (StreamReader reader = new StreamReader(stream))
			{
				return reader.ReadToEnd();
			}
		}
	}
}
