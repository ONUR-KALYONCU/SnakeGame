using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        private Label yilanKafasi;
        private int _yilanprcArasiMesafe = 2;
        private int _yilanParcasiSayisi;
        private int yilanBoyutu = 20;
        private int yemBoyutu = 20;
        private Random random;
        private Label _yem;
        private HareketYonu _yon;

        public Form1()
        {
            InitializeComponent();
            random = new Random();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            _yilanParcasiSayisi = 0;
            YemOlustur();
            YeminYeriniDeğiştir();
            YilaniYerleştir();
            
            
            _yon = HareketYonu.Saga;
            timerYilanHareket.Enabled = true;


        }

        private void yenidenbaslat()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.pnl.Controls.Clear();
            _yilanParcasiSayisi = 0;
            YemOlustur();
            YeminYeriniDeğiştir();
            YilaniYerleştir();


            _yon = HareketYonu.Saga;
            lblpuan.Text = "0";
            lblsure.Text = "0";
            timerYilanHareket.Enabled = true;
            timersaat.Enabled = true;
        }



        private void Sifirla()
        {

            YemOlustur();
            YeminYeriniDeğiştir();
        }

        private Label YilanParcasiOlustur(int locationX, int locationY)
        {
            _yilanParcasiSayisi++;
            Label lbl = new Label()
            {
                Name = "yilanParca" + _yilanParcasiSayisi,
                BackColor = Color.Red,
                Width = yilanBoyutu,
                Height = yilanBoyutu,
                Location = new Point(locationX, locationY)

            };

            this.pnl.Controls.Add(lbl);
            return lbl;
        }

        private void YilaniYerleştir()
        {
            yilanKafasi = YilanParcasiOlustur(0, 0);
            yilanKafasi.Text = ":";
            yilanKafasi.ForeColor = Color.White;
            yilanKafasi.TextAlign = ContentAlignment.MiddleCenter;
            var LocationX = (pnl.Width / 2) - (yilanKafasi.Width / 2);
            var LocationY = (pnl.Width / 2) - (yilanKafasi.Width / 2);
            yilanKafasi.Location = new Point(LocationX, LocationY);
        }

        private void YemOlustur()
        {
            Label lbl = new Label()
            {
                Name = "yem" + _yilanParcasiSayisi,
                BackColor = Color.Yellow,
                Width = yemBoyutu,
                Height = yemBoyutu,


            };
            _yem = lbl;
            this.pnl.Controls.Add(lbl);

        }

        private void YeminYeriniDeğiştir()
        {
            var locationX = 0;
            var locationY = 0;


            bool durum;
            do
            {
                locationX = random.Next(0, pnl.Width - yemBoyutu);
                locationY = random.Next(0, pnl.Height - yemBoyutu);

                durum = false;
                var rectl = new Rectangle(new Point(locationX, locationY), _yem.Size);
                foreach (Control control in pnl.Controls)
                {
                    if (control is Label && control.Name.Contains("yilanParca"))
                    {
                        var rect2 = new Rectangle(control.Location, control.Size);
                        if (rectl.IntersectsWith(rect2))
                        {
                            durum = true;
                            break;
                        }
                    }
                }

            } while (durum);

            _yem.Location = new Point(locationX, locationY);
        }
        private enum HareketYonu
        {
            Yukari,
            Asagi,
            Sola,
            Saga
        }



        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            var keyCode = e.KeyCode;

            if (_yon==HareketYonu.Sola && keyCode== Keys.Right
                || _yon==HareketYonu.Saga && keyCode==Keys.Left
                || _yon==HareketYonu.Yukari && keyCode==Keys.Down
                || _yon==HareketYonu.Asagi && keyCode==Keys.Up)
            {

            }


            switch (keyCode)
            {
                case Keys.Up:
                    _yon = HareketYonu.Yukari;
                    break;
                case Keys.Down:
                    _yon = HareketYonu.Asagi;
                    break;
                case Keys.Left:
                    _yon = HareketYonu.Sola;
                    break;
                case Keys.Right:
                    _yon = HareketYonu.Saga;
                    break;
                case Keys.P:
                    timerYilanHareket.Enabled = false;
                    timersaat.Enabled = false;
                    break;
                case Keys.C:
                    timerYilanHareket.Enabled = true;
                    timersaat.Enabled = true;
                    break;
                default:
                    break;
            }
        }

        private void timerYilanHareket_Tick(object sender, EventArgs e)
        {
            YilanKafasiniTakipEt();

            var locationX = yilanKafasi.Location.X;
            var locationY = yilanKafasi.Location.Y;
            switch (_yon)
            {
                case HareketYonu.Yukari:
                    yilanKafasi.Location = new Point(locationX, locationY - (yilanKafasi.Width + _yilanprcArasiMesafe));
                    break;
                case HareketYonu.Asagi:
                    yilanKafasi.Location = new Point(locationX, locationY + (yilanKafasi.Width + _yilanprcArasiMesafe));
                    break;
                case HareketYonu.Sola:
                    yilanKafasi.Location = new Point(locationX - (yilanKafasi.Width + _yilanprcArasiMesafe), locationY);
                    break;
                case HareketYonu.Saga:
                    yilanKafasi.Location = new Point(locationX + (yilanKafasi.Width + _yilanprcArasiMesafe), locationY);
                    break;
                default:
                    break;
            }

            OyunBittimi();

            YilanYemiYedimi();
           

        }

        private void OyunBittimi()
        {
            bool oyunbittimi = false;
            var rect1 = new Rectangle(yilanKafasi.Location, yilanKafasi.Size);

            foreach  ( Control  control in pnl.Controls)
            {
                if (control is Label && control.Name.Contains("yilanParca") &&control.Name != yilanKafasi.Name)
                {
                    var rect2 = new Rectangle(control.Location, control.Size);
                    if (rect1.IntersectsWith(rect2))
                    {
                        oyunbittimi = true;
                        break;
                    }
                }

            }
            if (oyunbittimi)
            {
                timerYilanHareket.Enabled = false;
                timersaat.Enabled = false;
                DialogResult sonuc = MessageBox.Show("Puanınız : " + lblpuan.Text , "Oyun Bitti",MessageBoxButtons.OKCancel,MessageBoxIcon.Information);

                if (sonuc==DialogResult.OK)
                {
                    yenidenbaslat();
                }

            }


        }

        private void YilanYemiYedimi()
        {
            var rect1 = new Rectangle(yilanKafasi.Location, yilanKafasi.Size);
            var rect2 = new Rectangle(_yem.Location, _yem.Size);

            if (rect1.IntersectsWith(rect2))
            {
                lblpuan.Text = (Convert.ToInt32(lblpuan.Text) + 10).ToString();
                YeminYeriniDeğiştir();
                YilanParcasiOlustur(- yilanBoyutu,- yilanBoyutu);

            }

        }

        private void YilanKafasiniTakipEt()
        {
            if (_yilanParcasiSayisi <= 1) return;
            

            
            for (int i = _yilanParcasiSayisi; i >1 ; i--)
            {
                var sonrakiParca =(Label) pnl.Controls[i];
                var öncekiParca = (Label)pnl.Controls[i-1];

                sonrakiParca.Location = öncekiParca.Location;
            }

        }

        private void timersaat_Tick(object sender, EventArgs e)
        {
            lblsure.Text = (Convert.ToInt32(lblsure.Text) +1).ToString();
        }
    }
}
