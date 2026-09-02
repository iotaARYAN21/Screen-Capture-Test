using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Diagnostics;
namespace ScreenCaptureGUI
{
    public partial class Form1 : Form
    {
        private bool isCapturing = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void TakeScreenshot(
    Rectangle screen,
    string folder,
    int number)
        {
            using (Bitmap screenshot =
                new Bitmap(screen.Width, screen.Height))
            {
                using (Graphics graphics =
                    Graphics.FromImage(screenshot))
                {
                    graphics.CopyFromScreen(
                        screen.Left,
                        screen.Top,
                        0,
                        0,
                        screen.Size
                    );
                }

                string fileName =
                    $"screenshot_{number:D3}.png";

                string path =
                    Path.Combine(folder, fileName);

                screenshot.Save(path, ImageFormat.Png);
            }
        }
        private async void button2_Click(object sender, EventArgs e)
        {
            // Check interval
            if (!int.TryParse(txtInterval.Text, out int intervalSeconds))
            {
                MessageBox.Show("Please enter a valid interval.");
                return;
            }

            if (intervalSeconds <= 0)
            {
                MessageBox.Show("Interval must be greater than 0.");
                return;
            }
            isCapturing = true;
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            // Check folder
            string folder = txtFolder.Text;

            if (string.IsNullOrWhiteSpace(folder))
            {
                MessageBox.Show("Please select a save folder.");
                return;
            }

            Directory.CreateDirectory(folder);

            // Get screen information
            Rectangle screen = Screen.PrimaryScreen.Bounds;

            btnStart.Enabled = false;

            // COUNT MODE
            if (rbCount.Checked)
            {
                if (!int.TryParse(txtNumber.Text, out int numberOfScreenshots))
                {
                    MessageBox.Show("Please enter a valid number of screenshots.");
                    btnStart.Enabled = true;
                    return;
                }

                if (numberOfScreenshots <= 0)
                {
                    MessageBox.Show("Number of screenshots must be greater than 0.");
                    btnStart.Enabled = true;
                    return;
                }

                for (int i = 1; i <= numberOfScreenshots; i++)
                {
                    if (!isCapturing) break;

                    TakeScreenshot(screen, folder, i);

                    lblStatus.Text =
                        $"Status: Screenshot {i} of {numberOfScreenshots}";

                    await Task.Delay(intervalSeconds * 1000);
                }
            }

            // DURATION MODE
            else
            {
                if (!int.TryParse(txtDuration.Text, out int durationSeconds))
                {
                    MessageBox.Show("Please enter a valid duration.");
                    btnStart.Enabled = true;
                    return;
                }

                if (durationSeconds <= 0)
                {
                    MessageBox.Show("Duration must be greater than 0.");
                    btnStart.Enabled = true;
                    return;
                }

                Stopwatch stopwatch = Stopwatch.StartNew();

                int screenshotNumber = 1;

                while (isCapturing && stopwatch.Elapsed.TotalSeconds < durationSeconds)
                {
                    TakeScreenshot(screen, folder, screenshotNumber);

                    lblStatus.Text =
                        $"Status: {stopwatch.Elapsed.TotalSeconds:F0} / {durationSeconds} seconds";

                    screenshotNumber++;

                    await Task.Delay(intervalSeconds * 1000);
                }
            }

            lblStatus.Text = "Status: Finished!";

            //btnStart.Enabled = true;
            isCapturing = false;
            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtFolder.Text = dialog.SelectedPath;
                }
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            isCapturing = false;

            lblStatus.Text = "Status: Stopping...";
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4 && isCapturing)
            {
                isCapturing = false;

                lblStatus.Text = "Status: Stopping...";
            }
        }
    }
}
