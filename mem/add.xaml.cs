using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;

namespace mem
{
    /// <summary>
    /// Логика взаимодействия для add.xaml
    /// </summary>
    public partial class add : Window
    {
        private string path = "";//путь до файла

        private List<string> tags = new List<string>(); //список тэгов

        private memType mem; //экземпляр мем

        public memType _mem { get { return mem; } set { mem = value; } }


        public add(List<string> surs) //конструктор формы добавить, передаем в эту форму из главной айди и список категорий
        {
            InitializeComponent();
            foreach (string categor in surs)
            {
                m_category.Items.Add(categor);
            }
            filepick.IsEnabled = false; //кнопка переключения файла \ ссылки выключена
        }
        private memType formMeme(string tags, string name, string category, string filename) // фукнция формирования мемов
        {
            string[] temp = tags.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); //делим тэги на слова
            foreach (string sTemp in temp)
            {
                this.tags.Add(sTemp); // пишем в список тэгов тэги
            }
            return new memType(name, category, this.tags, filename); // возвращаем новый экземпляр мема с параметрами
        }
        private string openimg() //функция открытие картинки 
        {
            OpenFileDialog openFileDialog = new OpenFileDialog(); // форма
            openFileDialog.Filter = "Файлы изображений (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png)"; // филтр файлов изображения
            var result = openFileDialog.ShowDialog(); // открытие формы
            if (result == true)
            {
                m_name_tb.Text = openFileDialog.SafeFileName; //в текстобокс вывод пути до файла
                return openFileDialog.FileName; // вернуть путь до файла
            }
            else return null; //иначе ничего 
        }

        

        

        private void openIMG_Click(object sender, RoutedEventArgs e) //кнопка открытие изобр
        {
            path = openimg(); // в путь до файла пишем из функции путь до файла

            if (path != null) //если путь не пустой
            {

                BitmapImage bitmap = new BitmapImage(); //новый экзмплр картинки
                bitmap.BeginInit(); //начало редкатирования 
                bitmap.UriSource = new Uri(path); //путь до файла
                bitmap.EndInit(); // конец редактирования

                memPrev.Source = bitmap; //предпросмотр картинки
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((m_tag_tb.Text != "") && (m_name_tb.Text != "") && (m_category.SelectedIndex != -1) && (path != null)) //если поля заполнены
            {
               mem = formMeme(m_tag_tb.Text, m_name_tb.Text, m_category.SelectedItem.ToString(), path); // в переменную мем записываем мем с параметрами из функции гетмем

                this.Hide(); //прячем форму

                this.Close(); //закрываем форму
            }
            else
            {
                warn.Content = "не всё заполнено"; //если какие то поля пустые, предупреждаем
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) //событие закрытия формы
        {
            this.Hide(); //прячем форму
        }


        private void filepick_Click(object sender, RoutedEventArgs e) //переключаем кнопки 
        {
            filepick.IsEnabled = false;
            m_urlpick.Visibility = (Visibility)0;
            m_urlpath.Visibility = (Visibility)1; 
            openIMG.Visibility = 0;
        }

        private void urlpick_Click(object sender, RoutedEventArgs e) //тоже самое ничего сложного
        {
            filepick.IsEnabled = true;
            m_urlpick.Visibility = (Visibility)1;
            m_urlpath.Visibility = 0;
            openIMG.Visibility = (Visibility)1;
        }

    }
}
