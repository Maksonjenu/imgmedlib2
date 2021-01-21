﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace mem
{
    public partial class memType
    {
        private string mName = "";//имя

        private string mCategory = "";//категор

        private List<string> mTags = new List<string>(); //списк

        private BitmapImage bitmap = new BitmapImage(); //картинка 

        [JsonProperty("_mName")] //эта шляпа нужна чтоб указать метода прочтения из файла что в это свойство нужно писать свойство из файла
        public string _mName { get { return mName; } set { mName = value; } } //это свойство, не переменная. гет значит что мы вернем переменную (не путать со свойством) мНаме(она выше) 
        //когда прога попытается получить доступ к этому свойству. сет значит что если мы будем писать что то в это ствойство, то оно запишется в переменную
        [JsonProperty("_mCategory")] //аналогично
        public string _mCategory { get { return mCategory; } set { mCategory = value; } }

        [JsonProperty("_mTags")]
        public List<string> _mTags { get { return mTags; } set { mTags = value; } }

        [JsonProperty("_bitmap")]
        public string _bitmap { get { return bitmap.UriSource.ToString(); } set {
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(value);
                bitmap.EndInit();
                
            } } //сет - вернуть путь до файла картинки, гет назначить путь к файлу для битмапа.
        public BitmapImage getBit()
        {
            return bitmap;
        }
        public string getJson() //функция закатки строки в жсон строку
        {
            string json = System.Text.Json.JsonSerializer.Serialize(this); //конвертируем в строку джейсон этот (зис) элемент 

            return json; //вернули строку
        }

        public memType(string mName, string mCategory, List<string> mTags, string mPath) // конструктор класса
        {
            //зис означает что мы берем переменную из этого класса и пишем туда значение переданное в конструкторе
            this.mName = mName;
            this.mCategory = mCategory;
            this.mTags = mTags;

            this.bitmap.BeginInit();
            this.bitmap.UriSource = new Uri(mPath);
            this.bitmap.EndInit();
        }
        public memType() //пустой конструктор для джейсон десириализатора
        {
            
        }

    }
    public partial class memType 
    {
        public static memType FromJson(string json) => JsonConvert.DeserializeObject<memType>(json, Converter.Settings); //конвертация из джейсон в строку
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings //стандартная функция конвертации, по сути оно там все само делает
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
