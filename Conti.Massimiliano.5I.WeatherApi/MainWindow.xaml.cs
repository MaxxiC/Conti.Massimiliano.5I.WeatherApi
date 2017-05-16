using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace Conti.Massimiliano._5I.WeatherApi
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public List<RootObject> dt = new List<RootObject>();
        public string NomeCitta = "Rimini";

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Download();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txtNomeCitta.Text != "")
            {
                NomeCitta = txtNomeCitta.Text;
                Download();
            }
        }




        private async void Download()
        {
            //Recupero dei dati
            RootObject Meteo = null;

            try
            {
                HttpClient client = new HttpClient();
                string result = await client.GetStringAsync(
                    new Uri(@"http://api.wunderground.com/api/5843908a06eecf2b/conditions/q/IT/" + NomeCitta + ".json"));

                Meteo = JsonConvert.DeserializeObject<RootObject>(result);



                if (dt.Count == 0)
                {
                    dt.Add(Meteo);
                    dt.Add(Meteo);
                    dt.Add(Meteo);
                }

                dt.Add(Meteo);

                if (dt.Count > 2)
                {
                    dt.RemoveAt(0);
                }

                btnUno.Content = dt[0].current_observation.display_location.city;
                btnDue.Content = dt[1].current_observation.display_location.city;
                btnTre.Content = dt[2].current_observation.display_location.city;

                Display(Meteo);
            }
            catch
            { MessageBox.Show("Errore nel recuperare i dati.\n Probabile causa è l'assenza di connessione a internet."); return; }
        }

        private void Display(RootObject Meteo = null)
        {
            //Archiviazione dei dati e stampa di essi
            try
            {
                //Aggiunta alle liste
                DatiCitta aa = new DatiCitta
                {
                    //Descrizione = Meteo.current_observation.observation_location.state,
                    Citta = Meteo.current_observation.display_location.city,
                    Altitudine = Meteo.current_observation.observation_location.elevation,
                    Latitudine = Meteo.current_observation.observation_location.latitude,
                    Longitudine = Meteo.current_observation.observation_location.longitude,
                    Temperatura = Meteo.current_observation.temp_c + "°C",
                    Umidita = Meteo.current_observation.relative_humidity,
                    Vento_kph = Meteo.current_observation.wind_kph.ToString()
                };

                //stringa "Sole" "Pioggia" "Nuvoloso" ecc.
                lblDescrizione.Content = Meteo.current_observation.weather;
                lblNomeCitta.Content = Meteo.current_observation.display_location.city;

                //icona relaztiva
                BitmapImage icona = new BitmapImage(new Uri(Meteo.current_observation.icon_url));
                ImgPrint.Source = icona;

                //Aggiunta del nome della città nel titolo e dell'icona
                this.Icon = icona;
                this.Title = "Conti Massimiliano 5I WeatherApi - " + Meteo.current_observation.display_location.city;

                //collegamento alle datagrid
                dgCitta.ItemsSource = aa.MLista();

            }
            catch
            { MessageBox.Show("Errore nel trovare la citta inserita."); return; }
        }

        private void btnUno_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Display(dt[0]);
            }
            catch { }
        }

        private void btnDue_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Display(dt[1]);
            }
            catch { }
        }

        private void btnTre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Display(dt[2]);
            }
            catch { }
        }
    }

    public class DatiCitta
    {
        //public string Descrizione { get; set; }
        public string Citta { get; set; }
        public string Altitudine { get; set; }
        public string Latitudine { get; set; }
        public string Longitudine { get; set; }
        public string Temperatura { get; set; }
        public string Umidita { get; set; }
        public string Vento_kph { get; set; }

        public DatiCitta() { }

        public List<DatiCitta> MLista()
        {
            List<DatiCitta> aa = new List<DatiCitta>();
            aa.Add(this);
            return (aa);
        }
    }

}
