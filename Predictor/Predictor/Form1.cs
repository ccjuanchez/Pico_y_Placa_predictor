using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Predictor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
       
    }
    public class pico_y_placa
    {
        public DateTime dateandtime;
        public string license_plate_number;
        public bool error;
        public pico_y_placa(string placa, string date, string time)
        {
            this.license_plate_number = placa;
            this.dateandtime = new DateTime();
            error = false;//formats //date dd/mm/yyyy //time hh:mm:ss
            if (date.Length != 10 || time.Length != 8)//The size of the date and hor is chck by the lenght.
                error = true;
            else
            {
                if (date[2] != '/' || date[5] != '/' || time[2] != ':' || time[5] != ':')
                    error = true;
                else
                {
                    DateTime temp;
                    if (DateTime.TryParse((date + " " + time), out temp))
                        this.dateandtime = temp;
                    else
                    {
                        MessageBox.Show("Incorrect Date!" + date + " " + time);
                        error = true;
                    }
                }
            }
        }
        public int get_week_day()
        {
            return (int)this.dateandtime.DayOfWeek;
        }
        public bool check_license_format()
        {
            char[] arr = this.license_plate_number.ToCharArray(0, this.license_plate_number.Length);
            if (arr.Length != 8)
                return false;
            for (int i = 0; i < arr.Length; i++)//ej PCC-2585
                if ((i < ((arr.Length / 2) - 1) && Char.IsDigit(arr[i])) || (i == ((arr.Length / 2) - 1) && (!arr[i].Equals('-'))) || (i >= ((arr.Length / 2)) && (!Char.IsDigit(arr[i]))))
                    return false;
            return true;
        }
        public int check_last_num()
        {//it is necesarry to chack that the license have 3 letters at the begining and 4 numbers at the end, divided by a "-"
            if (!this.check_license_format())
                return -1;
            int number = -1;
            string string2 = this.license_plate_number.Substring(this.license_plate_number.Length - 1, 1);
            bool success = Int32.TryParse(string2, out number);
            if (!success)
                number = -1;
            return number;  //if the license is incorrect it returns -1 as the kast number
        }
        public bool late()
        {//it is important to consider, that if you are on 9:30 or 19:30 you are free to drive on the road
            TimeSpan start1 = new TimeSpan(7, 0, 0);
            TimeSpan end1 = new TimeSpan(9, 30, 0);
            TimeSpan start2 = new TimeSpan(16, 0, 0);
            TimeSpan end2 = new TimeSpan(19, 30, 0);
            if ((this.dateandtime.TimeOfDay >= start1 && this.dateandtime.TimeOfDay < end1) || (this.dateandtime.TimeOfDay >= start2 && this.dateandtime.TimeOfDay < end2))//checks if the time specified is between the two intervals 
                return true;//if returns true, the car is cant move
            return false;
        }
        public bool can_be_on_the_road()
        {
            if (error || this.check_last_num() == -1)//if the license number is incorrect
            {
                MessageBox.Show("Fill correctly the inputs!");
                return false;
            }
            if ((((2 * this.get_week_day()) % 10) == this.check_last_num() || ((2 * this.get_week_day()) - 1) == this.check_last_num()) && (this.late()))//In this part pico y placa is checked by the date and hour
                return false;

            return true;
        }
    }
}
