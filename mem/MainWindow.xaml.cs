using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
using mem;

namespace mem
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        List<memType> list_of_mem = new List<memType>(); //список объектов типа мем

        List<string> categories = new List<string>() {"*", "category_1", "category_2", "category_3", "category_4", "category_5" }; // список категорий, можно менять, кол-во не важно, "any" рекомендую оставить
        public MainWindow()
        {
            InitializeComponent();
            foreach(string categor in categories)
            {
                m_category_cb.Items.Add(categor);
            }
            //говорим листбоксу с категориями из какого списка брать категории
            m_list.DisplayMemberPath = "_mName"; // говорим списку с мемами, что нужно отображать не тип класса, а определенное поле из класса
        }

        private void m_Search_Click(object sender, RoutedEventArgs e) //поиск
        {
            m_list.Items.Clear();
           foreach (memType mTemp in list_of_mem) //каждый элемент типа мем в списке мемеов
            {
                if (mTemp._mName.Contains(m_name_lab.Content.ToString())) //проверяем: если  имя экземпляра содержит текст из текстбокса, то добавляем этот экземпляр в листбокс
                {
                    m_list.Items.Add(mTemp); // добавили
                }

            }
        }

       

        private void mList_SelectionChanged(object sender, SelectionChangedEventArgs e) //обработка выбора мемов в листбоксе
        {
            if (m_list.SelectedItem != null) // если выбран не пустой элемент
            {
                m_tag_tb.Text = ""; 
                memType temp = m_list.SelectedItem as memType; //создаем переменную типа мем из выбранного элемента из листбокса с мемами
                imageBox1.Source = temp.getBit(); //в имаджбокс пишем путь до картинки 
                m_name_lab.Content = temp._mName; // в текстбокс имени пишем имя 
                foreach (string str in temp._mTags) //получаем список тэгов 
                    m_tag_tb.Text += str + ", "; //пишем тэги в бокс для тэгов
                m_categ_lab.Content = temp._mCategory; //вывод категории в тб категорий
            }
        }

        private void m_CategoryCb_SelectionChanged(object sender, SelectionChangedEventArgs e) //сортировка по категориям
        {
            if ((m_category_cb.SelectedIndex != -1) && m_category_cb.SelectedItem.ToString() != "*") // если выбранная категория не за гранницей и не равно любой 
            {
                m_list.Items.Clear();
                foreach (memType memSampl in list_of_mem) //проход по списку мемов
                {
                    if ((memSampl._mCategory) == m_category_cb.SelectedItem.ToString()) //сравнием категории мема и выбраннную
                    {
                        m_list.Items.Add(memSampl); // добавляем если сошлось
                    }
                }
            }
            else  //если нет
                if (m_category_cb.SelectedItem.ToString() == "*")  //если категория любая 
            {
                m_list.Items.Clear();
                    foreach (memType temp in list_of_mem) // выводим все мемы по очереди
                    m_list.Items.Add(temp);
                }
        }

        private void remov_Button_Click(object sender, RoutedEventArgs e) // кнопка удаления
        {
            if (m_list.SelectedItem != null) // если ывбран не пустой
            {
                list_of_mem.Remove(m_list.SelectedItem as memType); // удаляем мем из списка мемов выбранный элемент из листбокса мемов, который приведен к типу мем

                m_list.Items.Remove(m_list.SelectedItem); // удаление из листбокса мемов
            }
        }

        private void tagsSearch_But_Click(object sender, RoutedEventArgs e) //поиск по тэгам
        {
            m_list.Items.Clear();
            List<string> sTgs = new List<string>(); //список тэгов создаем
            string[] temp = m_tag_search_tb.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); //из текстбокса введеные тэги через пробел делим на слова, пихаем их в массив слов темп
            foreach (string sTemp in temp)
            {
                sTgs.Add(sTemp); //добавляем слова из массива в список
            }

            foreach (memType memSample in list_of_mem) // проходим список мемов
            {
                List<string> tagsList = memSample._mTags; //получаем список тэгов мема
                foreach (string tag in tagsList) // получаем конкретный тэг 
                    foreach (string st in sTgs) //получаем тэг мема
                        if (tag == st) // если тэг есть
                        {
                            m_list.Items.Add(memSample);//добавляем мем в список
                            break; //выходим из цикла чтоб не искать дальше
                        }
            }
        }

        private void add_mems_Click(object sender, RoutedEventArgs e) //кнопка открытия формы добавления
        {
            add add = new add(categories); // создаем экземпляр формы "добавить", передаем в него количество мемов в списке и список категорий 
            add.Show(); //выводим на экран форму
            add_mem.IsEnabled = false; //выключаем кнопку добавления
            add.Closing += Add_Closed; //создаем событие закрытие формы добавить, при закрытие будет вызываться метод add_closed, он ниже
        }

        private void Add_Closed(object sender, EventArgs e)
        {

            var s = sender as add; //тут мы создаем для удобсва переменную, сендер это и есть форма, нужно обозначить что она имеет тип добавить

            if (s._mem != null)  //если функция получить мем сработала
            {
                list_of_mem.Add(s._mem); //добавляем мем в список мемеов

                m_list.Items.Add(list_of_mem.Last()); //добавляем последний мем из списка в листбокс
            }
            add_mem.IsEnabled = true; //включаем кнопку

            Closing -= Add_Closed; //отписуемся от события 
        }
        private void save_json_Click(object sender, RoutedEventArgs e) //сохранение в джейсон файл 
        {
            if (list_of_mem.Count != 0) // если список не пустой 
            {
                string json = ""; //создлаем пусту строку 
                foreach (memType temp in list_of_mem) //получаем мем из списка мемов
                    json +=  temp.getJson() + "\n"; //формируем строку джейсон гетджейсон и пишем ее в строку джосн

                SaveFileDialog filed = new SaveFileDialog(); //создаем экземпляр сохранения файла
                filed.Filter = "json files (*.json)|*.json|All files (*.*)|*.*"; //ставим фильтры и тип файла который сохраняем

                filed.ShowDialog(); //показываем форму сохранения

                File.WriteAllText(filed.FileName, json); //пишем строку джсон по заданному пути в шоудиалог
            }
            else
                MessageBox.Show("Список мемов пустой"); //мемрв нет
        }

        private void open_Json_Click(object sender, RoutedEventArgs e) //открыть из джсон
        {
            m_list.Items.Clear();
            OpenFileDialog openFileDialog = new OpenFileDialog(); //экземпляр открытия файла
            openFileDialog.Filter = "Файлы изображений (*.json) | *.json;)"; //тип файла для открытия
            openFileDialog.ShowDialog(); //показываем форму
            List<string> mass = File.ReadAllLines(openFileDialog.FileName).ToList(); //в список строк читаем построчно открытый джсон файл
            foreach (string js in mass) //каждая строка в списке строк
            {
                if (js != "") // если строка не пустая
                {
                    memType.FromJson(js); //формируем тип мем из строки 
                    list_of_mem.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<memType>(js)); //добавляем мем в список
                    m_list.Items.Add(list_of_mem.Last()); //добавляем мем в листбокс

                }
            }
        }
    }
}
