using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cambios
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadRates();
        }

        private async void LoadRates()
        {
            // Processo assíncrono para o carregamento das taxas
            bool load;

            progressBar1.Value = 0;

            labelStatus.Text = "A carregar taxas...";

            var client = new HttpClient();
            client.BaseAddress = new Uri("http://rates.somee.com/api/rates");


            // Obter as taxas da API
            var response = await client.GetAsync("/api/rates");

            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Error loading rates: " + response.StatusCode + response.ReasonPhrase);
                return;
            }
            
            var rates = JsonConvert.DeserializeObject<List<Rates>>(result);

            // Adicionar as taxas ao ComboBox

            comboBoxOrigens.DataSource = rates;

            comboBoxOrigens.DisplayMember = "Name";


            // Corrigir o bug do ComboBox
            comboBoxDestino.BindingContext = new BindingContext();

            // Adicionar as taxas ao ComboBox 2
            comboBoxDestino.DataSource = rates;

            comboBoxDestino.DisplayMember = "Name";

            progressBar1.Value = 100;
            labelStatus.Text = "Taxas carregadas com sucesso!";
        }


    }
}
