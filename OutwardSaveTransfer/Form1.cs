using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OutwardSaveTransfer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void check_save_location_button_Click(object sender, EventArgs e)
        {
            if(textBox1.Text != "")
            {
                if(Directory.Exists(textBox1.Text))//SaveGames
                {
                    string saveGamesDirectory = textBox1.Text + "\\SaveGames".Replace(@"\\", @"\");

                    if (Directory.Exists(saveGamesDirectory))
                    {
                        var directories = Directory.GetDirectories(saveGamesDirectory);

                        if (directories.Length > 0)
                        {
                            int totalSaves = getTotalSaves(saveGamesDirectory, directories);
                            openTransfer(totalSaves);
                            MessageBox.Show("We managed to locate your save files! Total saved characters found " + totalSaves + "!", "Success");
                        }
                        else
                        {
                            MessageBox.Show("Missing saves in '" + saveGamesDirectory + "'", "Failed!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Missing 'SaveGames' directory in " + textBox1.Text, "Failed!");
                    }
                }
                else
                {
                    MessageBox.Show("Typed in directory doesn't exist!", "Failed!");
                }
            }
            else
            {
                MessageBox.Show("You forgot to type in location!", "Failed!");
            }
        }

        private int getTotalSaves(string saveGameDirectory, string[] steamIds)
        {
            int totalSteamIds = steamIds.Length;
            int totalSaves = 0;
            int directoriesLength = 0;

            for (int currentSteamId = 0; currentSteamId < totalSteamIds; currentSteamId++)
            {
                var directories = Directory.GetDirectories(steamIds[currentSteamId]);
                directoriesLength = directories.Length;

                for (int currentSaveDirectory = 0; currentSaveDirectory < directoriesLength; currentSaveDirectory++)
                {
                    if(directories[currentSaveDirectory].Contains("Save_"))
                    {
                        totalSaves++;
                    }
                }
            }

            return totalSaves;
        }

        private void openTransfer(int totalSaves)
        {
            this.Hide();
            Form2 options = new Form2(textBox1.Text, totalSaves);
            options.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog() { Description="Select your path." })
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                    textBox1.Text = fbd.SelectedPath;
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
