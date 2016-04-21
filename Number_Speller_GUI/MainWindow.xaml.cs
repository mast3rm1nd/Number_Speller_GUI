using System;
using System.Collections.Generic;
using System.Linq;
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

using System.IO;
using System.Text.RegularExpressions;

namespace Number_Speller_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string[] single_nums = {"один", "два", "три", "четыре", "пять", "шесть",
                                              "семь", "восемь", "девять"};

        public static string[] double_nums = {"десять", "одиннадцать", "двенадцать", "тринадцать",
                                              "четырнадцать", "пятнадцать", "шестнадцать", "семнадцать",
                                              "восемнадцать", "девятнадцать"};

        public static string[] tens = {"двадцать", "тридцать", "сорок", "пятьдесят", "шестьдесят",
                                       "семдьесят", "восемьдесят", "девяносто"};

        public static string[] hundreds = {"сто", "двести", "триста", "четыреста", "пятьсот", "шестьсот",
                                           "семьсот", "восемьсот", "девятьсот"};

        public static string[] thousands = {"тысяч", "тысяча", "тысячи", "тысячи",
                                            "тысячи", "тысяч", "тысяч", "тысяч", "тысяч", "тысяч"};



        public static string[] powers_of_10 = File.ReadAllLines("powers_of_10.txt");




        public static string[] endings = { "ов", "", "а", "а", "а", "ов", "ов", "ов", "ов", "ов" };


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }



        static string NumberSpeller(string number)
        {
            var spelling = "";

            bool isSign = false;

            if (number[0] == '-')
            {
                spelling = "минус ";
                isSign = true;
            }




            var tenth_powers = ((number.Length - 1) / 3) - 2;

            for (int i = isSign == true ? 1 : 0; i < number.Length; i++)
            {
                var curr_digit = int.Parse(number[i].ToString());
                var digits_left = number.Length - i;

                if (digits_left == 4)
                {
                    var prev_digit = int.Parse(number[i].ToString());
                    switch (number[i])
                    {
                        case '0':
                            {
                                if (i >= 2)
                                {
                                    if (!(number[i - 1] == '0' && number[i - 2] == '0'))
                                        spelling += thousands[int.Parse(number[i].ToString())] + " ";
                                }
                                else
                                    spelling += thousands[int.Parse(number[i].ToString())] + " ";

                            } break;




                        case '1':
                            {
                                if (number.Length % 2 == 0)
                                {
                                    if (i >= 1)
                                    {
                                        if (number[i - 1] == '0')
                                            spelling += "одна " + thousands[prev_digit] + " ";
                                        else
                                            spelling += "тысяч ";
                                    }
                                    else
                                    {
                                        spelling += "одна " + thousands[prev_digit] + " ";
                                    }
                                }
                                else
                                {
                                    spelling += thousands[prev_digit - 1] + " ";
                                }
                            } break;




                        case '2':
                            {
                                if (i >= 1)
                                {
                                    if (number[i - 1] == '0')
                                        spelling += "две " + thousands[prev_digit - 1] + " ";
                                    else
                                        spelling += "тысяч ";
                                }
                                else
                                {
                                    spelling += "две " + thousands[prev_digit] + " ";
                                }

                            } break;





                        default:
                            {
                                var index = int.Parse(number[i].ToString()) - 1;

                                if (number.Length >= 4)
                                    if (i >= 1)
                                    {
                                        if (number[i - 1] != '1')
                                            spelling += single_nums[index] + " " + thousands[index + 1] + " ";
                                        else
                                            spelling += "тысяч ";
                                    }
                                    else
                                        spelling += single_nums[index] + " " + thousands[index + 1] + " ";
                            } break;

                    }
                }





                if (curr_digit == 0 && digits_left < 3)
                {
                    continue;
                }


                if (tenth_powers >= 0 && (number.Length - i) % 3 == 0)
                {
                    if (i >= 3)
                    {
                        if (number[i - 1] != '0' || number[i - 2] != '0' || number[i - 3] != '0')
                        {
                            if (number[i - 2] == '0')
                            {
                                var suffix_index = int.Parse(number[i - 1].ToString());
                                var ending = endings[suffix_index];

                                spelling += powers_of_10[tenth_powers] + ending + " ";
                            }
                            else
                            {
                                var suffix_index = int.Parse(number[i - 1].ToString());
                                var ending = endings[suffix_index];

                                if (number[i - 2] == '1')
                                    spelling += powers_of_10[tenth_powers] + endings[0] + " ";
                                else
                                    spelling += powers_of_10[tenth_powers] + ending + " ";
                            }


                            tenth_powers--;
                        }
                    }
                    else if (i == 2)
                    {
                        if (number[i - 2] == '1')
                            spelling += powers_of_10[tenth_powers] + endings[0] + " ";
                        else
                        {
                            var suffix_index = int.Parse(number[i - 1].ToString());
                            spelling += powers_of_10[tenth_powers] + endings[suffix_index] + " ";
                        }

                        tenth_powers--;
                    }
                    else if (i == 1)
                    {
                        var suffix_index = int.Parse(number[i - 1].ToString());
                        spelling += powers_of_10[tenth_powers] + endings[suffix_index] + " ";

                        tenth_powers--;
                    }
                }








                switch (digits_left % 3)
                {
                    case 1:
                        {
                            if (digits_left > 4)
                            {
                                if (i == 0)
                                    spelling += single_nums[curr_digit - 1] + " ";
                                else if (number[i] != '0' && digits_left > 4 && number[i - 1] != '1')
                                    spelling += single_nums[curr_digit - 1] + " ";
                            }
                            else if (digits_left == 1)
                                spelling += single_nums[curr_digit - 1];
                        }
                        break;



                    case 2: if (curr_digit == 1)
                        {
                            var next_digit = int.Parse(number[i + 1].ToString());
                            spelling += double_nums[next_digit] + " ";

                            if (digits_left < 5)
                                i++;
                            continue;
                        }
                        else if (curr_digit >= 2)
                        {
                            spelling += tens[curr_digit - 2] + " ";
                        }
                        break;



                    case 0:
                        {
                            if (curr_digit != 0)
                                spelling += hundreds[curr_digit - 1] + " ";
                        }
                        break;
                }
            }

            return spelling;
        }

        private void TextBox_Number_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Regex.IsMatch(TextBox_Number.Text, @"^-*\d+$"))
                    TextBox_Spelling.Text = NumberSpeller(TextBox_Number.Text);
                else
                    TextBox_Spelling.Text = "";
            }
            catch
            {
                TextBox_Spelling.Text = "Извините, но это число слишком большое =)";
            }
            
        }
    }
}
